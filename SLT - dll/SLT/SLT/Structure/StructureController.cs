﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class StructureController
    {
        int SubprogramID_Counter;
        //*************
        Model ParentModel;
        public Unit CurrentUnit;
        public Subprogram CurrentSubprogram;
        public Operator LastOperator;
        public int NextWaitLabelNumber;
        public LabelsTable LT;
        public List<Unit> Units;

        public List<Subprogram> Tracks;

        public StructureController(Model model)
        {
            this.LT = new LabelsTable();
            this.Tracks = new List<Subprogram>();
            this.ParentModel = model;
            this.NextWaitLabelNumber = 1;
            this.Units = new List<Unit>();
            SubprogramID_Counter = 0;
        }

        public void AddSubprogram(Subprogram subp)
        {
            subp.ID = this.SubprogramID_Counter;
            this.SubprogramID_Counter++;
            subp.Unit = this.CurrentUnit;
            this.Tracks.Add(subp);
            this.CurrentSubprogram = subp;
        }
        public void RevertSubprogram()
        {
            this.Tracks.Remove(this.CurrentSubprogram);
            this.CurrentSubprogram = this.Tracks.Last();
        }

        public void AddOperator(Operator oper)
        {
            this.CurrentSubprogram.AddOperator(oper);
            this.LastOperator = oper;
        }

        public Subprogram FindSubprogramByLabelAndUnit(string label, string unit)
        {
            return this.LT.GetSubprogram(label, unit);

        }

        //UNIT
        public void CreateUnit(int position)
        {
            Unit NewUnit = new Unit(position);
            this.Units.Add(NewUnit);
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
            this.LT.Add(label);
        }

        public void CheckLabels()
        {
        }

    }
}
