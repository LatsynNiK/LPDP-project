using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Structure
{
    public class StructureController
    {
        int SubprogramID_Counter;
        int OperatorID_Counter;

        //*************
        Model ParentModel;
        public Unit CurrentUnit;
        public Subprogram CurrentSubprogram;
        public Operator LastOperator;
        public int NextWaitLabelNumber;

        public StructureController(Model model)
        {
            this.ParentModel = model;
            this.NextWaitLabelNumber = 1;
            SubprogramID_Counter = 0;
        }

        //public void CreateSubprogram()
        //{
        //    Subprogram NewSubp = new Subprogram(SubprogramID_Counter);
        //    //model.Tracks.Add(NewSubp);
        //    this.SubprogramID_Counter++;
        //    this.CurrentSubprogram = NewSubp;
        //}


        public void AddSubprogram(Subprogram subp)
        {
            subp.ID = this.SubprogramID_Counter;
            this.SubprogramID_Counter++;
            subp.Unit = this.CurrentUnit;
            this.ParentModel.Tracks.Add(subp);
            this.CurrentSubprogram = subp;
        }
        public void RevertSubprogram()
        {
            this.ParentModel.Tracks.Remove(this.CurrentSubprogram);
            this.CurrentSubprogram = this.ParentModel.Tracks.Last();
        }

        public void AddOperator(Operator oper)
        {
            //oper.ID = this.OperatorID_Counter;
            //this.OperatorID_Counter++;
            this.CurrentSubprogram.AddOperator(oper);
            this.LastOperator = oper;
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
            this.ParentModel.O_Cont.GVT.AddUnit(name);
        }

        public void AddLabel(Label label)
        {
            label.Subprogram = this.CurrentSubprogram;
            label.Unit = this.CurrentUnit.Name;
            if (label.Visible == false)
            {
                label.Name = "$L_" + this.NextWaitLabelNumber;
                this.NextWaitLabelNumber++;
            }
            this.ParentModel.LT.Add(label);
        }

        //public int GetOperatorID()
        //{
        //    return this.OperatorID_Counter;
        //}
    }
}
