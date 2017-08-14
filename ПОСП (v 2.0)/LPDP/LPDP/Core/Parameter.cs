﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Core
{
    class Parameter
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
