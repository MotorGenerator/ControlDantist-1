using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    /// <summary>
    /// Поиск номера договора до 2019 года.
    /// </summary>
    public class FindContractTo2019 : IFindPerson
    {
        string numCOntract = string.Empty;

        public FindContractTo2019(string numCOntract)
        {
            this.numCOntract = numCOntract ?? throw new ArgumentNullException(nameof(numCOntract));
        }

        /// <summary>
        /// Возвращает строку запроса.
        /// </summary>
        /// <returns></returns>
        public string Query()
        {
            // Помечаем 2018 год и более ранний как 2017 для того чтобы отфильтровать потом в поиске.
            string query = @"declare @numContract nchar(100)
                            set @numContract = '" + this.numCOntract + "' " +
                            @"SELECT Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, 
                            Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',
                            Договор.ДатаЗаписиДоговора, АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания, Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован,'2017' As Год FROM Договор
                            INNER JOIN Льготник
                            ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник
                            INNER JOIN dbo.ЛьготнаяКатегория
                            ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории
                            INNER JOIN dbo.УслугиПоДоговору
                            ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор
                            left outer join dbo.АктВыполненныхРабот
                            on dbo.АктВыполненныхРабот.id_договор = Договор.id_договор
                            where Договор.ФлагПроверки = 'True'
                            and Договор.НомерДоговора = @numContract
                            and YEAR(Договор.ДатаЗаписиДоговора) < 2019
                            Group by Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество,
                            ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, 
                            АктВыполненныхРабот.ДатаПодписания,Договор.logWrite,Договор.flag2019AddWrite,Договор.flagАнулирован";

            return query;
        }
    }
}
