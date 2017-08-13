using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Core
{
    class StructureController
    {
        int SubprogramID_Counter;
        int OperatorID_Counter;

        public StructureController()
        {
            SubprogramID_Counter = 0;
        }

        public void CreateSubprogram(LPDP_Model model)
        {
            Subprogram NewSubp = new Subprogram(SubprogramID_Counter);
            model.Tracks.Add(NewSubp);
            this.SubprogramID_Counter++;
        }

        public int GetOperatorID()
        {
            return this.OperatorID_Counter;
        }
    }
}
