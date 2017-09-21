using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public enum UnitType
    {
        Processor,
        Controller,
        Aggregate
    }

    public class Unit
    {
        public string Name; 
        public UnitType Type;

        public Unit(string name, UnitType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public Unit() {}

        public void SetHeader(string name, UnitType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
