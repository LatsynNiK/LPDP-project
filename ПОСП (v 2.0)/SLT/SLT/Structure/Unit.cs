using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT.Structure
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
        public int StartPosition;

        public Unit(string name, UnitType type , int pos)
        {
            this.Name = name;
            this.Type = type;
            this.StartPosition = pos;
        }

        public Unit(int pos) 
        {
            this.StartPosition = pos;
        }

        public void SetHeader(string name, UnitType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
