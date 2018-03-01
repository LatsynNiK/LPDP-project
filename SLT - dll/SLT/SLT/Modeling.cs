using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SLT
{
    public class Modeling
    {
        Model Model;
        public OutputData Output;
        public InputData Input;
        public int Precision;

        public Modeling(int precision)
        {
            this.Precision = precision;
            ModelTextRules.SetRules();
            this.Model = new Model();
            this.Input = new InputData();
            this.Output = new OutputData();
        }

        public void CreateModel()
        {
            this.Model = new Model();
        }

        public void Building()
        {
            this.Model.Analyzer.Building(this.Input.CodeTxt);
        }

        public void CheckText()
        {
            this.Model.Analyzer.CheckText(this.Input.CodeTxt);
        }

        public void StartUntil(double LaunchTime)
        {
            this.Model.Executor.StartUntil(LaunchTime);
        }

        public void StartStep()
        {
            this.Model.Executor.StartStep();
        }

        public void StartSEC()
        {
            this.Model.Executor.StartSEC();
        }

        public void Stop()
        {
            this.Model.Executor.Stop();
        }

        //public void ClearOutput()
        //{
        //    this.Output = new OutputData(this.Precision);
        //}


        #region Rewrite
        void Rewrite_Objects()
        {
            GlobalVarsTable GVT = this.Model.O_Cont.GVT;
            this.Output.Objects.Rows.Clear();
            foreach (string unit in GVT.Vars.Keys.ToArray())
            {
                foreach (SLT.Object obj in GVT.Vars[unit])
                {
                    if (unit == obj.Unit)
                    {
                        string name = obj.Name;
                        object value_obj = obj.GetValue();
                        try
                        {
                            value_obj = Convert.ToDouble(value_obj);
                            value_obj = Math.Round((double)value_obj, this.Precision);
                        }
                        catch { }
                        string value = Convert.ToString(value_obj);
                        string type = "";
                        switch (obj.Type)
                        {
                            case SLT.ObjectType.Scalar:
                                type = "Скаляр";
                                break;
                            case SLT.ObjectType.Link:
                                type = "Ссылка";
                                break;
                            case SLT.ObjectType.Vector:
                                type = "Вектор";
                                break;
                            case SLT.ObjectType.Macro:
                                type = "Макрос";
                                break;
                        }

                        if (obj.Type == SLT.ObjectType.Vector)
                        {
                            value = ((SLT.Vector)obj).GetTree();
                        }
                        this.Output.Objects.Rows.Add(obj.Unit, obj.Name, value, type);
                        if (obj.Type == SLT.ObjectType.Vector)
                        {
                            SLT.Vector Vec = (SLT.Vector)obj;
                            SLT.Vector SubVector = (SLT.Vector)Vec;
                            List<SLT.Object> vector_value = (List<SLT.Object>)(SubVector.GetValue());
                            this.Write_Vector_to_DataTable(this.Output.Objects, vector_value);
                        }
                    }
                }
            }
        }
        void Rewrite_Initiators()
        {
            InitiatorsTable IT = this.Model.O_Cont.IT;
            this.Output.Initiators.Rows.Clear();
            foreach (Initiator init in IT.Initiators)
            {
                int number = init.Number;

                string name;
                string value;
                string type = "";

                SLT.Object obj = this.Model.O_Cont.GetObjectFromInitiator(init);
                name = obj.Name;
                object value_obj = obj.GetValue();
                try
                {
                    value_obj = Convert.ToDouble(value_obj);
                    value_obj = Math.Round((double)value_obj, this.Precision);
                }
                catch { }

                value = Convert.ToString(value_obj);
                switch (obj.Type)
                {
                    case SLT.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case SLT.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case SLT.ObjectType.Vector:
                        type = "Вектор";
                        break;
                    case SLT.ObjectType.Macro:
                        type = "Макрос";
                        break;
                }
                if (obj.Type == SLT.ObjectType.Vector)
                {
                    value = ((SLT.Vector)obj).GetTree();
                }
                this.Output.Initiators.Rows.Add(number, name, value, type);
                if (obj.Type == SLT.ObjectType.Vector)
                {
                    SLT.Vector Vec = (SLT.Vector)obj;
                    SLT.Vector SubVector = (SLT.Vector)Vec;
                    List<SLT.Object> vector_value = (List<SLT.Object>)(SubVector.GetValue());
                    Write_Vector_to_DataTable(this.Output.Initiators, vector_value);
                }
            }

        }
        void Rewrite_FTT()
        {
            List<RecordFTT> ftt = this.Model.Executor.TC_Cont.FutureTimesTable;
            this.Output.FTT.Rows.Clear();

            foreach (RecordFTT rec in ftt)
            {
                Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
                this.Output.FTT.Rows.Add(Math.Round((double)rec.ActiveTime, this.Precision), rec.Initiator.Number, label.Name, label.Unit);
            }
        }
        void Rewrite_CT()
        {
            List<RecordCT> ct = this.Model.Executor.TC_Cont.ConditionsTable;
            this.Output.CT.Rows.Clear();

            foreach (RecordCT rec in ct)
            {
                Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
                this.Output.CT.Rows.Add(rec.Condition, rec.Initiator.Number, label.Name, label.Unit);
            }
        }
        void Rewrite_Queues()
        {
            this.Output.Queues.Rows.Clear();
            List<Queue> qt = this.Model.Executor.QT.Queues;
            foreach (Queue q in qt)
            {
                Label l = this.Model.ST_Cont.LT.FindFirstBySubprogram(q.Place);
                Unit u = q.Place.Unit;
                string inits_str = "";
                foreach (Queue.QueueItem qi in q.Items)
                {
                    inits_str += qi.Initiator.Number + ", ";
                }
                inits_str = inits_str.TrimEnd(',', ' ');
                if (q.Items.Count > 0)
                {
                    this.Output.Queues.Rows.Add(u.Name, l.Name, inits_str);
                }
            }
        }

        void Rewrite_QueueArrows()
        {
            this.Output.QueueArrows.Rows.Clear();
            List<Queue> qt = this.Model.Executor.QT.Queues;
            foreach (Queue q in qt)
            {
                int first = (int)Queue.ArrowType.None;
                int second = (int)Queue.ArrowType.None;
                int third = (int)Queue.ArrowType.None;

                int position = q.Place.Operators[0].Start;
                first = GetArrowType(q, 0);
                second = GetArrowType(q, 1);
                third = GetArrowType(q, 2);
                if (q.Items.Count > 3)
                {
                    second = (int)Queue.ArrowType.Several;
                }
                this.Output.QueueArrows.Rows.Add(position, first, second, third);
            }
        }
        void Rewrite_TextSelections()
        {
            this.Output.TextSelections.Rows.Clear();
            List<TextSelection> ts_list = this.Model.Analyzer.Selections.SelectionList; 
            foreach (TextSelection ts in ts_list)
            {
                this.Output.TextSelections.Rows.Add(ts.Start, ts.Length, ts.Type.ToString());
            }

            if (this.Output.ModelIsBuilt)
            {
                int start_op = this.Model.Executor.GetInitiator().NextOperator.Start;// GetNextOperatorPosition().Start;
                int length_op = this.Model.Executor.GetInitiator().NextOperator.Length;// .GetNextOperatorPosition().Length;
                string type;
                switch (this.Model.Executor.GetInitiator().Type)
                {
                    case InitiatorType.Aggregate:
                        type = TextSelectionType.NextAggregateOperator.ToString();
                        break;
                    case InitiatorType.Flow:
                        type = TextSelectionType.NextOperator.ToString();
                        break;
                    default:
                        type = TextSelectionType.Error.ToString();
                        break;
                }
                this.Output.TextSelections.Rows.Add(start_op, length_op, type);
            }

        }
        void Rewrite_HiddenLabel()
        {
            this.Output.HiddenLabel.Clear();
            foreach (Subprogram subp in this.Model.ST_Cont.Tracks)
            {
                Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(subp);
                if (label.Visible == false)
                {
                    int start = subp.Operators[0].Start;
                    this.Output.HiddenLabel.Rows.Add(label.Name, start);
                    //this.Model.Analysis.Selections.Add(start, label.Name.Length, TextSelectionType.SystemLabel);
                }
            }            
        }

        int GetArrowType(Queue queue, int index)
        {
            int arrow = (int)Queue.ArrowType.None;
            int position = queue.Place.Operators[0].Start;
            if (queue.Items.Count < index + 1)
            {
                return (int)Queue.ArrowType.None;
            }
            Initiator init = queue.Items[index].Initiator;

            bool finded = false;
            List<Queue> qt = this.Model.Executor.QT.Queues;
            foreach (Queue inner_q in qt)
            {
                if (inner_q.Items.Exists(i => (i.Initiator == init) && (inner_q != queue)))
                {
                    finded = true;
                    break;
                }
            }
            arrow += (int)Queue.ArrowType.Full;
            if (finded)
            {
                arrow = (int)Queue.ArrowType.Half;
            }
            switch (queue.Items[index].Delay)
            {
                case Queue.DelayType.Ready:
                    arrow += (int)Queue.ArrowType.Ready;
                    break;
                case Queue.DelayType.WaitTime:
                    arrow += (int)Queue.ArrowType.WaitTime;
                    break;
                case Queue.DelayType.Stopped:
                    arrow += (int)Queue.ArrowType.Stopped;
                    break;
            }
            return arrow;
        }    
        void Write_Vector_to_DataTable(DataTable DT, List<SLT.Object> list)
        {
            foreach (SLT.Object obj in list)
            {
                string type = "";
                switch (obj.Type)
                {
                    case SLT.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case SLT.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case SLT.ObjectType.Vector:
                        type = "Вектор";
                        break;
                    case SLT.ObjectType.Macro:
                        type = "Макрос";
                        break;
                }
                if (obj.Type == SLT.ObjectType.Vector)
                {
                    SLT.Vector Subvec = (SLT.Vector)obj;
                    DT.Rows.Add("", Subvec.Name, Subvec.GetTree(), type);
                    this.Write_Vector_to_DataTable(DT, (List<SLT.Object>)Subvec.GetValue());
                }
                else
                {
                    object value_obj = obj.GetValue();
                    try
                    {
                        value_obj = Convert.ToDouble(value_obj);
                        value_obj = Math.Round((double)value_obj, this.Precision);
                    }
                    catch { }
                    DT.Rows.Add("", obj.Name, value_obj, type);
                }

            }            
        }
        #endregion
        public void Output_All_Data()
        {
            this.Output = new OutputData();
            this.Output.ModelIsBuilt = this.Model.Built;
            if (this.Output.ModelIsBuilt)
            {
                this.Rewrite_Objects();
                this.Rewrite_Initiators();
                this.Rewrite_FTT();
                this.Rewrite_CT();
                this.Rewrite_Queues();

                this.Rewrite_QueueArrows();
                this.Rewrite_HiddenLabel();


                this.Output.TIME = this.Model.Executor.GetTIME();
                this.Output.UnitPosition = this.Model.Executor.GetInitiator().NextOperator.ParentSubprogram.Unit.StartPosition;
                this.Output.InitiatorNumber = this.Model.Executor.GetInitiator().Number;

            }
            this.Rewrite_TextSelections();

            this.Output.CodeTxt = this.Model.Analyzer.SourceText;//ResultTxtCode;

            this.Output.InfoTxt = this.Model.StatusInfo;// Out.InfoTxt;

        }
    }
}
