using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class LabelNotFound:RunTimeError
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
