using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

using LPDP.DataSets;
using LPDP.Objects;

namespace ПОСП
{
    static class TextFormat
    {
        static Color next_operator_color = Color.SpringGreen;
        static Color next_operator_aggregat_color = Color.YellowGreen;
        static Color error_color = Color.DarkRed;

        static Color stopped_color = Color.Red;
        static Color waittime_color = Color.Yellow;
        static Color ready_color = Color.Green;

        static Color comment_color = Color.Green;
        static Color string_color = Color.Firebrick;
        static Color keyword_color = Color.Navy;

        static Color initialfont_color = Color.Black;
        static Color initialback_color = Color.White;

        static Color system_label_color = Color.Gray;


        //public static void ColorizeNextOperator(RichTextBox rtb, int start, int len, bool is_flow, int unit_pos)
        //{
        //    Color save_color = rtb.SelectionBackColor; 
        //    rtb.Select(unit_pos, 0);
        //    rtb.ScrollToCaret();

        //    rtb.Select(start, len);
        //    Color color;
        //    if (is_flow)
        //    {
        //        color = next_operator_color;
        //    }
        //    else
        //    {
        //        color = next_operator_aggregat_color;
        //    }
            
        //    rtb.SelectionBackColor = color;
        //    rtb.DeselectAll();

        //    rtb.SelectionBackColor = save_color;
        //    //return rtb;
        //}

        public static void ColorizeTextSelection(RichTextBox rtb, DataTable dt, int unit_pos, bool show_next_oper, bool show_sys_label)
        {
            //Color save_color = rtb.SelectionColor;
            //Color save_bcolor = rtb.SelectionBackColor;
            
            

            bool built = false;

            foreach (DataRow row in dt.Rows)
            {
                int start = Convert.ToInt32(row.ItemArray[0]);
                int len = Convert.ToInt32(row.ItemArray[1]);
                string type = Convert.ToString(row.ItemArray[2]);

                Color font_color = initialfont_color;// rtb.SelectionColor;
                Color back_color = initialback_color;// rtb.SelectionBackColor;
                Font font = new Font(rtb.SelectionFont,FontStyle.Regular);

                switch (type)
                {
                    case "Error":
                        font_color = error_color;// Color.White;
                        //back_color = rtb.SelectionBackColor;// Color.Red;   
                        font = new Font(rtb.SelectionFont, FontStyle.Underline);
                        break;
                    case "Comment":                        
                        font = new Font(rtb.SelectionFont, FontStyle.Italic);
                        font_color = comment_color;
                        break;
                    case "String":
                        //font = new Font(rtb.SelectionFont, FontStyle.Italic);
                        font_color = string_color;
                        break;
                    case "KeyWord":
                        //font = new Font(rtb.SelectionFont, FontStyle.Italic);
                        font_color = keyword_color;
                        font = new Font(rtb.SelectionFont, FontStyle.Bold);
                        break;
                    case "NextAggregateOperator":
                        if (show_next_oper)
                        {
                            rtb.Select(unit_pos, 0);
                            rtb.ScrollToCaret();
                            font_color = initialfont_color;
                            back_color = next_operator_aggregat_color;
                        }
                        built = true;
                        break;
                    
                    case "NextOperator":
                        if (show_next_oper)
                        {
                            rtb.Select(unit_pos, 0);
                            rtb.ScrollToCaret();
                            font_color = initialfont_color;
                            back_color = next_operator_color;
                        }
                        built = true;
                        break;
                    case "SystemLabel":
                        if (show_sys_label)
                        {
                            font_color = system_label_color;
                            //back_color = next_operator_aggregat_color;
                        }
                        break;
                    default:
                        break;
                }
                rtb.SelectionStart = start;
                rtb.SelectionLength = len;
                rtb.SelectionColor = font_color;
                rtb.SelectionBackColor = back_color;
                rtb.SelectionFont = font;                
            }


            if (built == false)
            {
                rtb.Select(unit_pos, 0);
                rtb.ScrollToCaret();
            }

            rtb.DeselectAll();
            rtb.ForeColor = initialfont_color;
            rtb.BackColor = initialback_color;            
        }


