using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data;

using LPDP.Objects;
//using LPDP.Structure;
//using LPDP.Dynamics;

namespace LPDP.DataSets
{
    public struct Selection
    {
        public int Start;
        public int Length;
        public Selection(int start, int len)
        {
            this.Start = start;
            this.Length = len;
        }
    }
    public class InputOutputData
    {
        //public InputData Input;
        //public OutputData Output;

        //public string CodeTxt;
        //public string CodeRtf;
        //public string InfoTxt;

        //public bool ShowSysMark;
        //public bool ShowNextOperator;
        //public bool ShowQueues;

        //protected LPDP.Structure.Model Model;
        //public DataTable Objects;
        //public DataTable Initiators;
        //public DataTable Queues;
        //public DataTable FTT;
        //public DataTable CT;

        //public DataTable QueueArrows;

        //public double TIME;
        //public bool ModelIsBuilt;

        public int Precision;

        //public int NextOperatorPosition_Start;
        //public int NextOperatorPosition_Length;
        //public bool NextInitiatorIsFlow;
        //public int UnitPosition;

        //public int InitiatorNumber;

        //public void SetParentModel(LPDP.Structure.Model model)
        //{
        //    this.Model = model;
        //    //this.ClearTable(this.Objects);
        //    //this.ClearTable(this.Initiators);
        //    //this.ClearTable(this.Queues);
        //    //this.ClearTable(this.FTT);
        //    //this.ClearTable(this.CT);

        //    //this.ClearTable(this.QueueArrows);
        //}

        public InputOutputData(int precision)
        {
            //this.CodeTxt = this.Model.
            this.Precision = precision;
            //this.Input = new InputData(precision);
            //this.Output = new OutputData(precision);

            ////this.Model = model;
            //this.Objects = new DataTable("Objects");
            //this.Initiators = new DataTable("Initiators");
            //this.Queues = new DataTable("Queues");
            //this.FTT = new DataTable("FTT");
            //this.CT = new DataTable("CT");

            //this.QueueArrows = new DataTable("QueueArrows");


            ////this.ClearTable(this.Objects);
            ////this.ClearTable(this.Initiators);
            ////this.ClearTable(this.Queues);
            ////this.ClearTable(this.FTT);
            ////this.ClearTable(this.CT);
            ////this.ClearTable(this.QueueArrows);

            //this.CreateTable(this.Objects, "Unit", "Name", "Value", "Type");
            //this.CreateTable(this.Initiators, "Number", "Name", "Value", "Type");
            //this.CreateTable(this.Queues, "Unit", "Label", "Initiators");
            //this.CreateTable(this.FTT, "Time", "Initiator", "Label", "Unit");
            //this.CreateTable(this.CT, "Condition", "Initiator", "Label", "Unit");
            //this.CreateTable(this.QueueArrows, "Position", "First", "Second", "Third");

            //this.RenameTable(this.Objects, "Блок", "Объект", "Значение", "Тип");
            //this.RenameTable(this.Initiators, "Номер", "Инициатор", "Значение", "Тип");
            //this.RenameTable(this.Queues, "Блок", "Метка", "Инициаторы");
            //this.RenameTable(this.FTT, "Время", "Инициатор", "Метка", "Блок");
            //this.RenameTable(this.CT, "Условие", "Инициатор", "Метка", "Блок");
            ////this.CreateTable(this.QueueArrows, "Position", "First", "Second", "Third");
        }

    //    void CreateTable(DataTable table, params string[] ColumnNames)
    //    {
    //        foreach (string colname in ColumnNames)
    //        {
    //            table.Columns.Add(colname);
    //        }
    //    }
    //    //void ClearTable(DataTable table)
    //    //{
    //    //    foreach (DataColumn col in table.Columns)
    //    //        col.Dispose();
    //    //}
    //    void RenameTable(DataTable table, params string[] ColumnNames)
    //    {
    //        for (int i = 0; i< table.Columns.Count;i++)
    //        {
    //            table.Columns[i].Caption = ColumnNames[i];
    //        }
    //    }

    //    void Rewrite_Objects()
    //    {
    //        GlobalVarsTable GVT = this.Model.O_Cont.GVT;
    //        this.Objects.Rows.Clear();
    //        foreach (string unit in GVT.Vars.Keys.ToArray())
    //        {
    //            foreach (LPDP.Objects.Object obj in GVT.Vars[unit])
    //            {
    //                if (unit == obj.Unit)
    //                {
    //                    string name = obj.Name;
    //                    object value_obj = obj.GetValue();
    //                    try
    //                    {
    //                        value_obj = Convert.ToDouble(value_obj);
    //                        value_obj = Math.Round((double)value_obj, this.Precision);
    //                    }
    //                    catch { }
    //                    string value = Convert.ToString(value_obj);
    //                    string type = "";
    //                    switch (obj.Type)
    //                    {
    //                        case LPDP.Objects.ObjectType.Scalar:
    //                            type = "Скаляр";
    //                            break;
    //                        case LPDP.Objects.ObjectType.Link:
    //                            type = "Ссылка";
    //                            break;
    //                        case LPDP.Objects.ObjectType.Vector:
    //                            type = "Вектор";
    //                            break;
    //                        case LPDP.Objects.ObjectType.Macro:
    //                            type = "Макрос";
    //                            break;
    //                    }

