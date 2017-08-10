using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LPDP
{
    static class LPDP_Graphics
    {

        public struct GraphicSubp
        {
            public int Index;
            public int positionX;
            public int positionY;
            public int SubpWidth;
            public int SubpHeight;

            public string Queue_RTF;
        }
         const int SubpWidth = 50;
         const int SubpHeight = 60;
         const int first_margine = 30;
         const int IntervalX = 70;
         const int IntervalY = 70;

         const int interline = 5;

        public enum TypeOfObject { Unit, Terminate, Parameter }
        public struct GraphicObject
        {
            public string Name;
            public string Value;
            public string Unit;

            public TypeOfObject Type;

            public int positionX;
            public int positionY;
            public int ObjWidth;
            public int ObjHeight;

            public List<GraphicSubp> GraphicTrack;
        }

        public static List<GraphicObject> GraphicModel = new List<GraphicObject>();
        public static void ChangePosition(int IndexOfSelected, Point New)
        {
            GraphicObject NewObj = GraphicModel[IndexOfSelected];
            int DifferenceX = New.X - NewObj.positionX;
            int DifferenceY = New.Y - NewObj.positionY;
            NewObj.positionX = New.X;
            NewObj.positionY = New.Y;

            //для блоков
            if (NewObj.Type == TypeOfObject.Unit)
            {
                for (int j = 0; j < GraphicModel[IndexOfSelected].GraphicTrack.Count; j++)
                {
                    GraphicSubp NewSubp = GraphicModel[IndexOfSelected].GraphicTrack[j];
                    NewSubp.positionX += DifferenceX;
                    NewSubp.positionY += DifferenceY;
                    GraphicModel[IndexOfSelected].GraphicTrack[j] = NewSubp;
                }
            }

            //для связей
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].StartObjInd == IndexOfSelected)
                {
                    Connection NewConnection = Connections[i];
                    NewConnection.startX += DifferenceX;
                    NewConnection.startY += DifferenceY;
                    Connections[i] = NewConnection;
                }
                if (Connections[i].EndObjInd == IndexOfSelected)
                {
                    Connection NewConnection = Connections[i];
                    NewConnection.endX += DifferenceX;
                    NewConnection.endY += DifferenceY;
                    Connections[i] = NewConnection;
                }
                CreatePath(i);
            }

            GraphicModel[IndexOfSelected] = NewObj;
        }

        public enum TypeOfConnection { Parameter, Path, Branch }
        public struct Connection
        {
            public TypeOfConnection Type;
            public int startX;
            public int startY;
            public int endX;
            public int endY;
            public Point[] Path;

            public int StartObjInd;
            public int StartSubpInd;
            public int EndObjInd;
            public int EndSubpInd;
        }
        public static List<Connection> Connections = new List<Connection>();
        public static void CreatePath(int IndexOfConnection)
        {
            Connection NewConnection = Connections[IndexOfConnection];
            Point[] NewPath = new Point[4];
            int DifferenceX = NewConnection.endX - NewConnection.startX;
            int DifferenceY = NewConnection.endY - NewConnection.startY;

            //int Direction = 0; //1-справа, 2- слева, 3-сверху, 4-снизу, 12-горизонтально, 34-вертикально

            // по Y
            if (((NewConnection.StartSubpInd >= 0) && (NewConnection.StartSubpInd < GraphicModel[NewConnection.StartObjInd].GraphicTrack.Count - 1)) ||// начало в середине трека
                ((NewConnection.EndSubpInd >= 0) && (NewConnection.EndSubpInd < GraphicModel[NewConnection.EndObjInd].GraphicTrack.Count - 1)) ||  // конец в середине трека
                (Math.Abs(DifferenceX) <= Math.Abs(DifferenceY)) || //вытянута по Y
                ((DifferenceX > 0) && (NewConnection.EndSubpInd == GraphicModel[NewConnection.EndObjInd].GraphicTrack.Count - 1)) ||//слева направо и приходит к последней
                ((DifferenceX <= 0) && (NewConnection.StartSubpInd == GraphicModel[NewConnection.StartObjInd].GraphicTrack.Count - 1)))//справа налево и уходит от последней
            {
                NewPath[0].X = NewConnection.startX;
                NewPath[1].X = NewPath[0].X;
                NewPath[3].X = NewConnection.endX;
                NewPath[2].X = NewPath[3].X;

                //с одной стороны
                if (Math.Abs(DifferenceY) <= GraphicModel[NewConnection.StartObjInd].ObjHeight / 2)
                {
                    // оба снизу
                    if ((NewConnection.StartObjInd == NewConnection.EndObjInd) && (NewConnection.StartSubpInd > NewConnection.EndSubpInd))
                    {
                        NewPath[0].Y = NewConnection.startY + SubpHeight / 2;
                        NewPath[1].Y = NewPath[0].Y + (first_margine / 3) + (interline * NewConnection.StartSubpInd);
                        NewPath[3].Y = NewConnection.endY + SubpHeight / 2;
                        NewPath[2].Y = NewPath[3].Y + (first_margine / 3) + (interline * NewConnection.StartSubpInd);
                    }
                    // оба сверху
                    else
                    {
                        NewPath[0].Y = NewConnection.startY - SubpHeight / 2;
                        NewPath[1].Y = NewPath[0].Y - (first_margine / 3) - (interline * NewConnection.StartSubpInd);
                        NewPath[3].Y = NewConnection.endY - SubpHeight / 2;
                        NewPath[2].Y = NewPath[3].Y - (first_margine / 3) - (interline * NewConnection.StartSubpInd);
                    }
                }

                //с разных сторон
                else
                {
                    if (DifferenceY > 0) //сверху вниз \/
                    {
                        NewPath[0].Y = NewConnection.startY + SubpHeight / 2;
                        NewPath[1].Y = NewPath[0].Y + first_margine / 3;
                        NewPath[3].Y = NewConnection.endY - SubpHeight / 2;
                        NewPath[2].Y = NewPath[3].Y - first_margine / 3;
                    }
                    else //снизу вверх /\
                    {
                        NewPath[0].Y = NewConnection.startY - SubpHeight / 2;
                        NewPath[1].Y = NewPath[0].Y - first_margine / 3;
                        NewPath[3].Y = NewConnection.endY + SubpHeight / 2;
                        NewPath[2].Y = NewPath[3].Y + first_margine / 3;
                    }
                }
            }

            //по X
            else
            {
                NewPath[0].Y = NewConnection.startY;
                NewPath[1].Y = NewPath[0].Y;
                NewPath[3].Y = NewConnection.endY;
                NewPath[2].Y = NewPath[3].Y;
                if (DifferenceX > 0)
                {
                    NewPath[0].X = NewConnection.startX + SubpHeight / 2;
                    NewPath[1].X = NewPath[0].X + first_margine / 3;
                    NewPath[3].X = NewConnection.endX - SubpHeight / 2;
                    NewPath[2].X = NewPath[3].X - first_margine / 3;
                }
                else
                {
                    NewPath[0].X = NewConnection.startX - SubpHeight / 2;
                    NewPath[1].X = NewPath[0].X - first_margine / 3;
                    NewPath[3].X = NewConnection.endX + SubpHeight / 2;
                    NewPath[2].X = NewPath[3].X + first_margine / 3;
                }
            }
            NewConnection.Path = NewPath;
            Connections[IndexOfConnection] = NewConnection;
        }

        public static void Load_GraphicModel()
        {
            //GraphicModel.Clear();
            //Connections.Clear();

            int CurrentY = first_margine;

            for (int u = 0; u < LPDP_Code.Units.Count; u++)
            {
                int CurrentX = first_margine;

                GraphicObject New_GraphicObject = new GraphicObject();
                New_GraphicObject.GraphicTrack = new List<GraphicSubp>();

                //Header
                GraphicSubp Header = new GraphicSubp();
                Header.Index = -1;
                Header.positionX = CurrentX;
                Header.positionY = CurrentY;
                Header.SubpWidth = SubpWidth * 8 / 5;
                Header.SubpHeight = SubpHeight;
                New_GraphicObject.GraphicTrack.Add(Header);
                CurrentX += SubpWidth * 8 / 5;


                for (int i = 0; i < LPDP_Core.POSP_Model.Count; i++)
                {
                    if (LPDP_Core.POSP_Model[i].Operations[0].Unit_index != u) continue;

                    GraphicSubp New_GraphicSubp = new GraphicSubp();
                    // описание подпрограммы

                    // конец
                    New_GraphicSubp.Index = i;
                    New_GraphicSubp.positionX = CurrentX;
                    New_GraphicSubp.positionY = CurrentY;
                    New_GraphicSubp.SubpWidth = SubpWidth;
                    New_GraphicSubp.SubpHeight = SubpHeight;

                    New_GraphicObject.GraphicTrack.Add(New_GraphicSubp);
                    CurrentX += SubpWidth;
                }
                New_GraphicObject.Name = LPDP_Code.Units[u].Name;
                New_GraphicObject.Value = LPDP_Code.Units[u].Type;
                New_GraphicObject.Type = TypeOfObject.Unit;
                New_GraphicObject.Unit = New_GraphicObject.Name;

                New_GraphicObject.positionX = first_margine;
                New_GraphicObject.positionY = CurrentY;
                New_GraphicObject.ObjWidth = CurrentX - first_margine;
                New_GraphicObject.ObjHeight = SubpHeight;
                GraphicModel.Add(New_GraphicObject);


                //проверка общих параметров 
                for (int i = 0; i < LPDP_Core.Pairs.Count; i++)
                {
                    if ((LPDP_Core.Pairs[i].From == LPDP_Code.Units[u].Name) &&      //относится к этому блоку
                        (GraphicModel.FindIndex(obj => (obj.Name == LPDP_Core.Pairs[i].Name) && ((obj.Unit == LPDP_Core.Pairs[i].From) || (obj.Unit == LPDP_Core.Pairs[i].To))) == -1)) //либо параметр с новым именен, либо не связанный с предыдущим блоком
                    {
                        GraphicObject NewObj = new GraphicObject();
                        NewObj.GraphicTrack = new List<GraphicSubp>();

                        NewObj.Name = LPDP_Core.Pairs[i].Name;
                        //NewObj.Value = LPDP_Core.GetValue(LPDP_Core.Pairs[i].Name, LPDP_Core.Pairs[i].From);
                        NewObj.Value = "";
                        NewObj.Type = TypeOfObject.Parameter;
                        NewObj.Unit = LPDP_Code.Units[u].Name;

                        CurrentX += IntervalX;
                        NewObj.positionX = CurrentX;
                        NewObj.positionY = CurrentY;
                        NewObj.ObjWidth = SubpHeight;
                        NewObj.ObjHeight = SubpHeight;

                        CurrentX += NewObj.ObjWidth;
                        GraphicModel.Add(NewObj);
                    }
                }

                for (int i = 0; i < LPDP_Core.POSP_Model.Count; i++)
                {
                    if (LPDP_Core.POSP_Model[i].Operations[0].Unit_index != u) continue;

                    //проверка "активизации", "пассивизации" и "уничтожения"
                    for (int op = 0; op < LPDP_Core.POSP_Model[i].Operations.Count; op++)
                    {
                        if ((LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "активизировать") ||
                            (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "пассивизировать") ||
                            (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "уничтожить"))
                        {
                            GraphicObject NewObj = new GraphicObject();
                            NewObj.GraphicTrack = new List<GraphicSubp>();

                            if ((LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "активизировать") ||
                                (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "пассивизировать"))
                            {
                                NewObj.Name = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                                NewObj.Value = LPDP_Core.GetValue(LPDP_Core.POSP_Model[i].Operations[op].Parameters[0], LPDP_Core.POSP_Model[i].Operations[op].Parameters[1]);
                                NewObj.Type = TypeOfObject.Parameter;
                                NewObj.Unit = LPDP_Code.Units[u].Name;
                            }
                            else
                            {
                                NewObj.Name = "Уничтожить";
                                NewObj.Value = "";
                                NewObj.Type = TypeOfObject.Terminate;
                                NewObj.Unit = LPDP_Code.Units[u].Name;
                            }

                            CurrentX += IntervalX;
                            NewObj.positionX = CurrentX;
                            NewObj.positionY = CurrentY;
                            NewObj.ObjWidth = SubpHeight;
                            NewObj.ObjHeight = SubpHeight;

                            CurrentX += NewObj.ObjWidth;
                            if (GraphicModel.FindIndex(obj => (obj.Name == NewObj.Name) && (obj.Unit == NewObj.Unit)) == -1)
                            {
                                if ((NewObj.Type == TypeOfObject.Terminate) ||
                                    (LPDP_Core.Pairs.FindIndex(rec => (rec.Name == NewObj.Name) && ((rec.From == NewObj.Unit) || (rec.To == NewObj.Unit))) == -1))
                                    GraphicModel.Add(NewObj);
                            }
                        }
                    }
                }
                CurrentY += SubpHeight + IntervalY;
            }

            // загрузка связей
            for (int i = 0; i < LPDP_Core.POSP_Model.Count; i++)
            {
                for (int op = 0; op < LPDP_Core.POSP_Model[i].Operations.Count; op++)
                {
                    Connection NewConnection = new Connection();
                    //поиск начала
                    for (NewConnection.StartObjInd = 0; NewConnection.StartObjInd < GraphicModel.Count; NewConnection.StartObjInd++)
                    {
                        if (GraphicModel[NewConnection.StartObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == i) == -1) continue;
                        NewConnection.StartSubpInd = GraphicModel[NewConnection.StartObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == i);
                        GraphicSubp NewSubp = GraphicModel[NewConnection.StartObjInd].GraphicTrack.Find(sbp => sbp.Index == i);
                        NewConnection.startX = NewSubp.positionX + (NewSubp.SubpWidth / 2);
                        NewConnection.startY = NewSubp.positionY + (NewSubp.SubpHeight / 2);
                        break;
                    }

                    if (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "направить")
                    {
                        NewConnection.Type = TypeOfConnection.Path;
                        // поиск конца
                        string NameMark = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string FromUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        string ToUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[2];

                        LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(mark => (mark.Name == NameMark) && (mark.Unit == ToUnit));

                        for (NewConnection.EndObjInd = 0; NewConnection.EndObjInd < GraphicModel.Count; NewConnection.EndObjInd++)
                        {
                            if (GraphicModel[NewConnection.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index) == -1) continue;
                            NewConnection.EndSubpInd = GraphicModel[NewConnection.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index);
                            GraphicSubp NewSubp = GraphicModel[NewConnection.EndObjInd].GraphicTrack.Find(sbp => sbp.Index == NewMark.Subprogram_Index);
                            NewConnection.endX = NewSubp.positionX + (NewSubp.SubpWidth / 2);
                            NewConnection.endY = NewSubp.positionY + (NewSubp.SubpHeight / 2);
                            break;
                        }
                        if ((NewConnection.StartObjInd != NewConnection.EndObjInd) || (NewConnection.StartSubpInd + 1 != NewConnection.EndSubpInd)) Connections.Add(NewConnection);
                        continue;
                    }

                    if (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "активизировать")
                    {
                        // Соединение подпрограммы со ссылкой
                        NewConnection.Type = TypeOfConnection.Parameter;

                        //поиск конца в ссылке
                        string NameLink = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string NameUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        NewConnection.EndObjInd = GraphicModel.FindIndex(obj => (obj.Name == NameLink) && (obj.Unit == NameUnit));
                        NewConnection.EndSubpInd = 0;
                        GraphicObject NewObj = GraphicModel.Find(obj => (obj.Name == NameLink) && (obj.Unit == NameUnit));
                        NewConnection.endX = NewObj.positionX + (NewObj.ObjWidth / 2);
                        NewConnection.endY = NewObj.positionY + (NewObj.ObjHeight / 2);
                        if ((NewConnection.StartObjInd != NewConnection.EndObjInd) || (NewConnection.StartSubpInd + 1 != NewConnection.EndSubpInd)) Connections.Add(NewConnection);

                        // Соединение ссылки с подпрограммый
                        Connection NewConnection2 = new Connection();
                        NewConnection2.Type = TypeOfConnection.Path;
                        //поиск начала в ссылке
                        NewConnection2.StartObjInd = NewConnection.EndObjInd;
                        NewConnection2.StartSubpInd = NewConnection.EndSubpInd;
                        NewConnection2.startX = NewConnection.endX;
                        NewConnection2.startY = NewConnection.endY;

                        // поиск конца в подпрограмме
                        string NameMark = LPDP_Core.POSP_Model[i].Operations[op].Parameters[2];
                        string ToUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[3];
                        LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(mark => (mark.Name == NameMark) && (mark.Unit == ToUnit));
                        for (NewConnection2.EndObjInd = 0; NewConnection2.EndObjInd < GraphicModel.Count; NewConnection2.EndObjInd++)
                        {
                            if (GraphicModel[NewConnection2.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index) == -1) continue;
                            NewConnection2.EndSubpInd = GraphicModel[NewConnection2.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index);
                            GraphicSubp NewSubp = GraphicModel[NewConnection2.EndObjInd].GraphicTrack.Find(sbp => sbp.Index == NewMark.Subprogram_Index);
                            NewConnection2.endX = NewSubp.positionX + (NewSubp.SubpWidth / 2);
                            NewConnection2.endY = NewSubp.positionY + (NewSubp.SubpHeight / 2);
                            break;
                        }
                        if ((NewConnection2.StartObjInd != NewConnection2.EndObjInd) || (NewConnection2.StartSubpInd + 1 != NewConnection2.EndSubpInd)) Connections.Add(NewConnection2);
                        continue;
                    }

                    if (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "пассивизировать")
                    {
                        NewConnection.Type = TypeOfConnection.Path;
                        //поиск конца в ссылке
                        string NameLink = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string NameUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        NewConnection.EndObjInd = GraphicModel.FindIndex(obj => (obj.Name == NameLink) && (obj.Unit == NameUnit));
                        NewConnection.EndSubpInd = 0;
                        GraphicObject NewObj = GraphicModel.Find(obj => (obj.Name == NameLink) && (obj.Unit == NameUnit));
                        NewConnection.endX = NewObj.positionX + (NewObj.ObjWidth / 2);
                        NewConnection.endY = NewObj.positionY + (NewObj.ObjHeight / 2);
                        if ((NewConnection.StartObjInd != NewConnection.EndObjInd) || (NewConnection.StartSubpInd + 1 != NewConnection.EndSubpInd)) Connections.Add(NewConnection);
                        continue;
                    }

                    if ((LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "если") ||
                        (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "ждать"))
                    {
                        NewConnection.Type = TypeOfConnection.Branch;

                        // поиск конца
                        string NameMark = LPDP_Core.POSP_Model[i].Operations[op].Parameters[2];
                        string ToUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[3];
                        LPDP_Core.record_MARK NewMark = LPDP_Core.MARKS.Find(mark => (mark.Name == NameMark) && (mark.Unit == ToUnit));
                        if (NewMark.Name.Contains("$") == false) //continue;
                        {
                            for (NewConnection.EndObjInd = 0; NewConnection.EndObjInd < GraphicModel.Count; NewConnection.EndObjInd++)
                            {
                                if (GraphicModel[NewConnection.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index) == -1) continue;
                                NewConnection.EndSubpInd = GraphicModel[NewConnection.EndObjInd].GraphicTrack.FindIndex(sbp => sbp.Index == NewMark.Subprogram_Index);
                                GraphicSubp NewSubp = GraphicModel[NewConnection.EndObjInd].GraphicTrack.Find(sbp => sbp.Index == NewMark.Subprogram_Index);
                                NewConnection.endX = NewSubp.positionX + (NewSubp.SubpWidth / 2);
                                NewConnection.endY = NewSubp.positionY + (NewSubp.SubpHeight / 2);
                                break;
                            }
                            Connections.Add(NewConnection);
                        }

                        string Condition = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string NameUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        for (int j = 0; j < GraphicModel.Count; j++)
                        {
                            if ((GraphicModel[j].Type == TypeOfObject.Parameter) && /*далее нужно исправить*/(LPDP_Core.Here_Contains_NameOfVar(Condition, GraphicModel[j].Name))/*(Condition.Contains(GraphicModel[j].Name)) */&&
                                (LPDP_Core.Pairs.Exists(rec => (rec.Name == GraphicModel[j].Name) && (rec.To == NameUnit))))
                            {
                                Connection NewConnection2 = new Connection();
                                NewConnection2.Type = TypeOfConnection.Parameter;
                                //поиск начала в параметре
                                NewConnection2.StartObjInd = j;
                                NewConnection2.StartSubpInd = 0;
                                NewConnection2.startX = GraphicModel[j].positionX + (GraphicModel[j].ObjWidth / 2);
                                NewConnection2.startY = GraphicModel[j].positionY + (GraphicModel[j].ObjHeight / 2);

                                //поиск конца в подпрограмме
                                NewConnection2.EndObjInd = NewConnection.StartObjInd;
                                NewConnection2.EndSubpInd = NewConnection.StartSubpInd;
                                NewConnection2.endX = NewConnection.startX;
                                NewConnection2.endY = NewConnection.startY;
                                Connections.Add(NewConnection2);
                            }
                        }
                        continue;
                    }

                    if ((LPDP_Core.POSP_Model[i].Operations[op].NameOperation == ":=") ||
                        (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == ":=ссылка"))
                    {
                        string Var = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string NameUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        string Exp = LPDP_Core.POSP_Model[i].Operations[op].Parameters[2];
                        for (int j = 0; j < GraphicModel.Count; j++)
                        {
                            if ((GraphicModel[j].Type == TypeOfObject.Parameter) && (Var == GraphicModel[j].Name) &&
                                ((LPDP_Core.Pairs.Exists(rec => ((rec.Name == Var) && ((rec.From == NameUnit) || (rec.To == NameUnit))))) || (NameUnit == GraphicModel[j].Unit)))
                            {
                                NewConnection.Type = TypeOfConnection.Parameter;
                                //поиск конца в параметре
                                NewConnection.EndObjInd = j;
                                NewConnection.EndSubpInd = 0;
                                NewConnection.endX = GraphicModel[j].positionX + (GraphicModel[j].ObjWidth / 2);
                                NewConnection.endY = GraphicModel[j].positionY + (GraphicModel[j].ObjHeight / 2);
                                Connections.Add(NewConnection);
                            }
                            if ((GraphicModel[j].Type == TypeOfObject.Parameter) && (Exp.Contains(GraphicModel[j].Name)) &&
                                ((LPDP_Core.Pairs.Exists(rec => ((Exp.Contains(rec.Name)) && ((rec.From == NameUnit) || (rec.To == NameUnit))))) || (NameUnit == GraphicModel[j].Unit)))
                            {
                                NewConnection.Type = TypeOfConnection.Parameter;
                                //поиск начала в параметре
                                NewConnection.EndObjInd = NewConnection.StartObjInd;
                                NewConnection.EndSubpInd = NewConnection.StartSubpInd;
                                NewConnection.endX = NewConnection.startX;
                                NewConnection.endY = NewConnection.startY;

                                NewConnection.StartObjInd = j;
                                NewConnection.StartSubpInd = 0;
                                NewConnection.startX = GraphicModel[j].positionX + (GraphicModel[j].ObjWidth / 2);
                                NewConnection.startY = GraphicModel[j].positionY + (GraphicModel[j].ObjHeight / 2);
                                Connections.Add(NewConnection);
                            }
                        }
                    }


                    if (LPDP_Core.POSP_Model[i].Operations[op].NameOperation == "уничтожить")
                    {
                        string NameObj = LPDP_Core.POSP_Model[i].Operations[op].Parameters[0];
                        string NameUnit = LPDP_Core.POSP_Model[i].Operations[op].Parameters[1];
                        if (NameObj == "ИНИЦИАТОР")
                        {
                            NewConnection.Type = TypeOfConnection.Path;
                            // поиск конца
                            NewConnection.EndObjInd = GraphicModel.FindIndex(obj => (obj.Name == "Уничтожить") && (obj.Unit == NameUnit));
                            NewConnection.EndSubpInd = 0;
                            GraphicObject NewObj = GraphicModel.Find(obj => (obj.Name == "Уничтожить") && (obj.Unit == NameUnit));
                            NewConnection.endX = NewObj.positionX + (NewObj.ObjWidth / 2);
                            NewConnection.endY = NewObj.positionY + (NewObj.ObjHeight / 2);
                            Connections.Add(NewConnection);
                            continue;
                        }
                    }
                }
            }
            for (int i = 0; i < Connections.Count - 1; i++)
            {
                for (int j = i + 1; j < Connections.Count; j++)
                {
                    if ((Connections[j].StartObjInd == Connections[i].StartObjInd) &&
                        (Connections[j].StartSubpInd == Connections[i].StartSubpInd) &&
                        (Connections[j].EndObjInd == Connections[i].EndObjInd) &&
                        (Connections[j].EndSubpInd == Connections[i].EndSubpInd) &&
                        (Connections[j].Type == Connections[i].Type))
                        Connections.RemoveAt(j);
                }
            }
            for (int i = 0; i < Connections.Count; i++)
            {
                CreatePath(i);
            }
        }
        public static void Reload_Values_and_Queues(bool Show_Queue)
        {
            for (int i = 0; i < GraphicModel.Count; i++)
            {
                GraphicObject NewGraph = GraphicModel[i];
                //обновление параметров
                if (GraphicModel[i].Type == TypeOfObject.Parameter)
                {
                    LPDP_Object NewObj = LPDP_Core.GlobalVar_Table.Find(obj => ((obj.Name == GraphicModel[i].Name) && (obj.Unit == GraphicModel[i].Unit)));

                    if ((NewObj == null) || (NewObj.GetValue() == ""))
                        NewGraph.Value = "---";
                    else
                        NewGraph.Value = NewObj.GetValue();
                }

                // обновление очередей
                if (GraphicModel[i].Type == TypeOfObject.Unit)
                {
                    for (int j = 1; j < NewGraph.GraphicTrack.Count; j++)
                    {
                        GraphicSubp NewSubp = NewGraph.GraphicTrack[j];
                        if (Show_Queue)
                            NewSubp.Queue_RTF = LPDP_Code.GetRTF_Pointers(NewGraph.GraphicTrack[j].Index);
                        else
                            NewSubp.Queue_RTF = LPDP_Code.ConvertToRTF("");
                        NewGraph.GraphicTrack[j] = NewSubp;
                    }
                }
                GraphicModel[i] = NewGraph;

            }
        }

        static Graphics Draw_GraphicSubp(Graphics Canvas, int Obj_Index, int Subp_Index)
        {
            const int margine = 5;

            Pen NewPen = new Pen(Color.Black, 3);
            GraphicSubp NewSubp = GraphicModel[Obj_Index].GraphicTrack[Subp_Index];
            Canvas.DrawRectangle(NewPen, NewSubp.positionX, NewSubp.positionY, NewSubp.SubpWidth, NewSubp.SubpHeight);

            //Font NewFont = new Font("Microsoft Sans Serif", 14);
            //RectangleF NewRectF = new RectangleF(NewSubp.positionX + 2 * margine, NewSubp.positionY + NewSubp.SubpHeight / 5 * 2 + margine, NewSubp.SubpWidth - 2 * margine, NewSubp.SubpHeight * 2 / 5 - margine);

            //Header
            if (Subp_Index == 0)
            {
                Font NewFont = new Font("Microsoft Sans Serif", 14);
                RectangleF NewRectF = new RectangleF(NewSubp.positionX + 2 * margine, NewSubp.positionY + NewSubp.SubpHeight / 5 * 2 + margine, NewSubp.SubpWidth - 2 * margine, NewSubp.SubpHeight * 2 / 5 - margine);
                Canvas.DrawString(GraphicModel[Obj_Index].Name, NewFont, new SolidBrush(Color.Black), NewRectF);

                NewFont = new Font("Microsoft Sans Serif", 12);
                NewRectF = new RectangleF(NewSubp.positionX + 5, NewSubp.positionY + margine, NewSubp.SubpWidth, NewSubp.SubpHeight);
                string letter = GraphicModel[Obj_Index].Value.Substring(0, 1);
                Canvas.DrawString(letter, NewFont, new SolidBrush(Color.Black), NewRectF);

                NewFont.Dispose();
            }
            //подпрограмма
            else
            {
                Font NewFont = new Font("Microsoft Sans Serif", 11);
                Font NewFont4 = new Font("Microsoft Sans Serif", 14);

                RectangleF NewRectF1 = new RectangleF(NewSubp.positionX, NewSubp.positionY + NewSubp.SubpHeight / 5 * 2, NewSubp.SubpWidth / 3, NewSubp.SubpHeight / 5 + margine);
                RectangleF NewRectF2 = new RectangleF(NewRectF1.X + NewRectF1.Width, NewSubp.positionY + NewSubp.SubpHeight / 5 * 2, NewSubp.SubpWidth / 3, NewSubp.SubpHeight / 5 + margine);
                RectangleF NewRectF3 = new RectangleF(NewRectF2.X + NewRectF2.Width, NewSubp.positionY + NewSubp.SubpHeight / 5 * 2, NewSubp.SubpWidth / 3, NewSubp.SubpHeight / 5 + margine);

                RectangleF NewRectF4 = new RectangleF(NewRectF1.X + NewRectF1.Width, NewSubp.positionY + NewSubp.SubpHeight / 5 * 3 + margine, NewSubp.SubpWidth / 3, NewSubp.SubpHeight * 2 / 5 - margine);

                RichTextBox NewRTB = new RichTextBox();

                NewRTB.Rtf = GraphicModel[Obj_Index].GraphicTrack[Subp_Index].Queue_RTF;
                //NewRTB.Rtf = LPDP_Code.ReplaceRTF(NewRTB.Rtf, "\t", "");

                if (NewRTB.Text.Length > 1)
                {
                    NewRTB.SelectionStart = 0; NewRTB.SelectionLength = 1;
                    Color NewColor = NewRTB.SelectionColor;
                    Canvas.DrawString("" + NewRTB.Text[0], NewFont, new SolidBrush(NewColor), NewRectF1);
                }
                if (NewRTB.Text.Length > 2)
                {
                    if (NewRTB.Text[1] != '.')
                    {
                        NewRTB.SelectionStart = 1; NewRTB.SelectionLength = 1;
                        Color NewColor = NewRTB.SelectionColor;
                        Canvas.DrawString("" + NewRTB.Text[1], NewFont, new SolidBrush(NewColor), NewRectF2);

                        if (NewRTB.Text[2] != '\t')
                        {
                            NewRTB.SelectionStart = 2; NewRTB.SelectionLength = 1;
                            NewColor = NewRTB.SelectionColor;
                            Canvas.DrawString("" + NewRTB.Text[2], NewFont, new SolidBrush(NewColor), NewRectF3);
                        }
                    }
                    else
                    {
                        Color NewColor = Color.Black;
                        Canvas.DrawString("" + NewRTB.Text.Substring(1, 2), NewFont, new SolidBrush(NewColor), NewRectF2);

                        NewRTB.SelectionStart = 3; NewRTB.SelectionLength = 1;
                        NewColor = NewRTB.SelectionColor;
                        Canvas.DrawString("" + NewRTB.Text[3], NewFont, new SolidBrush(NewColor), NewRectF3);
                    }
                }

                // отображение текущего
                if (LPDP_Core.SubProg_IsBroken)
                {
                    Color color_for_pointer;
                    if (LPDP_Core.INITIATOR == -1)
                        color_for_pointer = Color.DarkGreen;
                    else
                        color_for_pointer = Color.Green;

                    NewRTB.Rtf = LPDP_Code.GetRTF_CurrentPointer(color_for_pointer);

                    if (LPDP_Core.NEXT_SUBPROGRAMM == LPDP_Graphics.GraphicModel[Obj_Index].GraphicTrack[Subp_Index].Index)
                    {
                        Canvas.DrawString("" + NewRTB.Text, NewFont, new SolidBrush(color_for_pointer), NewRectF4);
                    }
                }

                NewRTB.Dispose();
            }

            NewPen.Dispose();
            return Canvas;
        }
        public static Graphics Draw_GraphicParameter(Graphics Canvas, int Obj_Index)
        {
            const int margine = 5;

            // рисуем эллипс
            Pen NewPen = new Pen(Color.Black, 3);
            GraphicObject NewObj = GraphicModel[Obj_Index];
            Canvas.DrawEllipse(NewPen, NewObj.positionX, NewObj.positionY, NewObj.ObjWidth, NewObj.ObjHeight);

            //подпись имени параметра
            RectangleF NewRectF = new RectangleF(NewObj.positionX + margine, NewObj.positionY + margine, NewObj.ObjWidth - margine * 2, margine * 4);
            Font NewFont = new Font("Microsoft Sans Serif", 14);
            StringFormat NameFormat = new StringFormat(); NameFormat.Alignment = StringAlignment.Center;
            Canvas.DrawString(NewObj.Name, NewFont, new SolidBrush(Color.Black), NewRectF, NameFormat);

            //подпись значения параметра
            NewRectF = new RectangleF(NewObj.positionX + margine, NewObj.positionY + margine * 6, NewObj.ObjWidth - margine * 2, margine * 4);
            NewFont = new Font("Microsoft Sans Serif", 12);
            Canvas.DrawString(NewObj.Value, NewFont, new SolidBrush(Color.Black), NewRectF, NameFormat);

            NewPen.Dispose();
            NewFont.Dispose();
            NameFormat.Dispose();

            return Canvas;
        }
        public static Graphics Draw_GraphicTerminate(Graphics Canvas, int Obj_Index)
        {
            const int margine = 5;

            Pen NewPen = new Pen(Color.Black, 3);
            GraphicObject NewObj = GraphicModel[Obj_Index];
            Canvas.DrawEllipse(NewPen, NewObj.positionX, NewObj.positionY, NewObj.ObjWidth, NewObj.ObjHeight);
            PointF p11 = new PointF(NewObj.positionX + margine * 3, NewObj.positionY + margine * 3);
            PointF p12 = new PointF(NewObj.positionX + NewObj.ObjWidth - margine * 3, NewObj.positionY + margine * 3);
            PointF p21 = new PointF(NewObj.positionX + margine * 3, NewObj.positionY + NewObj.ObjHeight - margine * 3);
            PointF p22 = new PointF(NewObj.positionX + NewObj.ObjWidth - margine * 3, NewObj.positionY + NewObj.ObjHeight - margine * 3);

            Canvas.DrawLine(NewPen, p11, p22);
            Canvas.DrawLine(NewPen, p12, p21);

            NewPen.Dispose();

            return Canvas;
        }

        public static Graphics Draw_GraphicObject(Graphics Canvas, int Obj_Index)
        {
            if (LPDP_Graphics.GraphicModel[Obj_Index].Type == TypeOfObject.Unit)
            {
                for (int j = 0; j < LPDP_Graphics.GraphicModel[Obj_Index].GraphicTrack.Count; j++)
                    Canvas = LPDP_Graphics.Draw_GraphicSubp(Canvas, Obj_Index, j);
            }

            if (LPDP_Graphics.GraphicModel[Obj_Index].Type == TypeOfObject.Parameter)
                LPDP_Graphics.Draw_GraphicParameter(Canvas, Obj_Index);

            if (LPDP_Graphics.GraphicModel[Obj_Index].Type == TypeOfObject.Terminate)
                LPDP_Graphics.Draw_GraphicTerminate(Canvas, Obj_Index);

            return Canvas;
        }

        public static Graphics Draw_Connection(Graphics Canvas, int Connection_Index)
        {
            Pen NewPen = new Pen(Color.Black, 4);
            NewPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            NewPen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
            NewPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            switch (Connections[Connection_Index].Type)
            {
                case TypeOfConnection.Path:
                    break;
                case TypeOfConnection.Parameter:
                    //NewPen.Width = 5;
                    NewPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    break;
                case TypeOfConnection.Branch:
                    //NewPen.Width = 3;
                    NewPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
            }
            // для повторной двусторонней связи
            for (int i = 0; i < Connection_Index; i++)
            {

                if ((Connections[Connection_Index].Type == Connections[i].Type) &&
                    (Connections[Connection_Index].startX == Connections[i].endX) &&
                    (Connections[Connection_Index].startY == Connections[i].endY) &&
                    (Connections[Connection_Index].endX == Connections[i].startX) &&
                    (Connections[Connection_Index].endY == Connections[i].startY))
                {
                    NewPen.Dispose();
                    return Canvas;
                }
            }
            // проверяем, будет ли связь двусторонней
            for (int i = Connection_Index + 1; i < Connections.Count; i++)
            {
                if ((Connections[Connection_Index].Type == Connections[i].Type) &&
                    (Connections[Connection_Index].startX == Connections[i].endX) &&
                    (Connections[Connection_Index].startY == Connections[i].endY) &&
                    (Connections[Connection_Index].endX == Connections[i].startX) &&
                    (Connections[Connection_Index].endY == Connections[i].startY))
                {
                    NewPen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                }
            }
            System.Drawing.Drawing2D.GraphicsPath NewPath = new System.Drawing.Drawing2D.GraphicsPath();
            NewPath.AddLines(LPDP_Graphics.Connections[Connection_Index].Path);
            Canvas.DrawPath(NewPen, NewPath);

            NewPen.Dispose();
            NewPath.Dispose();

            return Canvas;
        }

        public static int MouseInGraphicObject(Point position)
        {
            for (int i = 0; i < GraphicModel.Count; i++)
            {
                if ((position.X >= GraphicModel[i].positionX) &&
                    (position.Y >= GraphicModel[i].positionY) &&
                    (position.X <= GraphicModel[i].positionX + GraphicModel[i].ObjWidth) &&
                    (position.Y <= GraphicModel[i].positionY + GraphicModel[i].ObjHeight))
                    return i;
            }
            return -1;
        }

    }
}
