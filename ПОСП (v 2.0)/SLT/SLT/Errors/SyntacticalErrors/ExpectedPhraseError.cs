using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLT.TextAnalysis;

namespace SLT
{
    public class ExpectedPhraseError:SyntacticalError
    {
        //Phrase phrase;
        public ExpectedPhraseError(Phrase ph):
            base(ph.Start, ph.Length, ph.Line)
        {
            string stack_ph = this.GetStack(ph);
            this.Text = String.Format("Ожидаемая фраза: \"{0}\". Строка {1}", GetStack(ph),ph.Line);          
        }

        string GetStack(Phrase ph)
        {
            if (ph.Value.Count == 0)
            {
                return ph.PhType.ToString();
            }
            else
            {
                return ph.PhType.ToString() + " -> " + this.GetStack(ph.Value[0]);
            }
        }
    }
}
