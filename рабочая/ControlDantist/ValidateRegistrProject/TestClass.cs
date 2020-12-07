using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidateRegistrProject
{
    public class TestClass 
    {
        public IRegistrItem Test()
        {
            RegistrProject registrProject = new RegistrProject();

            registrProject.DataLetter = new DateTime();

            return registrProject;
        }
    }
}