        static Dictionary<int, Color> InsertQueueArrows(RichTextBox rtb, DataTable arrs, DataTable text_selections)
        {
            string full_arrow = "\u27A4";
            string half_arrow = "\u27A2";
            string several = "..";

            Dictionary<int, Color> result = new Dictionary<int, Color>();

            //int shifting = 0;
            foreach (DataRow row in arrs.Rows)
            {
                int shifting = 0;
                int position = Convert.ToInt32(row["Position"]);
                position -= 4; // сдвиг для tab
                string[] arrows = new string[3];
                Color[] colors = new Color[3];

                for (int j = 1; j < row.ItemArray.Length; j++)
                {
                    string arrow ="";
                    Color color = initialfont_color;

                    int type = Convert.ToInt32(row[j]);
                    if (type == 0) //None
                    {
                        arrows[j - 1] = arrow;
                        continue;
                    }
                    if (type == 100)
                    {
                        arrows[j - 1] = several;
                        type -= 100;
                        continue;
                    }
                    
                    if (type < 20) //Full
                    {
                        arrow = full_arrow;
                        type -= 10;
                    }
                    else
                    {
                        arrow = half_arrow;
                        type -= 20;
                    }
                    switch (type)
                    {
                        //case 0:
                        //    color = Color.Black;
                        //    break;
                        case 1:
                            color = ready_color;
                            break;
                        case 2:
                            color = waittime_color;
                            break;
                        case 3:
                            color = stopped_color;
                            break;
                        default:
                            color = error_color;
                            break;
                    }
                    arrows[j - 1] = arrow;
                    colors[j - 1] = color;
                }
                for (int i = arrows.Length -1; i >= 0; i--)
                {
                    if (arrows[i].Length > 0)
                    {
                        int new_position = position + shifting;

                        rtb.Text = rtb.Text.Remove(new_position, arrows[i].Length); //удаление пробела для вставки стрелки
                        rtb.Text = rtb.Text.Insert(new_position, arrows[i]);
                        //TextFormat.Shift(text_selections, new_position);
                        shifting += arrows[i].Length;
                        result.Add(new_position, colors[i]);
                    }
                }
                

                //result

                //for (int i = colors.Length -1; i >= 0; i--)
                //{
                //    rtb.Select(new_position, arrows[i].Length);
                //    rtb.SelectionColor = colors[i];
                //    rtb.DeselectAll();
                //    new_position += arrows[i].Length;
                //}
            }
            return result;
        }

        public static void InsertSystemLabel(RichTextBox rtb, DataTable sys_label_tabel, DataTable text_selection)
        {
            foreach (DataRow row in sys_label_tabel.Rows)
            {
                string name = row["Name"].ToString()+":";
                int position = Convert.ToInt32( row["Position"].ToString());
                position -= 4*3; // 3 tab 
                rtb.Text = rtb.Text.Remove(position,name.Length);
                rtb.Text = rtb.Text.Insert(position, name);

                DataRow new_row = text_selection.NewRow();
                new_row["Start"] = position;
                
                new_row["Length"] = name.Length;
                new_row["Type"] = "SystemLabel";
                text_selection.Rows.InsertAt(new_row,0);
            }
        }

        public static void InsertColorQueueArrows(RichTextBox rtb, DataTable arrows, DataTable text_selections)//, int start)
        {
            Dictionary<int, Color> dict = InsertQueueArrows(rtb, arrows, text_selections);

            //int shifting = 0;
            int save_cursor = rtb.SelectionStart;
            foreach (int position in dict.Keys)
            {
                //if (position <= start + shifting)
                //{
                //    shifting++;
                //}
                ////else
                ////{

                ////}
                //text_selections

                rtb.Select(position, 1);
                rtb.SelectionColor = dict[position];
            }
            rtb.Select(save_cursor, 0);
            //return shifting;
            rtb.DeselectAll();
        }

        public static void UseCaptions(DataGridView dgv, DataTable dt)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderText = dt.Columns[col.Name].Caption;
            }
        }
        
        //public static void Shift(DataTable text_sel, int start)
        //{
        //    foreach (DataRow row in text_sel.Rows)
        //    {
        //        if (Convert.ToInt32(row[0]) >= start)
        //        {
        //            row[0] = Convert.ToInt32(row[0]) + 1;
        //        }
        //    }
        //}        
    }
}
