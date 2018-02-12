using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP
{
    public class ReplacingError:SyntacticalError
    {
        public ReplacingError(PhraseType ph_type, int start, int len, int line) :
            base(start, len, line)
        {
            this.Text = String.Format("На этом месте ожидалось: \"{0}\". Строка {1}", ph_type.ToString(),line);          
        }
    }
}
