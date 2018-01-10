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


        public static void ColorizeNextOperator(RichTextBox rtb, int start, int len, bool is_flow, int unit_pos)
        {

            rtb.Select(unit_pos, 0);
            rtb.ScrollToCaret();

            rtb.Select(start, len);
            Color color;
            if (is_flow)
            {
                color = next_operator_color;
            }
            else
            {
                color = next_operator_aggregat_color;
            }
            
            rtb.SelectionBackColor = color;
            rtb.DeselectAll();
            //return rtb;
        }

        static Dictionary<int,Color> InsertQueueArrows(RichTextBox rtb, DataTable dt)
        {
            string full_arrow = "\u27A4";
            string half_arrow = "\u27A2";
            string several = "-";

            Dictionary<int, Color> result = new Dictionary<int, Color>();

            int shifting = 0;
            foreach (DataRow row in dt.Rows)
            {
                int position = Convert.ToInt32(row["Position"]);
                string[] arrows = new string[3];
                Color[] colors = new Color[3];

                for (int j = 1; j < row.ItemArray.Length; j++)
                {
                    string arrow ="";
                    Color color = Color.Black;

                    int type = Convert.ToInt32(row[j]);
                    if (type == 0) //None
                    {
                        arrows[j - 1] = arrow;
                        continue;
                    }
                    if (type == 100)
                    {
                        arrow = several;
                        type -= 100;
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
                        case 0:
                            color = Color.Black;
                            break;
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
                        rtb.Text = rtb.Text.Insert(new_position, arrows[i]);
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

        public static int InsertColorQueueArrows(RichTextBox rtb,DataTable dt, int start)
        {
            Dictionary<int, Color> dict = InsertQueueArrows(rtb, dt);

            int shifting = 0;
            int save_cursor = rtb.SelectionStart;
            foreach (int position in dict.Keys)
            {
                if (position <= start + shifting)
                {
                    shifting++;
                }
                //else
                //{

                //}

                rtb.Select(position, 1);
                rtb.SelectionColor = dict[position];
            }
            rtb.Select(save_cursor, 0);
            return shifting;
        }

        public static void UseCaptions(DataGridView dgv, DataTable dt)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderText = dt.Columns[col.Name].Caption;
            }
        }
        

        //static void InsertArrow(RichTextBox rtb, int position, string arrow, Color color)
        //{
        //    int new_position = position;
        //    //if (rtb.Text[position - 1] == '\t')
        //    //{
        //    //    new_position = position - 1;
        //    //}
        //    rtb.Text. = rtb.Text.Insert(new_position, arrow);
        //    rtb.Select(new_position, arrow.Length);
        //    rtb.SelectionColor = color;
        //    rtb.DeselectAll();
        //}
        //        static void InsertArrow(RichTextBox rtb, int position, string arrow, Color color)
        //{
        //    int new_position = position;
        //    //if (rtb.Text[position - 1] == '\t')
        //    //{
        //    //    new_position = position - 1;
        //    //}
        //    rtb.Text. = rtb.Text.Insert(new_position, arrow);
        //    rtb.Select(new_position, arrow.Length);
        //    rtb.SelectionColor = color;
        //    rtb.DeselectAll();
        //}
    }
}
