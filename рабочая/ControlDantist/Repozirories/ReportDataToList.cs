using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ControlDantist.Classes;

namespace ControlDantist.Repozirories
{
    public class ReportDataToList
    {
        public List<ReportYear> ConvertToList(DataTable tab)
        {
            List<ReportYear> list = new List<ReportYear>();

            if (tab == null)
            {
                System.Windows.Forms.MessageBox.Show("Отсутствуют данные для отчета");
                return null;
            }

            foreach(DataRow row in tab.Rows)
            {
                ReportYear itm = new ReportYear();
                itm.Район = row["Район"].ToString();
                itm.Поликлинника = row["Поликлинника"].ToString();
                if (row["Пропускная способность за 2019 год"] != DBNull.Value)
                {
                    itm.ПропускнаяСпособностьГод = Convert.ToInt16(row["Пропускная способность за 2019 год"]);
                }
                else
                {
                    itm.ПропускнаяСпособностьГод = 0;
                }

                if (row["Очередность за 2019 год"] != DBNull.Value)
                {
                    itm.ОчередностьГод = Convert.ToInt16(row["Очередность за 2019 год"]);
                }
                else
                {
                    itm.ОчередностьГод = 0;
                }

                if (row["количество заключенных договоров"] != DBNull.Value)
                {
                    itm.КоличествоЗаключенныхДоговоров = Convert.ToInt16(row["количество заключенных договоров"]);
                }
                else
                {
                    itm.КоличествоЗаключенныхДоговоров = 0;
                }

                if (row["сумма заключенных договоров"] != DBNull.Value)
                {
                    itm.СуммаЗаключенныхДоговоров = Convert.ToDecimal(row["сумма заключенных договоров"]);
                }
                else
                {
                    itm.СуммаЗаключенныхДоговоров = 0.0m;
                }


                itm.SerialNumber = Convert.ToInt16(row["SerialNumber"]);

                if (row["кол-во договоров находящихся в деле"] != DBNull.Value)
                {
                    itm.КоличествоДоговоровВДеле = Convert.ToInt16(row["кол-во договоров находящихся в деле"]);
                }
                else
                {
                    itm.КоличествоДоговоровВДеле = 0;
                }

                if (row["сумма договоров находящихся в деле"] != DBNull.Value)
                {
                    itm.СуммаДоговоровВДеле = Convert.ToDecimal(row["сумма договоров находящихся в деле"]);
                }
                else
                {
                    itm.СуммаДоговоровВДеле = 0.0m;
                }

                if (row["кол-во договоров поступивших на оплату"] != DBNull.Value)
                {
                    itm.КоличествоДоговоровПоступившихНаОплату = Convert.ToInt16(row["кол-во договоров поступивших на оплату"]);
                }

                if (row["сумма договоро поступивщих на оплату"] != DBNull.Value)
                {
                    itm.СуммаДоговороПоступившихНаОплату = Convert.ToDecimal(row["сумма договоро поступивщих на оплату"]);
                }
                else
                {
                    itm.СуммаДоговороПоступившихНаОплату = 0.0m;
                }

                list.Add(itm);
                
            }

            return list;
        }
    }
}
