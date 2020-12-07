using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;
using System.Windows.Forms;

namespace ControlDantist.ReceptionDocuments
{
    public class PrintContract : IPrintContract
    {
        private Registr registr;

        public PrintContract(Registr registr)
        {
            if (registr != null)
            {
                this.registr = registr;
            }
               
        }
        public bool PrintContractDraft()
        {
            // Флаг разрешающий запись реестра в БД.
            bool flagWriteDB = false;

            FormPrintContract formPrintContract = new FormPrintContract();
            formPrintContract.Registr = this.registr;

            DialogResult dialogResultPC = formPrintContract.ShowDialog();

            if (dialogResultPC == DialogResult.OK)
            {
                //// Репозиторий для доступа к БД.
                //UnitDate unitDate = new UnitDate();

                //// Откроем для теста окно сохранения проектов договоров.
                //ReceptionContractsForm receptionContractsForm = new ReceptionContractsForm(unitDate);
                //DialogResult dialogResult = receptionContractsForm.ShowDialog();

                //if (dialogResult == DialogResult.OK)
                //{
                //    // Разрешим запись рееста в БД.
                //    FlagWriteDB = true;

                //    receptionContractsForm.
                //}

                flagWriteDB = true;
            }

            return flagWriteDB;

        }
    }
}
