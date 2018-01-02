using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using LPDP.Objects;
using LPDP.Structure;
using LPDP.Dynamics;

namespace LPDP.DataSets
{
    public class InputOutputData
    {
        public string CodeTxt;
        public string CodeRtf;
        public string InfoTxt;

        public bool ShowSysMark;
        public bool ShowNextOperator;
        public bool ShowQueues;

        Model Model;
        public DataTable Objects;
        public DataTable Initiators;
        public DataTable Queues;
        public DataTable FTT;
        public DataTable CT;

        public double TIME;
        public bool ModelIsBuilt;

        public int Precision;

        //public double GetTIME()
        //{
        //    return this.Model.Executor.GetTIME();
        //}
        //public bool GetModelIsBuilt()
        //{
        //    return this.Model.Built;
        //}


        public void SetParentModel(Model model)
        {
            this.Model = model;
            this.ClearTable(this.Objects);
            this.ClearTable(this.Initiators);
            LPDP_Data.ClearTable(LPDP_Data.Queues);
            LPDP_Data.ClearTable(LPDP_Data.FTT);
            LPDP_Data.ClearTable(LPDP_Data.CT);
        }

        public InputOutputData()
        {
            //this.CodeTxt = this.Model.
            this.Precision = 2;

            //this.Model = model;
            this.Objects = new DataTable("Objects");
            this.Initiators = new DataTable("Initiators");
            this.Queues = new DataTable("Queues");
            this.FTT = new DataTable("FTT");
            this.CT = new DataTable("CT");


            this.ClearTable(this.Objects);
            this.ClearTable(this.Initiators);
            //LPDP_Data.ClearTable(LPDP_Data.Queues);
            this.ClearTable(this.FTT);
            this.ClearTable(this.CT);

            this.CreateTable(this.Objects, "Unit", "Name", "Value", "Type");
            this.CreateTable(this.Initiators, "Number", "Name", "Value", "Type");
            //this.CreateTable(LPDP_Data.Queues, "ID", "Unit", "Mark", "Initiators");
            this.CreateTable(this.FTT, "Time", "Initiator", "Mark", "Unit");
            this.CreateTable(this.CT, "Condition", "Initiator", "Mark", "Unit");
        }

        void CreateTable(DataTable table, params string[] ColumnNames)
        {
            foreach (string colname in ColumnNames)
            {
                table.Columns.Add(colname);
            }
        }
        void ClearTable(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
                col.Dispose();
        }


        void Rewrite_Objects()
        {
            GlobalVarsTable GVT = this.Model.O_Cont.GVT;
            this.Objects.Rows.Clear();
            foreach (string unit in GVT.Vars.Keys.ToArray())
            {
                foreach (LPDP.Objects.Object obj in GVT.Vars[unit])
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
                            case LPDP.Objects.ObjectType.Scalar:
                                type = "Скаляр";
                                break;
                            case LPDP.Objects.ObjectType.Link:
                                type = "Ссылка";
                                break;
                            case LPDP.Objects.ObjectType.Vector:
                                type = "Вектор";
                                break;
                            case LPDP.Objects.ObjectType.Macro:
                                type = "Макрос";
                                break;
                        }

                        if (obj.Type == LPDP.Objects.ObjectType.Vector)
                        {
                            value = ((LPDP.Objects.Vector)obj).GetTree();
                        }
                        Objects.Rows.Add(obj.Unit, obj.Name, value, type);
                        if (obj.Type == LPDP.Objects.ObjectType.Vector)
                        {
                            LPDP.Objects.Vector Vec = (LPDP.Objects.Vector)obj;
                            LPDP.Objects.Vector SubVector = (LPDP.Objects.Vector)Vec;
                            List<LPDP.Objects.Object> vector_value = (List<LPDP.Objects.Object>)(SubVector.GetValue());
                            this.Write_Vector_to_DataTable(this.Objects, vector_value);
                        }
                    }
                }
            }
        }

        void Rewrite_Initiators()
        {
            InitiatorsTable IT = this.Model.O_Cont.IT;
            this.Initiators.Rows.Clear();
            foreach (Initiator init in IT.Initiators)
            {
                int number = init.Number;

                string name;
                string value;
                string type ="";

                LPDP.Objects.Object obj = this.Model.O_Cont.GetObjectFromInitiator(init);
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
                    case LPDP.Objects.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case LPDP.Objects.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case LPDP.Objects.ObjectType.Vector:
                        type = "Вектор";
                        break;
                    case LPDP.Objects.ObjectType.Macro:
                        type = "Макрос";
                        break;
                }
                if (obj.Type == LPDP.Objects.ObjectType.Vector)
                {
                    value = ((LPDP.Objects.Vector)obj).GetTree();
                }
                this.Initiators.Rows.Add(number,name, value, type);
                if (obj.Type == LPDP.Objects.ObjectType.Vector)
                {
                    LPDP.Objects.Vector Vec = (LPDP.Objects.Vector)obj;
                    LPDP.Objects.Vector SubVector = (LPDP.Objects.Vector)Vec;
                    List<LPDP.Objects.Object> vector_value = (List<LPDP.Objects.Object>)(SubVector.GetValue());
                    Write_Vector_to_DataTable(this.Initiators, vector_value);
                }
            }
 
        }

        void Rewrite_FTT()
        {
            List<RecordFTT> ftt = this.Model.Executor.TC_Cont.FutureTimesTable;
            this.FTT.Rows.Clear();

            foreach (RecordFTT rec in ftt)
            {
                Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
                FTT.Rows.Add(rec.ActiveTime, rec.Initiator.Number,label.Name, label.Unit);
            }
        }

        void Rewrite_CT()
        {
            List<RecordCT> ct = this.Model.Executor.TC_Cont.ConditionsTable;
            this.CT.Rows.Clear();

            foreach (RecordCT rec in ct)
            {
                Label label = this.Model.ST_Cont.LT.FindFirstBySubprogram(rec.Subprogram);
                CT.Rows.Add(rec.Condition, rec.Initiator.Number, label.Name, label.Unit);
            }
        }

        public void Output_All_Data()
        {
            this.Rewrite_Objects();
            this.Rewrite_Initiators();
            this.Rewrite_FTT();
            this.Rewrite_CT();

            //this.CodeRtf = 
            this.CodeTxt = this.Model.Analysis.SourceText;
            //this.InfoTxt = this.Model.Analysis.
            this.ModelIsBuilt = this.Model.Built;

            this.TIME = this.Model.Executor.GetTIME();
        }

        void Write_Vector_to_DataTable(DataTable DT, List<LPDP.Objects.Object> list)
        {
            foreach (LPDP.Objects.Object obj in list)
            {
                string type = "";
                switch (obj.Type)
                {
                    case LPDP.Objects.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case LPDP.Objects.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case LPDP.Objects.ObjectType.Vector:
                        type = "Вектор";
                        break;
                    case LPDP.Objects.ObjectType.Macro:
                        type = "Макрос";
                        break;
                }
                if (obj.Type == LPDP.Objects.ObjectType.Vector)
                {
                    LPDP.Objects.Vector Subvec = (LPDP.Objects.Vector)obj;
                    DT.Rows.Add("", Subvec.Name, Subvec.GetTree(), type);
                    this.Write_Vector_to_DataTable(DT,(List<LPDP.Objects.Object>)Subvec.GetValue());
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
            //return Grid;
        }
    }
}
