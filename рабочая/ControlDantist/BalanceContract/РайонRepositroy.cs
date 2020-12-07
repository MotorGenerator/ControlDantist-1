using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ControlDantist.BalanceContract
{
    public class РайонRepositroy : IRepository
    {

        //private int IdRegion = 0;

        private string NameRegion = string.Empty;

        public РайонRepositroy(string nameRegion)
        {
            if (nameRegion != null && nameRegion != "")
            {
                this.NameRegion = nameRegion;
            }
            else
            {
                throw new Exception("Отсуьтсвует название района области");
            }

        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            string queryInsert = "insert РайонОбласти(NameRegion) values('" + this.NameRegion + "')";



            //throw new NotImplementedException();
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }
    }
}
