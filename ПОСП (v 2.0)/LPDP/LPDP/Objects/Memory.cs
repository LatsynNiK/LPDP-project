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
            this.ID_Object_Counter = 1;
        }

        //public void CreateObject(ObjectType type)
        //{
        //    this.Cells.Add()
        //}

        public Object AddSingletonObject(Object o)
        {
            Objects.Object finded = this.Cells.Find(c => ((c.Name == o.Name)&&(c.Unit == o.Unit)));
            if (finded != null)
            {
                finded.SetValue(o.GetValue());
                return finded;
            }
            o.ID = this.ID_Object_Counter;
            this.Cells.Add(o);
            this.ID_Object_Counter++;
            return o;
        }
        public Object AddNewObject(Object o)
        {
            o.ID = this.ID_Object_Counter;
            this.Cells.Add(o);
            this.ID_Object_Counter++;
            return o;
        }
        public Object GetObjectByID(int id)
        {
            Object result = this.Cells.Find(o => o.ID == id);
            return result;
        }

        public void DeleteObject(int id)
        {
            Object deleted = this.Cells.Find(o => o.ID == id);
            this.Cells.Remove(deleted);
        }


    }
}
