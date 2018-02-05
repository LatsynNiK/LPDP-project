using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    public class EmptyTextError:UserError
    {
        //public string Text;
        public EmptyTextError()
        {
            this.Text = "Нет текста модели.";
        }
    }
}