    //                    if (obj.Type == LPDP.Objects.ObjectType.Vector)
    //                    {
    //                        value = ((LPDP.Objects.Vector)obj).GetTree();
    //                    }
    //                    Objects.Rows.Add(obj.Unit, obj.Name, value, type);
    //                    if (obj.Type == LPDP.Objects.ObjectType.Vector)
    //                    {
    //                        LPDP.Objects.Vector Vec = (LPDP.Objects.Vector)obj;
    //                        LPDP.Objects.Vector SubVector = (LPDP.Objects.Vector)Vec;
    //                        List<LPDP.Objects.Object> vector_value = (List<LPDP.Objects.Object>)(SubVector.GetValue());
    //                        this.Write_Vector_to_DataTable(this.Objects, vector_value);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    void Rewrite_Initiators()
    //    {
    //        InitiatorsTable IT = this.Model.O_Cont.IT;
    //        this.Initiators.Rows.Clear();
    //        foreach (Initiator init in IT.Initiators)
    //        {
    //            int number = init.Number;

    //            string name;
    //            string value;
    //            string type ="";

    //            LPDP.Objects.Object obj = this.Model.O_Cont.GetObjectFromInitiator(init);
    //            name = obj.Name;
    //            object value_obj = obj.GetValue();
    //            try
    //            {
    //                value_obj = Convert.ToDouble(value_obj);
    //                value_obj = Math.Round((double)value_obj, this.Precision);
    //            }
    //            catch { }

    //            value = Convert.ToString(value_obj);
    //            switch (obj.Type)
    //            {
    //                case LPDP.Objects.ObjectType.Scalar:
    //                    type = "Скаляр";
    //                    break;
    //                case LPDP.Objects.ObjectType.Link:
    //                    type = "Ссылка";
    //                    break;
    //                case LPDP.Objects.ObjectType.Vector:
    //                    type = "Вектор";
    //                    break;
    //                case LPDP.Objects.ObjectType.Macro:
    //                    type = "Макрос";
    //                    break;
    //            }
    //            if (obj.Type == LPDP.Objects.ObjectType.Vector)
    //            {
    //                value = ((LPDP.Objects.Vector)obj).GetTree();
    //            }
    //            this.Initiators.Rows.Add(number,name, value, type);
    //            if (obj.Type == LPDP.Objects.ObjectType.Vector)
    //            {
    //                LPDP.Objects.Vector Vec = (LPDP.Objects.Vector)obj;
    //                LPDP.Objects.Vector SubVector = (LPDP.Objects.Vector)Vec;
    //                List<LPDP.Objects.Object> vector_value = (List<LPDP.Objects.Object>)(SubVector.GetValue());
    //                Write_Vector_to_DataTable(this.Initiators, vector_value);
    //            }
    //        }
 
    //    }

    //    void Rewrite_FTT()
    //    {
    //        List<RecordFTT> ftt = this.Model.Executor.TC_Cont.FutureTimesTable;
    //        this.FTT.Rows.Clear();

    //        foreach (RecordFTT rec in ftt)
    //        {
    //            Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
    //            FTT.Rows.Add(Math.Round((double)rec.ActiveTime, this.Precision), rec.Initiator.Number, label.Name, label.Unit);
    //        }
    //    }

    //    void Rewrite_CT()
    //    {
    //        List<RecordCT> ct = this.Model.Executor.TC_Cont.ConditionsTable;
    //        this.CT.Rows.Clear();

    //        foreach (RecordCT rec in ct)
    //        {
    //            Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
    //            CT.Rows.Add(rec.Condition, rec.Initiator.Number, label.Name, label.Unit);
    //        }
    //    }

    //    void Rewrite_Queues()
    //    {
    //        this.Queues.Rows.Clear();
    //        List<Queue> qt = this.Model.Executor.QT.Queues;
    //        foreach(Queue q in qt)
    //        {
    //            Label l = this.Model.ST_Cont.LT.FindFirstBySubprogram(q.Place);
    //            Unit u = q.Place.Unit;
    //            string inits_str = "";
    //            foreach(Queue.QueueItem qi in q.Items)
    //            {
    //                inits_str += qi.Initiator.Number + ", ";
    //            }
    //            inits_str = inits_str.TrimEnd(',', ' ');
    //            if (q.Items.Count > 0)
    //            {
    //                this.Queues.Rows.Add(u.Name, l.Name, inits_str);
    //            }                
    //        }
    //    }

