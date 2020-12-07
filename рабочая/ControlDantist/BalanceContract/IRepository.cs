using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.BalanceContract
{
    public interface IRepository
    {
        void Insert();

        void Delete(int id);

        void Update(int id);
    }
}
