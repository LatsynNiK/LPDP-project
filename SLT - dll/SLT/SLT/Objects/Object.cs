using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    enum ObjectType { Scalar, Link, Vector, Macro}
    abstract class Object
    {
        public int ID;
        
        public string Name;
        public string Unit;
        public ObjectType Type;

        public Object(int id, string name, string unit)
        {
            this.ID = id;
            this.Name = name;
            this.Unit = unit;            
        }

        public Object(int id)
        {
            this.ID = id;
        }

        public Object(string name, string unit)
        {
            this.Name = name;
            this.Unit = unit;
        }

        public abstract void SetValue(object value);
        public abstract object GetValue();        
    }
}
