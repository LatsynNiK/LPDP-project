using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.TextAnalysis;

namespace SLT.Structure
{
    public class Parameter
    {
        public string Name;
        public Phrase Value;

        public Parameter(string name, Phrase ph)
        {
            this.Name = name;
            this.Value = ph;
        }
    }
}
