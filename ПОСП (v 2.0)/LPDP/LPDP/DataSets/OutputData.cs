using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using LPDP.Objects;
using LPDP.Structure;

namespace LPDP.DataSets
{
    public class OutputData
    {
        Model Model;
        public DataTable Objects;
        public DataTable Initiators;
        public DataTable Queues;
        public DataTable FTT;
        public DataTable CT;

        public void SetParentModel(Model model)
        {
            this.Model = model;
            //this.Objects = new DataTable("Objects");
            //this.Initiators = new DataTable("Initiators");
            //this.Queues = new DataTable("Queues");
            //this.FTT = new DataTable("FTT");
            //this.CT = new DataTable("CT");



            this.ClearTable(this.Objects);
            //LPDP_Data.ClearTable(LPDP_Data.Initiators);
            //LPDP_Data.ClearTable(LPDP_Data.Queues);
            //LPDP_Data.ClearTable(LPDP_Data.FTT);
            //LPDP_Data.ClearTable(LPDP_Data.CT);

            //this.CreateTable(this.Objects, "Unit", "Name", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Objects, "Unit", "Name", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Initiators, "ID", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Queues, "ID", "Unit", "Mark", "Initiators");
            //LPDP_Data.CreateTable(LPDP_Data.FTT, "Time", "Initiator", "Mark", "Unit");
            //LPDP_Data.CreateTable(LPDP_Data.CT, "Condition", "Initiator", "Mark", "Unit");
        }
        public OutputData()
        {
            //this.Model = model;
            this.Objects = new DataTable("Objects");
            this.Initiators = new DataTable("Initiators");
            this.Queues = new DataTable("Queues");
            this.FTT = new DataTable("FTT");
            this.CT = new DataTable("CT");


            this.ClearTable(this.Objects);
            //LPDP_Data.ClearTable(LPDP_Data.Initiators);
            //LPDP_Data.ClearTable(LPDP_Data.Queues);
            //LPDP_Data.ClearTable(LPDP_Data.FTT);
            //LPDP_Data.ClearTable(LPDP_Data.CT);

            this.CreateTable(this.Objects, "Unit", "Name", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Objects, "Unit", "Name", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Initiators, "ID", "Value", "Type");
            //LPDP_Data.CreateTable(LPDP_Data.Queues, "ID", "Unit", "Mark", "Initiators");
            //LPDP_Data.CreateTable(LPDP_Data.FTT, "Time", "Initiator", "Mark", "Unit");
            //LPDP_Data.CreateTable(LPDP_Data.CT, "Condition", "Initiator", "Mark", "Unit");


        }

        public void CreateTable(DataTable table, params string[] ColumnNames)
        {
            foreach (string colname in ColumnNames)
            {
                table.Columns.Add(colname);
            }
        }
        public void ClearTable(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
                col.Dispose();
        }


        public void Rewrite_Objects()
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
                        string value = Convert.ToString(obj.GetValue());
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
                            Objects = Write_Vector_to_Objects(Objects, vector_value);
                        }
                        //else
                        //{
                        //    value = Convert.ToString(obj.GetValue());
                        //    //Objects.Rows.Add(unit, name, value, type);
                        //}
                    }
                }
                


                //string name = Var.Name;
                //string unit = Var.Unit;
                //string type = "";
                //switch (Var.Type)
                //    {
                //        case LPDP_Object.ObjectType.Scalar:
                //            type = "Скаляр";
                //            break;
                //        case LPDP_Object.ObjectType.Link:
                //            type = "Ссылка";
                //            break;
                //        case LPDP_Object.ObjectType.Vector:
                //            type = "Вектор";
                //            break;
                //    }
            }


            
        }
        DataTable Write_Vector_to_Objects(DataTable Grid, List<LPDP.Objects.Object> list)
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
                    Grid.Rows.Add("", Subvec.Name, Subvec.GetTree(), type);
                    Grid = Write_Vector_to_Objects(Grid,(List<LPDP.Objects.Object>)Subvec.GetValue());
                }
                else
                {
                    Grid.Rows.Add("", obj.Name, obj.GetValue(), type);
                }

            }
            return Grid;
        }

        public double GetTIME()
        {
            return this.Model.Executor.GetTIME();
        }
    }
}
