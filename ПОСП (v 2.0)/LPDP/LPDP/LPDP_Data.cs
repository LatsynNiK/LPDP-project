using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPDP
{
    public static class LPDP_Data
    {
        public static string CodeTxt;
        public static string CodeRtf;
        public static string InfoTxt;

        public static bool ShowSysMark;
        public static bool ShowNextOperator;
        public static bool ShowQueues;

        public static bool GetModelIsBuilt()
        {
            return LPDP_Core.Model_Is_Built;
        }

        //public static bool Model_Is_Built = false;

        public static double GetTIME()
        {
            return LPDP_Core.TIME;
        }

        public static DataTable Objects = new DataTable("Objects");
        public static DataTable Initiators = new DataTable("Initiators");
        public static DataTable Queues = new DataTable("Queues");
        public static DataTable FTT = new DataTable("FTT");
        public static DataTable CT = new DataTable("CT");

        public static void CreateTable(DataTable table, params string[] ColumnNames) 
        {
            foreach (string colname in ColumnNames)
            {
                table.Columns.Add(colname);
            }
        }
        public static void ClearTable(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
                col.Dispose();
        }

        static DataTable Write_Vector_to_Objects(DataTable Grid, List<LPDP_Object> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string type = "";
                switch (list[i].Type)
                {
                    case LPDP_Object.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case LPDP_Object.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case LPDP_Object.ObjectType.Vector:
                        type = "Вектор";
                        break;
                }


                if (list[i].Type == LPDP_Object.ObjectType.Vector)
                {
                    Vector Subvec = (Vector)list[i];
                    Grid.Rows.Add(Subvec.Unit, Subvec.Name, Subvec.GetTree(), type);
                    //Grid.Rows[Grid.Rows.Count - 1].ItemArray[0].Style.ForeColor = Color.White;
                    //Grid.Rows[Grid.RowCount - 1].Visible = false;
                    Grid = Write_Vector_to_Objects(Grid, Subvec.Value);
                }
                else
                {
                    Grid.Rows.Add(list[i].Unit, list[i].Name, list[i].GetValue(), type);
                    //Grid.Rows[Grid.RowCount - 1].Visible = false;
                    //Grid.Rows.SharedRow(Grid.Rows.Count - 1).ItemArray[0].Style.ForeColor = Color.White;

                    //if (LPDP_Core.IsNumber((string)list[i].GetValue()))
                    //    Grid.Rows.Add("", list[i].Name, Convert.ToDouble(list[i].GetValue()), type);
                    //else
                    //    Grid.Rows.Add("", list[i].Name, list[i].GetValue(), type);
                }

            }
            return Grid;
        }
        static void Rewrite_Objects()
        {
            Objects.Rows.Clear();
            for (int i = 0; i < LPDP_Core.GlobalVar_Table.Count; i++)
            {
                LPDP_Object Var = LPDP_Core.GlobalVar_Table[i];

                if (LPDP_Core.Pairs.Exists(rec => (rec.Name == Var.Name) && (rec.To == Var.Unit)) == false)
                {
                    string name = Var.Name;
                    string unit = Var.Unit;
                    string type = "";
                    switch (Var.Type)
                    {
                        case LPDP_Object.ObjectType.Scalar:
                            type = "Скаляр";
                            break;
                        case LPDP_Object.ObjectType.Link:
                            type = "Ссылка";
                            break;
                        case LPDP_Object.ObjectType.Vector:
                            type = "Вектор";
                            break;
                    }

                    if (Var.Type == LPDP_Object.ObjectType.Vector)
                    {
                        Vector Vec = (Vector)Var;
                        Objects.Rows.Add(unit, name, Vec.GetTree(), type);
                        Vector SubVector = (Vector)Vec;
                        Objects = Write_Vector_to_Objects(Objects, SubVector.Value);
                    }
                    else
                    {
                        string value = Var.GetValue();
                        Objects.Rows.Add(unit, name, value, type);
                    }
                }
            }
            //Objects.ClearSelection();
        }

        static DataTable Write_Vector_to_Initiators(DataTable Grid, List<LPDP_Object> list, int ID)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string type = "";
                switch (list[i].Type)
                {
                    case LPDP_Object.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case LPDP_Object.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case LPDP_Object.ObjectType.Vector:
                        type = "Вектор";
                        break;
                }

                if (list[i].Type == LPDP_Object.ObjectType.Vector)
                {
                    Vector Subvec = (Vector)list[i];
                    Grid.Rows.Add("   " + ID + " -> " + Subvec.Name, Subvec.GetValue(), type);
                    //Grid.Rows[Grid.Rows.Count - 1].Visible = false;
                    Grid = Write_Vector_to_Initiators(Grid, Subvec.Value, ID);
                }
                else
                {
                    Grid.Rows.Add("   " + ID + " -> " + list[i].Name, list[i].GetValue(), type);
                    //Grid.Rows[Grid.RowCount - 1].Visible = false;
                }

            }
            return Grid;
        }
        static void Rewrite_Initiators()
        {
            Initiators.Rows.Clear();
            for (int i = 0; i < LPDP_Core.LocalArea_Table.Count; i++)
            {
                int ID = LPDP_Core.LocalArea_Table[i].ID;

                LPDP_Object Var = LPDP_Core.LocalArea_Table[i].Value;

                string type = "";
                switch (Var.Type)
                {
                    case LPDP_Object.ObjectType.Scalar:
                        type = "Скаляр";
                        break;
                    case LPDP_Object.ObjectType.Link:
                        type = "Ссылка";
                        break;
                    case LPDP_Object.ObjectType.Vector:
                        type = "Вектор";
                        break;
                }

                if (Var.Type == LPDP_Object.ObjectType.Vector)
                {
                    Vector Vec = (Vector)Var;
                    Initiators.Rows.Add(" " + ID + " -> ", Vec.GetValue(), type);
                    Vector SubVector = (Vector)Vec;
                    Initiators = Write_Vector_to_Initiators(Initiators, SubVector.Value, ID);
                }
                else
                {
                    Initiators.Rows.Add(" " + ID + " -> ", Var.GetValue(), type);
                }
            }
            //Initiators.ClearSelection();
        }
        static void Rewrite_Queues()
        {
            Queues.Rows.Clear();

            for (int q = 0; q < LPDP_Core.Queues.Count; q++)
            {
                if (LPDP_Core.Queues[q].Queue.Count > 0)
                {
                    if (LPDP_Core.Queues[q].Queue[0].Initiator != -1)
                    {
                        //заголовочная запись
                        string initiators_order = "";
                        for (int i = 0; i < LPDP_Core.Queues[q].Queue.Count - 1; i++)
                            initiators_order += LPDP_Core.Queues[q].Queue[i].Initiator + " << ";
                        initiators_order += LPDP_Core.Queues[q].Queue[LPDP_Core.Queues[q].Queue.Count - 1].Initiator;

                        LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(m => m.Subprogram_Index == q);
                        Queues.Rows.Add(q, NewMark.Unit, NewMark.Name, initiators_order);

                        //развертка
                        for (int i = 0; i < LPDP_Core.Queues[q].Queue.Count; i++)
                        {
                            Vector Init = (Vector)LPDP_Core.LocalArea_Table.Find(la => la.ID == LPDP_Core.Queues[q].Queue[i].Initiator).Value;
                            Queues.Rows.Add(q, "", "", LPDP_Core.Queues[q].Queue[i].Initiator + " -> " + Init.GetValue());
                            //Queues.Rows[Queues.Rows.Count - 1].Visible = false;
                        }
                    }
                }
            }
            //Queues.ClearSelection();

            //for (int i = 0; i < LPDP_Core.LocalArea_Table.Count; i++)
            //{
            //    int ID = LPDP_Core.LocalArea_Table[i].ID;

            //    LPDP_Object Var = LPDP_Core.LocalArea_Table[i].Value;

            //    string type = "";
            //    switch (Var.Type)
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

            //    if (Var.Type == LPDP_Object.ObjectType.Vector)
            //    {
            //        Vector Vec = (Vector)Var;
            //        Initiators.Rows.Add(" " + ID + " -> ", Vec.GetValue(), type);
            //        Vector SubVector = (Vector)Vec;
            //        Initiators = Write_Vector_to_Initiators(Initiators, SubVector.Value, ID);
            //    }
            //    else
            //    {
            //        Initiators.Rows.Add(" " + ID + " -> ", Var.GetValue(), type);
            //    }
            //}


        }
        static void Rewrite_FTT()
        {
            FTT.Rows.Clear();
            for (int i = 0; i < LPDP_Core.FTT.Count; i++)
            {
                LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(name => name.Subprogram_Index == LPDP_Core.FTT[i].Subprogram_Index);
                FTT.Rows.Add(LPDP_Core.FTT[i].ActiveTime, LPDP_Core.FTT[i].Initiator, NewMark.Name, NewMark.Unit);
            }

            //if (FTT.Rows.Count > 0)
            //{
            //    double min_time = (double)FTT.Rows[0].ItemArray[0];
            //    int PaintedIndex = 0;
            //    for (int i = 1; i < FTT.Rows.Count; i++)
            //    {
            //        if ((double)FTT.Rows[i].ItemArray[0] < min_time)
            //        {
            //            PaintedIndex = i;
            //            min_time = (double)FTT.Rows[0].ItemArray[0];
            //        }
            //    }
            //    //FTT.Rows.SharedRow(PaintedIndex).ItemArray[0].Style.ForeColor = Color.Green;
            //    //FTT.Rows.SharedRow(PaintedIndex).ItemArray[0].Style.SelectionForeColor = Color.Green;
            //    //FTT.ClearSelection();
            //}

        }
        static void Rewrite_CT()
        {
            CT.Rows.Clear();
            for (int i = 0; i < LPDP_Core.CT.Count; i++)
            {
                LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(name => name.Subprogram_Index == LPDP_Core.CT[i].Subprogram_Index);
                CT.Rows.Add(LPDP_Core.CT[i].Condition, LPDP_Core.CT[i].Initiator, NewMark.Name, NewMark.Unit);
            }

            //if (CT.Rows.Count > 0)
            //{
            //    for (int i = 0; i < CT.Rows.Count; i++)
            //    {
            //        int SaveInitiatior = LPDP_Core.INITIATOR;
            //        LPDP_Core.INITIATOR = LPDP_Core.CT[i].Initiator;
            //        if (LPDP_Core.Logic_Expression(LPDP_Core.CT[i].Condition, LPDP_Core.CT[i].FromUnit) == true)
            //        {
            //            //CT.Rows[i].ItemArray[0].Style.ForeColor = Color.Green;
            //            //CT.Rows[i].ItemArray[0].Style.SelectionForeColor = Color.Green;
            //            //CT.ClearSelection();
            //        }
            //        LPDP_Core.INITIATOR = SaveInitiatior;
            //    }
            //}

        }

        public static void Rewrite_All_Views()
        {
            Rewrite_FTT();
            Rewrite_CT();
            Rewrite_Objects();
            Rewrite_Initiators();
            Rewrite_Queues();

            //GraphicModel_View.Refresh();
        }
    }
}
