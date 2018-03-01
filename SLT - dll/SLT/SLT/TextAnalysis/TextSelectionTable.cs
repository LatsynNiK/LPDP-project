using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    enum TextSelectionType
    {
        KeyWord,
        Comment,
        Error,
        String,
        NextOperator,
        NextAggregateOperator,
        SystemLabel, 
        SystemWord
    }

    struct TextSelection
    {
        public int Start;
        public int Length;
        public TextSelectionType Type;
    }

    class TextSelectionTable
    {
        public List<TextSelection> SelectionList;
        public TextSelectionTable()
        {
            SelectionList = new List<TextSelection>();
        }

        public void Add(int start, int len, TextSelectionType type)
        {
            TextSelection ts =  new TextSelection();
            ts.Start = start;
            ts.Length = len;
            ts.Type = type;
            this.SelectionList.Add(ts);
        }

        public void AddError(Error e)
        {
            if (e.InnerException == null)
            {
                if (e is ErrorList)
                {
                    foreach(Error e2 in ((ErrorList)e).Errors)
                    {
                        this.AddError(e2);
                    }
                }
                else
                {
                    UserError ue = (UserError)e;
                    this.Add(ue.Start,ue.Length,TextSelectionType.Error);
                }
            }
            else
            {
                this.AddError((Error)e.InnerException);
            }
        }

    }
}
