using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    class TextAnalysis
    {
        public string SourceText;
        public List<Error> Errors;
        public List<Lexeme> Lexemes;
        //List<Phrase> Phrases;
        public Phrase ParsedText;
        public string ResultTxtCode;
        public string ResultRTFCode;

        struct Response
        {
            public Phrase BackPhrase;
            public ScanningState ScanState;
            public List<Error> Finded_Errors;
            public int ConcattedLexemes;
        }

        Error CurrentError;

        public TextAnalysis(string SourceText)
        {
            this.SourceText = SourceText;
            //this.Errors = new List<Error>();
            
            // Пустой текст модели
            //if (SourceText == "")
            //{
            //    Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.EmptyText, LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.EmptyText], 0, 0, 0));
            //}

            this.Lexemes = LexicalAnalysis(this.SourceText);
            //this.ParsedText = SyntacticalAnalysis_Preparation(this.Lexemes);

            //this.ParsedText = new List<Phrase>();
            //foreach (Lexeme lex in this.Lexemes)
            //{
            //    this.ParsedText.Add(lex);
            //}
            this.ParsedText = SyntacticalAnalysis(this.Lexemes);

            this.ParsedText = MadeStruct(this.ParsedText);

            this.ResultTxtCode = "";
            this.ResultRTFCode = "";
        }

        List<Lexeme> LexicalAnalysis(string Text)
        {
            Text = Text.Replace('ё', 'е');
            List<Lexeme> Stack = new List<Lexeme>();
            if (Text.Length == 0)
            {
                Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.EmptyText, LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.EmptyText], 0, 0, 0));
                return Stack;
            }
            int line_counter = 1;
            int position = 0;

            while (Text.Length != 0)
            {
                char ch = Text[0];
                if (LPDP_ModelTextRules.DetermineSymbol(ch) == LPDP_ModelTextRules.LexemeType.Unknown_Symbol)
                {
                    Stack.Add(new Lexeme(LPDP_ModelTextRules.LexemeType.Unknown_Symbol, "" + ch, line_counter, position, 1));
                    Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.UnknownSimbol, LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.UnknownSimbol] + ch, line_counter, position, 1));
                }
                else
                {
                    Stack.Add(new Lexeme(LPDP_ModelTextRules.DetermineSymbol(ch), "" + ch, line_counter, position, 1));
                    if (LPDP_ModelTextRules.DetermineSymbol(ch) == LPDP_ModelTextRules.LexemeType.Enter)
                    {
                        line_counter++;
                    }
                }
                Text = Text.Remove(0, 1);
                position++;
                bool modified = true;
                while (modified)
                {
                    modified = false;
                    int start;
                    foreach (LexemeTypeTemplate temp in LPDP_ModelTextRules.LexicalTemplates)
                    {
                        bool to_concat = true;
                        start = Stack.Count - temp.ConcatedLexemes;
                        if (start < 0)
                        {
                            continue;
                        }
                        List<Lexeme> LexemeList = new List<Lexeme>();
                        for (int i = 0; i < temp.ConcatedLexemes; i++)
                        {
                            if ((Stack[start + i].LType == temp.LTemplate[i]) ||
                                (temp.LTemplate[i] == LPDP_ModelTextRules.LexemeType.Anything))
                            {
                                LexemeList.Add(Stack[start + i]);
                            }
                            else
                            {
                                to_concat = false;
                                break;
                            }
                        }
                        if (to_concat)
                        {
                            Lexeme NewLexeme = new Lexeme(temp.LType,
                                    LexemeList[0].LValue,
                                    LexemeList[0].Line,
                                    LexemeList[0].Start,
                                    LexemeList[0].Length);
                            for (int j = 1;j<LexemeList.Count;j++)
                            {
                                NewLexeme = new Lexeme(
                                    temp.LType,
                                    NewLexeme.LValue + LexemeList[j].LValue,
                                    NewLexeme.Line,
                                    NewLexeme.Start,
                                    NewLexeme.Length + LexemeList[j].Length);
                            }
                            Stack.RemoveRange(start, temp.ConcatedLexemes);
                            Stack.Add(NewLexeme);
                            modified = true;
                            break;
                        }
                    } 
                }
                if (Stack[Stack.Count-1].LType == LPDP_ModelTextRules.LexemeType.Word)
                {
                    Word NewWord;
                    LPDP_ModelTextRules.PhraseType PhType = LPDP_ModelTextRules.DetermineWord(Stack[Stack.Count-1]);
                    switch (PhType)
                    {
                        case LPDP_ModelTextRules.PhraseType.Time_Word:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.SystemVar,Stack[Stack.Count-1]);
                            break;
                        case LPDP_ModelTextRules.PhraseType.Initiator_Word:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.SystemVar,Stack[Stack.Count-1]);
                            break;
                        case LPDP_ModelTextRules.PhraseType.Rand_Word:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.SystemVar,Stack[Stack.Count-1]);
                            break;
                        case LPDP_ModelTextRules.PhraseType.Name:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.Name,Stack[Stack.Count-1]);
                            break;
                        case LPDP_ModelTextRules.PhraseType.ArithmeticFunction_Word:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.ArithmeticFunction, Stack[Stack.Count-1]);
                            break;
                        default:
                            NewWord = new Word(LPDP_ModelTextRules.WordType.KeyWord, Stack[Stack.Count-1]);
                            break;
                    }
                    Stack.RemoveAt(Stack.Count - 1);
                    Stack.Add(NewWord);
                }
            }

            return Stack;
        }

        //**************
        enum ScanningState
        {
            WithoutErrors,
            WithErrors,
            Initial,
            Fallen
        }
        Response FindTemplate(PhraseTypeTemplate template, List<Phrase> Phrases)
        {
            Response result = new Response(); //template.PhType == LPDP.LPDP_ModelTextRules.PhraseType.Algorithm
            result.Finded_Errors = new List<Error>();
            result.ScanState = ScanningState.Initial;
            result.ConcattedLexemes = 0;

            Phrase[] ListNewPhrase = new Phrase[template.ConcatedPhrases];
            for (int i = 0; i < template.ConcatedPhrases; i++)
            {
                if (Phrases.Count == 0)
                {
                    result.BackPhrase = null;
                    //////*****
                    //int ii;
                    //for (ii = 0; ii < ListNewPhrase.Length; ii++)
                    //{
                    //    if (ListNewPhrase[ii] == null)
                    //    {
                    //        break;
                    //    }
                    //}
                    //Phrase[] ErrListNewPhrase = new Phrase[ii];
                    //for (int ei = 0; ei < ErrListNewPhrase.Length; ei++)
                    //{
                    //    ErrListNewPhrase[ei] = ListNewPhrase[ei];
                    //}
                    //Request.BackPhrase = new Phrase(template.PhType, ErrListNewPhrase);
                    //////*****

                    result.Finded_Errors = new List<Error>();
                    result.Finded_Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.ExpectedPhrase,
                        LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.ExpectedPhrase] + template.PhType.ToString(),
                        //ListNewPhrase[i-1].Line,ListNewPhrase[i-1].Start,ListNewPhrase[i-1].Length
                        0, 0, 0
                        ));
                    result.ScanState = ScanningState.Fallen;
                    return result;
                }


                //если терминал (конечное значение, уровня 0)
                if (LPDP_ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])
                {
                    if (Phrases[0].PhType == template.PhTemplate[i])
                    {
                        ListNewPhrase[i] = Phrases[0];
                        result.ConcattedLexemes++;
                        Phrases.RemoveAt(0);
                        if (result.ScanState == ScanningState.Initial) //для первого
                            result.ScanState = ScanningState.WithoutErrors;
                        //part_finded = true;
                        continue;
                    }
                    else
                    {
                        //
                        List<Phrase> PhrasesCopy = new List<Phrase>();
                        foreach (Phrase ph in Phrases)
                        {
                            PhrasesCopy.Add(ph);
                        }

                        if (TryToReplace(PhrasesCopy, i, template))
                        {
                            result.Finded_Errors = new List<Error>();
                            result.Finded_Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.Replacing,
                                LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.Replacing] + template.PhTemplate[i].ToString(),
                                Phrases[0].Line, Phrases[0].Start, Phrases[0].Length));

                            ListNewPhrase[i] = Phrases[0];
                            result.ConcattedLexemes++;
                            Phrases.RemoveAt(0);
                            result.ScanState = ScanningState.WithErrors;
                            continue;
                        }
                        //

                        result.BackPhrase = null;

                        //////*****
                        //int ii;
                        //for (ii = 0; ii < ListNewPhrase.Length; ii++)
                        //{
                        //    if (ListNewPhrase[ii] == null)
                        //    {
                        //        break;
                        //    }
                        //}
                        //Phrase[] ErrListNewPhrase = new Phrase[ii];
                        //for (int ei = 0; ei < ErrListNewPhrase.Length; ei++)
                        //{
                        //    ErrListNewPhrase[ei] = ListNewPhrase[ei];
                        //}
                        //result.BackPhrase = new Phrase(template.PhType, ErrListNewPhrase);
                        //////*****

                        result.Finded_Errors = new List<Error>();
                        result.Finded_Errors.Add( new Error (LPDP_ModelTextRules.ErrorType.ExpectedPhrase,
                            LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.ExpectedPhrase] +"111"+ template.PhTemplate[i].ToString(),
                            Phrases[0].Line,Phrases[0].Start,Phrases[0].Length));
                        result.ScanState = ScanningState.Fallen;
                        return result;
                    }
                }
                //если нетерминал (составное значение)
                else
                {
                    //bool part_finded = false;
                    Response BestRequest = new Response();
                    BestRequest.ScanState = ScanningState.Initial;
                    BestRequest.Finded_Errors = new List<Error>();
                    
                    if (Phrases.Count == 0)
                    {
                        result.BackPhrase = null;
                            //////*****
                            //int ii;
                            //for (ii = 0; ii < ListNewPhrase.Length; ii++)
                            //{
                            //    if (ListNewPhrase[ii] == null)
                            //    {
                            //        break;
                            //    }
                            //}
                            //Phrase[] ErrListNewPhrase = new Phrase[ii];
                            //for (int ei = 0; ei < ErrListNewPhrase.Length; ei++)
                            //{
                            //    ErrListNewPhrase[ei] = ListNewPhrase[ei];
                            //}
                            //Request.BackPhrase = new Phrase(template.PhType, ErrListNewPhrase);
                            //////*****
                        
                        result.Finded_Errors = new List<Error>();
                        result.Finded_Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.ExpectedPhrase,
                            LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.ExpectedPhrase]+template.PhType.ToString(),
                            //ListNewPhrase[i-1].Line,ListNewPhrase[i-1].Start,ListNewPhrase[i-1].Length
                            0,0,0
                            ));
                        result.ScanState = ScanningState.Fallen;
                        return result;
                    }

                    foreach (PhraseTypeTemplate temp_part in LPDP_ModelTextRules.SyntacticalTemplates)
                    {
                        Response Request = new Response();
                        if (temp_part.PhType == template.PhTemplate[i])  //if (temp_part.PhType == LPDP_ModelTextRules.PhraseType.Var)
                        {
                            List<Phrase> PhrasesCopy = new List<Phrase>();
                            foreach (Phrase ph in Phrases)
                            {
                                PhrasesCopy.Add(ph);
                            }

                            Request = FindTemplate(temp_part, PhrasesCopy);

                            bool br_circle = false;
                            switch (Request.ScanState)
                            {
                                case ScanningState.WithoutErrors:
                                    //part_finded = true;
                                    br_circle = true;
                                    break;
                                case ScanningState.Fallen:
                                    //result.Finded_Errors = Request.Finded_Errors;
                                    break;
                                case ScanningState.WithErrors:

                                    //part_finded = true;
                                    break;
                            }

                            if (BestRequest.ScanState == ScanningState.Initial) //для первого
                            {
                                BestRequest = Request;
                            }

                            if ((int)Request.ScanState < (int)BestRequest.ScanState)
                            {
                                if ((BestRequest.ScanState == ScanningState.WithErrors) && (BestRequest.ConcattedLexemes > Request.ConcattedLexemes))
                                { }
                                else
                                {
                                    BestRequest = Request;
                                    //if (Request.ScanState == ScanningState.Initial)
                                    //{
                                    //    foreach (Error err in Request.Finded_Errors)
                                    //        BestRequest.Finded_Errors.Add(err);
                                    //}
                                    //BestRequest.BackPhrase = Request.BackPhrase;
                                    //BestRequest.ConcattedLexemes = Request.ConcattedLexemes;
                                    //BestRequest.ScanState = Request.ScanState;
                                }
                                if (br_circle) break;
                               
                            }
                            if ((Request.ScanState == BestRequest.ScanState) && (Request.ScanState == ScanningState.WithErrors))
                            {
                                if (Request.Finded_Errors.Count < BestRequest.Finded_Errors.Count)
                                {
                                    if (BestRequest.ConcattedLexemes > Request.ConcattedLexemes)
                                    { }
                                    else
                                    {
                                        BestRequest = Request;
                                    }
                                }
                            }
                        }
                    }
                    //if (part_finded == false) //если не нашли шаблон
                    if (BestRequest.ScanState == ScanningState.Fallen)
                    {
                        result.BackPhrase = null;

                        //////*****
                        //int ii;
                        //for (ii = 0; ii < ListNewPhrase.Length; ii++)
                        //{
                        //    if (ListNewPhrase[ii] == null)
                        //    {
                        //        break;
                        //    }
                        //}
                        //Phrase[] ErrListNewPhrase = new Phrase[ii];
                        //for (int ei = 0; ei < ErrListNewPhrase.Length; ei++)
                        //{
                        //    ErrListNewPhrase[ei] = ListNewPhrase[ei];
                        //}
                        //result.BackPhrase = new Phrase(template.PhType, ErrListNewPhrase);
                        //////*****

                        if (result.Finded_Errors == null)
                        {
                            if (Phrases.Count == 0) { }
                            else
                            {
                                result.Finded_Errors = new List<Error>();
                                result.Finded_Errors.Add( new Error(LPDP_ModelTextRules.ErrorType.ExpectedPhrase,
                                    LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.ExpectedPhrase] + template.PhTemplate[i].ToString(),
                                    Phrases[0].Line, Phrases[0].Start, Phrases[0].Length));
                            }
                        }
                        else
                        {
                            result.Finded_Errors = BestRequest.Finded_Errors;
                        }
                        result.ScanState = ScanningState.Fallen;
                        return result;
                    }

                    //вставляем наиболее подходящий
                    ListNewPhrase[i] = BestRequest.BackPhrase;
                    result.ConcattedLexemes += BestRequest.ConcattedLexemes;

                    if (BestRequest.ScanState == ScanningState.Initial) //если лучший пустой, то он безошибочный
                    {
                        BestRequest.ScanState = ScanningState.WithoutErrors;
                    }

                    if (result.ScanState == ScanningState.Initial) //для первого
                    {
                        result.ScanState = BestRequest.ScanState;
                    }

                    if ((int)BestRequest.ScanState > (int)result.ScanState) //если стал хуже
                    {
                        result.ScanState = BestRequest.ScanState;
                    }

                    foreach (Error err in BestRequest.Finded_Errors)
                    {
                        result.Finded_Errors.Add(err);
                    }
                    Phrases.RemoveRange(0, BestRequest.ConcattedLexemes);
                }
            }
            result.BackPhrase = new Phrase(template.PhType, ListNewPhrase);

            //if (result.ScanState == ScanningState.Initial) //если пустой шаблон
            //{
            //    result.ScanState = ScanningState.WithoutErrors;
            //}
            Phrases = null;
            return result;
        }
        //List<Phrase> TryToCorrect(List<Phrase> Phrases,int corrected_index, PhraseTypeTemplate temp)
        //{
        //    //List<Phrase> PhrasesCopy = new List<Phrase>();
        //    //foreach (Phrase ph in Phrases)
        //    //{
        //    //    PhrasesCopy.Add(ph);
        //    //}
        //    if (TryToReplace(Phrases, corrected_index, temp))
        //    {
        //        //Phrases.RemoveAt();
        //    }


        //    return Phrases;
        //}
        bool TryToReplace(List<Phrase> Phrases, int corrected_index, PhraseTypeTemplate temp)
        {
            if ((Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.EoL)
                ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.Round_Bracket_Open) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.Round_Bracket_Close) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.Comma) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.TypeSeparator) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.ArithmeticOperator_3lvl) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.ArithmeticOperator_2lvl) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.ArithmeticOperator_1lvl) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.ComparisonOperator) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.LogicOperator) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.ArithmeticFunction_Word) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.AssignOperator_Word) ||
                (Phrases[0].PhType == LPDP_ModelTextRules.PhraseType.MarkSeparator)
                )
            {
                return false;
            }


            Phrases.RemoveAt(0);
            Phrases.Insert(0, new Phrase(temp.PhTemplate[corrected_index]));

            int to_concat =0;
            return CheckCorrect(Phrases, corrected_index, temp, ref to_concat);

            //for (int i = corrected_index; i < temp.ConcatedPhrases; i++)
            //{
            //    if (LPDP_ModelTextRules.PrimaryPhraseTypes[temp.PhTemplate[i]])
            //    {
            //        if (PhrasesCopy[i].PhType != temp.PhTemplate[i])
            //        {
            //            return false;
            //        }
            //    }
            //    else 
            //    {
            //        TryToReplace
            //    }
            //}
            //return true;
        }
        bool CheckCorrect(List<Phrase> Phrases, int start_index, PhraseTypeTemplate template, ref int concatted_lex)
        {
            concatted_lex = 0;
            for (int i = start_index; i < template.ConcatedPhrases; i++)
            {
                if (Phrases.Count == 0)
                    return false;
                //терминал
                if (LPDP_ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])  
                {
                    if (Phrases[0].PhType == template.PhTemplate[i])
                    {
                        Phrases.RemoveAt(0);
                        concatted_lex++;
                    }
                    else
                    {
                        return false;
                    }
                }
                //нетерминал
                else 
                {
                    bool part_finded = false;
                    //Response response = new Response();
                    PhraseTypeTemplate best_temp = LPDP_ModelTextRules.SyntacticalTemplates[0];
                    foreach (PhraseTypeTemplate temp_part in LPDP_ModelTextRules.SyntacticalTemplates)
                    {
                        if (temp_part.PhType == template.PhTemplate[i]) //if (temp_part.PhType == LPDP_ModelTextRules.PhraseType.ActivateOperator )
                        {
                            List<Phrase> PhrasesCopy = new List<Phrase>();
                            foreach (Phrase ph in Phrases)
                            {
                                PhrasesCopy.Add(ph);
                            }
                            //Response res_resp = FindTemplate(template, PhrasesCopy);
                            //if ((res_resp.ScanState == ScanningState.WithoutErrors) /*|| (res_resp.ScanState == ScanningState.WithoutErrors)*/)
                            //response =FindTemplate(temp_part, PhrasesCopy);
                            //if (response.ScanState== ScanningState.WithoutErrors)
                            int internal_concatted_lex =0;
                            if (CheckCorrect(PhrasesCopy, 0, temp_part, ref  internal_concatted_lex))
                            {
                                best_temp = temp_part;
                                part_finded = true;
                                Phrases.RemoveRange(0, internal_concatted_lex);
                                concatted_lex += internal_concatted_lex;
                                break;
                             }
                        }
                    }
                    if (part_finded == false)
                    {
                        return false;
                    }
                    //else
                    //{
                        
                    //}
                }
            }
            return true;
        }

        Phrase SyntacticalAnalysis(List<Lexeme> Lexemes)
        {
            List<Phrase> Phrases = new List<Phrase>();
            this.Errors = new List<Error>();
            foreach (Lexeme lex in Lexemes)
            {
                lex.PhType = LPDP_ModelTextRules.DeterminePhrase(lex);
                if (lex.PhType == LPDP_ModelTextRules.PhraseType.UnknownLexeme)
                {
                    Errors.Add(new Error(LPDP_ModelTextRules.ErrorType.UnknownLexeme, LPDP_ModelTextRules.ErrorTypes[LPDP_ModelTextRules.ErrorType.UnknownLexeme] + lex.Value, lex.Line, lex.Start, lex.Length));
                }
                if ((lex.PhType != LPDP_ModelTextRules.PhraseType.Empty) && (lex.PhType != LPDP_ModelTextRules.PhraseType.Comment))
                Phrases.Add((Phrase)lex);
            }

            Response resp = FindTemplate(LPDP_ModelTextRules.SyntacticalTemplates[0], Phrases);

            return resp.BackPhrase;
 
        }

        //******************
        Phrase MadeStruct(Phrase InitialPhrase/*, LPDP_ModelTextRules.PhraseType BasePhraseType*/)
        {
            for (int i=0;i<InitialPhrase.Value.Count;i++)// (Phrase ph in InitialPhrase.Value)
            {
                if (InitialPhrase.Value[i] is Lexeme)
                    continue;
                else
                {
                    InitialPhrase.Value[i] = MadeStruct(InitialPhrase.Value[i]);
                    if (InitialPhrase.Value[i].PhType == InitialPhrase.PhType)
                    {
                        int index = InitialPhrase.Value.IndexOf(InitialPhrase.Value[i]);
                        InitialPhrase.Value.InsertRange(index + 1, InitialPhrase.Value[index].Value);
                        InitialPhrase.Value.RemoveAt(index);
                    }
                }
            }
            return InitialPhrase;
        }
    }


    // Лексический анализ
    //class Literal
    //{    
    //    public enum LiteralType
    //    {
    //        Letter,
    //        Simbol,
    //        Numeral
    //    }
    //    public LiteralType Type;
    //    public char Value;
    //}

    class Lexeme : Phrase
    {
        public LPDP_ModelTextRules.LexemeType LType;
        public string LValue;
        //public int Line;
        //public int Index;

        //public int Start;
        //public int Length;

        public Lexeme()
        {
        }

        public Lexeme(LPDP_ModelTextRules.LexemeType type, string value, int line, int start, int length)
            : base(LPDP_ModelTextRules.PhraseType.UnknownLexeme)
        {
            base.Start = start;
            base.Length = length;
            base.Line = line;

            base.Value = null;
            base.Level = 0;
            //    this.Value.Add(lexeme);

            this.LType = type;
            this.LValue = value;
            
        }
    }

    //class Word : Lexeme
    //{
    //    public enum WordType
    //    {
    //        VarName,
    //        UnitType,
    //        SysVar,
    //        KeyWord,
    //        MacrosWord
    //    }
    //    public WordType WType;

    //    public Word(Lexeme lex, WordType type)
    //    {
 
    //    }
    //}

    class LexemeTypeTemplate
    {
        public LPDP_ModelTextRules.LexemeType LType;
        public LPDP_ModelTextRules.LexemeType[] LTemplate;
        public int ConcatedLexemes;

        public LexemeTypeTemplate(LPDP_ModelTextRules.LexemeType type, params LPDP_ModelTextRules.LexemeType[] template)
        {
            this.LType = type;
            this.LTemplate = template;
            this.ConcatedLexemes = template.Length;
        }
    }

    
    // Синтаксический анализ
    class Phrase
    {
        public LPDP_ModelTextRules.PhraseType PhType;
        public List<Phrase> Value;

        public int Level;
        public int Start;
        public int Length;
        public int Line;

        public Phrase() { }

        //public Phrase(LPDP_ModelTextRules.PhraseType type, int level, int start, int length, Phrase lexeme)
        //{
        //    this.PhType = type;
        //    this.Level = level;
        //    this.Start = start;
        //    this.Length = length;

        //    this.Value = new List<Phrase>();
        //    this.Value.Add(lexeme);
        //}

        public Phrase (LPDP_ModelTextRules.PhraseType type, params Phrase[] ConcatedPhrase)
        {
            this.PhType = type;
            if (ConcatedPhrase.Length == 0)
            {
                this.Level = -1;
            }
            else
            {
                this.Level = FindDeep(ConcatedPhrase);
            }
            if (ConcatedPhrase.Length != 0)
            {
                this.Start = ConcatedPhrase[0].Start;
                this.Line = ConcatedPhrase[0].Line;
            }
            this.Length = FindLength(ConcatedPhrase);
            

            this.Value = new List<Phrase>();
            foreach (Phrase ph in ConcatedPhrase)
            {
                this.Value.Add(ph);
            }
            
        }

        int FindDeep(params Phrase[] ConcatedPhrase)
        {
            int max_level = 0;
            foreach (Phrase ph in ConcatedPhrase)
            {
                if (ph.Level > max_level)
                    max_level = ph.Level;
            }
            return max_level + 1;
        }
        int FindLength(params Phrase[] ConcatedPhrase)
        {
            int length = 0;
            foreach (Phrase ph in ConcatedPhrase)
            {
                length += ph.Length;
            }
            return length;
        }
    }

    class PhraseTypeTemplate
    {
        public LPDP_ModelTextRules.PhraseType PhType;
        public LPDP_ModelTextRules.PhraseType[] PhTemplate;
        
        public int ConcatedPhrases;

        public PhraseTypeTemplate(LPDP_ModelTextRules.PhraseType type, params LPDP_ModelTextRules.PhraseType[] template)
        {
            this.PhType = type;
            this.PhTemplate = template;
            this.ConcatedPhrases = template.Length;
        }
    }

    class Word : Lexeme
    {
        public LPDP_ModelTextRules.WordType WType;
        public Word(LPDP_ModelTextRules.WordType type, Lexeme lex)
        {
            base.Start = lex.Start;
            base.Length = lex.Length;
            base.Line = lex.Line;

            base.Value = lex.Value;
            base.Level = lex.Level;
            //    this.Value.Add(lexeme);

            base.LType = lex.LType;
            base.LValue = lex.LValue;

            this.WType = type;
        }
    }

    // Ошибки
    class Error
    {
        public LPDP_ModelTextRules.ErrorType Type;
        public string ErrorText;
        public int Line;
        public int Start;
        public int Length;


        public Error(LPDP_ModelTextRules.ErrorType type, string err_txt, int line, int start, int len)
        {
            this.Type = type;
            this.ErrorText = err_txt;
            this.Line = line;
            this.Start = start;
            this.Length = len;
        }
    }
}
