using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindContract2019Add : IFindPerson
    {
        string numContract = string.Empty;

        public FindContract2019Add(string numContract)
        {
            this.numContract = numContract ?? throw new ArgumentNullException(nameof(numContract));
        }

        public string Query()
        {
            string query = @"declare @numContract nchar(100)
                            set @numContract = '" + this.numContract.Trim() + "' " +
                            @"  SELECT ДоговорAdd.id_договор,ДоговорAdd.НомерДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, 
                            ЛьготникAdd.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговоруAdd.Сумма) as 'Сумма',
                            ДоговорAdd.ДатаЗаписиДоговора, АктВыполненныхРаботAdd.НомерАкта, АктВыполненныхРаботAdd.ДатаПодписания, ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,ДоговорAdd.flagАнулирован,'2019' As Год FROM ДоговорAdd
                            INNER JOIN ЛьготникAdd
                            ON dbo.ДоговорAdd.id_льготник = dbo.ЛьготникAdd.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.ЛьготникAdd.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговоруAdd
                            ON dbo.ДоговорAdd.id_договор = dbo.УслугиПоДоговоруAdd.id_договор
                            left outer join dbo.АктВыполненныхРаботAdd
                            on dbo.АктВыполненныхРаботAdd.id_договор = ДоговорAdd.id_ТабДоговор
                            where ДоговорAdd.ФлагПроверки = 'True' --and idFileRegistProgect is not null
                            --and flagОжиданиеПроверки = 1 and ФлагВозвратНаДоработку = 0 
							and ДоговорAdd.НомерДоговора = @numContract
                            Group by ДоговорAdd.id_договор,ДоговорAdd.НомерДоговора, ЛьготникAdd.Фамилия, ЛьготникAdd.Имя, ЛьготникAdd.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,ДоговорAdd.ДатаЗаписиДоговора,АктВыполненныхРаботAdd.НомерАкта, 
                            АктВыполненныхРаботAdd.ДатаПодписания,ДоговорAdd.logWrite,ДоговорAdd.flag2019AddWrite,ДоговорAdd.flagАнулирован";

            return query;
        }
    }
}
