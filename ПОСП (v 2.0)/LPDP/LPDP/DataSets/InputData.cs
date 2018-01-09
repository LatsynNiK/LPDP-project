using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.DataSets
{
    public class InputData : InputOutputData
    {
        public string CodeTxt;
        public bool ShowSysMark;
        public bool ShowNextOperator;
        public bool ShowQueues;

        public InputData(int precision):base(precision)
        {
            this.CodeTxt = "";
            this.ShowNextOperator = false;
            this.ShowQueues = false;
            this.ShowSysMark = false;
            base.Precision = precision;
        }

    }
}
