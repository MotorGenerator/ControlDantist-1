using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DantistLibrary;
using System.Data;
using ControlDantist.Repository;


namespace ControlDantist.ReceptionDocuments
{
    /// <summary>
    /// Реестр проектов договоров.
    /// </summary>
    public class Registr
    {
        private Dictionary<string, Unload> unloads;
        private List<DisplayContract> listContracts;

        private bool flagListContract = false;

        /// <summary>
        /// Флаг указывает что не у всех льготников установлен район области.
        /// </summary>
        public  bool FlagErrorRegionRegistr { get; set; }

        public Registr(Dictionary<string, Unload> unloads)
        {
            if(unloads != null)
            {
                this.unloads = unloads;
                listContracts = new List<DisplayContract>();
            }
            else
            {
                throw new Exception("Файл проектов договоров отсутствует. ");
            }
            
        }

        /// <summary>
        /// Сумма реестра проектов договоров.
        /// </summary>
        /// <returns></returns>
        public decimal SumContracts()
        {
            return listContracts.Sum(w => w.SummContract);
        }

        /// <summary>
        /// Количество договоров.
        /// </summary>
        /// <returns></returns>
        public int GetCountContracts()
        {
            return listContracts.Count();
        }

        public string GetPrivelegetCategory()
        {
            return unloads.Values.First().ЛьготнаяКатегория.Trim();
        }


        /// <summary>
        /// Возвращает список проектов договоров.
        /// </summary>
        
        public List<DisplayContract> GetDisplayContract()
        {

            UnitDate unitDate = new UnitDate();

            if (this.flagListContract == false)
            {
                foreach (var unload in unloads.Values)
                {
                    // Проект договора.
                    DisplayContract dc = new DisplayContract();

                    decimal sum = 0.0m;

                    foreach (DataRow row in unload.Договор.Rows)
                    {
                        dc.NumberContract = row["НомерДоговора"].ToString().Trim();
                    }

                    foreach (DataRow row in unload.УслугиПоДоговору.Rows)
                    {
                        sum += Convert.ToDecimal(row["Сумма"]);
                    }

                    if (unload.Льготник.Columns.Contains("FlagRaion")== true)
                    {
                        foreach (DataRow row in unload.Льготник.Rows)
                        {
                                    if (row["FlagRaion"].ToString().Length > 0)
                                    {
                                        dc.FlagRegion = true;
                                    }
                                    else
                                    {
                                        // Флаг установили в ошибку.
                                        FlagErrorRegionRegistr = true;
                                    }
                        }
                    }
                    else
                    {
                        // Флаг установили в ошибку.
                        FlagErrorRegionRegistr = true;
                    }

                    dc.SummContract = sum;

                    StringBuilder fioBuilder = new StringBuilder();

                    fioBuilder.Append(unload.Льготник.Rows[0]["Фамилия"].ToString().Trim());
                    fioBuilder.Append(" ");
                    fioBuilder.Append(unload.Льготник.Rows[0]["Имя"].ToString().Trim());
                    fioBuilder.Append(" ");
                    fioBuilder.Append(unload.Льготник.Rows[0]["Отчество"].ToString().Trim());

                    dc.FioPerson = fioBuilder.ToString();

                    listContracts.Add(dc);
                }
            }

            this.flagListContract = true;

            return listContracts;

        }
    }
}
