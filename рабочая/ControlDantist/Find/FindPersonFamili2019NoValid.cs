using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonFamili2019NoValid : IFindPerson
    {
        string personFamili = string.Empty;

        public FindPersonFamili2019NoValid(string personFamili)
        {
            this.personFamili = personFamili ?? throw new ArgumentNullException(nameof(personFamili));
        }

        public virtual string Query()
        {
            string query = @"declare @FirstName nvarchar(150)
                                declare @Name nvarchar(150)
                                declare @Surname nvarchar(150)
                                declare @DateBirth date
                                declare @NumContract nchar(100)
			                    set @FirstName = '" + this.personFamili + "' " +
                                @" select Договор.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    SUM(УслугиПоДоговору.Сумма) as Сумма,Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура
			                    ,АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,НомерАкта,Договор.flag2019AddWrite, 2018 as 'Год',flagАнулирован from dbo.Договор
                              inner join Льготник
                                ON Договор.id_льготник = Льготник.id_льготник
                                inner join ЛьготнаяКатегория
                                ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                                inner join УслугиПоДоговору
                                ON УслугиПоДоговору.id_договор = Договор.id_договор
                                left outer join АктВыполненныхРабот
                                ON АктВыполненныхРабот.id_договор = Договор.id_договор
                                 where[Фамилия] = @FirstName
                                and Договор.ФлагПроверки = 'False'
                                and YEAR(Договор.ДатаЗаписиДоговора) = 2019
                                --and(ДатаАктаВыполненныхРабот is not null) and(YEAR(ДатаАктаВыполненныхРабот) <> 1900)
                                group by Договор.id_договор,НомерДоговора,Льготник.Фамилия,Льготник.Имя,Льготник.Отчество,ЛьготнаяКатегория.ЛьготнаяКатегория,
			                    Договор.ДатаЗаписиДоговора,Договор.НомерРеестра,Договор.НомерСчётФактрура ,АктВыполненныхРабот.НомерАкта
			                    ,АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,flagАнулирован";

            return query;
        }
    }
}
