using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    public class InputData
    {
        public string CodeTxt;
        public bool ShowSysLabel;
        public bool ShowNextOperator;
        public bool ShowQueues;

        public InputData()
        {
            this.CodeTxt = "";
            this.ShowNextOperator = false;
            this.ShowQueues = false;
            this.ShowSysLabel = false;
        }

    }
}
