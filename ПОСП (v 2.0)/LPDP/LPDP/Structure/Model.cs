using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.Dynamics;
using LPDP.Objects;

namespace LPDP.Structure
{

    public class Model
    {
        public string Name;
        public FutureTimesTable FTT;
        public ConditionsTable CT;
        public TimeAndConditionController TC_Cont;

        public List<Subprogram> Tracks;
        public LabelsTable LT;
        public MacrosTable MT;
        public List<Unit> Units;
        public StructureController ST_Cont;

        public Memory Memory;
        public GlobalVarsTable GVT;
        public InitiatorsTable IT;
        public ObjectController O_Cont;
        
        double TIME;
        int INITIATOR;
        int SUBPROGRAM;
        //int OPERATION;
        Random RAND;

        public Model (string name)
        {
            this.Name = name;
            //this.ObjectCounter = 0;
            this.FTT = new FutureTimesTable();
            this.CT = new ConditionsTable();
            this.TC_Cont = new TimeAndConditionController(this);

            this.Tracks = new List<Subprogram>();
            this.LT = new LabelsTable();
            this.MT = new MacrosTable();
            this.Units = new List<Unit>();
            this.ST_Cont = new StructureController(this);

            this.Memory = new Memory();
            this.GVT = new GlobalVarsTable();
            this.IT = new InitiatorsTable();
            this.O_Cont = new ObjectController(this);

            this.TIME = 0;
            this.RAND = new Random();
        }
    }
}
