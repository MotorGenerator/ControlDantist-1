using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonNumContract : IFindPerson
    {

        private string numContract;

        public FindPersonNumContract(string numContract)
        {
            this.numContract = numContract;
        }

        string IFindPerson.Query()
        {
            string query = "SELECT Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания, Договор.logWrite,Договор.flagАнулирован " +
                         "FROM Договор INNER JOIN Льготник " +
                         "ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник INNER JOIN " +
                         "dbo.ЛьготнаяКатегория ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории INNER JOIN " +
                         "dbo.УслугиПоДоговору ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор " +
                         "inner join dbo.АктВыполненныхРабот " +
                         "on dbo.АктВыполненныхРабот.id_договор = Договор.id_договор " +
                         //"where Договор.ФлагПроверки = 'False' and Договор.НомерДоговора like '%" + this.textBox1.Text + "%' " +
                         "where Договор.ФлагПроверки = 'False' and Договор.НомерДоговора = '" + this.numContract.Trim() + "' " +
                         "Group by Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта, АктВыполненныхРабот.ДатаПодписания,Договор.logWrite ";

            return query;
        }
    }
}
