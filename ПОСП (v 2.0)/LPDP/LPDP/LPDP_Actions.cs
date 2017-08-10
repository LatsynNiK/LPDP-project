using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP
{
    public static class LPDP_Actions
    {
        public static void Building(/*string CodeTxt, bool ShowSysMark, bool ShowNextOperatar, bool ShowQueues*/)
        {
            LPDP_Core.RESET();
            //LPDP_Data.CodeTxt = "Выполняется построение модели...";
            //CodeField.Rtf = LPDP_Code.ClearFromPointer_RTF(CodeField.Rtf);
            //LPDP_Data.CodeTxt = LPDP_Data.CodeTxt;

            // Новый Анализ текста
            //LPDP_TextAnalysis.InitializeErrorTypes();
            //LPDP_TextAnalysis.LexicalAnalysis(LPDP_Data.CodeTxt);
            Analysis NewAnalysis = new Analysis(LPDP_Data.CodeTxt);

            // Конец Нового Анализа

            LPDP_Data.InfoTxt = LPDP_Core.Build_POSP_Model(LPDP_Data.CodeTxt);

            if (LPDP_Core.Model_Is_Built == true)
            {
                LPDP_Data.CodeTxt = String.Empty;
                LPDP_Core.Rewrite_Queues();

                //string CodeRtf;
                LPDP_Data.CodeRtf = LPDP_Code.Build_RTF_Code(LPDP_Data.ShowSysMark);
                LPDP_Data.CodeRtf = LPDP_Code.Rewrite_Initiators_RTF(LPDP_Data.CodeRtf, LPDP_Data.ShowNextOperator, LPDP_Data.ShowQueues);

                LPDP_Graphics.Load_GraphicModel();
                LPDP_Graphics.Reload_Values_and_Queues(LPDP_Data.ShowQueues);
                //GraphicModel_View.Refresh();

                //Rewrite_All_Views();

                //запускToolStripMenuItem.Enabled = true;
                //выполнениеКОСToolStripMenuItem.Enabled = true;
                //шагToolStripMenuItem.Enabled = true;

                //CodeField.ReadOnly = true;
                //построениеToolStripMenuItem.Enabled = false;
                //файлToolStripMenuItem.Enabled = false;
                //стопToolStripMenuItem.Enabled = true;
            }
            else { }
            //BuildingField.Text = ErrorText;
        }

        public static void StartUntil(double time)
        {
            string NewStr = LPDP_Core.Launch_POSP_Model(time, LPDP_Core.Mode.time);
            if (LPDP_Core.Model_Is_Built != false)
            {
                //вводВремениToolStripMenuItem.Text = SaveText;

                LPDP_Core.Rewrite_Queues();
                //CodeField.Rtf = LPDP_Code.Build_RTF_Code();
                LPDP_Data.CodeRtf = LPDP_Code.Rewrite_Initiators_RTF(LPDP_Data.CodeRtf, LPDP_Data.ShowNextOperator, LPDP_Data.ShowQueues);
                LPDP_Graphics.Reload_Values_and_Queues(LPDP_Data.ShowQueues);
                //TIME_Value.Text = Convert.ToString(Math.Round(LPDP_Core.TIME, 2));
                //Rewrite_All_Views();
            }
            LPDP_Data.InfoTxt = NewStr;
        }

        public static void StartSEC()
        {
            //BuildingField.Focus();
            //BuildingField.Text = "Выполняется Класс Одновременных Событий...";
            string NewStr = LPDP_Core.Launch_POSP_Model(0, LPDP_Core.Mode.timestep);
            if (LPDP_Core.Model_Is_Built != false)
            {
                //    стопToolStripMenuItem.PerformClick();
                LPDP_Core.Rewrite_Queues();
                //CodeField.Rtf = LPDP_Code.Build_RTF_Code();
                LPDP_Data.CodeRtf = LPDP_Code.Rewrite_Initiators_RTF(LPDP_Data.CodeRtf, LPDP_Data.ShowNextOperator, LPDP_Data.ShowQueues);
                LPDP_Graphics.Reload_Values_and_Queues(LPDP_Data.ShowQueues);
                //TIME_Value.Text = Convert.ToString(Math.Round(LPDP_Core.TIME, 2));
                //Rewrite_All_Views();
                LPDP_Data.InfoTxt = NewStr;
            }
        }

        public static void StartStep()
        {
            //BuildingField.Focus();
            //BuildingField.Text = "Выполняется элементарный оператор...";
            string NewStr = LPDP_Core.Launch_POSP_Model(0, LPDP_Core.Mode.step);
            if (LPDP_Core.Model_Is_Built != false)
            {
                LPDP_Core.Rewrite_Queues();
                //CodeField.Rtf = LPDP_Code.Build_RTF_Code();
                LPDP_Data.CodeRtf = LPDP_Code.Rewrite_Initiators_RTF(LPDP_Data.CodeRtf, LPDP_Data.ShowNextOperator,LPDP_Data.ShowQueues);

                LPDP_Graphics.Reload_Values_and_Queues(LPDP_Data.ShowQueues);

                //TIME_Value.Text = Convert.ToString(Math.Round(LPDP_Core.TIME, 2));
                //Rewrite_All_Views();

                //BuildingField.Text = NewStr;
            }
                //стопToolStripMenuItem.PerformClick();
            LPDP_Data.InfoTxt = NewStr;
            
        }

        public static void UpGradeCode()
        {
            LPDP_Code.Rewrite_Initiators_RTF(LPDP_Data.CodeRtf, LPDP_Data.ShowNextOperator, LPDP_Data.ShowQueues);
        }

        public static void Stop() 
        {
            LPDP_Data.CodeRtf = LPDP_Code.Build_RTF_Code(false);
            LPDP_Graphics.Reload_Values_and_Queues(false);
            //GraphicModel_View.Refresh();

            //CodeField.ReadOnly = false;
            //построениеToolStripMenuItem.Enabled = true;
            //файлToolStripMenuItem.Enabled = true;
            //запускToolStripMenuItem.Enabled = false;
            //запускToolStripMenuItem.Enabled = false;

            //выполнениеКОСToolStripMenuItem.Enabled = false;
            //шагToolStripMenuItem.Enabled = false;

            //стопToolStripMenuItem.Enabled = false;

            LPDP_Core.Model_Is_Built = false;

            //очищение данных
            //Rewrite_All_Views();
            LPDP_Data.InfoTxt = "Модель остановлена.";
            //BuildingField.Text = "Модель остановлена.";
        }

        public static void Initialize_LPDP_ModelTextRules()
        {
            
            ModelTextRules.InitializeLexicalTemplates();
            //LPDP_ModelTextRules.InitializeLexicalRules();

            ModelTextRules.InitializeWordTypes();
            //LPDP_ModelTextRules.InitializeBracketRules();
            ModelTextRules.InitializePrimaryPhraseTypes();

            ModelTextRules.InitializeSyntacticalTemplates();


            ModelTextRules.InitializeErrorTypes();
        }
    }
}
