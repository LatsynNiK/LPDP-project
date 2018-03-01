using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT.Objects
{
    public enum ObjectType { Scalar, Link, Vector, Macro}
    public abstract class Object
    {
        public int ID;
        
        public string Name;
        public string Unit;
        public ObjectType Type;

        //int ParentID;

        public Object(int id, string name, string unit)
        {
            this.ID = id;
            this.Name = name;
            this.Unit = unit;
            //this.Type = type;
        }

        //public Object(int id, string name, string unit, int parent)
        //{
        //    this.ID = id;
        //    this.Name = name;
        //    this.Unit = unit;
        //    //this.ParentID = parent;
        //    //this.Type = type;
        //}

        public Object(int id)
        {
            this.ID = id;
            //this.Name = name;
            //this.Unit = unit;
            //this.Type = type;
        }

        public Object(string name, string unit)
        {
            this.Name = name;
            this.Unit = unit;
        }

        public abstract void SetValue(object value);
        public abstract object GetValue();

        //public abstract object GetValue();
        
    }
}
