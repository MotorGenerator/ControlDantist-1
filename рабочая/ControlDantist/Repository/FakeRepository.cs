using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;

namespace ControlDantist.Repository
{
    public class FakeRepository : IRepository<ViewDisplayLimit>
    {
        ViewDisplayLimit[] viewDisplayLimits =
        {
            new ViewDisplayLimit { id_льготнойКатегории = 40, idLimitMoney = 1, Limit = 229700000, ЛьготнаяКатегория = "Ветеран  военной службы", Year = 2019 },
            new ViewDisplayLimit { id_льготнойКатегории = 37, idLimitMoney = 1, Limit = 229700000, ЛьготнаяКатегория = "Ветеран труда", Year = 2019 }
        };

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(ViewDisplayLimit item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewDisplayLimit> Select(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewDisplayLimit> SelectFull()
        {
            return viewDisplayLimits;
        }


        public void Update(ViewDisplayLimit item)
        {
            throw new NotImplementedException();
        }
    }
}
