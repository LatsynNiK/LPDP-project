using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class UnitNotFound:RunTimeError
    {
        string Name;
        public UnitNotFound(string name)
        {
            this.Name = name;
            this.Text = "Не найден блок: " + name;
        }
        public string GetName()
        {
            return Name;
        }
    }
}
