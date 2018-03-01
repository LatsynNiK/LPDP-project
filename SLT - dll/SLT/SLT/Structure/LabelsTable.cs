using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT.Structure
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
            //if (this.Table.Exists(l => l.Subprogram == label.Subprogram))
            //{
            //    Label old_label = this.Table.Find(l => l.Subprogram == label.Subprogram);
            //    if (old_label.Visible == false)
            //    {
            //    }
            //    else
            //    {
            //        //error
            //    }
            //}
            this.Table.Add(label);
        }

        public Label FindFirstBySubprogram(Subprogram subp)
        {
            return this.Table.Find(l => l.Subprogram == subp);
        }

        public Subprogram GetSubprogram(string name, string unit)
        {
            Label label = this.Table.Find(l => ((l.Name == name) && (l.Unit == unit)));
            if (label == null)
            {
                throw new LabelNotFound(name, unit);
                //if (this.Table.Exists(l => (l.Unit == unit)))
                //{
                    
                //}
                //else
                //{
                //    throw new UnitNotFound(unit);
                //}
            }
            return label.Subprogram;
        }

        //public void Delete(int id_rec)
        //{
        //    Label rec = this.Table.Find(r => r.SubprogramID == id_rec);
        //    this.Table.Remove(rec);
        //}
    }
}
