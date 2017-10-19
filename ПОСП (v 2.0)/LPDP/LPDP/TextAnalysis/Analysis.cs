﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.Structure;
using LPDP.Objects;

namespace LPDP.TextAnalysis
{
    public class Analysis
    {
        public string SourceText;
        public List<Error> Errors;
        List<Lexeme> Lexemes;
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

        public Model ResultModel;

        public Analysis()
        {
            this.SourceText = "";
            this.Errors = new List<Error>();
            this.Lexemes = new List<Lexeme>();
            this.ParsedText = new Phrase();
            this.ResultRTFCode = "";
            this.ResultTxtCode = "";
        }

        public int AnalyzeText(string SourceText) // возвращает 1 если успешно
        {
            this.SourceText = SourceText;
            //this.Errors = new List<Error>();

            // Пустой текст модели
            //if (SourceText == "")
            //{
            //    Errors.Add(new Error(ErrorType.EmptyText, ModelTextRules.ErrorTypes[ErrorType.EmptyText], 0, 0, 0));
            //}

            this.Lexemes = LexicalAnalysis(this.SourceText);
            if (Errors.Count > 0)
            {
                return 0;
            }

            //this.ParsedText = SyntacticalAnalysis_Preparation(this.Lexemes);

            //this.ParsedText = new List<Phrase>();
            //foreach (Lexeme lex in this.Lexemes)
            //{
            //    this.ParsedText.Add(lex);
            //}
            this.ParsedText = SyntacticalAnalysis(this.Lexemes);
            if (Errors.Count > 0)
            {
                return 0;
            }
            this.ParsedText = MadeStruct(this.ParsedText);
            if (Errors.Count > 0)
            {
                return 0;
            }

            this.ResultTxtCode = "";
            this.ResultRTFCode = "";
            return 1;
        }

        public int AnalyzeStructure(Phrase source_phrase)
        {
            switch (source_phrase.PhType)
            {
                //Создание пустой модели
                case PhraseType.Model:
                    this.ResultModel = new Model(EjectStringName(source_phrase));

                    Phrase units_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.Units);
                    foreach (Phrase unit_ph in units_ph.Value)
                    {
                        AnalyzeStructure(unit_ph);
                    }
                    break;

                case PhraseType.Unit:
                    this.ResultModel.ST_Cont.CreateUnit();
                    foreach (Phrase unit_part_ph in source_phrase.Value)
                    {
                        AnalyzeStructure(unit_part_ph);
                    }
                    break;

                case PhraseType.UnitHeader:
                    Phrase unit_type_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.UnitType_Word);
                    Word unit_type_w = (Word) unit_type_ph;
                    string unit_type_s = unit_type_w.LValue;
                    UnitType u_type;
                    switch (unit_type_s)
                    {
                        case "контроллер":
                            u_type = UnitType.Controller;
                            break;    
                        case "процессор":
                            u_type = UnitType.Processor;
                            break;
                        case "агрегат":
                            u_type = UnitType.Aggregate;
                            break;
                        default:
                            u_type = UnitType.Processor;
                            break;
                    }
                    this.ResultModel.ST_Cont.SetUnitHeader(EjectStringName(source_phrase), u_type);
                    break;

                case PhraseType.Description:
                    Phrase descriptionlines_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.DescriptionLines);
                    foreach (Phrase descriptionline_ph in descriptionlines_ph.Value)
                    {
                        AnalyzeStructure(descriptionline_ph);
                    }
                    break;

                case PhraseType.DescriptionLine:
                    List<Objects.Object> list_obj = this.CreateObjectsFromDesciptionLine(source_phrase);
                    foreach (Objects.Object o in list_obj)
                    {
                        this.ResultModel.O_Cont.AddObject(o);
                    }
                    break;

