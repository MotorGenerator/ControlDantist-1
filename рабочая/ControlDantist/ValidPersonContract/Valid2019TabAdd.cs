using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;

namespace ControlDantist.ValidPersonContract
{
    /// <summary>
    /// Проверка льготников 2019 года по таблицам ИмяТаблицыAdd.
    /// </summary>
    public class Valid2019TabAdd : IValidatePersonContract
    {
        private string famili = string.Empty;
        private string name = string.Empty;
        private string secondName = string.Empty;
        private DateTime dateBirth;
        private string query = string.Empty;

        public Valid2019TabAdd(string famili, string name, string secondName, DateTime dateBirth)
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
                                @" select ДоговорAdd.id_договор,НомерДоговора,АктВыполненныхРаботAdd.ДатаПодписания as ДатаПодписания,ЛьготникAdd.Фамилия,ЛьготникAdd.Имя,ЛьготникAdd.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    SUM(УслугиПоДоговоруAdd.Сумма) as Сумма,ДоговорAdd.ДатаЗаписиДоговора,ДоговорAdd.НомерРеестра,ДоговорAdd.НомерСчётФактрура
			                    ,ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,2019 as Год,flagАнулирован from dbo.ДоговорAdd
                              inner join ЛьготникAdd
                                ON ДоговорAdd.id_льготник = ЛьготникAdd.id_льготник
                                inner join ЛьготнаяКатегория
                                ON ДоговорAdd.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                                inner join УслугиПоДоговоруAdd
                                ON УслугиПоДоговоруAdd.id_договор = ДоговорAdd.id_договор
                                left outer join АктВыполненныхРаботAdd
                                ON АктВыполненныхРаботAdd.id_договор = ДоговорAdd.id_ТабДоговор
                                 where LOWER(RTRIM(LTRIM([Фамилия]))) = LOWER(LTRIM(RTRIM(@FirstName))) and LOWER(LTRIM(RTRIM(Имя))) = LOWER(LTRIM(RTRIM(@Name))) and((LOWER(LTRIM(RTRIM(Отчество))) = LOWER(LTRIM(RTRIM(@Surname)))) or Отчество is null)
                                 and ЛьготникAdd.ДатаРождения = @DateBirth
                                group by ДоговорAdd.id_договор,НомерДоговора,ЛьготникAdd.Фамилия,ЛьготникAdd.Имя,ЛьготникAdd.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    ДоговорAdd.ДатаЗаписиДоговора,ДоговорAdd.НомерРеестра,ДоговорAdd.НомерСчётФактрура ,АктВыполненныхРаботAdd.НомерАкта
			                    ,АктВыполненныхРаботAdd.ДатаПодписания,ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,ДатаДоговора,flagАнулирован ";

            return this.query;
        }
    }
}
