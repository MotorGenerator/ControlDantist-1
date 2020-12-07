using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DantistLibrary;
using ControlDantist.Repository;
using System.Data;

namespace ControlDantist.ReceptionDocuments
{
    public class ValidateЛьготник : IValideCore<Льготник>
    {
        private Dictionary<string, Unload> unloads;

        public ValidateЛьготник(Dictionary<string, Unload> unloadContracts)
        {
            if (unloadContracts != null)
                unloads = unloadContracts;
        }

        public List<Льготник> Execute()
        {
            foreach(Unload unload in unloads.Values)
            {
                foreach(DataRow row in unload.Льготник.Rows)
                {
                    Льготник person = new Льготник();
                    person.id_льготнойКатегории = Preveliget.Get(unload.ЛьготнаяКатегория);
                    //person.

                }
            }

            return null;
        }

        
    }
}
