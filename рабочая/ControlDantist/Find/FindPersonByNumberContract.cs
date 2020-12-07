using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonByNumberContract : IFindPerson
    {
        private string numberContract;

        public FindPersonByNumberContract(string numberContract)
        {
            this.numberContract = numberContract;
        }

        public string Query()
        {

            string query = @"declare @numContract nchar(100)
                            set @numContract = '" + this.numberContract + "' " +
                            @"SELECT Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, 
                            Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',
                            Договор.ДатаЗаписиДоговора, АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания, Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован FROM Договор
                            INNER JOIN Льготник
                            ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговору
                            ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор
                            inner join dbo.АктВыполненныхРабот
                            on dbo.АктВыполненныхРабот.id_договор = Договор.id_договор
                            where Договор.ФлагПроверки = 'True' and idFileRegistProgect is not null
                            and flagОжиданиеПроверки = 1 and ФлагВозвратНаДоработку = 0 and Договор.НомерДоговора = @numContract and YEAR(Договор.ДатаЗаписиДоговора) < 2019
                            Group by Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, 
                            АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован
                            union
                            SELECT ДоговорAdd.id_договор,ДоговорAdd.НомерДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, 
                            ЛьготникAdd.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговоруAdd.Сумма) as 'Сумма',
                            ДоговорAdd.ДатаЗаписиДоговора, АктВыполненныхРаботAdd.НомерАкта, АктВыполненныхРаботAdd.ДатаПодписания, ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,ДоговорAdd.flagАнулирован FROM ДоговорAdd
                            INNER JOIN ЛьготникAdd
                            ON dbo.ДоговорAdd.id_льготник = dbo.ЛьготникAdd.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.ЛьготникAdd.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговоруAdd
                            ON dbo.ДоговорAdd.id_договор = dbo.УслугиПоДоговоруAdd.id_договор
                            inner join dbo.АктВыполненныхРаботAdd
                            on dbo.АктВыполненныхРаботAdd.id_договор = ДоговорAdd.id_ТабДоговор
                            where ДоговорAdd.ФлагПроверки = 'True' and idFileRegistProgect is not null
                            and flagОжиданиеПроверки = 1 and ФлагВозвратНаДоработку = 0 and ДоговорAdd.НомерДоговора = @numContract
                            Group by ДоговорAdd.id_договор,ДоговорAdd.НомерДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, ЛьготникAdd.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,ДоговорAdd.ДатаЗаписиДоговора,АктВыполненныхРаботAdd.НомерАкта, 
                            АктВыполненныхРаботAdd.ДатаПодписания,ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,ДоговорAdd.flagАнулирован
                            union
                            SELECT Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, 
                            Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',
                            Договор.ДатаЗаписиДоговора, АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания, Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован FROM Договор
                            INNER JOIN Льготник
                            ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговору
                            ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор
                            left join dbo.АктВыполненныхРабот
                            on dbo.АктВыполненныхРабот.id_договор = Договор.id_договор
                            left outer join ProjectRegistrFiles
                            ON Договор.idFileRegistProgect = ProjectRegistrFiles.IdProjectRegistr
                            where Договор.ФлагПроверки = 'True' and idFileRegistProgect is not null
                            and flagОжиданиеПроверки = 1 and ФлагВозвратНаДоработку = 0 and Договор.НомерДоговора = @numContract and YEAR(Договор.ДатаЗаписиДоговора) > 2019
                            Group by Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, 
                            АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован ";

            return query;
        }
    }
}
