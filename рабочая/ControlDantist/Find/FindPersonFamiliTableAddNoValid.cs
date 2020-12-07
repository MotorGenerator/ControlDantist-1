using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonFamiliTableAddNoValid : IFindPerson
    {
        private string famili = string.Empty;

        public FindPersonFamiliTableAddNoValid(string famili)
        {
            this.famili = famili ?? throw new ArgumentNullException(nameof(famili));
        }

        public virtual string Query()
        {
            string query = @"declare @FirstName nvarchar(150)
                                declare @Name nvarchar(150)
                                declare @Surname nvarchar(150)
                                declare @DateBirth date
                                declare @NumContract nchar(100)
			                    set @FirstName = '" + this.famili + "' " +
                               @" select ДоговорAdd.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    SUM(УслугиПоДоговоруAdd.Сумма) as Сумма,ДоговорAdd.ДатаЗаписиДоговора,ДоговорAdd.НомерРеестра,ДоговорAdd.НомерСчётФактрура
			                    ,АктВыполненныхРаботAdd.ДатаПодписания,ДоговорAdd.logWrite,НомерАкта,ДоговорAdd.flag2019AddWrite, 2019 as 'Год',flagАнулирован from dbo.ДоговорAdd
                              inner join Льготник
                                ON ДоговорAdd.id_льготник = Льготник.id_льготник
                                inner join ЛьготнаяКатегория
                                ON ДоговорAdd.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                                inner join УслугиПоДоговоруAdd
                                ON УслугиПоДоговоруAdd.id_договор = ДоговорAdd.id_договор
                                left outer join АктВыполненныхРаботAdd
                                ON АктВыполненныхРаботAdd.id_договор = ДоговорAdd.id_договор
                                 where[Фамилия] = @FirstName
                                and ДоговорAdd.ФлагПроверки = 'False'
                                --and YEAR(ДоговорAdd.ДатаЗаписиДоговора)  2019
                                --and(ДатаАктаВыполненныхРабот is not null) and(YEAR(ДатаАктаВыполненныхРабот) <> 1900)
                                group by ДоговорAdd.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    ДоговорAdd.ДатаЗаписиДоговора,ДоговорAdd.НомерРеестра,ДоговорAdd.НомерСчётФактрура ,АктВыполненныхРаботAdd.НомерАкта
			                    ,АктВыполненныхРаботAdd.ДатаПодписания,ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,flagАнулирован";

            return query;
        }
    }
}
