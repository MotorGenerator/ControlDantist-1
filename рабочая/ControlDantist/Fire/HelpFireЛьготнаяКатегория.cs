using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Fire
{
    public class HelpFireЛьготнаяКатегория : IFireExecute
    {
        public string Query(List<FireHelp> list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (FireHelp itm in list)
            {
                //string query = "update Договор " +
                //               "set id_льготнаяКатегория = (select id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория = '" + itm.ЛьготнаяКатегория + "') " +
                //               "where НомерДоговора = '" + itm.NumberContract + "' "; //and ФлагПроверки = 'True' ";

                string query = " update Договор " +
                               "set id_льготнаяКатегория = (select id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория = '" + itm.ЛьготнаяКатегория + "') " +
                               "where НомерДоговора = '" + itm.NumberContract + "' " +
                               "update Льготник " +
                               "set id_льготнойКатегории = (select id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория = '" + itm.ЛьготнаяКатегория + "') " +
                               "where id_льготник = (select MAX(id_льготник) from Договор " +
                               "Where НомерДоговора = '" + itm.NumberContract + "') ";

                builder.Append(query);
            }

            return builder.ToString();
        }
    }
}
