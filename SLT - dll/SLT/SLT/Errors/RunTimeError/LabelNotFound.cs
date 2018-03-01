using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class LabelNotFound:RunTimeError
    {
        string Name;
        public LabelNotFound(string name, string unit)
        {
            this.Name = name;
            this.Text = "Не найдена метка \"" + name + "\" в блоке \""+ unit+"\"";
        }
        public string GetName()
        {
            return Name;
        }
    }
}
