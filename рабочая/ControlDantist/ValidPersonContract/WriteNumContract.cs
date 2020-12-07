using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidPersonContract
{
    public class WriteNumContract : IWriteNumContract
    {
        private List<ValidItemsContract> listContract;

        public WriteNumContract(List<ValidItemsContract> listContract)
        {
            this.listContract = listContract ?? throw new ArgumentNullException(nameof(listContract));
        }

        public string Write()
        {
            // Строка для хранения текстового сообщения.
            StringBuilder builder = new StringBuilder();

            CompareContract.Compare(this.listContract);

            foreach(var itm in CompareContract.Compare(this.listContract))
            {
                if (itm.DateContract != null)
                {
                    if (itm.DateContract.Trim() != "01.01.1900".Trim())
                    {
                        builder.Append(itm.NumContract +  " от - " + itm.DateContract.Trim() + ";  \n ");
                    }
                    else
                    {
                        builder.Append(itm.NumContract + " - проект договора ; \n ");
                    }
                }
            }

            return builder.ToString();
        }
    }
}
