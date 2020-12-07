using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;

namespace ControlDantist.ValidateRegistrProject
{
    public class RegistrProjectContract : IVisableRegistrs
    {
        /// <summary>
        /// Номер письма.
        /// </summary>
        private string NumberLetter = string.Empty;

        // Дата письма.
        private DateTime DateLetter;

                // Id поликлинники.
        private int IdHospital;

        private bool FlagConnectServer;

        public RegistrProjectContract(string numberLetter, DateTime dateLetter, int idHospital, bool FlagConnectServer)
        {
            if (numberLetter != null && dateLetter != null && idHospital > 0)
            {
                NumberLetter = numberLetter;
                DateLetter = dateLetter;
                IdHospital = idHospital;
                this.FlagConnectServer = FlagConnectServer;
            }
            else
            {
                throw new Exception("Ошибка входных параметров реестров проектов договоров");
            }
        }

        public void Create()
        {
            UnitDate unitDate = new UnitDate();
            IQueryable<RegistrProject> registrProject = unitDate.ProjectRegistrFilesRepository.GetRegistr(this.NumberLetter, this.DateLetter, this.IdHospital);

            foreach(var itm in registrProject)
            {
                var test = itm;
            }

            if(registrProject != null)
            {
                FormRegistrProjectList formRegistrProjectList = new FormRegistrProjectList();
                formRegistrProjectList.RegistrItems = registrProject;
                formRegistrProjectList.FlagConnectServer = this.FlagConnectServer;
                formRegistrProjectList.Show();
            }


        }
    }
}
