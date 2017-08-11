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
        LabelsTable LT;
        GlobalVarsTable GVT;

        TimeAndConditionController TC_Controller;



        Tracks;

        InitiatorsTable IT;
        
        int RecordCounter;
        int ObjectCounter;
        
        double TIME;
        int INITIATOR;
        int SUBPROGRAM;
        int OPERATION;
        Random RAND;

        public LPDP_Model ()
        {
            RecordCounter = 0;
            ObjectCounter = 0;
            TIME = 0;
            RAND = new Random();

            TC_Controller = new TimeAndConditionController();

        }
    }
}
