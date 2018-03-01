using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using SLT.Dynamics;
using SLT.Objects;
using SLT.TextAnalysis;

namespace SLT.Structure
{

    public class Model
    {
        public string Name;
        //public FutureTimesTable FTT;
        //public ConditionsTable CT;
        //public TimeAndConditionController TC_Cont;

        //public List<Subprogram> Tracks;
        //public LabelsTable LT;
        public MacrosTable MT;
        public List<Unit> Units;
        public StructureController ST_Cont;

        public Memory Memory;
        //public GlobalVarsTable GVT;
        //public InitiatorsTable IT;
        public ObjectController O_Cont;

        public Executor Executor;

        public Analysis Analysis;

        public string StatusInfo;

        //public SLT.DataSets.OutputData Out;
        
        //double TIME;
        //Initiator INITIATOR;
        //Subprogram SUBPROGRAM;
        //int OPERATION;
        //Random RAND;

        public bool Built;

        public Model ()//string name)
        {
            //this.Name = name;
            
            //this.Tracks = new List<Subprogram>();
            this.MT = new MacrosTable();
            this.Units = new List<Unit>();
            this.ST_Cont = new StructureController(this);

            this.Memory = new Memory();
            this.O_Cont = new ObjectController(this);
            this.Executor = new Executor(this);
            this.Built = false;

            this.Analysis = new Analysis(this);

            this.StatusInfo = "";

            //this.Out = new DataSets.OutputData();

        }

        public void Clear()
        {

        }
    }
}
