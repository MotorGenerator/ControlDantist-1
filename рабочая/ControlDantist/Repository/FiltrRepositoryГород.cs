using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class FiltrRepositoryГород : IFiltrRepository<НаселённыйПункт>
    {
        private НаселенныйПунктRepository населенныйПунктRepository;

        public FiltrRepositoryГород(НаселенныйПунктRepository населенныйПунктRepository)
        {
            this.населенныйПунктRepository = населенныйПунктRepository;
        }


        public НаселённыйПункт Select(int id)
        {
            return this.населенныйПунктRepository.SelectAll().Where(w => w.id_насПункт == id).FirstOrDefault();
        }
    }
}