    //    void Rewrite_QueueArrows()
    //    {
    //        this.QueueArrows.Rows.Clear();
    //        List<Queue> qt = this.Model.Executor.QT.Queues;
    //        foreach (Queue q in qt)
    //        {
    //            int first = (int)Queue.ArrowType.None;
    //            int second = (int)Queue.ArrowType.None;
    //            int third = (int)Queue.ArrowType.None;

    //            int position = q.Place.Operators[0].Position.Start;
    //            first = GetArrowType(q, 0);
    //            second = GetArrowType(q, 1);
    //            third = GetArrowType(q, 2);
    //            if (q.Items.Count > 3)
    //            {
    //                second = (int)Queue.ArrowType.Several;
    //            }
    //            this.QueueArrows.Rows.Add(position, first, second, third);
    //        }
    //    }

    //    int GetArrowType(Queue queue ,int index)
    //    {
    //        int arrow = (int)Queue.ArrowType.None;
    //        int position = queue.Place.Operators[0].Position.Start;
    //        if (queue.Items.Count < index+1)
    //        {
    //            return (int)Queue.ArrowType.None;
    //        }
    //        Initiator init = queue.Items[index].Initiator;

    //        bool finded = false;
    //        List<Queue> qt = this.Model.Executor.QT.Queues;
    //        foreach (Queue inner_q in qt)
    //        {
    //            if (inner_q.Items.Exists(i => (i.Initiator == init)&&(inner_q != queue)))
    //            {
    //                finded = true;
    //                break;
    //            }
    //        }
    //        arrow += (int)Queue.ArrowType.Full;
    //        if (finded)
    //        {
    //            arrow = (int)Queue.ArrowType.Half;
    //        }
    //        switch (queue.Items[index].Delay)
    //        {
    //            case Queue.DelayType.Ready:
    //                arrow += (int)Queue.ArrowType.Ready;
    //                break;
    //            case Queue.DelayType.WaitTime:
    //                arrow += (int)Queue.ArrowType.WaitTime;
    //                break;
    //            case Queue.DelayType.Stopped:
    //                arrow += (int)Queue.ArrowType.Stopped;
    //                break;
    //        }
    //        return arrow;
    //    }

    //    public void Output_All_Data()
    //    {
    //        this.Rewrite_Objects();
    //        this.Rewrite_Initiators();
    //        this.Rewrite_FTT();
    //        this.Rewrite_CT();
    //        this.Rewrite_Queues();

    //        this.Rewrite_QueueArrows();

    //        //this.CodeRtf = 
    //        this.CodeTxt = this.Model.Analysis.SourceText;//ResultTxtCode;

            

    //        //this.InfoTxt = this.Model.Analysis.
    //        this.ModelIsBuilt = this.Model.Built;

    //        this.TIME = this.Model.Executor.GetTIME();

    //        this.NextOperatorPosition_Start = this.Model.Executor.GetNextOperatorPosition().Start;
    //        this.NextOperatorPosition_Length = this.Model.Executor.GetNextOperatorPosition().Length;


    //        if (this.Model.Executor.GetInitiatorType() == InitiatorType.Flow)
    //        {
    //            this.NextInitiatorIsFlow = true; 
    //        }
    //        else
    //        {
    //            this.NextInitiatorIsFlow = false;
    //        }
    //        this.UnitPosition = this.Model.Executor.GetInitiator().Position.Unit.StartPosition;

    //        this.InitiatorNumber = this.Model.Executor.GetInitiator().Number;
    //    }

    //    void Write_Vector_to_DataTable(DataTable DT, List<LPDP.Objects.Object> list)
    //    {
    //        foreach (LPDP.Objects.Object obj in list)
    //        {
    //            string type = "";
    //            switch (obj.Type)
    //            {
    //                case LPDP.Objects.ObjectType.Scalar:
    //                    type = "Скаляр";
    //                    break;
    //                case LPDP.Objects.ObjectType.Link:
    //                    type = "Ссылка";
    //                    break;
    //                case LPDP.Objects.ObjectType.Vector:
    //                    type = "Вектор";
    //                    break;
    //                case LPDP.Objects.ObjectType.Macro:
    //                    type = "Макрос";
    //                    break;
    //            }
    //            if (obj.Type == LPDP.Objects.ObjectType.Vector)
    //            {
    //                LPDP.Objects.Vector Subvec = (LPDP.Objects.Vector)obj;
    //                DT.Rows.Add("", Subvec.Name, Subvec.GetTree(), type);
    //                this.Write_Vector_to_DataTable(DT,(List<LPDP.Objects.Object>)Subvec.GetValue());
    //            }
    //            else
    //            {
    //                object value_obj = obj.GetValue();
    //                try
    //                {
    //                    value_obj = Convert.ToDouble(value_obj);
    //                    value_obj = Math.Round((double)value_obj, this.Precision);
    //                }
    //                catch { }
    //                DT.Rows.Add("", obj.Name, value_obj, type);
    //            }

    //        }
    //        //return Grid;
    //    }
    }
}
