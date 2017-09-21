using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public class Memory
    {
        int ID_Object_Counter;
        List<Object> Cells;

        public Memory()
        {
            this.Cells = new List<Object>();
        }

        //public void CreateObject(ObjectType type)
        //{
        //    this.Cells.Add()
        //}

        public void Add(Object o) 
        {
            o.ID = this.ID_Object_Counter;
            this.Cells.Add(o);
            this.ID_Object_Counter++;
        }


    }
}
