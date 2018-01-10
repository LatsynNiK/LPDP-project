using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using LPDP;
using LPDP.DataSets;
using LPDP.Structure;
//нужно скрыть!
//using LPDP.Structure;

namespace ПОСП
{
    public partial class POSP_Form : Form
    {
        bool ModelTextIsModified = false;
        int precision = 2;

        //InputOutputData DataSets;

        InputData Input;
        OutputData Output;
        Model ExploredModel;


        public POSP_Form()
        {
            InitializeComponent();
            LPDP.TextAnalysis.ModelTextRules.SetRules();
            this.Input = new InputData(precision);
            //this.DataSets = new InputOutputData(precision);
            //UseAllCaptions();
            //Initiators_Tab.Parent = null;
            //Queues_Tab.Parent = null;
            GraphicModel_Tab.Parent = null;
        }
        private void POSP_Form_Load(object sender, EventArgs e)
        {
            
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSP_Form));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.построениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запускToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запускПоВремениToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вводВремениToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.выполнениеКОСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.шагToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.стопToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.окнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.объектыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инициаторыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблицаБудущихВременToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблицаУсловийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очереди_окноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отображенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.следующийОператорToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очередиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.системныеМеткиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.путьКФайлуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.TopContainer = new System.Windows.Forms.SplitContainer();
            this.FilePath_label = new System.Windows.Forms.Label();
            this.CodeField = new System.Windows.Forms.RichTextBox();
            this.General_Indicators = new System.Windows.Forms.Panel();
            this.INITIATOR_Label = new System.Windows.Forms.Label();
            this.INITIATOR_Value = new System.Windows.Forms.Label();
            this.TIME_Value = new System.Windows.Forms.Label();
            this.TIME_Lable = new System.Windows.Forms.Label();
            this.ResultField = new System.Windows.Forms.TabControl();
            this.Objects_Tab = new System.Windows.Forms.TabPage();
            this.Objects_View = new System.Windows.Forms.DataGridView();
            this.Initiators_Tab = new System.Windows.Forms.TabPage();
            this.Initiators_Tab_Container = new System.Windows.Forms.SplitContainer();
            this.Initiators_View = new System.Windows.Forms.DataGridView();
            this.Search_by_ID_initiator_splitContainer = new System.Windows.Forms.SplitContainer();
            this.Search_by_ID_initiator_textBox = new System.Windows.Forms.TextBox();
            this.Search_by_ID_initiator_button = new System.Windows.Forms.Button();
            this.Search_by_ID_initiator_label = new System.Windows.Forms.Label();
            this.CT_Tab = new System.Windows.Forms.TabPage();
            this.CT_View = new System.Windows.Forms.DataGridView();
            this.FTT_Tab = new System.Windows.Forms.TabPage();
            this.FTT_View = new System.Windows.Forms.DataGridView();
            this.Queues_Tab = new System.Windows.Forms.TabPage();
            this.Queues_Tab_Container = new System.Windows.Forms.SplitContainer();
            this.Queues_View = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.Search_initiator_in_queues_textBox = new System.Windows.Forms.TextBox();
            this.Search_initiator_in_queues_button = new System.Windows.Forms.Button();
            this.Search_initiator_in_queues_label = new System.Windows.Forms.Label();
            this.GraphicModel_Tab = new System.Windows.Forms.TabPage();
            this.GraphicModel_Panel = new System.Windows.Forms.Panel();
            this.GraphicModel_View = new System.Windows.Forms.PictureBox();
            this.BuildingField = new System.Windows.Forms.RichTextBox();
            this.FileName_label = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.TopContainer.Panel1.SuspendLayout();
            this.TopContainer.Panel2.SuspendLayout();
            this.TopContainer.SuspendLayout();
            this.General_Indicators.SuspendLayout();
            this.ResultField.SuspendLayout();
            this.Objects_Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Objects_View)).BeginInit();
            this.Initiators_Tab.SuspendLayout();
            this.Initiators_Tab_Container.Panel1.SuspendLayout();
            this.Initiators_Tab_Container.Panel2.SuspendLayout();
            this.Initiators_Tab_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Initiators_View)).BeginInit();
            this.Search_by_ID_initiator_splitContainer.Panel1.SuspendLayout();
            this.Search_by_ID_initiator_splitContainer.Panel2.SuspendLayout();
            this.Search_by_ID_initiator_splitContainer.SuspendLayout();
            this.CT_Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CT_View)).BeginInit();
            this.FTT_Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FTT_View)).BeginInit();
            this.Queues_Tab.SuspendLayout();
            this.Queues_Tab_Container.Panel1.SuspendLayout();
            this.Queues_Tab_Container.Panel2.SuspendLayout();
            this.Queues_Tab_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Queues_View)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.GraphicModel_Tab.SuspendLayout();
            this.GraphicModel_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GraphicModel_View)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.построениеToolStripMenuItem,
            this.запускToolStripMenuItem,
            this.стопToolStripMenuItem,
            this.окнаToolStripMenuItem,
            this.отображенияToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(892, 29);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 25);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как ...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // построениеToolStripMenuItem
            // 
            this.построениеToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.построениеToolStripMenuItem.Name = "построениеToolStripMenuItem";
            this.построениеToolStripMenuItem.ShortcutKeyDisplayString = "F5";
            this.построениеToolStripMenuItem.Size = new System.Drawing.Size(108, 25);
            this.построениеToolStripMenuItem.Text = "Построение";
            this.построениеToolStripMenuItem.Click += new System.EventHandler(this.построениеToolStripMenuItem_Click);
            // 
            // запускToolStripMenuItem
            // 
            this.запускToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запускПоВремениToolStripMenuItem,
            this.выполнениеКОСToolStripMenuItem,
            this.шагToolStripMenuItem});
            this.запускToolStripMenuItem.Enabled = false;
            this.запускToolStripMenuItem.Name = "запускToolStripMenuItem";
            this.запускToolStripMenuItem.Size = new System.Drawing.Size(71, 25);
            this.запускToolStripMenuItem.Text = "Запуск";
            this.запускToolStripMenuItem.DropDownClosed += new System.EventHandler(this.запускToolStripMenuItem_DropDownClosed);
            // 
            // запускПоВремениToolStripMenuItem
            // 
            this.запускПоВремениToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вводВремениToolStripMenuItem});
            this.запускПоВремениToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.запускПоВремениToolStripMenuItem.Name = "запускПоВремениToolStripMenuItem";
            this.запускПоВремениToolStripMenuItem.ShortcutKeyDisplayString = "F5";
            this.запускПоВремениToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.запускПоВремениToolStripMenuItem.Text = "Запуск по времени...";
            this.запускПоВремениToolStripMenuItem.Click += new System.EventHandler(this.запускПоВремениToolStripMenuItem_Click);
            // 
            // вводВремениToolStripMenuItem
            // 
            this.вводВремениToolStripMenuItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.вводВремениToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.вводВремениToolStripMenuItem.Name = "вводВремениToolStripMenuItem";
            this.вводВремениToolStripMenuItem.Size = new System.Drawing.Size(100, 25);
            this.вводВремениToolStripMenuItem.Visible = false;
            this.вводВремениToolStripMenuItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.вводВремениToolStripMenuItem_KeyDown);
            // 
            // выполнениеКОСToolStripMenuItem
            // 
            this.выполнениеКОСToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.выполнениеКОСToolStripMenuItem.Name = "выполнениеКОСToolStripMenuItem";
            this.выполнениеКОСToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            this.выполнениеКОСToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.выполнениеКОСToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.выполнениеКОСToolStripMenuItem.Text = "Выполнение КОС";
            this.выполнениеКОСToolStripMenuItem.Click += new System.EventHandler(this.выполнениеКОСToolStripMenuItem_Click);
            // 
            // шагToolStripMenuItem
            // 
            this.шагToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.шагToolStripMenuItem.Name = "шагToolStripMenuItem";
            this.шагToolStripMenuItem.ShortcutKeyDisplayString = "F11";
            this.шагToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.шагToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.шагToolStripMenuItem.Text = "Шаг";
            this.шагToolStripMenuItem.Click += new System.EventHandler(this.шагToolStripMenuItem_Click);
            // 
            // стопToolStripMenuItem
            // 
            this.стопToolStripMenuItem.Enabled = false;
            this.стопToolStripMenuItem.Name = "стопToolStripMenuItem";
            this.стопToolStripMenuItem.Size = new System.Drawing.Size(57, 25);
            this.стопToolStripMenuItem.Text = "Стоп";
            this.стопToolStripMenuItem.Click += new System.EventHandler(this.стопToolStripMenuItem_Click);
            // 
            // окнаToolStripMenuItem
            // 
            this.окнаToolStripMenuItem.CheckOnClick = true;
            this.окнаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.объектыToolStripMenuItem,
            this.инициаторыToolStripMenuItem,
            this.таблицаБудущихВременToolStripMenuItem,
            this.таблицаУсловийToolStripMenuItem,
            this.очереди_окноToolStripMenuItem});
            this.окнаToolStripMenuItem.Name = "окнаToolStripMenuItem";
            this.окнаToolStripMenuItem.Size = new System.Drawing.Size(59, 25);
            this.окнаToolStripMenuItem.Text = "Окна";
            // 
            // объектыToolStripMenuItem
            // 
            this.объектыToolStripMenuItem.Checked = true;
            this.объектыToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.объектыToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.объектыToolStripMenuItem.Name = "объектыToolStripMenuItem";
            this.объектыToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.объектыToolStripMenuItem.Text = "Объекты";
            this.объектыToolStripMenuItem.Click += new System.EventHandler(this.объектыToolStripMenuItem_Click);
            // 
            // инициаторыToolStripMenuItem
            // 
            this.инициаторыToolStripMenuItem.Checked = true;
            this.инициаторыToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.инициаторыToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.инициаторыToolStripMenuItem.Name = "инициаторыToolStripMenuItem";
            this.инициаторыToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.инициаторыToolStripMenuItem.Text = "Инициаторы";
            this.инициаторыToolStripMenuItem.Click += new System.EventHandler(this.инициаторыToolStripMenuItem_Click);
            // 
            // таблицаБудущихВременToolStripMenuItem
            // 
            this.таблицаБудущихВременToolStripMenuItem.Checked = true;
            this.таблицаБудущихВременToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.таблицаБудущихВременToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.таблицаБудущихВременToolStripMenuItem.Name = "таблицаБудущихВременToolStripMenuItem";
            this.таблицаБудущихВременToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.таблицаБудущихВременToolStripMenuItem.Text = "Таблица Будущих Времен";
            this.таблицаБудущихВременToolStripMenuItem.Click += new System.EventHandler(this.таблицаБудущихВременToolStripMenuItem_Click);
            // 
            // таблицаУсловийToolStripMenuItem
            // 
            this.таблицаУсловийToolStripMenuItem.Checked = true;
            this.таблицаУсловийToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.таблицаУсловийToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.таблицаУсловийToolStripMenuItem.Name = "таблицаУсловийToolStripMenuItem";
            this.таблицаУсловийToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.таблицаУсловийToolStripMenuItem.Text = "Таблица Условий";
            this.таблицаУсловийToolStripMenuItem.Click += new System.EventHandler(this.таблицаУсловийToolStripMenuItem_Click);
            // 
            // очереди_окноToolStripMenuItem
            // 
            this.очереди_окноToolStripMenuItem.Checked = true;
            this.очереди_окноToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.очереди_окноToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.очереди_окноToolStripMenuItem.Name = "очереди_окноToolStripMenuItem";
            this.очереди_окноToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.очереди_окноToolStripMenuItem.Text = "Очереди";
            this.очереди_окноToolStripMenuItem.Click += new System.EventHandler(this.очереди_окноToolStripMenuItem_Click);
            // 
            // отображенияToolStripMenuItem
            // 
            this.отображенияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.следующийОператорToolStripMenuItem,
            this.очередиToolStripMenuItem,
            this.системныеМеткиToolStripMenuItem,
            this.путьКФайлуToolStripMenuItem});
            this.отображенияToolStripMenuItem.Name = "отображенияToolStripMenuItem";
            this.отображенияToolStripMenuItem.Size = new System.Drawing.Size(122, 25);
            this.отображенияToolStripMenuItem.Text = "Отображения";
            this.отображенияToolStripMenuItem.Click += new System.EventHandler(this.отображенияToolStripMenuItem_Click);
            // 
            // следующийОператорToolStripMenuItem
            // 
            this.следующийОператорToolStripMenuItem.Checked = true;
            this.следующийОператорToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.следующийОператорToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.следующийОператорToolStripMenuItem.Name = "следующийОператорToolStripMenuItem";
            this.следующийОператорToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            this.следующийОператорToolStripMenuItem.Text = "Следующий оператор";
            this.следующийОператорToolStripMenuItem.Click += new System.EventHandler(this.следующийОператорToolStripMenuItem_Click);
            // 
            // очередиToolStripMenuItem
            // 
            this.очередиToolStripMenuItem.Checked = true;
            this.очередиToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.очередиToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.очередиToolStripMenuItem.Name = "очередиToolStripMenuItem";
            this.очередиToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            this.очередиToolStripMenuItem.Text = "Очереди";
            this.очередиToolStripMenuItem.Click += new System.EventHandler(this.очередиToolStripMenuItem_Click);
            // 
            // системныеМеткиToolStripMenuItem
            // 
            this.системныеМеткиToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.системныеМеткиToolStripMenuItem.Name = "системныеМеткиToolStripMenuItem";
            this.системныеМеткиToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            this.системныеМеткиToolStripMenuItem.Text = "Системные метки";
            this.системныеМеткиToolStripMenuItem.Click += new System.EventHandler(this.системныеМеткиToolStripMenuItem_Click);
            // 
            // путьКФайлуToolStripMenuItem
            // 
            this.путьКФайлуToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.путьКФайлуToolStripMenuItem.Name = "путьКФайлуToolStripMenuItem";
            this.путьКФайлуToolStripMenuItem.Size = new System.Drawing.Size(220, 24);
            this.путьКФайлуToolStripMenuItem.Text = "Путь к файлу";
            this.путьКФайлуToolStripMenuItem.Click += new System.EventHandler(this.путьКФайлуToolStripMenuItem_Click);
            // 
            // MainContainer
            // 
            this.MainContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainContainer.Location = new System.Drawing.Point(12, 45);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.TopContainer);
            this.MainContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.BuildingField);
            this.MainContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MainContainer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MainContainer.Size = new System.Drawing.Size(868, 527);
            this.MainContainer.SplitterDistance = 422;
            this.MainContainer.TabIndex = 2;
            // 
            // TopContainer
            // 
            this.TopContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TopContainer.Location = new System.Drawing.Point(0, 0);
            this.TopContainer.Name = "TopContainer";
            // 
            // TopContainer.Panel1
            // 
            this.TopContainer.Panel1.AutoScroll = true;
            this.TopContainer.Panel1.Controls.Add(this.FilePath_label);
            this.TopContainer.Panel1.Controls.Add(this.CodeField);
            // 
            // TopContainer.Panel2
            // 
            this.TopContainer.Panel2.Controls.Add(this.General_Indicators);
            this.TopContainer.Panel2.Controls.Add(this.ResultField);
            this.TopContainer.Size = new System.Drawing.Size(868, 422);
            this.TopContainer.SplitterDistance = 551;
            this.TopContainer.TabIndex = 0;
            // 
            // FilePath_label
            // 
            this.FilePath_label.AutoSize = true;
            this.FilePath_label.Dock = System.Windows.Forms.DockStyle.Right;
            this.FilePath_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FilePath_label.Location = new System.Drawing.Point(551, 0);
            this.FilePath_label.Name = "FilePath_label";
            this.FilePath_label.Size = new System.Drawing.Size(0, 17);
            this.FilePath_label.TabIndex = 4;
            this.FilePath_label.Visible = false;
            // 
            // CodeField
            // 
            this.CodeField.AcceptsTab = true;
            this.CodeField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CodeField.EnableAutoDragDrop = true;
            this.CodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CodeField.Location = new System.Drawing.Point(0, 16);
            this.CodeField.Name = "CodeField";
            this.CodeField.Size = new System.Drawing.Size(551, 406);
            this.CodeField.TabIndex = 0;
            this.CodeField.Text = "";
            this.CodeField.WordWrap = false;
            // 
            // General_Indicators
            // 
            this.General_Indicators.Controls.Add(this.INITIATOR_Label);
            this.General_Indicators.Controls.Add(this.INITIATOR_Value);
            this.General_Indicators.Controls.Add(this.TIME_Value);
            this.General_Indicators.Controls.Add(this.TIME_Lable);
            this.General_Indicators.Dock = System.Windows.Forms.DockStyle.Top;
            this.General_Indicators.Location = new System.Drawing.Point(0, 0);
            this.General_Indicators.Name = "General_Indicators";
            this.General_Indicators.Size = new System.Drawing.Size(313, 24);
            this.General_Indicators.TabIndex = 1;
            // 
            // INITIATOR_Label
            // 
            this.INITIATOR_Label.AutoSize = true;
            this.INITIATOR_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.INITIATOR_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.INITIATOR_Label.Location = new System.Drawing.Point(160, 0);
            this.INITIATOR_Label.Name = "INITIATOR_Label";
            this.INITIATOR_Label.Size = new System.Drawing.Size(129, 26);
            this.INITIATOR_Label.TabIndex = 3;
            this.INITIATOR_Label.Text = "Инициатор:";
            // 
            // INITIATOR_Value
            // 
            this.INITIATOR_Value.AutoSize = true;
            this.INITIATOR_Value.Dock = System.Windows.Forms.DockStyle.Right;
            this.INITIATOR_Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.INITIATOR_Value.Location = new System.Drawing.Point(289, 0);
            this.INITIATOR_Value.Name = "INITIATOR_Value";
            this.INITIATOR_Value.Size = new System.Drawing.Size(24, 26);
            this.INITIATOR_Value.TabIndex = 5;
            this.INITIATOR_Value.Text = "0";
            // 
            // TIME_Value
            // 
            this.TIME_Value.AutoSize = true;
            this.TIME_Value.Dock = System.Windows.Forms.DockStyle.Left;
            this.TIME_Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TIME_Value.Location = new System.Drawing.Point(90, 0);
            this.TIME_Value.Name = "TIME_Value";
            this.TIME_Value.Size = new System.Drawing.Size(24, 26);
            this.TIME_Value.TabIndex = 2;
            this.TIME_Value.Text = "0";
            // 
            // TIME_Lable
            // 
            this.TIME_Lable.AutoSize = true;
            this.TIME_Lable.Dock = System.Windows.Forms.DockStyle.Left;
            this.TIME_Lable.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TIME_Lable.Location = new System.Drawing.Point(0, 0);
            this.TIME_Lable.Name = "TIME_Lable";
            this.TIME_Lable.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.TIME_Lable.Size = new System.Drawing.Size(90, 26);
            this.TIME_Lable.TabIndex = 1;
            this.TIME_Lable.Text = "Время:";
            // 
            // ResultField
            // 
            this.ResultField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultField.Controls.Add(this.Objects_Tab);
            this.ResultField.Controls.Add(this.Initiators_Tab);
            this.ResultField.Controls.Add(this.CT_Tab);
            this.ResultField.Controls.Add(this.FTT_Tab);
            this.ResultField.Controls.Add(this.Queues_Tab);
            this.ResultField.Controls.Add(this.GraphicModel_Tab);
            this.ResultField.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ResultField.Location = new System.Drawing.Point(0, 43);
            this.ResultField.Name = "ResultField";
            this.ResultField.SelectedIndex = 0;
            this.ResultField.Size = new System.Drawing.Size(313, 379);
            this.ResultField.TabIndex = 0;
            // 
            // Objects_Tab
            // 
            this.Objects_Tab.Controls.Add(this.Objects_View);
            this.Objects_Tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Objects_Tab.Location = new System.Drawing.Point(4, 25);
            this.Objects_Tab.Name = "Objects_Tab";
            this.Objects_Tab.Size = new System.Drawing.Size(305, 350);
            this.Objects_Tab.TabIndex = 3;
            this.Objects_Tab.Text = "Объекты";
            this.Objects_Tab.UseVisualStyleBackColor = true;
            // 
            // Objects_View
            // 
            this.Objects_View.AllowUserToAddRows = false;
            this.Objects_View.AllowUserToDeleteRows = false;
            this.Objects_View.AllowUserToResizeRows = false;
            this.Objects_View.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Objects_View.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Objects_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Objects_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Objects_View.Location = new System.Drawing.Point(0, 0);
            this.Objects_View.MultiSelect = false;
            this.Objects_View.Name = "Objects_View";
            this.Objects_View.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.Objects_View.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            this.Objects_View.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Objects_View.Size = new System.Drawing.Size(305, 350);
            this.Objects_View.TabIndex = 0;
            this.Objects_View.SelectionChanged += new System.EventHandler(this.Objects_View_SelectionChanged);
            this.Objects_View.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Objects_View_MouseDoubleClick);
            // 
            // Initiators_Tab
            // 
            this.Initiators_Tab.Controls.Add(this.Initiators_Tab_Container);
            this.Initiators_Tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Initiators_Tab.Location = new System.Drawing.Point(4, 25);
            this.Initiators_Tab.Name = "Initiators_Tab";
            this.Initiators_Tab.Size = new System.Drawing.Size(305, 350);
            this.Initiators_Tab.TabIndex = 4;
            this.Initiators_Tab.Text = "Инициаторы";
            this.Initiators_Tab.UseVisualStyleBackColor = true;
            // 
            // Initiators_Tab_Container
            // 
            this.Initiators_Tab_Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Initiators_Tab_Container.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Initiators_Tab_Container.Location = new System.Drawing.Point(0, 0);
            this.Initiators_Tab_Container.Name = "Initiators_Tab_Container";
            this.Initiators_Tab_Container.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Initiators_Tab_Container.Panel1
            // 
            this.Initiators_Tab_Container.Panel1.Controls.Add(this.Initiators_View);
            this.Initiators_Tab_Container.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // Initiators_Tab_Container.Panel2
            // 
            this.Initiators_Tab_Container.Panel2.Controls.Add(this.Search_by_ID_initiator_splitContainer);
            this.Initiators_Tab_Container.Panel2.Controls.Add(this.Search_by_ID_initiator_label);
            this.Initiators_Tab_Container.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Initiators_Tab_Container.Size = new System.Drawing.Size(305, 350);
            this.Initiators_Tab_Container.SplitterDistance = 263;
            this.Initiators_Tab_Container.TabIndex = 2;
            // 
            // Initiators_View
            // 
            this.Initiators_View.AllowUserToAddRows = false;
            this.Initiators_View.AllowUserToDeleteRows = false;
            this.Initiators_View.AllowUserToResizeRows = false;
            this.Initiators_View.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Initiators_View.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Initiators_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Initiators_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Initiators_View.Location = new System.Drawing.Point(0, 0);
            this.Initiators_View.MultiSelect = false;
            this.Initiators_View.Name = "Initiators_View";
            this.Initiators_View.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.Initiators_View.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            this.Initiators_View.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Initiators_View.Size = new System.Drawing.Size(305, 263);
            this.Initiators_View.TabIndex = 1;
            this.Initiators_View.SelectionChanged += new System.EventHandler(this.Initiators_View_SelectionChanged);
            this.Initiators_View.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Initiators_View_MouseDoubleClick);
            // 
            // Search_by_ID_initiator_splitContainer
            // 
            this.Search_by_ID_initiator_splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Search_by_ID_initiator_splitContainer.Location = new System.Drawing.Point(15, 41);
            this.Search_by_ID_initiator_splitContainer.Name = "Search_by_ID_initiator_splitContainer";
            // 
            // Search_by_ID_initiator_splitContainer.Panel1
            // 
            this.Search_by_ID_initiator_splitContainer.Panel1.Controls.Add(this.Search_by_ID_initiator_textBox);
            // 
            // Search_by_ID_initiator_splitContainer.Panel2
            // 
            this.Search_by_ID_initiator_splitContainer.Panel2.Controls.Add(this.Search_by_ID_initiator_button);
            this.Search_by_ID_initiator_splitContainer.Size = new System.Drawing.Size(278, 30);
            this.Search_by_ID_initiator_splitContainer.SplitterDistance = 178;
            this.Search_by_ID_initiator_splitContainer.TabIndex = 3;
            // 
            // Search_by_ID_initiator_textBox
            // 
            this.Search_by_ID_initiator_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Search_by_ID_initiator_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Search_by_ID_initiator_textBox.Location = new System.Drawing.Point(0, 0);
            this.Search_by_ID_initiator_textBox.Name = "Search_by_ID_initiator_textBox";
            this.Search_by_ID_initiator_textBox.Size = new System.Drawing.Size(178, 29);
            this.Search_by_ID_initiator_textBox.TabIndex = 0;
            this.Search_by_ID_initiator_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_by_ID_initiator_textBox_KeyDown);
            // 
            // Search_by_ID_initiator_button
            // 
            this.Search_by_ID_initiator_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Search_by_ID_initiator_button.Location = new System.Drawing.Point(0, 0);
            this.Search_by_ID_initiator_button.Name = "Search_by_ID_initiator_button";
            this.Search_by_ID_initiator_button.Size = new System.Drawing.Size(96, 30);
            this.Search_by_ID_initiator_button.TabIndex = 0;
            this.Search_by_ID_initiator_button.Text = "Искать";
            this.Search_by_ID_initiator_button.UseVisualStyleBackColor = true;
            this.Search_by_ID_initiator_button.Click += new System.EventHandler(this.Search_by_ID_initiator_button_Click);
            // 
            // Search_by_ID_initiator_label
            // 
            this.Search_by_ID_initiator_label.AutoSize = true;
            this.Search_by_ID_initiator_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Search_by_ID_initiator_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Search_by_ID_initiator_label.Location = new System.Drawing.Point(0, 0);
            this.Search_by_ID_initiator_label.Name = "Search_by_ID_initiator_label";
            this.Search_by_ID_initiator_label.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.Search_by_ID_initiator_label.Size = new System.Drawing.Size(245, 24);
            this.Search_by_ID_initiator_label.TabIndex = 2;
            this.Search_by_ID_initiator_label.Text = "Поиск инициатора по ID";
            // 
            // CT_Tab
            // 
            this.CT_Tab.AllowDrop = true;
            this.CT_Tab.Controls.Add(this.CT_View);
            this.CT_Tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CT_Tab.Location = new System.Drawing.Point(4, 25);
            this.CT_Tab.Name = "CT_Tab";
            this.CT_Tab.Size = new System.Drawing.Size(305, 350);
            this.CT_Tab.TabIndex = 1;
            this.CT_Tab.Text = "ТУ";
            this.CT_Tab.UseVisualStyleBackColor = true;
            // 
            // CT_View
            // 
            this.CT_View.AllowUserToAddRows = false;
            this.CT_View.AllowUserToDeleteRows = false;
            this.CT_View.AllowUserToResizeRows = false;
            this.CT_View.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CT_View.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CT_View.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CT_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CT_View.DefaultCellStyle = dataGridViewCellStyle2;
            this.CT_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CT_View.Location = new System.Drawing.Point(0, 0);
            this.CT_View.MultiSelect = false;
            this.CT_View.Name = "CT_View";
            dataGridViewCellStyle3.NullValue = null;
            this.CT_View.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.CT_View.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.CT_View.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            this.CT_View.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.CT_View.Size = new System.Drawing.Size(305, 350);
            this.CT_View.TabIndex = 1;
            this.CT_View.SelectionChanged += new System.EventHandler(this.CT_View_SelectionChanged);
            // 
            // FTT_Tab
            // 
            this.FTT_Tab.AllowDrop = true;
            this.FTT_Tab.Controls.Add(this.FTT_View);
            this.FTT_Tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FTT_Tab.Location = new System.Drawing.Point(4, 25);
            this.FTT_Tab.Name = "FTT_Tab";
            this.FTT_Tab.Size = new System.Drawing.Size(305, 350);
            this.FTT_Tab.TabIndex = 0;
            this.FTT_Tab.Text = "ТБВ";
            this.FTT_Tab.UseVisualStyleBackColor = true;
            // 
            // FTT_View
            // 
            this.FTT_View.AllowUserToAddRows = false;
            this.FTT_View.AllowUserToDeleteRows = false;
            this.FTT_View.AllowUserToResizeRows = false;
            this.FTT_View.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FTT_View.BackgroundColor = System.Drawing.SystemColors.Window;
            this.FTT_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.FTT_View.DefaultCellStyle = dataGridViewCellStyle4;
            this.FTT_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FTT_View.Location = new System.Drawing.Point(0, 0);
            this.FTT_View.MultiSelect = false;
            this.FTT_View.Name = "FTT_View";
            dataGridViewCellStyle5.NullValue = null;
            this.FTT_View.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.FTT_View.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.FTT_View.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            this.FTT_View.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.FTT_View.Size = new System.Drawing.Size(305, 350);
            this.FTT_View.TabIndex = 0;
            this.FTT_View.SelectionChanged += new System.EventHandler(this.FTT_View_SelectionChanged);
            // 
            // Queues_Tab
            // 
            this.Queues_Tab.Controls.Add(this.Queues_Tab_Container);
            this.Queues_Tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Queues_Tab.Location = new System.Drawing.Point(4, 25);
            this.Queues_Tab.Name = "Queues_Tab";
            this.Queues_Tab.Size = new System.Drawing.Size(305, 350);
            this.Queues_Tab.TabIndex = 5;
            this.Queues_Tab.Text = "Очереди";
            this.Queues_Tab.UseVisualStyleBackColor = true;
            // 
            // Queues_Tab_Container
            // 
            this.Queues_Tab_Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Queues_Tab_Container.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Queues_Tab_Container.Location = new System.Drawing.Point(0, 0);
            this.Queues_Tab_Container.Name = "Queues_Tab_Container";
            this.Queues_Tab_Container.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Queues_Tab_Container.Panel1
            // 
            this.Queues_Tab_Container.Panel1.Controls.Add(this.Queues_View);
            this.Queues_Tab_Container.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // Queues_Tab_Container.Panel2
            // 
            this.Queues_Tab_Container.Panel2.Controls.Add(this.splitContainer2);
            this.Queues_Tab_Container.Panel2.Controls.Add(this.Search_initiator_in_queues_label);
            this.Queues_Tab_Container.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Queues_Tab_Container.Size = new System.Drawing.Size(305, 350);
            this.Queues_Tab_Container.SplitterDistance = 263;
            this.Queues_Tab_Container.TabIndex = 3;
            // 
            // Queues_View
            // 
            this.Queues_View.AllowUserToAddRows = false;
            this.Queues_View.AllowUserToDeleteRows = false;
            this.Queues_View.AllowUserToResizeRows = false;
            this.Queues_View.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Queues_View.BackgroundColor = System.Drawing.SystemColors.Window;
            this.Queues_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Queues_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Queues_View.Location = new System.Drawing.Point(0, 0);
            this.Queues_View.MultiSelect = false;
            this.Queues_View.Name = "Queues_View";
            this.Queues_View.RowHeadersWidth = 15;
            this.Queues_View.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
            this.Queues_View.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Transparent;
            this.Queues_View.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.Queues_View.Size = new System.Drawing.Size(305, 263);
            this.Queues_View.TabIndex = 1;
            this.Queues_View.SelectionChanged += new System.EventHandler(this.Queues_View_SelectionChanged);
            this.Queues_View.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Queues_View_MouseDoubleClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(15, 41);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.Search_initiator_in_queues_textBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.Search_initiator_in_queues_button);
            this.splitContainer2.Size = new System.Drawing.Size(278, 30);
            this.splitContainer2.SplitterDistance = 178;
            this.splitContainer2.TabIndex = 3;
            // 
            // Search_initiator_in_queues_textBox
            // 
            this.Search_initiator_in_queues_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Search_initiator_in_queues_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Search_initiator_in_queues_textBox.Location = new System.Drawing.Point(0, 0);
            this.Search_initiator_in_queues_textBox.Name = "Search_initiator_in_queues_textBox";
            this.Search_initiator_in_queues_textBox.Size = new System.Drawing.Size(178, 29);
            this.Search_initiator_in_queues_textBox.TabIndex = 0;
            this.Search_initiator_in_queues_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_initiator_in_queues_textBox_KeyDown);
            // 
            // Search_initiator_in_queues_button
            // 
            this.Search_initiator_in_queues_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Search_initiator_in_queues_button.Location = new System.Drawing.Point(0, 0);
            this.Search_initiator_in_queues_button.Name = "Search_initiator_in_queues_button";
            this.Search_initiator_in_queues_button.Size = new System.Drawing.Size(96, 30);
            this.Search_initiator_in_queues_button.TabIndex = 0;
            this.Search_initiator_in_queues_button.Text = "Искать";
            this.Search_initiator_in_queues_button.UseVisualStyleBackColor = true;
            // 
            // Search_initiator_in_queues_label
            // 
            this.Search_initiator_in_queues_label.AutoSize = true;
            this.Search_initiator_in_queues_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Search_initiator_in_queues_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Search_initiator_in_queues_label.Location = new System.Drawing.Point(0, 0);
            this.Search_initiator_in_queues_label.Name = "Search_initiator_in_queues_label";
            this.Search_initiator_in_queues_label.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.Search_initiator_in_queues_label.Size = new System.Drawing.Size(294, 24);
            this.Search_initiator_in_queues_label.TabIndex = 2;
            this.Search_initiator_in_queues_label.Text = "Поиск инициатора в очереди";
            // 
            // GraphicModel_Tab
            // 
            this.GraphicModel_Tab.AllowDrop = true;
            this.GraphicModel_Tab.Controls.Add(this.GraphicModel_Panel);
            this.GraphicModel_Tab.Location = new System.Drawing.Point(4, 25);
            this.GraphicModel_Tab.Name = "GraphicModel_Tab";
            this.GraphicModel_Tab.Size = new System.Drawing.Size(305, 350);
            this.GraphicModel_Tab.TabIndex = 2;
            this.GraphicModel_Tab.Text = "Графическая модель";
            this.GraphicModel_Tab.UseVisualStyleBackColor = true;
            // 
            // GraphicModel_Panel
            // 
            this.GraphicModel_Panel.AutoScroll = true;
            this.GraphicModel_Panel.Controls.Add(this.GraphicModel_View);
            this.GraphicModel_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphicModel_Panel.Location = new System.Drawing.Point(0, 0);
            this.GraphicModel_Panel.Name = "GraphicModel_Panel";
            this.GraphicModel_Panel.Size = new System.Drawing.Size(305, 350);
            this.GraphicModel_Panel.TabIndex = 0;
            // 
            // GraphicModel_View
            // 
            this.GraphicModel_View.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GraphicModel_View.Location = new System.Drawing.Point(0, 0);
            this.GraphicModel_View.MinimumSize = new System.Drawing.Size(590, 330);
            this.GraphicModel_View.Name = "GraphicModel_View";
            this.GraphicModel_View.Size = new System.Drawing.Size(590, 330);
            this.GraphicModel_View.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.GraphicModel_View.TabIndex = 0;
            this.GraphicModel_View.TabStop = false;
            // 
            // BuildingField
            // 
            this.BuildingField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BuildingField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BuildingField.Location = new System.Drawing.Point(0, 0);
            this.BuildingField.Name = "BuildingField";
            this.BuildingField.ReadOnly = true;
            this.BuildingField.Size = new System.Drawing.Size(868, 101);
            this.BuildingField.TabIndex = 0;
            this.BuildingField.Text = "";
            // 
            // FileName_label
            // 
            this.FileName_label.AutoSize = true;
            this.FileName_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FileName_label.Location = new System.Drawing.Point(12, 41);
            this.FileName_label.Name = "FileName_label";
            this.FileName_label.Size = new System.Drawing.Size(101, 17);
            this.FileName_label.TabIndex = 3;
            this.FileName_label.Text = "Новая модель";
            // 
            // POSP_Form
            // 
            this.ClientSize = new System.Drawing.Size(892, 584);
            this.Controls.Add(this.FileName_label);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "POSP_Form";
            this.Text = "\"Процессы\"";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POSP_Form_FormClosing);
            this.Load += new System.EventHandler(this.POSP_Form_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.POSP_Form_KeyDown);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            this.MainContainer.ResumeLayout(false);
            this.TopContainer.Panel1.ResumeLayout(false);
            this.TopContainer.Panel1.PerformLayout();
            this.TopContainer.Panel2.ResumeLayout(false);
            this.TopContainer.ResumeLayout(false);
            this.General_Indicators.ResumeLayout(false);
            this.General_Indicators.PerformLayout();
            this.ResultField.ResumeLayout(false);
            this.Objects_Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Objects_View)).EndInit();
            this.Initiators_Tab.ResumeLayout(false);
            this.Initiators_Tab_Container.Panel1.ResumeLayout(false);
            this.Initiators_Tab_Container.Panel2.ResumeLayout(false);
            this.Initiators_Tab_Container.Panel2.PerformLayout();
            this.Initiators_Tab_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Initiators_View)).EndInit();
            this.Search_by_ID_initiator_splitContainer.Panel1.ResumeLayout(false);
            this.Search_by_ID_initiator_splitContainer.Panel1.PerformLayout();
            this.Search_by_ID_initiator_splitContainer.Panel2.ResumeLayout(false);
            this.Search_by_ID_initiator_splitContainer.ResumeLayout(false);
            this.CT_Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CT_View)).EndInit();
            this.FTT_Tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FTT_View)).EndInit();
            this.Queues_Tab.ResumeLayout(false);
            this.Queues_Tab_Container.Panel1.ResumeLayout(false);
            this.Queues_Tab_Container.Panel2.ResumeLayout(false);
            this.Queues_Tab_Container.Panel2.PerformLayout();
            this.Queues_Tab_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Queues_View)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.GraphicModel_Tab.ResumeLayout(false);
            this.GraphicModel_Panel.ResumeLayout(false);
            this.GraphicModel_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GraphicModel_View)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        private void POSP_Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                if ((this.Output == null) || (this.Output.ModelIsBuilt == false))
                {
                    построениеToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    запускПоВремениToolStripMenuItem_Click(sender, e);
                }
            }
            if (e.KeyCode == Keys.S && (e.Control))
            {
                if (this.Output.ModelIsBuilt)
                {
                    BuildingField.Text = "Для сохранения модели необходимо остановить моделирование.";
                }
                else
                {
                    сохранитьToolStripMenuItem.PerformClick();
                }
            }

        }

        void UpDate()
        {
            //this.DataSets.Input. CodeRtf = CodeField.Rtf;
            this.Input.CodeTxt = CodeField.Text;
            //this.DataSets.InfoTxt = BuildingField.Text;
            this.Input.ShowNextOperator = следующийОператорToolStripMenuItem.Checked;
            this.Input.ShowQueues = очередиToolStripMenuItem.Checked;
            this.Input.ShowSysMark = системныеМеткиToolStripMenuItem.Checked;
        }
        void UpLoad()
        {
            this.Output = new OutputData(this.precision, this.ExploredModel);
            OutputData out_data = this.Output;
            out_data.Output_All_Data();
            
            //CodeField.Rtf = LPDP_Data.CodeRtf;
            CodeField.Text = out_data.CodeTxt;// CodeRtf;


            int shifting = 0;
            if (this.Input.ShowQueues)
            {
                shifting = TextFormat.InsertColorQueueArrows(this.CodeField, out_data.QueueArrows, out_data.NextOperatorPosition_Start);
            }
            if (this.Input.ShowNextOperator)
            {
                TextFormat.ColorizeNextOperator(this.CodeField, 
                    out_data.NextOperatorPosition_Start + shifting, 
                    out_data.NextOperatorPosition_Length, 
                    out_data.NextInitiatorIsFlow,
                    out_data.UnitPosition);
            }

            BuildingField.Text = out_data.InfoTxt;
            //следующийОператорToolStripMenuItem.Checked = this.DataSets.ShowNextOperator;
            //очередиToolStripMenuItem.Checked = this.DataSets.ShowQueues;
            //системныеМеткиToolStripMenuItem.Checked = this.DataSets.ShowSysMark;

            TIME_Value.Text = Convert.ToString(Math.Round(out_data.TIME, out_data.Precision));
            INITIATOR_Value.Text = Convert.ToString(out_data.InitiatorNumber);

            this.Objects_View.DataSource = out_data.Objects;
            this.Initiators_View.DataSource = out_data.Initiators;
            this.FTT_View.DataSource = out_data.FTT;
            this.CT_View.DataSource = out_data.CT;
            this.Queues_View.DataSource = out_data.Queues;
            UseAllCaptions();
        }


        #region ВКЛАДКИ МЕНЮ
        // Файл
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog NewOpenFileDialog = new OpenFileDialog();
            //NewOpenFileDialog.InitialDirectory = 
            NewOpenFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Текст в формате RTF (*.rtf)|*.rtf|Все файлы|*.*"; //|Документы Word (*.docx)|*.docx|Документы Word 97-2003 (*.doc)|*.doc

            if (NewOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string SelectedFile = NewOpenFileDialog.FileName;
                    string FileName = SelectedFile.Substring(SelectedFile.LastIndexOf('\\') + 1, SelectedFile.LastIndexOf('.') - SelectedFile.LastIndexOf('\\') - 1);
                    string FileExtention = SelectedFile.Substring(SelectedFile.LastIndexOf('.'));

                    StreamReader NewStream;
                    string ModelText;
                    switch (FileExtention)
                    {
                        //CodeField.Font = new Font("Microsoft Sans Serif", 12);
                        //CodeField.ForeColor = Color.Black;
                        //CodeField.BackColor = Color.White;
                        case ".txt":
                            NewStream = new StreamReader(SelectedFile, Encoding.Default);
                            ModelText = NewStream.ReadToEnd();
                            break;
                        case ".rtf":
                            NewStream = new StreamReader(SelectedFile, Encoding.Default);
                            RichTextBox NewRTB = new RichTextBox();
                            NewRTB.Rtf = NewStream.ReadToEnd();
                            ModelText = NewRTB.Text;
                            NewRTB.Dispose();
                            break;
                        //case ".docx":
                        //    NewStream = new StreamReader(SelectedFile, Encoding.ASCII);
                        //    CodeField.Clear();
                        //    char[] NewC = new char[1000];
                        //    NewStream.Read(NewC, 0, 10);
                        //    break;
                        //case ".doc":
                        //    NewStream = new StreamReader(SelectedFile, Encoding.GetEncoding("windows-1251"));
                        //    break;
                        default:
                            NewStream = new StreamReader(SelectedFile, Encoding.Default);
                            ModelText = NewStream.ReadToEnd();
                            break;
                    }
                    NewStream.Dispose();

                    FileName_label.Text = FileName;
                    FilePath_label.Text = SelectedFile;
                    CodeField.Clear();
                    CodeField.Text = ModelText;

                    BuildingField.Text = "Файл " + "\"" + SelectedFile + "\" успешно открыт.";

                    //this.ExploredModel = new Model();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка открытия файла: " + ex.Message);
                } 
            }          
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FilePath_label.Text != "")
            {
                try
                {
                    string SavedFile = FilePath_label.Text;
                    string FileName = FileName_label.Text;
                    string FileExtention = SavedFile.Substring(SavedFile.LastIndexOf('.'));

                    StreamWriter NewStream;
                    string ModelText;
                    switch (FileExtention)
                    {
                        case ".txt":
                            NewStream = new StreamWriter(SavedFile, false, Encoding.GetEncoding("windows-1251"));
                            ModelText = CodeField.Text;
                            break;
                        case ".rtf":
                            NewStream = new StreamWriter(SavedFile, false, Encoding.Default);
                            ModelText = CodeField.Rtf;
                            break;

                        default:
                            NewStream = new StreamWriter(SavedFile, false, Encoding.GetEncoding("windows-1251"));
                            ModelText = CodeField.Text;
                            break;
                    }
                    using (NewStream)
                    {
                        foreach (char ch in ModelText)
                        {
                            if (ch == '\n')
                                NewStream.Write('\r');
                            NewStream.Write(ch);
                        }
                    }
                    ModelTextIsModified = false;
                    BuildingField.Text = "Файл " + "\"" + SavedFile + "\" успешно сохранен.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения файла: " + ex.Message);
                } 
            }
            else
                сохранитьКакToolStripMenuItem.PerformClick();
        } 
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog NewSaveFileDialog = new SaveFileDialog();
            //NewOpenFileDialog.InitialDirectory = 
            NewSaveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Текст в формате RTF (*.rtf)|*.rtf|Все файлы|*.*"; //|Документы Word (*.docx)|*.docx|Документы Word 97-2003 (*.doc)|*.doc
            NewSaveFileDialog.FileName = FileName_label.Text;
            if (NewSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string SavedFile = NewSaveFileDialog.FileName;
                    string FileName = SavedFile.Substring(SavedFile.LastIndexOf('\\') + 1, SavedFile.LastIndexOf('.') - SavedFile.LastIndexOf('\\') - 1);
                    string FileExtention = SavedFile.Substring(SavedFile.LastIndexOf('.'));

                    StreamWriter NewStream;
                    string ModelText;
                    switch (FileExtention)
                    {
                        case ".txt":
                            NewStream = new StreamWriter(SavedFile, false, Encoding.GetEncoding("windows-1251"));
                            ModelText = CodeField.Text;
                            break;
                        case ".rtf":
                            NewStream = new StreamWriter(SavedFile, false, Encoding.Default);
                            ModelText = CodeField.Rtf;
                            break;
                        default:
                            NewStream = new StreamWriter(SavedFile, false, Encoding.GetEncoding("windows-1251"));
                            ModelText = CodeField.Text;
                            break;
                    }

                    using (NewStream)
                    {
                        foreach (char ch in ModelText)
                        {
                            if (ch == '\n')
                                NewStream.Write('\r');
                            NewStream.Write(ch);
                        }
                    }
                    ModelTextIsModified = false;
                    FileName_label.Text = FileName;
                    FilePath_label.Text = SavedFile;
                    MessageBox.Show("Файл успешно сохранен");
                    BuildingField.Text = "Файл " + "\"" + SavedFile + "\" успешно сохранен.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения файла: " + ex.Message);
                }   
            }
        }

        //построение
        private void построениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpDate();
            this.ExploredModel = new Model();
            //this.DataSets.SetParentModel(this.ExploredModel);
            this.ExploredModel.Analysis.Building(this.Input.CodeTxt);
            
            this.UpLoad();

            if (this.Output.ModelIsBuilt == true)
            {

                запускToolStripMenuItem.Enabled = true;
                выполнениеКОСToolStripMenuItem.Enabled = true;
                шагToolStripMenuItem.Enabled = true;

                CodeField.ReadOnly = true;
                построениеToolStripMenuItem.Enabled = false;
                файлToolStripMenuItem.Enabled = false;
                стопToolStripMenuItem.Enabled = true;
            }
            else 
            {
                //errors
            }
        }

        //запуск
        private void запускToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            вводВремениToolStripMenuItem.Visible = false;
        }
        private void запускПоВремениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            вводВремениToolStripMenuItem.Visible = true;
            запускToolStripMenuItem.ShowDropDown();
            запускПоВремениToolStripMenuItem.ShowDropDown();
            вводВремениToolStripMenuItem.Focus();
        } 
        private void вводВремениToolStripMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.UpDate();
                double LaunchTime = Convert.ToDouble(вводВремениToolStripMenuItem.Text);
                this.ExploredModel.Executor.StartUntil(LaunchTime);
                if (this.Output.ModelIsBuilt == false)
                    стопToolStripMenuItem.PerformClick();
                this.UpLoad();
            }
        }
        private void выполнениеКОСToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpDate();
            //double LaunchTime = Convert.ToDouble(вводВремениToolStripMenuItem.Text);
            this.ExploredModel.Executor.StartSEC();
            if (this.Output.ModelIsBuilt == false)
                стопToolStripMenuItem.PerformClick();
            this.UpLoad();

        }
        private void шагToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpDate();
            this.ExploredModel.Executor.StartStep();

            if (this.Output.ModelIsBuilt == false)
                стопToolStripMenuItem.PerformClick();

            this.UpLoad();
        } 

        //стоп
        private void стопToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpDate();

            //LPDP_Actions.Stop();
            //this.ExploredModel.Built = false;
            this.ExploredModel.Executor.Stop();

            this.UpLoad();


            CodeField.ReadOnly = false;
            построениеToolStripMenuItem.Enabled = true;
            файлToolStripMenuItem.Enabled = true;
            запускToolStripMenuItem.Enabled = false;
            запускToolStripMenuItem.Enabled = false;

            выполнениеКОСToolStripMenuItem.Enabled = false;
            шагToolStripMenuItem.Enabled = false;

            стопToolStripMenuItem.Enabled = false;

        }

        //окна
        private void объектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (объектыToolStripMenuItem.Checked == false)
            {
                объектыToolStripMenuItem.Checked = true;
                Objects_Tab.Parent = ResultField;
                BuildingField.Focus();
            }
            else
            {
                объектыToolStripMenuItem.Checked = false;
                Objects_Tab.Parent = null;
            }
            окнаToolStripMenuItem.ShowDropDown();
        }
        private void инициаторыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (инициаторыToolStripMenuItem.Checked == false)
            {
                инициаторыToolStripMenuItem.Checked = true;
                Initiators_Tab.Parent = ResultField;
                BuildingField.Focus();
            }
            else
            {
                инициаторыToolStripMenuItem.Checked = false;
                Initiators_Tab.Parent = null;
            }
            окнаToolStripMenuItem.ShowDropDown();
        }
        private void таблицаБудущихВременToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (таблицаБудущихВременToolStripMenuItem.Checked == false)
            {
                таблицаБудущихВременToolStripMenuItem.Checked = true;
                FTT_Tab.Parent = ResultField;
                BuildingField.Focus();
            }
            else
            {
                таблицаБудущихВременToolStripMenuItem.Checked = false;
                FTT_Tab.Parent = null;
            }
            окнаToolStripMenuItem.ShowDropDown();
        }
        private void таблицаУсловийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (таблицаУсловийToolStripMenuItem.Checked == false)
            {
                таблицаУсловийToolStripMenuItem.Checked = true;
                CT_Tab.Parent = ResultField;
                BuildingField.Focus();
            }
            else
            {
                таблицаУсловийToolStripMenuItem.Checked = false;
                CT_Tab.Parent = null;
            }
            окнаToolStripMenuItem.ShowDropDown();
        }
        private void очереди_окноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (очереди_окноToolStripMenuItem.Checked == false)
            {
                очереди_окноToolStripMenuItem.Checked = true;
                Queues_Tab.Parent = ResultField;
                BuildingField.Focus();
            }
            else
            {
                очереди_окноToolStripMenuItem.Checked = false;
                Queues_Tab.Parent = null;
            }
            окнаToolStripMenuItem.ShowDropDown();
        }
        //private void графическаяМодельToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (графическаяМодельToolStripMenuItem.Checked == false)
        //    {
        //        графическаяМодельToolStripMenuItem.Checked = true;
        //        GraphicModel_Tab.Parent = ResultField;
        //        BuildingField.Focus();
        //    }
        //    else
        //    {
        //        графическаяМодельToolStripMenuItem.Checked = false;
        //        GraphicModel_Tab.Parent = null;
        //    }
        //    окнаToolStripMenuItem.ShowDropDown();
        //}
        
        //отображения
        private void отображенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            системныеМеткиToolStripMenuItem.Enabled = this.Output.ModelIsBuilt;
            следующийОператорToolStripMenuItem.Enabled = this.Output.ModelIsBuilt;
            очередиToolStripMenuItem.Enabled = this.Output.ModelIsBuilt;
        }
        private void системныеМеткиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (системныеМеткиToolStripMenuItem.Checked == false)
            {
                системныеМеткиToolStripMenuItem.Checked = true;
                BuildingField.Focus();
            }
            else
                системныеМеткиToolStripMenuItem.Checked = false;

            отображенияToolStripMenuItem.ShowDropDown();
            //LPDP_Actions.Building(this.DataSets.CodeTxt);
            //CodeField.Rtf = LPDP_Code.Build_RTF_Code(системныеМеткиToolStripMenuItem.Checked);
            //CodeField.Rtf = LPDP_Code.Rewrite_Initiators_RTF(CodeField.Rtf, следующийОператорToolStripMenuItem.Checked, очередиToolStripMenuItem.Checked);
        }
        private void следующийОператорToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (следующийОператорToolStripMenuItem.Checked == false)
            {
                следующийОператорToolStripMenuItem.Checked = true;
                BuildingField.Focus();
            }
            else
                следующийОператорToolStripMenuItem.Checked = false;

            отображенияToolStripMenuItem.ShowDropDown();
            //CodeField.Rtf = LPDP_Code.Rewrite_Initiators_RTF(CodeField.Rtf, следующийОператорToolStripMenuItem.Checked, очередиToolStripMenuItem.Checked);
            //LPDP_Actions.UpGradeCode();
            this.UpDate();
            this.UpLoad();
        }
        private void очередиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (очередиToolStripMenuItem.Checked == false)
            {
                очередиToolStripMenuItem.Checked = true;
                BuildingField.Focus();
            }
            else
                очередиToolStripMenuItem.Checked = false;

            отображенияToolStripMenuItem.ShowDropDown();
            //CodeField.Rtf = LPDP_Code.Rewrite_Initiators_RTF(CodeField.Rtf, следующийОператорToolStripMenuItem.Checked, очередиToolStripMenuItem.Checked);
            //LPDP_Actions.Building(this.DataSets.CodeTxt);
            //LPDP_Graphics.Reload_Values_and_Queues(очередиToolStripMenuItem.Checked);
            //GraphicModel_View.Refresh();
            this.UpDate();
            this.UpLoad();
        }

        private void путьКФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (путьКФайлуToolStripMenuItem.Checked == false)
            {
                путьКФайлуToolStripMenuItem.Checked = true;
                FilePath_label.Visible = true;
            }
            else
            {
                путьКФайлуToolStripMenuItem.Checked = false;
                FilePath_label.Visible = false;
            }
            BuildingField.Focus();
            отображенияToolStripMenuItem.ShowDropDown();
        }

        #endregion

        #region ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ
        //очищение от select
        private void Objects_View_SelectionChanged(object sender, EventArgs e)
        {
            Objects_View.ClearSelection();
        }
        private void Initiators_View_SelectionChanged(object sender, EventArgs e)
        {
            Initiators_View.ClearSelection();
        }
        private void FTT_View_SelectionChanged(object sender, EventArgs e)
        {
            FTT_View.ClearSelection();
        }
        private void CT_View_SelectionChanged(object sender, EventArgs e)
        {
            CT_View.ClearSelection();
        }
        private void Queues_View_SelectionChanged(object sender, EventArgs e)
        {
            Queues_View.ClearSelection();
        }

        // свернуть/развернуть вектор
        private void Objects_View_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Row_index = Objects_View.CurrentCellAddress.Y;
            int Column_index = Objects_View.CurrentCellAddress.X;

            string CellUnit = Objects_View[0, Row_index].Value.ToString();
            string CellName = Objects_View[1, Row_index].Value.ToString();
            string CellType = Objects_View[3, Row_index].Value.ToString();

            //if (CellType == "Вектор")
            //{
            //    // развернуть
            //    if (Objects_View[1, Row_index + 1].Visible == false)
            //    {
            //        for (int i = 0; i < Objects_View.RowCount; i++)
            //        {
            //            if ((Objects_View[1, i].Value.ToString().Contains(CellName)) && (Objects_View[0, i].Value.ToString() == CellUnit) &&
            //                (Objects_View[1, i].Value.ToString().Count(ch => ch == '.') == CellName.Count(ch => ch == '.') + 1))
            //            {
            //                Objects_View.Rows[i].Visible = true;
            //            }
            //        }
            //    }
            //    //свернуть
            //    else
            //    {
            //        for (int i = 0; i < Objects_View.RowCount; i++)
            //        {
            //            if ((Objects_View[1, i].Value.ToString().Contains(CellName)) && (Objects_View[0, i].Value.ToString() == CellUnit) &&
            //                (Objects_View[1, i].Value.ToString() != CellName))
            //            {
            //                //Objects_View.Rows.RemoveAt(i);
            //                Objects_View.Rows[i].Visible = false;
            //                //i--;
            //            }
            //        }
            //    }
            if (CellType == "Вектор")
            {
                // развернуть
                if (Objects_View[1, Row_index + 1].Visible == false)
                {
                    for (int i = Row_index + 1; i < Objects_View.RowCount; i++)
                    {
                        if (Objects_View[0, i].Value.ToString()=="")
                        {
                            Objects_View.Rows[i].Visible = true;
                        }
                        else
                            break;
                    }
                }
                //свернуть
                else
                {
                    for (int i = Row_index + 1; i < Objects_View.RowCount; i++)
                    {
                        if (Objects_View[0, i].Value.ToString() == "")
                        {
                            //Objects_View.Rows.RemoveAt(i);
                            Objects_View.Rows[i].Visible = false;
                            //i--;
                        }
                        else
                            break;
                    }
                }
            }

        }
        private void Initiators_View_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Row_index = Initiators_View.CurrentCellAddress.Y;
            int Column_index = Initiators_View.CurrentCellAddress.X;

            string CellID = Initiators_View[0, Row_index].Value.ToString();
            string CellValue = Initiators_View[1, Row_index].Value.ToString();
            string CellType = Initiators_View[3, Row_index].Value.ToString();

            if (CellType == "Вектор")
            {
                // развернуть
                if (Initiators_View[1, Row_index + 1].Visible == false)
                {
                    for (int i = Row_index + 1; i < Initiators_View.RowCount; i++)
                    {
                        if (Initiators_View[0, i].Value.ToString() == "")
                        {
                            Initiators_View.Rows[i].Visible = true;
                        }
                        else
                            break;
                    }
                }
                //свернуть
                else
                {
                    for (int i = Row_index + 1; i < Initiators_View.RowCount; i++)
                    {
                        if (Initiators_View[0, i].Value.ToString() == "")
                        {
                            Initiators_View.Rows[i].Visible = false;
                        }
                        else
                            break;
                    }
                }
            }

        }

        private void Queues_View_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        //    int Row_index = Queues_View.CurrentCellAddress.Y;
        //    int Column_index = Queues_View.CurrentCellAddress.X;

        //    string CellID = Queues_View[0, Row_index].Value.ToString();
        //    string CellUnit = Queues_View[1, Row_index].Value.ToString();
        //    string CellMark = Queues_View[2, Row_index].Value.ToString();
        //    string CellInitiatiorsOrder = Queues_View[3, Row_index].Value.ToString();

        //    if ((CellUnit != "")&&(CellMark!=""))
        //    {
        //        // развернуть
        //        if (Queues_View[1, Row_index + 1].Visible == false)
        //        {
        //            for (int i = 0; i < Queues_View.RowCount; i++)
        //            {
        //                if (Queues_View[0, i].Value.ToString() == Queues_View[0, Row_index].Value.ToString())
        //                {
        //                    Queues_View.Rows[i].Visible = true;
        //                    Queues_View.Columns[0].Visible = false;
        //                }
        //            }
        //        }
        //        //свернуть
        //        else
        //        {
        //            for (int i = 0; i < Queues_View.RowCount; i++)
        //            {
        //                if ((Queues_View[0, i].Value.ToString() == Queues_View[0, Row_index].Value.ToString())&&(i!=Row_index))
        //                {
        //                    Queues_View.Rows[i].Visible = false;
        //                }
        //            }
        //        }
        //    }

        }

        //поиск инициатора по ID 
        private List<int> Get_DisplayedList(string request)
        {
            List<int> Displayed_list = new List<int>();
            while (request != "")
            {
                int len = request.IndexOf(',');
                if (len == -1) len = request.Length;
                string subrequest = request.Substring(0, len);
                subrequest = subrequest.Replace(",", "");
                if (subrequest.Contains('-'))
                {
                    int first_number = Convert.ToInt32(subrequest.Substring(0, subrequest.IndexOf('-')));
                    int last_number = Convert.ToInt32(subrequest.Substring(subrequest.IndexOf('-') + 1));

                    for (int i = first_number; i < last_number + 1; i++)
                        Displayed_list.Add(i);
                }
                else
                    Displayed_list.Add(Convert.ToInt32(subrequest));

                request = request.Remove(0, subrequest.Length);
                if (request.Length == 0) break;
                if (request[0] == ',') request = request.Remove(0, 1);
            }
            return Displayed_list;
 
        }

        private void Search_by_ID_initiator_button_Click(object sender, EventArgs e)
        {
            string request = Search_by_ID_initiator_textBox.Text;
            List<int> Displayed_list = new List<int>();
            request = request.Replace(" ", "");
            if (request != "")
            {
                try
                {
                    //while (request != "")
                    //{
                    //    int len = request.IndexOf(',');
                    //    if (len == -1) len = request.Length;
                    //    string subrequest = request.Substring(0, len);
                    //    subrequest = subrequest.Replace(",", "");
                    //    if (subrequest.Contains('-'))
                    //    {
                    //        int first_number = Convert.ToInt32(subrequest.Substring(0, subrequest.IndexOf('-')));
                    //        int last_number = Convert.ToInt32(subrequest.Substring(subrequest.IndexOf('-') + 1));

                    //        for (int i = first_number; i < last_number + 1; i++)
                    //            Displayed_list.Add(i);
                    //    }
                    //    else
                    //        Displayed_list.Add(Convert.ToInt32(subrequest));

                    //    request = request.Remove(0, subrequest.Length);
                    //    if (request.Length == 0) break;
                    //    if (request[0] == ',') request = request.Remove(0, 1);
                    //}
                    Displayed_list = Get_DisplayedList(request);

                    for (int i = 0; i < Initiators_View.RowCount; i++)
                    {
                        string str_ID = Convert.ToString(Initiators_View[0, i].Value);
                        if (str_ID == "")
                        {
                            Initiators_View.Rows[i].Visible = false;
                            continue;
                        }
                        //str_ID = str_ID.Replace(" ", "");
                        //str_ID = str_ID.Substring(0, str_ID.IndexOf("->"));
                        int ID = Convert.ToInt32(str_ID);

                        if (Displayed_list.Contains(ID))
                        {
                            Initiators_View.Rows[i].Visible = true;
                            //Initiators_View.Rows.SharedRow(i).DefaultCellStyle.ForeColor = Color.Green;
                            //Initiators_View.Rows.SharedRow(i).DefaultCellStyle.SelectionForeColor = Color.Green;
                            //Initiators_View.ClearSelection();
                        }
                            
                        else
                            Initiators_View.Rows[i].Visible = false;
                    }
                }
                catch { }  
            }             
        }
        private void Search_by_ID_initiator_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Search_by_ID_initiator_button.PerformClick();
            }
        }

        //private void Search_initiator_in_queues_button_Click(object sender, EventArgs e)
        //{
        //    string request = Search_initiator_in_queues_textBox.Text;
        //    List<int> Displayed_list = new List<int>();
        //    List<int> Displayed_initiators_list = new List<int>();
        //    request = request.Replace(" ", "");
        //    if (request != "")
        //    {
        //        try
        //        {
        //            Displayed_initiators_list = Get_DisplayedList(request);

        //            for (int q = 0; q < this.Queues_View.RowCount; q++)
        //            {
        //                if (Queues_View.Rows[q].Cells[2].Value.ToString().Contains(init => Displayed_initiators_list.Contains(init.Initiator)))
        //                {
        //                    Displayed_list.Add(q);
        //                }
        //            }

        //            for (int i = 0; i < Queues_View.RowCount; i++)
        //            {
        //                string str_ID = Convert.ToString(Queues_View[0, i].Value);
        //                int ID = Convert.ToInt32(str_ID);
        //                if (Displayed_list.Contains(ID))
        //                {
        //                    Queues_View.Rows[i].Visible = true;

        //                    string CellID = Queues_View[0, i].Value.ToString();
        //                    string CellUnit = Queues_View[1, i].Value.ToString();
        //                    string CellMark = Queues_View[2, i].Value.ToString();
        //                    string CellInitiatiorsOrder = Queues_View[3, i].Value.ToString();
        //                    if ((CellUnit == "") && (CellMark == ""))
        //                    {
        //                        string str_ID_init = Convert.ToString(Queues_View[3, i].Value);
        //                        str_ID_init = str_ID_init.Replace(" ", "");
        //                        str_ID_init = str_ID_init.Substring(0, str_ID_init.IndexOf("->"));
        //                        int ID_init = Convert.ToInt32(str_ID_init);

        //                        if (Displayed_initiators_list.Contains(ID_init))
        //                        {
        //                            Queues_View.Rows.SharedRow(i).DefaultCellStyle.ForeColor = Color.Green;
        //                            Queues_View.Rows.SharedRow(i).DefaultCellStyle.SelectionForeColor = Color.Green;
        //                            Queues_View.ClearSelection();
        //                        }
        //                        else
        //                        {
        //                            Queues_View.Rows.SharedRow(i).DefaultCellStyle.ForeColor = Color.Black;
        //                            Queues_View.Rows.SharedRow(i).DefaultCellStyle.SelectionForeColor = Color.Black;
        //                            Queues_View.ClearSelection();
        //                        }
        //                    }
        //                }

        //                else
        //                    Queues_View.Rows[i].Visible = false;
        //            }
        //        }
        //        catch { }
        //    }
        //}
        private void Search_initiator_in_queues_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Search_initiator_in_queues_button.PerformClick();
            }
        }

        // применение шапок таблиц
        void UseAllCaptions()
        {
            OutputData out_data = this.Output;
            TextFormat.UseCaptions(this.Objects_View, out_data.Objects);
            TextFormat.UseCaptions(this.Initiators_View, out_data.Initiators);
            TextFormat.UseCaptions(this.FTT_View, out_data.FTT);
            TextFormat.UseCaptions(this.CT_View, out_data.CT);
            TextFormat.UseCaptions(this.Queues_View, out_data.Queues);
        }

        #endregion
        // закрытие формы
        private void POSP_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ModelTextIsModified)
            {
                Closing_Form NewClosing_Form = new Closing_Form();
                NewClosing_Form.ShowDialog();

                switch (NewClosing_Form.Result)
                {
                    case Closing_Form.ResultType.Save:
                        if (this.Output.ModelIsBuilt) 
                            стопToolStripMenuItem.PerformClick();
                        сохранитьToolStripMenuItem.PerformClick();

                        //LPDP_Data.ClearTable(LPDP_Data.Objects);
                        //LPDP_Data.ClearTable(LPDP_Data.Initiators);
                        //LPDP_Data.ClearTable(LPDP_Data.Queues);
                        //LPDP_Data.ClearTable(LPDP_Data.FTT);
                        //LPDP_Data.ClearTable(LPDP_Data.CT);

                        break;
                    case Closing_Form.ResultType.NoSave:
                        //LPDP_Data.ClearTable(LPDP_Data.Objects);
                        //LPDP_Data.ClearTable(LPDP_Data.Initiators);
                        //LPDP_Data.ClearTable(LPDP_Data.Queues);
                        //LPDP_Data.ClearTable(LPDP_Data.FTT);
                        //LPDP_Data.ClearTable(LPDP_Data.CT);
                        break;
                    case Closing_Form.ResultType.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }  
            }
        }
    }

}
