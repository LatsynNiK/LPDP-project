using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace LPDP //
{
    static class LPDP_Code
    {
        public struct Unit
        {
            public string Comments_before_Unit;
            public string Comments_for_Header;
            public string Comments_between_Description_and_Algorithm;
            public string Comments_for_End_of_Description;
            public string Comments_for_End_of_Algorithm;
            public string Comments_for_End_of_Unit;
            //string Comments_before_Algorithm;
            //string Comments_for_Algorithm;
            //string Comments_after_Algorithm;

            public string Type;
            public string Name;
            public List<Line> Description;
            public List<Line> Algorithm;
        }
        public struct Line
        {
            public int Number;
            public string Mark;
            public List<string> Words;
            public string Comments;
        }

        public static List<Unit> Units = new List<Unit>();
        public static string Comments_after_Units;

        public static Line Correct_Line(Line Old_Line)
        {
            Old_Line.Mark = "";
            Old_Line.Comments = "";
            //Old_Line.Index = -1;

            for (int i = 0; i < Old_Line.Words.Count; i++)
            {
                string word = Old_Line.Words[i];
                if ((word == " ") || (word == "\t") || (word == "\n") || (word == "\r"))
                {
                    Old_Line.Words.RemoveAt(i);
                    i--;
                    continue;
                }
                if ((word.Contains("//")) || (word.Contains("/*")))
                {
                    Old_Line.Comments += word;
                    Old_Line.Words.RemoveAt(i);
                    i--;
                    continue;
                }
                if ((word == "=") && (i != 0))
                {
                    string last_word = Old_Line.Words[i - 1];
                    if ((last_word == ":") || (last_word == "=") || (last_word == "!") ||
                        (last_word == ">") || (last_word == "<"))
                    {
                        Old_Line.Words[i - 1] += Old_Line.Words[i];
                        Old_Line.Words.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if ((word == "-") && (i != Old_Line.Words.Count))
                    if (Old_Line.Words[i + 1] == ">")
                    {
                        Old_Line.Words[i] += Old_Line.Words[i + 1];
                        Old_Line.Words.RemoveAt(i + 1);
                        continue;
                    }
                if (((word == "/") || (word == "\\")) && (i != 0))
                {
                    string last_word = Old_Line.Words[i - 1];
                    if ((last_word == "/") || (last_word == "\\"))
                    {
                        Old_Line.Words[i - 1] += Old_Line.Words[i];
                        Old_Line.Words.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if ((word == ",") && (i != Old_Line.Words.Count) && (i != 0))
                    if ((LPDP_Core.IsNumber(Old_Line.Words[i - 1])) && (LPDP_Core.IsNumber(Old_Line.Words[i + 1])))
                    {
                        Old_Line.Words[i - 1] = Old_Line.Words[i - 1] + Old_Line.Words[i] + Old_Line.Words[i + 1];
                        Old_Line.Words.RemoveRange(i, 2);
                        i--;
                        continue;
                    }
            }
            while (Old_Line.Words.Contains("\""))
            {
                int start_bracket = Old_Line.Words.IndexOf("\"");
                int end_bracket = Old_Line.Words.IndexOf("\"", start_bracket + 1);
                if (end_bracket != -1)
                    for (int i = start_bracket + 1; i <= end_bracket; i++)
                        Old_Line.Words[start_bracket] += Old_Line.Words[i];
                Old_Line.Words.RemoveRange(start_bracket + 1, end_bracket - start_bracket);
            }
            //выделение метки
            int mark_ind = Old_Line.Words.IndexOf(":");
            if (mark_ind != -1)
            {
                Old_Line.Mark = Old_Line.Words[0];
                Old_Line.Words.RemoveRange(0, 2);
            }

            return Old_Line;
        }

        public struct Selection
        {
            public int start;
            public int length;
            public Selection(int s, int l)
            {
                start = s; length = l;
            }

        }
        public static string Build_RTF_Code(bool Show_Marks)
        {
            string Code_for_insert = "";
            string RTF_Code = ConvertToRTF("");
            //string RTF_Inserted_Code = ConvertToRTF("");

            RichTextBox RTB_for_inserted_Code = new RichTextBox();
            RTB_for_inserted_Code.Font = new Font("Microsoft Sans Serif", 12);
            RTB_for_inserted_Code.ForeColor = Color.Black;
            RTB_for_inserted_Code.BackColor = Color.White;

            RichTextBox RTB_for_Code = new RichTextBox();
            RTB_for_Code.Font = new Font("Microsoft Sans Serif", 12);
            RTB_for_Code.ForeColor = Color.Black;
            RTB_for_Code.BackColor = Color.White;

            List<Selection> list_of_function_words = new List<Selection>();
            List<Selection> list_of_comments = new List<Selection>();
            List<Selection> list_of_ordinary_words = new List<Selection>();
            List<Selection> list_of_system_marks = new List<Selection>();

            int Current_Number_of_Line = 1;

            for (int u = 0; u < Units.Count; u++)
            {
                // предблочные комментарии
                if (Units[u].Comments_before_Unit != "")
                {
                    Code_for_insert = Units[u].Comments_before_Unit /*+ "\n"*/;
                    Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                    RTB_for_Code.Text += Code_for_insert;
                    list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                }

                // пример раскраски слова
                //int fStyle = /*(int)FontStyle.Bold + */(int)FontStyle.Underline;
                //FontStyle f = (FontStyle)fStyle;
                //NewRTB.Font = new Font(NewRTB.Font, f);

                //заголовок блока
                Code_for_insert = "блок " + Units[u].Type + " ";
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                Code_for_insert = Units[u].Name;
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                //комментарий после заголовка
                if (Units[u].Comments_for_Header != "")
                {
                    Code_for_insert = "\t" + Units[u].Comments_for_Header;
                    Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                    RTB_for_Code.Text += Code_for_insert;
                    list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                }

                //описание
                Code_for_insert = "описание";
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                for (int i = 0; i < Units[u].Description.Count; i++)
                {
                    // комментарий для предыдущей строки
                    if (Units[u].Description[i].Comments != "")
                    {
                        Code_for_insert = "\t\t" + Units[u].Description[i].Comments;
                        Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                        RTB_for_Code.Text += Code_for_insert;
                        list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                    }

                    //переход строки
                    Code_for_insert = "\n\t\t";
                    Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                    RTB_for_Code.Text += Code_for_insert;
                    list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                    // сама сторка
                    Line NewLine = Units[u].Description[i];
                    NewLine.Number = Current_Number_of_Line;
                    Units[u].Description[i] = NewLine;

                    for (int iw = 0; iw < Units[u].Description[i].Words.Count; iw++)
                    {
                        string word = Units[u].Description[i].Words[iw];
                        if (LPDP_Core.FunctionWord.Contains(word))
                        {
                            if (iw != 0)
                            {
                                Code_for_insert = " ";
                                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                                RTB_for_Code.Text += Code_for_insert;
                                //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                            }

                            Code_for_insert = word;
                            //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                            RTB_for_Code.Text += Code_for_insert;
                            list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                            if ((Units[u].Description[i].Words[iw + 1] != ";") &&
                                (LPDP_Core.FunctionWord.Contains(Units[u].Description[i].Words[iw + 1]) == false))
                            {
                                Code_for_insert = " ";
                                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                                RTB_for_Code.Text += Code_for_insert;
                                //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                            }
                        }
                        else
                        {
                            Code_for_insert = word;
                            //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                            RTB_for_Code.Text += Code_for_insert;
                            //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                        }
                    }
                }
                // комментарий перед "все описание"
                Code_for_insert = "\t\t" + Units[u].Comments_for_End_of_Description;
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                Code_for_insert = "все описание";
                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                // комментарий между "все описание" и "алгоритм"
                Code_for_insert = Units[u].Comments_between_Description_and_Algorithm;
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                // алгоритм
                Code_for_insert = "алгоритм";
                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                for (int i = 0; i < Units[u].Algorithm.Count; i++)
                {
                    // комментарий для предыдущей строки
                    if (Units[u].Algorithm[i].Comments != "")
                    {
                        Code_for_insert = "\t\t" + Units[u].Algorithm[i].Comments;
                        Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                        RTB_for_Code.Text += Code_for_insert;
                        list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                    }

                    //переход строки
                    Code_for_insert = "\n";
                    Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                    RTB_for_Code.Text += Code_for_insert;
                    list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                    // метка
                    if ((Units[u].Algorithm[i].Mark != "") && ((Units[u].Algorithm[i].Mark[0] != '$') || (Show_Marks)))
                    {
                        if ((i != 0) && (Units[u].Algorithm[i].Mark[0] != '$'))
                        {
                            Code_for_insert = "\n";
                            Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                            RTB_for_Code.Text += Code_for_insert;
                        }

                        Code_for_insert = Units[u].Algorithm[i].Mark + ":\t";
                        if (Units[u].Algorithm[i].Mark.Length < 5) Code_for_insert += "\t";
                    }
                    else
                        Code_for_insert = /*Units[u].Algorithm[i].Mark + */"\t\t";
                    //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');

                    RTB_for_Code.Text += Code_for_insert;
                    if (Code_for_insert[0] == '$')
                        list_of_system_marks.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                    //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));


                    Line NewLine = Units[u].Algorithm[i];
                    NewLine.Number = Current_Number_of_Line;
                    Units[u].Algorithm[i] = NewLine;

                    // сама сторка
                    for (int iw = 0; iw < Units[u].Algorithm[i].Words.Count; iw++)
                    {
                        string word = Units[u].Algorithm[i].Words[iw];
                        if (LPDP_Core.FunctionWord.Contains(word))
                        {
                            //для сложных если и ждать
                            if ((word == "если") || (word == "ждать") || (word == "иначе"))
                            {
                                if (iw != 0)
                                {
                                    Code_for_insert = "\n\t\t";
                                    Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                                    RTB_for_Code.Text += Code_for_insert;
                                }
                            }

                            if (iw != 0)
                            {
                                Code_for_insert = " ";
                                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                                RTB_for_Code.Text += Code_for_insert;
                                //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                            }
                            Code_for_insert = word;
                            //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                            RTB_for_Code.Text += Code_for_insert;
                            list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                            if ((Units[u].Algorithm[i].Words[iw + 1] != ";") &&
                                (LPDP_Core.FunctionWord.Contains(Units[u].Algorithm[i].Words[iw + 1]) == false))
                            {
                                Code_for_insert = " ";
                                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                                RTB_for_Code.Text += Code_for_insert;
                                //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                            }
                        }
                        else
                        {
                            Code_for_insert = word;
                            //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                            RTB_for_Code.Text += Code_for_insert;
                            //list_of_ordinary_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
                        }
                    }
                }

                // комментарий перед "все алгоритм"
                Code_for_insert = Units[u].Comments_for_End_of_Algorithm;
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                Code_for_insert = "все алгоритм";
                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                // комментарий перед "все блок"
                Code_for_insert = Units[u].Comments_for_End_of_Unit;
                Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

                Code_for_insert = "все блок";
                //Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
                RTB_for_Code.Text += Code_for_insert;
                list_of_function_words.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));
            }

            // последний комментарий
            Code_for_insert = Comments_after_Units;
            Current_Number_of_Line += Code_for_insert.Count(ch => ch == '\n');
            RTB_for_Code.Text += Code_for_insert;
            list_of_comments.Add(new Selection(RTB_for_Code.Text.Length - Code_for_insert.Length, Code_for_insert.Length));

            //раскраска текста
            for (int i = 0; i < list_of_function_words.Count; i++)
            {
                RTB_for_Code.SelectionStart = list_of_function_words[i].start;
                RTB_for_Code.SelectionLength = list_of_function_words[i].length;
                RTB_for_Code.Rtf = Colorize_FunctionWord(RTB_for_Code);
            }

            for (int i = 0; i < list_of_comments.Count; i++)
            {
                RTB_for_Code.SelectionStart = list_of_comments[i].start;
                RTB_for_Code.SelectionLength = list_of_comments[i].length;
                RTB_for_Code.Rtf = Colorize_CommentText(RTB_for_Code);
            }

            for (int i = 0; i < list_of_system_marks.Count; i++)
            {
                RTB_for_Code.SelectionStart = list_of_system_marks[i].start;
                RTB_for_Code.SelectionLength = list_of_system_marks[i].length;
                RTB_for_Code.Rtf = Colorize_SystemMark(RTB_for_Code);
            }



            RTF_Code = RTB_for_Code.Rtf;
            RTB_for_Code.Dispose();
            return RTF_Code;
        }

        public static string Rewrite_Initiators_RTF(string RTF_Code, bool Show_Current, bool Show_Queue)
        {
            //удаление старого
            RTF_Code = ClearFromBackcolor_RTF(RTF_Code);
            RTF_Code = ClearFromPointer_RTF(RTF_Code);

            //поиск след подпрограммы
            if (Show_Current)
            {
                int Next_subp = -1;
                int Next_init = -2;
                int Next_oper = LPDP_Core.Index_operation;
                if (LPDP_Core.SubProg_IsBroken) { Next_subp = LPDP_Core.NEXT_SUBPROGRAMM; Next_init = LPDP_Core.INITIATOR; }
                else
                {
                    //АПУ
                    int SaveInitiatior = LPDP_Core.INITIATOR;
                    int index;
                    for (index = 0; index < LPDP_Core.CT.Count; index++)
                    {
                        LPDP_Core.INITIATOR = LPDP_Core.CT[index].Initiator;
                        if (LPDP_Core.Logic_Expression(LPDP_Core.CT[index].Condition, LPDP_Core.CT[index].FromUnit) == true)
                            break;
                    }
                    LPDP_Core.INITIATOR = SaveInitiatior;
                    if (index == LPDP_Core.CT.Count) index = -1;

                    //int index = LPDP_Core.CT.FindIndex(rec => (LPDP_Core.Logic_Expression(rec.Condition, rec.FromUnit) == true));
                    if (index != -1)
                    {
                        Next_init = LPDP_Core.CT[index].Initiator;
                        Next_subp = LPDP_Core.CT[index].Subprogram_Index;
                    }
                    else
                    {
                        //КАЛЕНДАРЬ
                        //ищем мин
                        int IndMin = 0;
                        for (int i = 0; i < LPDP_Core.FTT.Count; i++)
                        {
                            if (LPDP_Core.FTT[i].ActiveTime < LPDP_Core.FTT[IndMin].ActiveTime) IndMin = i;
                        }
                        double Next_time = LPDP_Core.FTT[IndMin].ActiveTime;
                        Next_init = LPDP_Core.FTT[IndMin].Initiator;
                        Next_subp = LPDP_Core.FTT[IndMin].Subprogram_Index;
                        if (Next_subp == -1)
                        {
                            //снова АПУ
                            int SaveInitiatior1 = LPDP_Core.INITIATOR;
                            int index1;
                            for (index1 = 0; index1 < LPDP_Core.CT.Count; index1++)
                            {
                                LPDP_Core.INITIATOR = LPDP_Core.CT[index].Initiator;
                                if (LPDP_Core.Logic_Expression(LPDP_Core.CT[index1].Condition, LPDP_Core.CT[index1].FromUnit) == true)
                                    break;
                            }
                            LPDP_Core.INITIATOR = SaveInitiatior1;
                            if (index1 == LPDP_Core.CT.Count) index1 = -1;

                            //int index1 = LPDP_Core.CT.FindIndex(rec => (LPDP_Core.Logic_Expression(rec.Condition, rec.FromUnit) == true));
                            if (index1 != -1)
                            {
                                Next_init = LPDP_Core.CT[index1].Initiator;
                                Next_subp = LPDP_Core.CT[index1].Subprogram_Index;
                            }
                        }
                    }
                }

                int unit_for_painting = LPDP_Core.POSP_Model[Next_subp].Operations[Next_oper].Unit_index;
                int line_for_painting = LPDP_Core.POSP_Model[Next_subp].Operations[Next_oper].Line_index;

                //if (first_building == true)  // для того чтобы скрыть текущее положение
                //{
                //    unit_for_painting = -1;
                //    line_for_painting = -1;
                //}

                Color color_for_painting = new Color();
                Color color_for_pointer = new Color();
                if (Next_init == -1)
                {
                    color_for_painting = Color.Khaki;
                    color_for_pointer = Color.DarkGreen;
                }
                else
                {
                    color_for_painting = Color.PaleGreen;
                    color_for_pointer = Color.Green;
                }

                RTF_Code = PaintCurrent_Initiator_RTF(RTF_Code, color_for_painting, color_for_pointer, unit_for_painting, line_for_painting);
            }

            //отображение очередей
            if (Show_Queue)
            {


                for (int i = 0; i < LPDP_Core.POSP_Model.Count; i++)
                {
                    int unit = LPDP_Core.POSP_Model[i].Operations[0].Unit_index;
                    int line = LPDP_Core.POSP_Model[i].Operations[0].Line_index;
                    int Number_of_line = Units[unit].Algorithm[line].Number;

                    RichTextBox NewRTB = new RichTextBox();
                    NewRTB.Rtf = RTF_Code;
                    string Code = NewRTB.Text;
                    //для сохранения старого буфера обмена
                    RichTextBox ReservRTB = new RichTextBox();
                    ReservRTB.Paste();

                    //ищем '\t' для замены на стрелки
                    int Current_line = 1;
                    int Index_of_Tab = 0;
                    while (Current_line != Number_of_line)
                    {
                        Index_of_Tab = Code.IndexOf('\n', Index_of_Tab) + 1;
                        Current_line++;
                    }
                    Index_of_Tab = Code.IndexOf('\t', Index_of_Tab);
                    if (Code[Index_of_Tab + 1] == '\t') Index_of_Tab++;
                    // нашли, теперь удаляем его и вставляем стрелки
                    NewRTB.SelectionStart = Index_of_Tab;
                    NewRTB.SelectionLength = 1;
                    NewRTB.Cut();

                    NewRTB.Rtf = InsertRTF(NewRTB.Rtf, Index_of_Tab, GetRTF_Pointers(i));
                    RTF_Code = NewRTB.Rtf;

                    ReservRTB.SelectAll();
                    ReservRTB.Copy();
                    NewRTB.Dispose(); ReservRTB.Dispose();
                }
            }
            return RTF_Code;
        }

        //***************************************

        public static string Colorize_FunctionWord(RichTextBox RTB) //работает с уже выделенным текстом
        {
            RTB.SelectionColor = Color.Blue;
            int fStyle = /*(int)FontStyle.Bold + */(int)FontStyle.Underline;
            FontStyle f = (FontStyle)fStyle;
            RTB.SelectionFont = new Font(RTB.Font, f);
            return RTB.Rtf;
        }
        public static string Colorize_CommentText(RichTextBox RTB) //работает с уже выделенным текстом
        {
            RTB.SelectionColor = Color.Green;
            RTB.SelectionFont = RTB.Font;
            return RTB.Rtf;
        }
        public static string Colorize_SystemMark(RichTextBox RTB) //работает с уже выделенным текстом
        {
            RTB.SelectionColor = Color.Gray;
            RTB.SelectionFont = RTB.Font;
            return RTB.Rtf;
        }
        public static string Colorize_OrdinaryWord(RichTextBox RTB) //работает с уже выделенным текстом
        {
            RTB.SelectionColor = Color.Black;
            RTB.SelectionFont = RTB.Font;
            return RTB.Rtf;
        }

        public static string Colorize_RTF_Code(string RTF_Code)
        {
            //для сохранения старого буфера обмена
            RichTextBox ReservRTB = new RichTextBox();
            ReservRTB.Paste();

            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Font = new Font("Microsoft Sans Serif", 12);
            NewRTB.ForeColor = Color.Black;
            NewRTB.BackColor = Color.White;

            NewRTB.Rtf = RTF_Code;
            for (int i = 0; i < LPDP_Core.FunctionWord.Length; i++)
            {
                int start_index = 0;
                while (NewRTB.Text.IndexOf(LPDP_Core.FunctionWord[i], start_index) != -1)
                {
                    string left_border = "";
                    string right_border = "";

                    if (NewRTB.Text.IndexOf(LPDP_Core.FunctionWord[i]) == 0) left_border = " ";
                    else left_border = NewRTB.Text.Substring(NewRTB.Text.IndexOf(LPDP_Core.FunctionWord[i], start_index) - 1, 1);

                    if (LPDP_Core.FindWord(NewRTB.Text, LPDP_Core.FunctionWord[i]) == NewRTB.Text.Length) right_border = " ";
                    else right_border = NewRTB.Text.Substring(LPDP_Core.FindWord(NewRTB.Text, LPDP_Core.FunctionWord[i]), 1);


                    NewRTB.SelectionStart = NewRTB.Text.IndexOf(LPDP_Core.FunctionWord[i], start_index);
                    NewRTB.SelectionLength = LPDP_Core.FunctionWord[i].Length;

                    if ((left_border == " ") || (left_border == "\t") || (left_border == "\n") || LPDP_Core.Here_Contains_Any_From(left_border, LPDP_Core.OtherCharacters) &&
                        (right_border == " ") || (right_border == "\t") || (right_border == "\n") || LPDP_Core.Here_Contains_Any_From(right_border, LPDP_Core.OtherCharacters))
                    {
                        NewRTB.Rtf = Colorize_FunctionWord(NewRTB);
                    }
                    start_index = NewRTB.SelectionStart + NewRTB.SelectionLength;
                }
            }

            ReservRTB.SelectAll();
            ReservRTB.Cut();
            RTF_Code = NewRTB.Rtf;

            NewRTB.Dispose(); ReservRTB.Dispose();
            return RTF_Code;
        }

        //RTF парсинг
        public static string ConcatRTF(string strRTF_1, string strRTF_2)
        {
            //для сохранения старого буфера обмена
            RichTextBox ReservRTB = new RichTextBox();
            ReservRTB.Font = new Font("Microsoft Sans Serif", 12);
            ReservRTB.ForeColor = Color.Black;
            ReservRTB.BackColor = Color.White;
            ReservRTB.Paste();

            RichTextBox NewRTB1 = new RichTextBox();
            RichTextBox NewRTB2 = new RichTextBox();
            NewRTB1.Rtf = strRTF_1;
            NewRTB2.Rtf = strRTF_2;
            //Clipboard.SetData(DataFormats.Rtf, strRTF_1);

            RichTextBox resultRTB = new RichTextBox();
            resultRTB.Rtf = strRTF_2;
            if (NewRTB1.Text != "")
            {
                NewRTB1.SelectAll();
                NewRTB1.Copy();
                resultRTB.Paste();
                //resultRTB.SelectionStart = NewRTB1.Text.Length;
                //resultRTB.SelectionLength = 1;
                //resultRTB.Cut();
            }
            string result = resultRTB.Rtf;

            ReservRTB.SelectAll();
            ReservRTB.Copy();

            NewRTB1.Dispose(); NewRTB2.Dispose(); resultRTB.Dispose(); ReservRTB.Dispose();
            return result;
        }
        public static string ConvertToRTF(string str)
        {
            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Font = new Font("Microsoft Sans Serif", 12);
            NewRTB.ForeColor = Color.Black;
            NewRTB.BackColor = Color.White;
            NewRTB.Text = str;
            string strRTF = NewRTB.Rtf;
            NewRTB.Dispose();
            return strRTF;
        }
        public static string ReplaceRTF(string In, string str_1, string str_2)
        {
            //для сохранения старого буфера обмена
            RichTextBox ReservRTB = new RichTextBox();
            ReservRTB.Paste();

            RichTextBox NewRTBIn = new RichTextBox();
            RichTextBox NewRTB2 = new RichTextBox();
            NewRTBIn.Rtf = In;
            string str2_RTF = ConvertToRTF(str_2);

            RichTextBox result = NewRTBIn;
            while (result.Text.Contains(str_1))
            {
                // вырезаем строку до str1
                result.SelectionStart = 0;
                result.SelectionLength = result.Text.IndexOf(str_1);
                result.Cut();
                RichTextBox result1 = new RichTextBox();
                //RichTextBox empty = new RichTextBox();
                //empty.Text = "";

                //вставляем в result1
                result1.Paste();
                //result1.SelectionStart = result1.Text.Length - 1;
                //result1.SelectionLength = 1;
                //result1.Cut();

                //объединяем с нужной
                result1.Rtf = ConcatRTF(result1.Rtf, str2_RTF);

                //вырезаем из result ненужную строку
                result.SelectionStart = 0;
                result.SelectionLength = str_1.Length;
                result.Cut();

                //объединяем новую со старой
                result.Rtf = ConcatRTF(result1.Rtf, result.Rtf);
                result1.Dispose();
            }
            string strRTF = result.Rtf;

            ReservRTB.SelectAll();
            ReservRTB.Copy();
            NewRTBIn.Dispose(); NewRTB2.Dispose(); result.Dispose(); ReservRTB.Dispose();

            return strRTF;
        }

        public static string InsertRTF(string StrRTF, int index, string InsertedStrRTF)
        {
            //для сохранения старого буфера обмена
            RichTextBox ReservRTB = new RichTextBox();
            ReservRTB.Font = new Font("Microsoft Sans Serif", 12);
            ReservRTB.ForeColor = Color.Black;
            ReservRTB.BackColor = Color.White;
            ReservRTB.Paste();

            RichTextBox NewRTB1 = new RichTextBox();
            RichTextBox NewRTB2 = new RichTextBox();
            RichTextBox NewRTB3 = new RichTextBox();
            NewRTB1.Rtf = StrRTF;
            NewRTB1.SelectionStart = index;
            NewRTB1.SelectionLength = NewRTB1.Text.Length - index;
            if (NewRTB1.SelectedText != "")
            {
                NewRTB1.Cut();
                NewRTB3.Paste();
            }

            NewRTB2.Rtf = InsertedStrRTF;

            string result = ConcatRTF(ConcatRTF(NewRTB1.Rtf, NewRTB2.Rtf), NewRTB3.Rtf);

            ReservRTB.SelectAll();
            ReservRTB.Copy();

            NewRTB1.Dispose(); NewRTB2.Dispose(); NewRTB3.Dispose(); ReservRTB.Dispose();
            return result;
        }


        //получение цвета стрелки
        public static Color GetColor_ForPointer(LPDP_Core.record_Queue.condition cond, int init)
        {
            if ((cond == LPDP_Core.record_Queue.condition.True) && (init != -1)) return Color.Green;
            if ((cond == LPDP_Core.record_Queue.condition.Time) && (init != -1)) return Color.Gold;
            if ((cond == LPDP_Core.record_Queue.condition.False) && (init != -1)) return Color.Red;

            if ((cond == LPDP_Core.record_Queue.condition.True) && (init == -1)) return Color.DarkGreen;
            if ((cond == LPDP_Core.record_Queue.condition.Time) && (init == -1)) return Color.Goldenrod;
            if ((cond == LPDP_Core.record_Queue.condition.False) && (init == -1)) return Color.Firebrick;
            return Color.Black;
        }
        // получение RTF стрелки
        public static string GetRTF_Pointers(int subp_ind)
        {
            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Rtf = ConvertToRTF("");

            // если еще нет очередей
            if (LPDP_Core.Queues.Count == 0)
            {
                NewRTB.Text = "\t";
                return NewRTB.Rtf;
            }

            // если нет
            if (LPDP_Core.Queues[subp_ind].Queue.Count == 0)
            {
                NewRTB.Text = "\t";
                return NewRTB.Rtf;
            }

            //для первого
            RichTextBox PointerRTB = new RichTextBox();
            PointerRTB.Font = new Font("Microsoft Sans Serif", 12);
            PointerRTB.BackColor = Color.White;
            PointerRTB.ForeColor = GetColor_ForPointer(LPDP_Core.Queues[subp_ind].Queue[0].Condition, LPDP_Core.Queues[subp_ind].Queue[0].Initiator);
            if (LPDP_Core.Queues[subp_ind].Queue[0].ID == 0) PointerRTB.Text = "\u27A4";
            else PointerRTB.Text = "\u27A1";
            NewRTB.Rtf = ConcatRTF(PointerRTB.Rtf, NewRTB.Rtf);

            if (LPDP_Core.Queues[subp_ind].Queue.Count == 1)
            {
                PointerRTB.Text = "\t";
                NewRTB.Rtf = ConcatRTF(NewRTB.Rtf, PointerRTB.Rtf);
                return NewRTB.Rtf;
            }

            // для второго
            if (LPDP_Core.Queues[subp_ind].Queue.Count > 3)
            {
                PointerRTB.ForeColor = Color.Black;
                PointerRTB.Text = "..";
                NewRTB.Rtf = ConcatRTF(PointerRTB.Rtf, NewRTB.Rtf);
            }
            else
            {
                PointerRTB.ForeColor = GetColor_ForPointer(LPDP_Core.Queues[subp_ind].Queue[1].Condition, LPDP_Core.Queues[subp_ind].Queue[1].Initiator);
                if (LPDP_Core.Queues[subp_ind].Queue[1].ID == 0) PointerRTB.Text = "\u27A4";
                else PointerRTB.Text = "\u27A1";
                NewRTB.Rtf = ConcatRTF(PointerRTB.Rtf, NewRTB.Rtf);
                if (LPDP_Core.Queues[subp_ind].Queue.Count == 2)
                {
                    PointerRTB.Text = "\t";
                    NewRTB.Rtf = ConcatRTF(NewRTB.Rtf, PointerRTB.Rtf);
                    return NewRTB.Rtf;
                }
            }

            //для третьего
            if (LPDP_Core.Queues[subp_ind].Queue.Count > 3)
            {
                int last = LPDP_Core.Queues[subp_ind].Queue.Count - 1;
                PointerRTB.ForeColor = GetColor_ForPointer(LPDP_Core.Queues[subp_ind].Queue[last].Condition, LPDP_Core.Queues[subp_ind].Queue[last].Initiator);
                if (LPDP_Core.Queues[subp_ind].Queue[last].ID == 0) PointerRTB.Text = "\u27A4";
                else PointerRTB.Text = "\u27A1";
                NewRTB.Rtf = ConcatRTF(PointerRTB.Rtf, NewRTB.Rtf);
                PointerRTB.Text = "\t";
                NewRTB.Rtf = ConcatRTF(NewRTB.Rtf, PointerRTB.Rtf);
            }
            else
            {
                PointerRTB.ForeColor = GetColor_ForPointer(LPDP_Core.Queues[subp_ind].Queue[2].Condition, LPDP_Core.Queues[subp_ind].Queue[2].Initiator);
                if (LPDP_Core.Queues[subp_ind].Queue[2].ID == 0) PointerRTB.Text = "\u27A4";
                else PointerRTB.Text = "\u27A1";
                NewRTB.Rtf = ConcatRTF(PointerRTB.Rtf, NewRTB.Rtf);
            }
            //PointerRTB.Text = "\t";
            //NewRTB.Rtf = ConcatRTF(NewRTB.Rtf, PointerRTB.Rtf);

            string srtRTF = NewRTB.Rtf;
            NewRTB.Dispose(); PointerRTB.Dispose();

            return srtRTF;
        }

        public static string PaintCurrent_Initiator_RTF(string RTF_code, Color color, Color color_for_pointer, int unit, int index)
        {
            if ((unit == -1) || (index == -1)) return RTF_code;
            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Rtf = RTF_code;

            int Number_of_line = Units[unit].Algorithm[index].Number;
            int Current_line = 1;
            int start = 0;
            while (Current_line != Number_of_line)
            {
                start = NewRTB.Text.IndexOf('\n', start) + 1;
                Current_line++;
            }
            start = NewRTB.Text.IndexOf('\t', start) + 1;

            for (int i = 0; i < LPDP_Core.POSP_Model.Count; i++)
            {
                int index_of_operation = LPDP_Core.POSP_Model[i].Operations.FindIndex(op => (op.Unit_index == unit) && (op.Line_index == index));
                if (index_of_operation != -1)
                {
                    if (index_of_operation == 0) break;
                    else
                    {
                        NewRTB.Rtf = InsertRTF(NewRTB.Rtf, start, GetRTF_CurrentPointer(color_for_pointer));
                        start++;
                        break;
                    }
                }
            }



            NewRTB.SelectionStart = start;
            if (NewRTB.Text[NewRTB.SelectionStart] == '\t') NewRTB.SelectionStart++;

            while (true)
            {
                //для сложных если и ждать
                if (NewRTB.Text.IndexOf('\n', NewRTB.SelectionStart) < NewRTB.Text.IndexOf(';', NewRTB.SelectionStart))
                {
                    NewRTB.SelectionLength = NewRTB.Text.IndexOf('\n', NewRTB.SelectionStart) - NewRTB.SelectionStart;
                    NewRTB.SelectionBackColor = color;

                    NewRTB.SelectionStart += NewRTB.SelectionLength + 3; // +"\n\t\t"
                }
                else
                {
                    NewRTB.SelectionLength = NewRTB.Text.IndexOf(';', NewRTB.SelectionStart) - NewRTB.SelectionStart + 1;
                    NewRTB.SelectionBackColor = color;
                    break;
                }
            }

            //NewRTB.BackColor = color;
            string result = NewRTB.Rtf;
            NewRTB.Dispose();
            return result;
        }
        public static string GetRTF_CurrentPointer(Color color)
        {
            RichTextBox PointerRTB = new RichTextBox();
            PointerRTB.Font = new Font("Microsoft Sans Serif", 12);
            PointerRTB.BackColor = Color.White;
            PointerRTB.ForeColor = color;
            PointerRTB.Text = "\u27A4";
            return PointerRTB.Rtf;
        }

        public static string ClearFromPointer_RTF(string RTF_Code)
        {
            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Font = new Font("Microsoft Sans Serif", 12);
            NewRTB.ForeColor = Color.Black;
            NewRTB.BackColor = Color.White;
            NewRTB.Rtf = RTF_Code;
            NewRTB.SelectAll();
            //Colorize_OrdinaryWord(NewRTB); //Все обесцвечиваем

            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1\u27A1\u27A1", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1\u27A1\u27A4", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1\u27A4\u27A1", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1\u27A4\u27A4", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4\u27A1\u27A1", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4\u27A1\u27A4", "\t");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4\u27A4\u27A4", "\t");

            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4..\u27A4", "");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1..\u27A4", "");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4..\u27A1", "");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1..\u27A1", "");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A4", "");
            NewRTB.Rtf = ReplaceRTF(NewRTB.Rtf, "\u27A1", "");

            //NewRTB.Rtf = Colorize_RTF_Code(NewRTB.Rtf);

            string result = NewRTB.Rtf;
            NewRTB.Dispose();
            return result;
        }

        public static string ClearFromBackcolor_RTF(string RTF_Code)
        {
            RichTextBox NewRTB = new RichTextBox();
            NewRTB.Font = new Font("Microsoft Sans Serif", 12);
            NewRTB.ForeColor = Color.Black;
            NewRTB.BackColor = Color.White;
            NewRTB.Rtf = RTF_Code;
            NewRTB.SelectAll();
            NewRTB.SelectionBackColor = Color.White;
            string result = NewRTB.Rtf;
            NewRTB.Dispose();
            return result;
        }
    }
}
