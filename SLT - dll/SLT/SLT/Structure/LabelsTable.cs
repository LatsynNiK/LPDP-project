using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class LabelsTable
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
            }
            return label.Subprogram;
        }
    }
}
