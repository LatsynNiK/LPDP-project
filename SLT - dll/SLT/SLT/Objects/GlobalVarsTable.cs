using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class GlobalVarsTable
    {
        public Dictionary<string, List<Object>> Vars;

        public GlobalVarsTable()
        {
            this.Vars = new Dictionary<string, List<Object>>();
        }

        public void AddUnit(string unit_name)
        {
            this.Vars.Add(unit_name, new List<Object>());
        }

        public void AddToUnit(Object obj, string unit_name)
        {
            SLT.Object finded = this.Vars[unit_name].Find(o => (o.Name == obj.Name));            
            if (finded != null)
            {
                finded.SetValue(obj.GetValue());
            }
            else
            {
                this.Vars[unit_name].Add(obj);
            }
        }
        public void AddNewToUnit(Object obj, string unit_name)
        {
            Object finded = this.Vars[unit_name].Find(o => (o.Name == obj.Name));
            if (finded != null)
            {
                this.Vars[unit_name].Remove(finded);
                //finded.SetValue(obj.GetValue());
            }
            this.Vars[unit_name].Add(obj);            
        }

        public void DeleteObjectByID(int id)
        {
            List<string> unit_list = this.Vars.Keys.ToList<string>();
            foreach (string u in unit_list)
            {
                Object obj = this.Vars[u].Find(o => o.ID == id);
                this.Vars[u].Remove(obj);
            }
        }
    }
}
