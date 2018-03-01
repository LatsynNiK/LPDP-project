using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SLT
{
    public class OutputData
    {
        public string CodeTxt;
        public string CodeRtf;
        public string InfoTxt;

        public DataTable Objects;
        public DataTable Initiators;
        public DataTable Queues;
        public DataTable FTT;
        public DataTable CT;

        public DataTable QueueArrows;
        public DataTable TextSelections;
        public DataTable HiddenLabel;

        public double TIME;
        public bool ModelIsBuilt;

        public int UnitPosition;

        public int InitiatorNumber;

        public OutputData()
        {
            this.Objects = new DataTable("Objects");
            this.Initiators = new DataTable("Initiators");
            this.Queues = new DataTable("Queues");
            this.FTT = new DataTable("FTT");
            this.CT = new DataTable("CT");

            this.QueueArrows = new DataTable("QueueArrows");
            this.TextSelections = new DataTable("TextSelection");
            this.HiddenLabel = new DataTable("HiddenLabel");

            this.CreateTable(this.Objects, "Unit", "Name", "Value", "Type");
            this.CreateTable(this.Initiators, "Number", "Name", "Value", "Type");
            this.CreateTable(this.Queues, "Unit", "Label", "Initiators");
            this.CreateTable(this.FTT, "Time", "Initiator", "Label", "Unit");
            this.CreateTable(this.CT, "Condition", "Initiator", "Label", "Unit");

            this.CreateTable(this.QueueArrows, "Position", "First", "Second", "Third");
            this.CreateTable(this.TextSelections, "Start", "Length", "Type");
            this.CreateTable(this.HiddenLabel, "Name", "Position");

            this.RenameTable(this.Objects, "Блок", "Объект", "Значение", "Тип");
            this.RenameTable(this.Initiators, "Номер", "Инициатор", "Значение", "Тип");
            this.RenameTable(this.Queues, "Блок", "Метка", "Инициаторы");
            this.RenameTable(this.FTT, "Время", "Инициатор", "Метка", "Блок");
            this.RenameTable(this.CT, "Условие", "Инициатор", "Метка", "Блок");           
        }

        void CreateTable(DataTable table, params string[] ColumnNames)
        {
            foreach (string colname in ColumnNames)
            {
                table.Columns.Add(colname);
            }
        }

        void RenameTable(DataTable table, params string[] ColumnNames)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].Caption = ColumnNames[i];
            }
        }


    }
}
