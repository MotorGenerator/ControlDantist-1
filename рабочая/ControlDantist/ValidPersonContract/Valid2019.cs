using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;

namespace ControlDantist.ValidPersonContract
{
    /// <summary>
    /// Проверка 2019 года по обычным таблицам.
    /// </summary>
    public class Valid2019 : IValidatePersonContract
    {
        private string famili = string.Empty;
        private string name = string.Empty;
        private string secondName = string.Empty;
        private DateTime dateBirth;
        private string query = string.Empty;

        public Valid2019(string famili, string name, string secondName, DateTime dateBirth)
        {
            this.famili = famili ?? throw new ArgumentNullException(nameof(famili));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.secondName = secondName ?? throw new ArgumentNullException(nameof(secondName));
            this.dateBirth = dateBirth;
        }

        //public string GetQuery()
        //{
        //    return this.query;
        //}

        public string Execute()
        {
            this.query = @"declare @FirstName nvarchar(150)
                                declare @Name nvarchar(150)
                                declare @Surname nvarchar(150)
                                declare @DateBirth date
                                declare @NumContract nchar(100)
			                    set @FirstName = '" + this.famili.Trim() + "'   " +
                                " set @Name = '" + this.name.Trim() + "'   " +
                                " set @Surname = '" + this.secondName.Trim() + "'   " +
                                " set @DateBirth = '" + Время.Дата(this.dateBirth.ToShortDateString().Trim()) + "' " +
                                @" select Договор.id_договор,НомерДоговора,АктВыполненныхРабот.ДатаПодписания as ДатаПодписания,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    SUM(УслугиПоДоговору.Сумма) as Сумма,Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура
			                    ,Договор.logWrite,Договор.flag2019AddWrite,2018 as Год,flagАнулирован from dbo.Договор
                              inner join Льготник
                                ON Договор.id_льготник = Льготник.id_льготник
                                inner join ЛьготнаяКатегория
                                ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                                inner join УслугиПоДоговору
                                ON УслугиПоДоговору.id_договор = Договор.id_договор
                                inner join АктВыполненныхРабот
                                ON АктВыполненныхРабот.id_договор = Договор.id_договор
                                 where LOWER(RTRIM(LTRIM([Фамилия]))) = LOWER(LTRIM(RTRIM(@FirstName))) and LOWER(LTRIM(RTRIM(Имя))) = LOWER(LTRIM(RTRIM(@Name))) and((LOWER(LTRIM(RTRIM(Отчество))) = LOWER(LTRIM(RTRIM(@Surname)))) or Отчество is null)
                                 and Льготник.ДатаРождения = @DateBirth
                                --and Договор.ФлагПроверки = 'False'
                                and YEAR(Договор.ДатаЗаписиДоговора) = 2019
                                and(ДатаАктаВыполненныхРабот is not null) and(YEAR(ДатаАктаВыполненныхРабот) <> 1900) and flag2019AddWrite = 0
                                group by Договор.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура ,АктВыполненныхРабот.НомерАкта
			                    ,АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,ДатаДоговора,flagАнулирован ";

            return query;
        }
    }
}