                case PhraseType.Algorithm:
                    Phrase algorithmlines_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.AlgorithmLines);
                    foreach (Phrase algorithmline_ph in algorithmlines_ph.Value)
                    {
                        AnalyzeStructure(algorithmline_ph);
                    }
                    break;

                case PhraseType.AlgorithmLine:
                    if (source_phrase.Value.Exists(ph => ph.PhType == PhraseType.Label))
                    {
                        Phrase label_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.Label);
                        string label_name = EjectStringName(label_ph);
                        Subprogram NewSubp = new Subprogram();
                        this.ResultModel.ST_Cont.AddSubprogram(NewSubp);
                        Label NewLabel = new Label(label_name, true);
                    }

                    Phrase operator_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.Operator);

                    Operator oper = create_operator_from_phrase(operator_ph.Value[0]);
                    this.ResultModel.ST_Cont.AddOperator(oper);
                    break;


            }
            return 1;
        }

        List<Lexeme> LexicalAnalysis(string Text)
        {
            Text = Text.Replace('ё', 'е');
            List<Lexeme> Stack = new List<Lexeme>();
            if (Text.Length == 0)
            {
                Errors.Add(new Error(ErrorType.EmptyText, ModelTextRules.ErrorTypes[ErrorType.EmptyText], 0, 0, 0));
                return Stack;
            }
            int line_counter = 1;
            int position = 0;

            while (Text.Length != 0)
            {
                char ch = Text[0];
                if (ModelTextRules.DetermineSymbol(ch) == LexemeType.Unknown_Symbol)
                {
                    Stack.Add(new Lexeme(LexemeType.Unknown_Symbol, "" + ch, line_counter, position, 1));
                    Errors.Add(new Error(ErrorType.UnknownSimbol, ModelTextRules.ErrorTypes[ErrorType.UnknownSimbol] + ch, line_counter, position, 1));
                }
                else
                {
                    Stack.Add(new Lexeme(ModelTextRules.DetermineSymbol(ch), "" + ch, line_counter, position, 1));
                    if (ModelTextRules.DetermineSymbol(ch) == LexemeType.Enter)
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
                    foreach (LexemeTypeTemplate temp in ModelTextRules.LexicalTemplates)
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
                                (temp.LTemplate[i] == LexemeType.Anything))
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
                            for (int j = 1; j < LexemeList.Count; j++)
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
                if (Stack[Stack.Count - 1].LType == LexemeType.Word)
                {
                    Word NewWord;
                    PhraseType PhType = ModelTextRules.DetermineWord(Stack[Stack.Count - 1]);
                    switch (PhType)
                    {
                        case PhraseType.Time_Word:
                            NewWord = new Word(WordType.SystemVar, Stack[Stack.Count - 1]);
                            break;
                        case PhraseType.Initiator_Word:
                            NewWord = new Word(WordType.SystemVar, Stack[Stack.Count - 1]);
                            break;
                        case PhraseType.Rand_Word:
                            NewWord = new Word(WordType.SystemVar, Stack[Stack.Count - 1]);
                            break;
                        case PhraseType.Name:
                            NewWord = new Word(WordType.Name, Stack[Stack.Count - 1]);
                            break;
                        case PhraseType.ArithmeticFunction_Word:
                            NewWord = new Word(WordType.ArithmeticFunction, Stack[Stack.Count - 1]);
                            break;
                        default:
                            NewWord = new Word(WordType.KeyWord, Stack[Stack.Count - 1]);
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
            Response result = new Response(); //template.PhType == LPDP.PhraseType.Algorithm
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
                    result.Finded_Errors.Add(new Error(ErrorType.ExpectedPhrase,
                        ModelTextRules.ErrorTypes[ErrorType.ExpectedPhrase] + template.PhType.ToString(),
                        //ListNewPhrase[i-1].Line,ListNewPhrase[i-1].Start,ListNewPhrase[i-1].Length
                        0, 0, 0
                        ));
                    result.ScanState = ScanningState.Fallen;
                    return result;
                }


                //если терминал (конечное значение, уровня 0)
                if (ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])
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
                            result.Finded_Errors.Add(new Error(ErrorType.Replacing,
                                ModelTextRules.ErrorTypes[ErrorType.Replacing] + template.PhTemplate[i].ToString(),
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
                        result.Finded_Errors.Add(new Error(ErrorType.ExpectedPhrase,
                            ModelTextRules.ErrorTypes[ErrorType.ExpectedPhrase] + "111" + template.PhTemplate[i].ToString(),
                            Phrases[0].Line, Phrases[0].Start, Phrases[0].Length));
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
                        result.Finded_Errors.Add(new Error(ErrorType.ExpectedPhrase,
                            ModelTextRules.ErrorTypes[ErrorType.ExpectedPhrase] + template.PhType.ToString(),
                            //ListNewPhrase[i-1].Line,ListNewPhrase[i-1].Start,ListNewPhrase[i-1].Length
                            0, 0, 0
                            ));
                        result.ScanState = ScanningState.Fallen;
                        return result;
                    }

                    foreach (PhraseTypeTemplate temp_part in ModelTextRules.SyntacticalTemplates)
                    {
                        Response Request = new Response();
                        if (temp_part.PhType == template.PhTemplate[i])  //if (temp_part.PhType == PhraseType.Var)
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
                                result.Finded_Errors.Add(new Error(ErrorType.ExpectedPhrase,
                                    ModelTextRules.ErrorTypes[ErrorType.ExpectedPhrase] + template.PhTemplate[i].ToString(),
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
            if ((Phrases[0].PhType == PhraseType.EoL)
                ||
                (Phrases[0].PhType == PhraseType.Round_Bracket_Open) ||
                (Phrases[0].PhType == PhraseType.Round_Bracket_Close) ||
                (Phrases[0].PhType == PhraseType.Comma) ||
                (Phrases[0].PhType == PhraseType.TypeSeparator) ||
                (Phrases[0].PhType == PhraseType.ArithmeticOperator_3lvl) ||
                (Phrases[0].PhType == PhraseType.ArithmeticOperator_2lvl) ||
                (Phrases[0].PhType == PhraseType.ArithmeticOperator_1lvl) ||
                (Phrases[0].PhType == PhraseType.ComparisonOperator) ||
                (Phrases[0].PhType == PhraseType.LogicOperator) ||
                (Phrases[0].PhType == PhraseType.ArithmeticFunction_Word) ||
                (Phrases[0].PhType == PhraseType.AssignOperator_Word) ||
                (Phrases[0].PhType == PhraseType.LabelSeparator)
                )
            {
                return false;
            }


            Phrases.RemoveAt(0);
            Phrases.Insert(0, new Phrase(temp.PhTemplate[corrected_index]));

            int to_concat = 0;
            return CheckCorrect(Phrases, corrected_index, temp, ref to_concat);

            //for (int i = corrected_index; i < temp.ConcatedPhrases; i++)
            //{
            //    if (PrimaryPhraseTypes[temp.PhTemplate[i]])
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
                if (ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])
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
                    PhraseTypeTemplate best_temp = ModelTextRules.SyntacticalTemplates[0];
                    foreach (PhraseTypeTemplate temp_part in ModelTextRules.SyntacticalTemplates)
                    {
                        if (temp_part.PhType == template.PhTemplate[i]) //if (temp_part.PhType == PhraseType.ActivateOperator )
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
                            int internal_concatted_lex = 0;
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
                lex.PhType = ModelTextRules.DeterminePhrase(lex);
                if (lex.PhType == PhraseType.UnknownLexeme)
                {
                    Errors.Add(new Error(ErrorType.UnknownLexeme, ModelTextRules.ErrorTypes[ErrorType.UnknownLexeme] + lex.Value, lex.Line, lex.Start, lex.Length));
                }
                if ((lex.PhType != PhraseType.Empty) && (lex.PhType != PhraseType.Comment))
                    Phrases.Add((Phrase)lex);
            }

            Response resp = FindTemplate(ModelTextRules.SyntacticalTemplates[0], Phrases);


            this.Errors.AddRange(resp.Finded_Errors);
            return resp.BackPhrase;
        }

        //******************
        Phrase MadeStruct(Phrase InitialPhrase/*, PhraseType BasePhraseType*/)
        {
            for (int i = 0; i < InitialPhrase.Value.Count; i++)// (Phrase ph in InitialPhrase.Value)
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

        //******************

        string EjectStringName(Phrase ph)
        {
            Phrase name_ph = ph.Value.Find(p => p.PhType == PhraseType.Name);
            if (name_ph == null)
            { }
            Word name_w = (Word)name_ph;
            string name_s = name_w.LValue;
            return name_s;
        }

        List<LPDP.Objects.Object> CreateObjectsFromDesciptionLine(Phrase desc_line)
        {
            List<LPDP.Objects.Object> list = new List<Objects.Object>();

            Phrase vars_ph = desc_line.Value.Find(ph => ph.PhType == PhraseType.Vars);
            Dictionary<string,Phrase> vars_dictionary = new Dictionary<string,Phrase>();
            foreach (Phrase var_ph in vars_ph.Value)
            {
                if (var_ph.PhType == PhraseType.Comma)
                {
                    continue;
                }
                if (var_ph.PhType == PhraseType.Var)
                {
                    if (var_ph.Value.Exists(p => p.PhType == PhraseType.AssignOperator))
                    {
                        Phrase assign_op_ph = var_ph.Value.Find(p => p.PhType == PhraseType.AssignOperator);
                        string name = this.EjectStringName(assign_op_ph);
                        Phrase val = assign_op_ph.Value.Find(p => p.PhType == PhraseType.Value);
                        vars_dictionary.Add(name, val);
                        continue;
                    }
                    if (var_ph.Value.Exists(p => p.PhType == PhraseType.Name))
                    {
                        string name = this.EjectStringName(var_ph);
                        vars_dictionary.Add(name, null);
                        continue;
                    }
                }
            }

            string varunit = this.ResultModel.ST_Cont.CurrentUnit.Name;


            Phrase vardescription_ph = desc_line.Value.Find(ph => ph.PhType == PhraseType.VarDescription);
            Phrase vartype_ph = vardescription_ph.Value.Find(ph => ph.PhType == PhraseType.VarType);

            foreach (var v in vars_dictionary)
            {
                //для скаляра
                if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.ScalarVarType_Word))
                {
                    if (v.Value == null)
                    {
                        list.Add(new Objects.Scalar(v.Key, varunit));
                    }
                    else
                    {
                        //если выражение
                        //list.Add(new Scalar(v.Key., varunit,v.Value);
                    }
                }
                //для вектора
                if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.VectorVarType_Word))
                {
                    Objects.Vector NewVector = new Objects.Vector(v.Key, varunit, CreateObjectsFromDesciptionLine(v.Value));
                }   
                //для ссылки
                if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.LinkVarType_Word))
                {
                    if (v.Value == null)
                    {
                        list.Add(new Objects.Link(v.Key, varunit));
                    }
                    else
                    {
                        //если выражение
                        //list.Add(new Scalar(v.Key., varunit,v.Value);
                    }
                }
                //для макроса
                if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.MacroVarType_Word))
                {
                    Phrase names_ph = vartype_ph.Value.Find(ph => ph.PhType == PhraseType.Names);
                    List<string> names = new List<string>();
                    foreach(Phrase ph in names_ph.Value)
                    {
                        if (ph.PhType == PhraseType.Name)
                        {
                            Word name_w = (Word)ph;
                            names.Add(name_w.LValue);
                        }
                    }

                    list.Add(new Macro(v.Key,varunit,v.Value, names));
                }

            }//конец перебора переменных

            return list;
        }

        Operator create_operator_from_phrase(Phrase concret_operator_ph)
        {
            Operator result_oper = new Operator();

            //Phrase concret_operator_ph = operator_ph.Value[0];
            switch (concret_operator_ph.PhType)
            {
                case PhraseType.AssignOperator:
                    //Operator result_oper = new Operator();
                    result_oper.Name = OperatorName.Assign;
                    Phrase concrete_var = concret_operator_ph.Value[0];
                    switch (concrete_var.PhType)
                    {
                        case PhraseType.Name:
                            result_oper.Params.Add("var_name", EjectStringName(concret_operator_ph));
                            break;
                        case PhraseType.VectorNode:
                            result_oper.Params.Add("var_name", concrete_var);
                            break;
                        case PhraseType.ValueFromLink:
                            result_oper.Params.Add("var_name", concrete_var);
                            break;
                    }
                    result_oper.Params.Add("value", concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.Value));
                    //list_oper.Add(assign_oper);
                    break;
                case PhraseType.TransferOperator:
                    //Operator transfer_oper = new Operator();
                    result_oper.Name = OperatorName.Transfer;
                    Phrase destination_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                    result_oper.Params.Add("lable_name", EjectStringName(destination_ph));
                    if (destination_ph.Value.Exists(ph => ph.PhType == PhraseType.RefToUnit_Word))
                    {
                        result_oper.Params.Add("unit_name", destination_ph.Value.FindLast(ph => ph.PhType == PhraseType.Name));
                    }
                    //else 
                    //{
                    //    transfer_oper.Params.Add("unit_name", this.ResultModel.ST_Cont.CurrentUnit.Name);
                    //}
                    //list_oper.Add(transfer_oper);
                    break;
                case PhraseType.CreateOperator:
                    //Operator create_oper = new Operator();
                    result_oper.Name = OperatorName.Create;
                    result_oper.Params.Add("var_name", EjectStringName(concret_operator_ph));
                    result_oper.Params.Add("var_type", concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.VarType));
                    //list_oper.Add(create_oper);
                    break;
                case PhraseType.ActivateOperator:
                    //Operator activate_oper = new Operator();
                    result_oper.Name = OperatorName.Activate;
                    result_oper.Params.Add("link_name", EjectStringName(concret_operator_ph));
                    Phrase transfer_oper_in_activ_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                    Operator transfer_oper_in_activ = create_operator_from_phrase(transfer_oper_in_activ_ph);
                    foreach (KeyValuePair<string, object> par in transfer_oper_in_activ.Params)
                    {
                        result_oper.Params.Add(par.Key, par.Value);
                    }
                    //list_oper.Add(activate_oper);
                    break;
                case  PhraseType.PassivateOperator:
                    //Operator passivate_oper = new Operator();
                    result_oper.Name = OperatorName.Passivate;
                    result_oper.Params.Add("link_name", EjectStringName(concret_operator_ph));
                    //passivate_oper.Params.Add("unit_name", this.ResultModel.ST_Cont.CurrentUnit.Name);
                    //list_oper.Add(passivate_oper);
                    break;
                case PhraseType.TerminateOperator:
                    //Operator terminate_oper = new Operator();
                    result_oper.Name = OperatorName.Terminate;
                    if (concret_operator_ph.Value.Exists(ph => ph.PhType == PhraseType.Name))
                    {
                        result_oper.Params.Add("var_name", EjectStringName(concret_operator_ph));
                        //terminate_oper.Params.Add("unit_name", this.ResultModel.ST_Cont.CurrentUnit.Name);
                    }
                    //list_oper.Add(terminate_oper);
                    break;
                case PhraseType.IfOperator:
                    //Operator if_oper = new Operator();
                    result_oper.Name = OperatorName.If;

                    Phrase if_conditions_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.IfConditions);

                    int i = 1;

                    foreach (Phrase if_condition_ph in if_conditions_ph.Value)
                    {
                        if (if_condition_ph.PhType == PhraseType.EoL)
                        {
                            break;
                        }
                        //result_oper.Params.Add("condition_line_"+i, new Dictionary<string, object>());

                        //Phrase else_condition_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.AlternativeCondition);
                        //result_oper.Params.Add("logic_exp_" + i, true);
                        //Phrase transfer_oper_in_else_ph = else_condition_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                        //Operator transfer_oper_in_else = create_operator_from_phrase(transfer_oper_in_else_ph);
                        //foreach (KeyValuePair<string, object> par in transfer_oper_in_else.Params)
                        //{
                        //    result_oper.Params.Add(par.Key + i, par.Value);
                        //}


                        Phrase logic_exp = if_condition_ph.Value.Find(ph => ph.PhType == PhraseType.LogicExpression);
                        if (logic_exp != null)
                        {
                            result_oper.Params.Add("logic_exp_" + i, logic_exp);
                        }
                        else
                        {
                            result_oper.Params.Add("logic_exp_" + i, true); 
                        }

                        Phrase transfer_oper_in_if_ph = if_condition_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                        Operator transfer_oper_in_if = create_operator_from_phrase(transfer_oper_in_if_ph);

                        foreach (KeyValuePair<string, object> par in transfer_oper_in_if.Params)
                        {
                            result_oper.Params.Add(par.Key + i, par.Value);
                        }
                        i++;
                    }
                  
                    //i++;
                    


                    //if (concret_operator_ph.Value.Exists(ph => ph.PhType == PhraseType.Name))
                    //{
                    //    terminate_oper.Params.Add("var_name", EjectStringName(concret_operator_ph));
                    //    terminate_oper.Params.Add("unit_name", this.ResultModel.ST_Cont.CurrentUnit.Name);
                    //}
                    //list_oper.Add(terminate_oper);


                    break;


            }
            return result_oper;
        }
    }
}
