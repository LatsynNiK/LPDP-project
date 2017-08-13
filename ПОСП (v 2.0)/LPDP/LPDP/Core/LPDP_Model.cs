using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{

    class LPDP_Model
    {
        FutureTimesTable FTT;
        ConditionsTable CT;
        TimeAndConditionController TC_Controller;

        public List<Subprogram> Tracks;
        StructureController ST_Controller;

        LabelsTable LT;
        GlobalVarsTable GVT;

        



        Tracks;

        InitiatorsTable IT;
        

        int ObjectCounter;
        
        double TIME;
        int INITIATOR;
        int SUBPROGRAM;
        int OPERATION;
        Random RAND;

        public LPDP_Model ()
        {
            this.ObjectCounter = 0;
            this.TIME = 0;
            this.RAND = new Random();

            this.TC_Controller = new TimeAndConditionController();
            this.ST_Controller = new StructureController();
            this.Tracks = new List<Subprogram>();
        }
    }
}
