using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonAftar2019NoValid : IFindPerson
    {
        string numContract = string.Empty;

        public FindPersonAftar2019NoValid(string numContract)
        {
            this.numContract = numContract ?? throw new ArgumentNullException(nameof(numContract));
        }

        public string Query()
        {
            string query = "declare @numContract nchar(100) " +
                            " set @numContract = '" + this.numContract + "' " +
                            @"SELECT Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, 
                            Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',
                            Договор.ДатаЗаписиДоговора, АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания, Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован,'2020' As Год FROM Договор
                            INNER JOIN Льготник
                            ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговору
                            ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор
                            left outer join dbo.АктВыполненныхРабот
                            on dbo.АктВыполненныхРабот.id_договор = Договор.id_договор
                             inner join ProjectRegistrFiles
                            ON Договор.idFileRegistProgect = ProjectRegistrFiles.IdProjectRegistr
                            where Договор.ФлагПроверки = 'False' 
                            and Договор.НомерДоговора = @numContract and YEAR(Договор.ДатаЗаписиДоговора) > 2019
                            Group by Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, 
                            АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован ";

            return query;
        }
    }
}
