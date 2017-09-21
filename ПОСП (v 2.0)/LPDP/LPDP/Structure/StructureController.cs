using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public class StructureController
    {
        int SubprogramID_Counter;
        //int OperatorID_Counter;

        //*************
        Model ParentModel;
        public Unit CurrentUnit;
        public Subprogram CurrentSubprogram;

        public StructureController(Model model)
        {
            this.ParentModel = model;
            SubprogramID_Counter = 0;
        }

        public void CreateSubprogram()
        {
            Subprogram NewSubp = new Subprogram(SubprogramID_Counter);
            //model.Tracks.Add(NewSubp);
            this.SubprogramID_Counter++;
            this.CurrentSubprogram = NewSubp;
        }

        //UNIT
        public void CreateUnit()
        {
            Unit NewUnit = new Unit();
            this.ParentModel.Units.Add(NewUnit);
            this.CurrentUnit = NewUnit;
        }
        public void SetUnitHeader(string name, UnitType type)
        {
            this.CurrentUnit.SetHeader(name, type);
            this.ParentModel.GVT.AddUnit(name);
        }

        //public int GetOperatorID()
        //{
        //    return this.OperatorID_Counter;
        //}
    }
}
