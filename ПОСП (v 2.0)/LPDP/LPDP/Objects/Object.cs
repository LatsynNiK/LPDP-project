using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public enum ObjectType { Scalar, Link, Vector, Macro}
    abstract class Object
    {
        int ID;
        string Name;
        string Unit;
        public ObjectType Type;

        public Object(int id, string name, string unit)
        {
            this.ID = id;
            this.Name = name;
            this.Unit = unit;
            //this.Type = type;
        }
    }
}
