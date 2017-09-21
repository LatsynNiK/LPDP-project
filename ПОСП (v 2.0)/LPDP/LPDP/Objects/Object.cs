using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public enum ObjectType { Scalar, Link, Vector, Macro}
    public abstract class Object
    {
        public int ID;
        
        string Name;
        string Unit;
        public ObjectType Type;

        int ParentID;

        public Object(int id, string name, string unit)
        {
            this.ID = id;
            this.Name = name;
            this.Unit = unit;
            //this.Type = type;
        }

        public Object(int id, string name, string unit, int parent)
        {
            this.ID = id;
            this.Name = name;
            this.Unit = unit;
            this.ParentID = parent;
            //this.Type = type;
        }

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
    }
}
