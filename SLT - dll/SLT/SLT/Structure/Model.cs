using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Model
    {
        public string Name;
        public MacrosTable MT;
        public StructureController ST_Cont;
        public ObjectController O_Cont;
        public Executor Executor;
        public Analyzer Analyzer;
        public string StatusInfo;
        public bool Built;

        public Model ()
        {
            this.MT = new MacrosTable();
            
            this.ST_Cont = new StructureController(this);
            
            this.O_Cont = new ObjectController(this);
            this.Executor = new Executor(this);
            this.Built = false;

            this.Analyzer = new Analyzer(this);

            this.StatusInfo = "";
        }
    }
}
