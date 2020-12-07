using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Find
{
    public class FindPersonForNumberLetter
    {
        private string family = string.Empty;
        private string name = string.Empty;
        private string surname = string.Empty;

        public FindPersonForNumberLetter(string family, string name, string surname)
        {
            this.family = family;
            this.name = name;
            this.surname = surname;
        }

        public DataTable GetNumberLetter()
        {
            return ТаблицаБД.GetTableSQL(this.Query(), "Person");
        }

        /// <summary>
        /// Возвращает строку запроса.
        /// </summary>
        /// <returns></returns>
        private string Query()
        {
            string query = string.Empty;

            if (this.surname.Trim() != "".Trim())
            {
                query = @"select Фамилия,Имя,Отчество,ДатаРождения,ProjectRegistrFiles.NumberLatter,ProjectRegistrFiles.DateLetter  from ПриемРеестровЛьготник
                    inner join ПриемРеестрвДоговор
                    ON ПриемРеестрвДоговор.id_льготник = ПриемРеестровЛьготник.id_льготник
                    inner join ProjectRegistrFiles
                    ON ProjectRegistrFiles.IdProjectRegistr = ПриемРеестрвДоговор.idFileRegistProgect
                    where Фамилия = '" + this.family + "' and Имя = '" + this.name + "' and Отчество = '" + this.surname.Do(x => x, "") + "'";
            }
            else if(this.family.Trim() != "" && this.name.Trim() != "" && this.surname.Trim() == "")
            {
                query = @"select Фамилия,Имя,Отчество,ДатаРождения,ProjectRegistrFiles.NumberLatter,ProjectRegistrFiles.DateLetter  from ПриемРеестровЛьготник
                    inner join ПриемРеестрвДоговор
                    ON ПриемРеестрвДоговор.id_льготник = ПриемРеестровЛьготник.id_льготник
                    inner join ProjectRegistrFiles
                    ON ProjectRegistrFiles.IdProjectRegistr = ПриемРеестрвДоговор.idFileRegistProgect
                    where Фамилия = '" + this.family + "' and Имя = '" + this.name + "' ";
            }
            else if (this.family.Trim() != "" && this.name.Trim() == "" && this.surname.Trim() == "")
            {
                query = @"select Фамилия,Имя,Отчество,ДатаРождения,ProjectRegistrFiles.NumberLatter,ProjectRegistrFiles.DateLetter  from ПриемРеестровЛьготник
                    inner join ПриемРеестрвДоговор
                    ON ПриемРеестрвДоговор.id_льготник = ПриемРеестровЛьготник.id_льготник
                    inner join ProjectRegistrFiles
                    ON ProjectRegistrFiles.IdProjectRegistr = ПриемРеестрвДоговор.idFileRegistProgect
                    where Фамилия = '" + this.family + "' ";
            }

            return query;
        }
    }
}
