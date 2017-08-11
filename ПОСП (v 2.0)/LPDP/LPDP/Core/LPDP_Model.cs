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
            ObjectCounter = 0;
            TIME = 0;
            RAND = new Random();

            TC_Controller = new TimeAndConditionController();

        }
    }
}
