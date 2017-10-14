using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public class LabelsTable
    {
        List<Label> Table;

        public LabelsTable()
        {
            this.Table = new List<Label>();
        }

        //???
        public void Add(Label label)
        {
            this.Table.Add(label);
        }

        public void Delete(int id_rec)
        {
            Label rec = this.Table.Find(r => r.SubprogramID == id_rec);
            this.Table.Remove(rec);
        }
    }
}
