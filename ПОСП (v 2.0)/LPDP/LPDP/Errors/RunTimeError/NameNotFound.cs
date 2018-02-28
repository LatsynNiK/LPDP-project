using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    class NameNotFound:RunTimeError
    {
        string Name;
        public NameNotFound(string name)
        {
            this.Name = name;
            this.Text = "Не найдено наименование: " + name;
        }
        public string GetName()
        {
            return Name;
        }
    }
}
