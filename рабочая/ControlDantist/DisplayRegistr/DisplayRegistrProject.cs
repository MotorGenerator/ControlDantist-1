using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.ClassValidRegions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ControlDantist.Classes;

namespace ControlDantist.DisplayRegistr
{
    public class DisplayRegistrProject
    {
        private Dictionary<string, PersonValidEsrn> dictionary;

        // Сумма реестра проектов договоров.
        private decimal sumRegistr = 0.0m;

        private List<ResultValidEsrnDisplay> list;

        public DisplayRegistrProject(Dictionary<string, PersonValidEsrn> dictionary)
        {
            this.dictionary = dictionary;

            list = new List<ResultValidEsrnDisplay>();

            ConvertRegistr();
        }

        public List<ResultValidEsrnDisplay> GetRegistr()
        {
            return this.list.OrderByDescending(w => w.FlagValidEsrn).ToList();// (w=>w.FlagValidEsrn);
        }



        private void ConvertRegistr()
        {
            //List<ResultValidEsrnDisplay> list = new List<ResultValidEsrnDisplay>();

            foreach (var itm in dictionary.Values)
            {
                ResultValidEsrnDisplay r = new ResultValidEsrnDisplay();

                // id договор.
                r.IdContract = itm.IdContract;

                // Номер договора.
                r.НомерДоговора = itm.номерДоговора.Trim();

                if (itm.отчество != null)
                {
                    r.ФиоЛьготник = itm.фамилия.Trim() + " " + itm.имя.Trim() + " " + itm.отчество.Trim();
                }
                else
                {
                    r.ФиоЛьготник = itm.фамилия.Trim() + " " + itm.имя.Trim();
                }

                r.Адрес = itm.Адрес;

                r.СерияНомерУдостоверения = itm.серияДокумента + " " + itm.номерДокумента;

                // Флаг что прошел провреку в ЭСРН.
                r.FlagValidEsrn = itm.flagValidEsrn;

                // Флаг сверки услуг на сервере и в выгрузке.
                r.FlagValidServices = itm.flagValidMedicalServices;

                // Флаг сохранения в БД.
                r.FlagSaveContract = false;

                decimal sumContract = ProjectContractSum(itm.IdContract);

                sumRegistr += sumContract;

                r.SumContract = sumContract.ToString("c");

                list.Add(r);
            }

        }

        private decimal ProjectContractSum(int idContract)
        {
            DataTable dataTable = ТаблицаБД.GetTableSQL(GetQuery(idContract), "Сумма");

            return Convert.ToDecimal(dataTable.Rows[0][0]);
        }

        private string GetQuery(int idContract)
        {
            string query = @"select SUM(Сумма) from УслугиПоДоговору
                            inner join Договор
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            where Договор.id_договор = "+ idContract + " ";

            return query;
        }

        public decimal GetSumContracts()
        {
            return sumRegistr;
        }

        
    }
}
