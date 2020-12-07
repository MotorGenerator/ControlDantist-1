using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidPersonContract
{
    public class DocPrint
    {
        private IWordPrint wordPrint;

        public DocPrint(IWordPrint wordPrint)
        {
            this.wordPrint = wordPrint;
        }

        public void Execute()
        {
            wordPrint.Print();
        }
    }
}
