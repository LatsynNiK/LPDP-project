﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Analyzer
    {
        public string SourceText;
        List<Error> ErrorStack;
        List<Lexeme> Lexemes;
        List<Phrase> Phrases;        
        Phrase ParsedText;
        public string ResultTxtCode;
        public string ResultRTFCode;
        Model ParentModel;     
        List<Phrase> CommentPhrases;
        public TextSelectionTable Selections;

        public Analyzer(Model parent_model)
        {
            this.SourceText = "";
            //this.Errors = new List<Error>();
            this.Lexemes = new List<Lexeme>();
            //this.Phrases = new List<Phrase>();
            this.ParsedText = new Phrase();
            this.ResultRTFCode = "";
            this.ResultTxtCode = "";
            //this.ResultInfo = "";

            this.CommentPhrases = new List<Phrase>();

            this.Selections = new TextSelectionTable();
            
            this.ParentModel = parent_model;
        }

        public void Building(string source_text)
        {
            try
            {
                this.AnalyzeText(source_text);

                this.ResultTxtCode = this.RebuildingText(this.ParsedText, this.CommentPhrases);
                this.AnalyzeText(this.ResultTxtCode);

                this.AnalyzeStructure(this.ParsedText);

                this.LaunchPreparation();

                //this.ParentModel.Out.InfoTxt = "Модель построена успешно.";
            }
            catch (Error e)
            {
                this.Selections.AddError(e);
                this.ParentModel.StatusInfo = e.GetErrorStack();
            }
        }

        public void CheckText(string text)
        {
            try
            {
                this.LexicalAnalysis(text);
                this.ParentModel.StatusInfo = "";
            }
            catch (Error e)
            {
                this.Selections.AddError(e);
                this.ParentModel.StatusInfo = e.GetErrorStack();
            }
        }

        #region main functions

        List<Phrase> LexicalAnalysis(string Text)
        {
            this.Selections = new TextSelectionTable();
            this.SourceText = Text;

            List<Lexeme> Stack = new List<Lexeme>();
            try
            {
                Text = Text.Replace('ё', 'е');

                int line_counter = 1;
                int position = 0;

                List<Error> Errors = new List<Error>();

                while (Text.Length != 0)
                {
                    char ch = Text[0];
                    if (ModelTextRules.DetermineSymbol(ch) == LexemeType.Unknown_Symbol)
                    {
                        Stack.Add(new Lexeme(LexemeType.Unknown_Symbol, "" + ch, line_counter, position, 1));
                        //Errors.Add(new UnknownLexemeError(ErrorType.UnknownSimbol, ModelTextRules.ErrorTypes[ErrorType.UnknownSimbol] + ch, line_counter, position, 1));
                        //throw new UnknownLexemeError(ch, position, 1, line_counter);
                        Errors.Add(new UnknownLexemeError(ch, position, 1, line_counter));
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
                                NewWord = new Word(WordType.SystemWord, Stack[Stack.Count - 1]);
                                break;
                            case PhraseType.Initiator_Word:
                                NewWord = new Word(WordType.SystemWord, Stack[Stack.Count - 1]);
                                break;
                            case PhraseType.Rand_Word:
                                NewWord = new Word(WordType.SystemWord, Stack[Stack.Count - 1]);
                                break;
                            case PhraseType.Name:
                                NewWord = new Word(WordType.Name, Stack[Stack.Count - 1]);
                                break;
                            case PhraseType.ArithmeticFunction_Word:
                                NewWord = new Word(WordType.SystemWord, Stack[Stack.Count - 1]);
                                break;
                            case PhraseType.LogicOperator:
                                NewWord = new Word(WordType.SystemWord, Stack[Stack.Count - 1]);
                                break;
                            default:
                                NewWord = new Word(WordType.KeyWord, Stack[Stack.Count - 1]);                                
                                break;
                        }
                        
                        Stack.RemoveAt(Stack.Count - 1);
                        Stack.Add(NewWord);
                    }
                }

                if (Errors.Count > 0)
                {
                    throw new ErrorList(Errors);
                }                
            }
            catch (ErrorList e)
            {
                LexicalError LexErr = new LexicalError(e);
                throw new UserError(LexErr);
            }
            catch (LexicalError e)
            {
                throw new UserError(e);
            }

            return this.LexemesToPhrases(Stack);
        }        

        List<Phrase> LexemesToPhrases(List<Lexeme> Lexemes)
        {
            List<Phrase> Phrases = new List<Phrase>();
            foreach (Lexeme lex in Lexemes)
            {
                lex.PhType = ModelTextRules.DeterminePhrase(lex);
                if (lex.PhType == PhraseType.UnknownLexeme)
                {
                    //!!!Errors.Add(new Error(ErrorType.UnknownLexeme, ModelTextRules.ErrorTypes[ErrorType.UnknownLexeme] + lex.Value, lex.Line, lex.Start, lex.Length));
                }

                //удаление \n из комментария
                if (lex.PhType == PhraseType.Comment)
                {
                    if (lex.LValue.Last() == '\n')
                    {
                        lex.LValue = lex.LValue.TrimEnd('\n');// RemoveAt(lex.LValue.Length - 1);
                    }
                    this.CommentPhrases.Add((Phrase)lex);
                    this.Selections.Add(lex.Start, lex.Length, TextSelectionType.Comment);
                }
                if (lex.PhType == PhraseType.String)
                {
                    this.Selections.Add(lex.Start, lex.Length, TextSelectionType.String);
                }
                if (lex is Word)
                {
                    if (((Word)lex).WType == WordType.KeyWord)
                    {
                        this.Selections.Add(lex.Start, lex.Length, TextSelectionType.KeyWord);
                    }
                    if (((Word)lex).WType == WordType.SystemWord)
                    {
                        this.Selections.Add(lex.Start, lex.Length, TextSelectionType.SystemWord);
                    }
                }
                if ((lex.PhType != PhraseType.Empty) && (lex.PhType != PhraseType.Comment))
                    Phrases.Add((Phrase)lex);
            }
            return Phrases;
        }

        public int AnalyzeText(string source_text) // возвращает 1 если успешно
        {
            try
            {
                this.Phrases = LexicalAnalysis(source_text);
                // Пустой текст модели
                if (this.Phrases.Count == 0)
                {
                    throw new EmptyTextError();
                }
                //this.Phrases = this.LexemesToPhrases(this.Lexemes);
                this.ParsedText = SyntacticalAnalysis(this.Phrases);
                this.ParsedText = MadeStruct(this.ParsedText);
                //this.ResultTxtCode = "";
                //this.ResultRTFCode = "";
                return 1;
            }
            catch (UserError e)
            {
                
                throw new UserError(e);                
                //return 0;
            }
            catch (SystemError e)
            {
                throw new SystemError(e);
                //return 0;
            }
            
        }
      
        string RebuildingText(Phrase sourse_phrase, List<Phrase> comments)
        {
            string result = "";
            string tab = "    ";

            if (sourse_phrase.Value != null)
            {
                Phrase[] reverse_arr = new Phrase[sourse_phrase.Value.Count];
                sourse_phrase.Value.CopyTo(reverse_arr);
                List<Phrase> reverse = new List<Phrase>(reverse_arr);
                reverse.Reverse();
                //sourse_phrase.Value
                foreach (Phrase ph in reverse)
                {
                    string before = "";
                    string after = "";
                    switch (ph.PhType)
                    {
                        case PhraseType.Unit:
                            before = "\n";
                            after = "\n";
                            //result += before + this.RebuildingText(ph, comments) + after;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.UnitHeader:
                            before = tab;
                            after = "\n";
                            //result += before + this.RebuildingText(ph, comments) + after;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.Description:

                            before = tab+tab;
                            //after = "\n";
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.DescriptionLine:
                            if (reverse.IndexOf(ph) < reverse.Count - 1)
                            {
                                if (reverse[reverse.IndexOf(ph) + 1].PhType == PhraseType.Round_Bracket_Open)
                                {
                                    result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                                    break;
                                }
                            }                            
                            before = tab+tab+tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.DescriptionEnding:
                            before = tab + tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.Algorithm:
                            before = tab + tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            //after = "\n";
                            break;
                        case PhraseType.AlgorithmLine:
                            if (ph.Value[0].PhType != PhraseType.Label)
                            {
                                before = tab + tab + tab;
                            }                            
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.AlgorithmEnding:
                            before = tab + tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;                        
                        //case PhraseType.EoL:
                        //    after = "\n";
                        //    break;
                        case PhraseType.UnitEnding:
                            before = tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.Label:
                            after = tab + tab + tab;
                            if (ph.Length + tab.Length > after.Length)
                            {
                                after = '\n' + after;
                            }
                            else
                            {
                                after = after.Remove(0, ph.Length);
                            }
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);

                            break;
                        case PhraseType.Name:
                            if (sourse_phrase.PhType == PhraseType.Label)
                            {
                                result = result.Insert(0, before + ph.ToString() + after);
                                break;
                            }
                            if (sourse_phrase.PhType == PhraseType.Names)
                            {
                                result = result.Insert(0, before + ph.ToString() + after);
                                break;
                            }
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.IfCondition:                            
                            after = "\n" + tab + tab + tab;
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        case PhraseType.WaitOperator_Word:
                            if (sourse_phrase.PhType == PhraseType.ComplexWaitCondition)
                            {
                                before = "\n" + tab + tab + tab;
                                //result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            }
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                        default:
                            result = result.Insert(0, before + this.RebuildingText(ph, comments) + after);
                            break;
                    }
                }
            }
            else
            {
                string before = "";
                string after = "";
                switch (sourse_phrase.PhType)
                {
                    case PhraseType.EoL:
                        after = "\n";
                        break;
                    case PhraseType.DescriptionBracket_Word:
                        after = "\n";
                        break;
                    case PhraseType.LabelSeparator:
                        //after = "\n";
                        break;
                    case PhraseType.AlgorithmBracket_Word:
                        after = "\n";
                        break;


                    default:
                        after = " ";
                        break;
                }
                //while ((comments.Count > 0) && (comments[0].Start < sourse_phrase.Start))
                while ((comments.Count > 0) && (comments.Last().Start > sourse_phrase.Start))
                {
                    //before += ((Lexeme)comments[0]).LValue;
                    after = after.Insert(0, tab + ((Lexeme)comments.Last()).LValue);
                    // RemoveAt(0);
                    comments.Remove(comments.Last());
                }
                result = before+((Lexeme)sourse_phrase).LValue + after;
            }      
     

            return result;
        }

        int AnalyzeStructure(Phrase source_phrase)
        {
            switch (source_phrase.PhType)
            {
                //Определение имени модели
                case PhraseType.Model:
                    this.ParentModel.Name = EjectStringName(source_phrase);

                    Phrase units_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.Units);
                    foreach (Phrase unit_ph in units_ph.Value)
                    {
                        AnalyzeStructure(unit_ph);
                    }
                    break;

                case PhraseType.Unit:
                    //this.ParentModel.Executor.TC_Cont.
                    this.ParentModel.ST_Cont.CreateUnit(source_phrase.Start);
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
                    this.ParentModel.ST_Cont.SetUnitHeader(EjectStringName(source_phrase), u_type);
                    this.ParentModel.Executor.SetCurrentUnit(this.ParentModel.ST_Cont.CurrentUnit);
                    break;

                case PhraseType.Description:
                    Phrase descriptionlines_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.DescriptionLines);
                    foreach (Phrase descriptionline_ph in descriptionlines_ph.Value)
                    {
                        AnalyzeStructure(descriptionline_ph);
                    }
                    break;

                case PhraseType.DescriptionLine:
                    List<SLT.Object> list_obj = this.CreateObjectsFromDesciptionLine(source_phrase,this.ParentModel.ST_Cont.CurrentUnit.Name);
                    foreach (SLT.Object o in list_obj)
                    {
                        this.ParentModel.O_Cont.AddObject(o,this.ParentModel.ST_Cont.CurrentUnit.Name);
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

                    if (this.ParentModel.ST_Cont.CurrentSubprogram ==  null)
                    {
                        //error
                    }

                    //если встретили метку
                    if (source_phrase.Value.Exists(ph => ph.PhType == PhraseType.Label))
                    {
                        Subprogram NewSubp = new Subprogram();
                        this.ParentModel.ST_Cont.AddSubprogram(NewSubp);
                        foreach (Phrase ph in source_phrase.Value)
                        {
                            if (ph.PhType == PhraseType.Label)
                            {
                                string label_name = EjectStringName(ph);
                                Label NewLabel = new Label(label_name, true);
                                this.ParentModel.ST_Cont.AddLabel(NewLabel);
                            }
                        }
                    }

                    Phrase operator_ph = source_phrase.Value.Find(ph => ph.PhType == PhraseType.Operator);
                    Operator oper = CreateOperatorFromPhrase(operator_ph.Value[0]);
                    this.ParentModel.ST_Cont.AddOperator(oper);
                    if ((oper.Name == OperatorName.Transfer) ||
                        (oper.Name == OperatorName.ComplexWait) ||
                        (oper.Name == OperatorName.If)||
                        (oper.Name == OperatorName.Terminate)||
                        (oper.Name == OperatorName.Passivate))
                    {
                        this.ParentModel.ST_Cont.CurrentSubprogram = null;
                    }
                    if (oper.Name == OperatorName.SimpleWait)
                    {
                        Subprogram NewSubp = new Subprogram();
                        this.ParentModel.ST_Cont.AddSubprogram(NewSubp);

                        Label NewLabel = new Label("", false);
                        this.ParentModel.ST_Cont.AddLabel(NewLabel);
                    }
                    break;
            }
            return 1;
        }

        int LaunchPreparation()
        {
            foreach (Unit unit in this.ParentModel.ST_Cont.Units)
            {
                if ((unit.Type == UnitType.Aggregate) || (unit.Type == UnitType.Controller))
                {
                    Phrase true_ph = new Phrase(PhraseType.True);
                    Initiator new_init = this.ParentModel.O_Cont.CreateInitiator(unit.Name);
                    Subprogram to_subp = this.ParentModel.ST_Cont.Tracks.Find(sp => (sp.Unit == unit)&&
                                                                            (sp.Operators.All(op => op.Name != OperatorName.Passivate)));
                    new_init.NextOperator = to_subp.Operators[0];

                    this.ParentModel.Executor.TC_Cont.AddConditionRecord(true_ph, new_init, to_subp, false);
                }
            }
            this.ParentModel.Executor.Preparetion();
            
            this.ParentModel.Built = true;
            this.ParentModel.StatusInfo = "Модель построена успешно.";

            return 1;
        }

        #endregion

        #region for Syntactical Analysis

        //Функции для синтаксического анализа через (throw) -- медленно!
        //List<Phrase> ApplyTemplate(PhraseTypeTemplate template, List<Phrase> Phrases)
        //{
        //    //копирование списка фраз
        //    List<Phrase> CopyPhrases = new List<Phrase>();
        //    CopyPhrases.AddRange(Phrases);

        //    List<Phrase> ListNewPhrase = new List<Phrase>();
        //    for (int i = 0; i < template.ConcatedPhrases; i++)
        //    {
        //        //если терминал (конечное значение, уровня 0)
        //        if (ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])
        //        {
        //            if (CopyPhrases[0].PhType == template.PhTemplate[i])
        //            {
        //                //есть совпадение                        
        //                ListNewPhrase.Add(CopyPhrases[0]);
        //                CopyPhrases.RemoveAt(0);
        //            }
        //            else 
        //            {
        //                //есть ошибка
        //                throw new PhraseNotFound(CopyPhrases[0].Start, CopyPhrases[0].Length, CopyPhrases[0].Line, template.PhTemplate[i]);
        //            }
        //        }
        //        //если нетерминал (составное значение)
        //        else
        //        {
        //            try
        //            {
        //                PhraseTypeTemplate inner_temp = FindTemplate(template.PhTemplate[i], CopyPhrases[0]);
        //                List<Phrase> NewPhrases = new List<Phrase>();
        //                NewPhrases = ApplyTemplate(inner_temp, CopyPhrases);
        //                CopyPhrases = NewPhrases;
        //                ListNewPhrase.Add(CopyPhrases[0]);
        //                CopyPhrases.RemoveAt(0);

        //            }
        //            catch (PhraseNotFound e)
        //            {
        //                throw new PhraseNotFound(e, template.PhType);
        //            }                   
        //        }        
        //    }
        //    //создаем новую фразу
        //    foreach (Phrase ph in ListNewPhrase)
        //    {
        //        if ((ph.Value != null) && (ph.Value.Count == 0))
        //        {
        //            Phrase last = ListNewPhrase.ElementAt(ListNewPhrase.IndexOf(ph) - 1);
        //            ph.Start = last.Start+last.Length;
        //        }
        //    }            
        //    Phrase[] ArrayNewPhrase = new Phrase[ListNewPhrase.Count];
        //    ListNewPhrase.CopyTo(ArrayNewPhrase);
        //    Phrase NewPhrase = new Phrase(template.PhType, ArrayNewPhrase);

        //    //добавляем новую
        //    CopyPhrases.Insert(0, NewPhrase);
        //    return CopyPhrases;
        //}
        
        //PhraseTypeTemplate FindTemplate(PhraseType ph_type, Phrase first_ph)
        //{
        //    Error err = new Error();            
        //    foreach (PhraseTypeTemplate temp in ModelTextRules.SyntacticalTemplates)
        //    {
                
        //        if (temp.PhType == ph_type)
        //        {
        //            if (temp.ConcatedPhrases == 0)
        //            {
        //                return temp;
        //            }
        //            if (temp.PhTemplate[0] == first_ph.PhType)
        //            {
        //                return temp;
        //            }
        //            else
        //            {                        
        //                try
        //                {
        //                    //если терминал
        //                    if (ModelTextRules.PrimaryPhraseTypes[temp.PhTemplate[0]])
        //                    {
        //                        throw new PhraseNotFound(first_ph.Start,first_ph.Length,first_ph.Line, temp.PhTemplate[0]);
        //                    }
        //                    //если нетерминал
        //                    else
        //                    {
        //                        FindTemplate(temp.PhTemplate[0], first_ph);
        //                        return temp; 
        //                    }                          
        //                }
        //                catch (PhraseNotFound e)
        //                {
        //                    err = e;
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //    throw new PhraseNotFound(err, ph_type);
        //}

        List<Phrase> ApplyTemplate(PhraseTypeTemplate template, List<Phrase> Phrases)
        {
            //копирование списка фраз
            List<Phrase> CopyPhrases = new List<Phrase>();
            CopyPhrases.AddRange(Phrases);

            List<Phrase> ListNewPhrase = new List<Phrase>();
            for (int i = 0; i < template.ConcatedPhrases; i++)
            {
                //если терминал (конечное значение, уровня 0)
                if (ModelTextRules.PrimaryPhraseTypes[template.PhTemplate[i]])
                {
                    if (CopyPhrases[0].PhType == template.PhTemplate[i])
                    {
                        //есть совпадение                        
                        ListNewPhrase.Add(CopyPhrases[0]);
                        CopyPhrases.RemoveAt(0);
                    }
                    else
                    {
                        //есть ошибка
                        //throw new PhraseNotFound(CopyPhrases[0].Start, CopyPhrases[0].Length, CopyPhrases[0].Line, template.PhTemplate[i]);
                        this.ErrorStack.Add(new PhraseNotFound(CopyPhrases[0].Start, CopyPhrases[0].Length, CopyPhrases[0].Line, template.PhTemplate[i]));
                        return null;
                    }
                }
                //если нетерминал (составное значение)
                else
                {
                    int err_stack_deep = this.ErrorStack.Count;
                    PhraseTypeTemplate inner_temp = FindTemplate(template.PhTemplate[i], CopyPhrases[0]);
                    if (inner_temp == null)
                    {
                        this.ErrorStack.Add(new PhraseNotFound(ErrorStack.Last(), template.PhType));
                        return null;
                    }
                    else
                    {
                        if (this.ErrorStack.Count > 0)
                        {
                            this.ErrorStack.RemoveRange(err_stack_deep, this.ErrorStack.Count - err_stack_deep);
                        }
                    }
                    //List<Phrase> NewPhrases = new List<Phrase>();
                    CopyPhrases = ApplyTemplate(inner_temp, CopyPhrases);
                    if (CopyPhrases == null)
                    {
                        this.ErrorStack.Add(new PhraseNotFound(ErrorStack.Last(), template.PhType));
                        return null;
                    }
                    else
                    {
                        if (this.ErrorStack.Count > 0)
                        {
                            this.ErrorStack.RemoveRange(err_stack_deep, this.ErrorStack.Count - err_stack_deep);
                        }
                    }
                    ListNewPhrase.Add(CopyPhrases[0]);
                    CopyPhrases.RemoveAt(0);
                }
            }
            //создаем новую фразу
            foreach (Phrase ph in ListNewPhrase)
            {
                if ((ph.Value != null) && (ph.Value.Count == 0))
                {
                    Phrase last = ListNewPhrase.ElementAt(ListNewPhrase.IndexOf(ph) - 1);
                    ph.Start = last.Start + last.Length;
                }
            }
            Phrase[] ArrayNewPhrase = new Phrase[ListNewPhrase.Count];
            ListNewPhrase.CopyTo(ArrayNewPhrase);
            Phrase NewPhrase = new Phrase(template.PhType, ArrayNewPhrase);

            //добавляем новую
            CopyPhrases.Insert(0, NewPhrase);
            return CopyPhrases;
        }

        PhraseTypeTemplate FindTemplate(PhraseType ph_type, Phrase first_ph)
        {
            Error err = new Error();
            int err_stack_deep = this.ErrorStack.Count;
            foreach (PhraseTypeTemplate temp in ModelTextRules.SyntacticalTemplates)
            {
                if (this.ErrorStack.Count > 0)
                {
                    this.ErrorStack.RemoveRange(err_stack_deep, this.ErrorStack.Count - err_stack_deep);
                }
                if (temp.PhType == ph_type)
                {
                    if (temp.ConcatedPhrases == 0)
                    {
                        return temp;
                    }
                    if (temp.PhTemplate[0] == first_ph.PhType)
                    {
                        return temp;
                    }
                    else
                    {
                        //если терминал
                        if (ModelTextRules.PrimaryPhraseTypes[temp.PhTemplate[0]])
                        {
                            err = new PhraseNotFound(first_ph.Start, first_ph.Length, first_ph.Line, temp.PhTemplate[0]);
                            continue;                            
                        }
                        //если нетерминал
                        else
                        {                            
                            PhraseTypeTemplate inner_temp = FindTemplate(temp.PhTemplate[0], first_ph);
                            if (inner_temp == null)
                            {
                                err = this.ErrorStack.Last();
                                continue;
                            }
                            else
                            {
                                return temp;
                            }
                        }
                    }
                }
            }            
            this.ErrorStack.Add(new PhraseNotFound(err, ph_type));
            return null;
        }        

        Phrase SyntacticalAnalysis(List<Phrase> Phrases)
        {            
            List<Error> Errors = new List<Error>();
            this.ErrorStack = new List<Error>();
            Phrase result;
            try
            {
                //вход в рекурсивный спуск 
                List<Phrase> result_list = ApplyTemplate(ModelTextRules.SyntacticalTemplates[0], Phrases);
                if (result_list == null)
                {
                    PhraseNotFound inner_err = (PhraseNotFound)this.ErrorStack[0];
                    PhraseNotFound outer_err;// = this.ErrorStack[0];
                    this.ErrorStack.RemoveAt(0);
                    while(this.ErrorStack.Count > 0)
                    {
                        outer_err = new PhraseNotFound(inner_err,((PhraseNotFound)this.ErrorStack[0]).Type);
                        inner_err = outer_err;
                        this.ErrorStack.RemoveAt(0);
                    }
                    throw inner_err;
                }
                else
                {
                    result = result_list[0];
                }

            }

            catch (SyntacticalError e)
            {
                throw new SyntacticalError(e);
            }
            return result;
        }

        Phrase MadeStruct(Phrase InitialPhrase)
        {
            int i = 0;
            while (i < InitialPhrase.Value.Count)            
            {
                if (InitialPhrase.Value[i] is Lexeme)
                {
                    i++;
                    continue;
                }
                else
                {
                    InitialPhrase.Value[i] = MadeStruct(InitialPhrase.Value[i]);
                    if ((InitialPhrase.Value[i].PhType == InitialPhrase.PhType) ||
                        (InitialPhrase.Value[i].PhType.ToString().Contains("Another")))
                    {

                        if ((InitialPhrase.Value[i].PhType != PhraseType.ArithmeticExpression_1lvl) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.ArithmeticExpression_2lvl) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.ArithmeticExpression_3lvl) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.LogicExpression) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.ComparisonExpression) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.ArithmeticFunction) &&
                            (InitialPhrase.Value[i].PhType != PhraseType.VectorNode))
                        {
                            int index = InitialPhrase.Value.IndexOf(InitialPhrase.Value[i]);
                            InitialPhrase.Value.InsertRange(index + 1, InitialPhrase.Value[index].Value);
                            InitialPhrase.Value.RemoveAt(index);
                        }
                    }
                }
                i++;
            }

            return InitialPhrase;
        }
        #endregion

        #region for Structure Analysis

        string EjectStringName(Phrase ph)
        {
            Phrase name_ph = ph.Value.Find(p => p.PhType == PhraseType.Name);
            if (name_ph == null)
            { }
            Word name_w = (Word)name_ph;
            string name_s = name_w.LValue;
            return name_s;
        }

        public List<SLT.Object> CreateObjectsFromDesciptionLine(Phrase desc_line, string unit_name)
        {
            List<SLT.Object> list = new List<SLT.Object>();

            Phrase[] desc_line_value_copy = new Phrase[desc_line.Value.Count];
            desc_line.Value.CopyTo(desc_line_value_copy);
            
            while(desc_line.Value.Count >0 )
            {
                Phrase vars_ph = desc_line.Value.Find(ph => ph.PhType == PhraseType.Vars);
                Dictionary<string, Phrase> vars_dictionary = new Dictionary<string, Phrase>();
                foreach (Phrase var_ph in vars_ph.Value)
                {
                    if (var_ph.PhType == PhraseType.Comma)
                    {
                        continue;
                    }
                    if (var_ph.PhType == PhraseType.InitialVar)
                    {
                        string name = this.EjectStringName(var_ph);
                        Phrase val = null;
                        if (var_ph.Value.Exists(p => p.PhType == PhraseType.InitialValue))
                        {
                            Phrase initial_val_ph = var_ph.Value.Find(p => p.PhType == PhraseType.InitialValue);                            
                            val = initial_val_ph.Value.Find(p => p.PhType == PhraseType.Value);
                            vars_dictionary.Add(name, val);
                            continue;
                        }
                        vars_dictionary.Add(name, val);
                        continue;
                    }
                }

                Phrase vardescription_ph = desc_line.Value.Find(ph => ph.PhType == PhraseType.VarDescription);
                Phrase vartype_ph = vardescription_ph.Value.Find(ph => ph.PhType == PhraseType.VarType);

                string varunit;
                Phrase ref_to_unit_ph = vardescription_ph.Value.Find(ph => ph.PhType == PhraseType.RefToUnit);
                if (ref_to_unit_ph.Value.Count>0)
                {
                    varunit = this.EjectStringName(ref_to_unit_ph);
                }
                else
                {
                    varunit = unit_name;
                }

                foreach (var v in vars_dictionary)
                {
                    SLT.Object new_obj = new SLT.Scalar("", "");//заглушка
                    //для скаляра
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.ScalarVarType_Word))
                    {
                        new_obj = new SLT.Scalar(v.Key, varunit);
                        if (v.Value != null)
                        {
                            //если выражение
                            object value_obj = this.ParentModel.Executor.ConvertValueToObject(v.Value);
                            new_obj.SetValue(value_obj);
                        }
                        //list.Add(new_scalar);
                    }
                    //для вектора
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.VectorVarType_Word))
                    {
                        Phrase description_line_ph = vartype_ph.Value.Find(ph => ph.PhType == PhraseType.DescriptionLine);
                        new_obj = new SLT.Vector(v.Key, varunit, CreateObjectsFromDesciptionLine(description_line_ph,""));
                    }
                    //для ссылки
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.LinkVarType_Word))
                    {
                        if (v.Value == null)
                        {
                            new_obj = new SLT.Link(v.Key, varunit);
                            //list.Add(new_obj);
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
                        foreach (Phrase ph in names_ph.Value)
                        {
                            if (ph.PhType == PhraseType.Name)
                            {
                                Word name_w = (Word)ph;
                                names.Add(name_w.LValue);
                            }
                        }
                        new_obj = new Macro(v.Key, varunit, v.Value, names);
                        //list.Add(new_obj);
                    }
                    list.Add(new_obj);

                }//конец перебора переменных

                desc_line.Value.RemoveRange(0, 3);//vars , -- , var_description
                if (desc_line.Value.Count >0)
                {
                    desc_line.Value.RemoveAt(0);//comma
                }
            }

            desc_line.Value = desc_line_value_copy.ToList();

            return list;
        }

        Operator CreateOperatorFromPhrase(Phrase concret_operator_ph)
        {
            Operator result_oper = new Operator();

            Action action;
            Phrase destination_ph;
            KeyValuePair<string, string> destination_str;
            bool firstly = true;
            switch (concret_operator_ph.PhType)
            {
                case PhraseType.AssignOperator:
                    result_oper.Name = OperatorName.Assign;
                    action = new Action();
                    action.Name = ActionName.Assign;
                    Phrase concrete_var = concret_operator_ph.Value[1];
                    action.Parameters.Add(concrete_var);//var
                    action.Parameters.Add(concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.Value));//value
                    result_oper.AddAction(action);
                    break;

                case PhraseType.TransferOperator:
                    result_oper.Name = OperatorName.Transfer;
                    action = new Action();
                    action.Name = ActionName.Write_to_CT;
                    action.Parameters.Add(true);//condition
                    action.Parameters.Add(true); //true - self initiatior
                    action.Parameters.Add(true);//to_the_begining
                    destination_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                    destination_str = EjectStringDestination(destination_ph);
                    action.Parameters.Add(destination_str.Key);
                    action.Parameters.Add(destination_str.Value);
                    action.Parameters.Add(false);
                    result_oper.AddAction(action);
                    break;

                case PhraseType.CreateOperator:
                    result_oper.Name = OperatorName.Create;
                    action = new Action();
                    action.Name = ActionName.Create;
                    action.Parameters.Add(EjectStringName(concret_operator_ph));//name
                    action.Parameters.Add(concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.VarType));//type
                    result_oper.AddAction(action);
                    break;

                case PhraseType.ActivateOperator:
                    result_oper.Name = OperatorName.Activate;
                    action = new Action();
                    action.Name = ActionName.Write_to_CT;
                    action.Parameters.Add(true);//condition
                    action.Parameters.Add(EjectStringName(concret_operator_ph)); //link name
                    action.Parameters.Add(true);//to_the_begining
                    Phrase transfer_in_activate_operator_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                    destination_ph = transfer_in_activate_operator_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                    destination_str = EjectStringDestination(destination_ph);
                    action.Parameters.Add(destination_str.Key);
                    action.Parameters.Add(destination_str.Value);
                    action.Parameters.Add(false);
                    result_oper.AddAction(action);
                    break;

                case  PhraseType.PassivateOperator:                    
                    result_oper.Name = OperatorName.Passivate;
                    action = new Action();
                    action.Name = ActionName.Assign;
                    action.Parameters.Add(concret_operator_ph.Value[3]); //var (link name)
                    action.Parameters.Add(new Phrase(PhraseType.Initiator_Word));//value (true - initiator)
                    result_oper.AddAction(action);

                    //result_oper.Name = OperatorName.Terminate;
                    action = new Action();
                    action.Name = ActionName.Delete;
                    action.Parameters.Add(true); //self initiator
                    result_oper.AddAction(action);
                    break;

                case PhraseType.TerminateOperator:
                    result_oper.Name = OperatorName.Terminate;
                    action = new Action();
                    action.Name = ActionName.Terminate;
                    action.Parameters.Add(true); //self initiator
                    result_oper.AddAction(action);
                    break;

                case PhraseType.DeleteOperator:
                    result_oper.Name = OperatorName.Delete;
                    action = new Action();
                    action.Name = ActionName.Delete;
                    action.Parameters.Add(EjectStringName(concret_operator_ph));
                    result_oper.AddAction(action);
                    break;

                case PhraseType.IfOperator:
                    result_oper.Name = OperatorName.If;
                    Phrase if_conditions_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.IfConditions);
                    if_conditions_ph.Value.Reverse();
                    
                    foreach (Phrase if_condition_ph in if_conditions_ph.Value)
                    {
                        if (if_condition_ph.PhType == PhraseType.EoL)
                        {
                            continue;
                        }
                        action = new Action();
                        action.Name = ActionName.Write_to_CT;
                        Phrase logic_exp = if_condition_ph.Value.Find(ph => ph.PhType == PhraseType.LogicExpression);
                        if (logic_exp != null)
                        {
                            action.Parameters.Add(logic_exp);// condition
                        }
                        else
                        {
                            action.Parameters.Add(true); // condition 
                        }
                        action.Parameters.Add(true);//initiatior
                        action.Parameters.Add(true);//to begining
                        Phrase transfer_oper_in_if_ph = if_condition_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                        destination_ph = transfer_oper_in_if_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                        destination_str = EjectStringDestination(destination_ph);
                        action.Parameters.Add(destination_str.Key);
                        action.Parameters.Add(destination_str.Value);
                        if (firstly)
                        {
                            action.Parameters.Add(false); //islast
                            firstly = false;
                        }
                        else
                        {
                            action.Parameters.Add(true); //islast
                        }
                        result_oper.AddAction(action);
                    }
                    break;

                case PhraseType.WaitOperator:
                    //начало ждать
                    Phrase wait_condition_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.WaitCondition);
                    action = new Action();
                    if (wait_condition_ph.Value.Exists(ph => ph.PhType == PhraseType.WaitTime))
                    {
                        result_oper.Name = OperatorName.SimpleWait;                        
                        action.Name = ActionName.Write_to_FTT;
                        Phrase wait_time_ph = wait_condition_ph.Value.Find(ph => ph.PhType == PhraseType.WaitTime);
                        Phrase wait_time_value_ph = wait_time_ph.Value.Find(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl);
                        action.Parameters.Add(wait_time_value_ph);
                        action.Parameters.Add(true);//initiator                        
                        action.Parameters.Add(false);//to the begining
                        action.Parameters.Add("$L_" + this.ParentModel.ST_Cont.NextWaitLabelNumber);
                        action.Parameters.Add(this.ParentModel.ST_Cont.CurrentUnit.Name);
                        action.Parameters.Add(false);
                        //result_oper.AddAction(action);
                    }
                    if (wait_condition_ph.Value.Exists(ph => ph.PhType == PhraseType.WaitUntil))
                    {
                        result_oper.Name = OperatorName.SimpleWait;
                        //action = new Structure.Action();
                        action.Name = ActionName.Write_to_CT;
                        Phrase wait_until_ph = wait_condition_ph.Value.Find(ph => ph.PhType == PhraseType.WaitUntil);
                        Phrase wait_logic_expression_ph = wait_until_ph.Value.Find(ph => ph.PhType == PhraseType.LogicExpression);
                        action.Parameters.Add(wait_logic_expression_ph);//condition
                        action.Parameters.Add(true);//initiator                        
                        action.Parameters.Add(false);//to the begining
                        action.Parameters.Add("$L_" + this.ParentModel.ST_Cont.NextWaitLabelNumber);
                        action.Parameters.Add(this.ParentModel.ST_Cont.CurrentUnit.Name);
                        action.Parameters.Add(false);
                        //result_oper.AddAction(action);
                    }
                    ///
                    Phrase complex_wait_condition_ph = concret_operator_ph.Value.Find(ph => ph.PhType == PhraseType.ComplexWaitCondition);
                    if (complex_wait_condition_ph.Value[0].PhType == PhraseType.EoL)
                    {
                        result_oper.AddAction(action);
                    }
                    else
                    {
                        result_oper.Name = OperatorName.ComplexWait;
                        Phrase transfer_oper_in_wait_ph = complex_wait_condition_ph.Value.Find(ph => ph.PhType == PhraseType.TransferOperator);
                        destination_ph = transfer_oper_in_wait_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                        destination_str = EjectStringDestination(destination_ph);
                        action.Parameters[3] = destination_str.Key;
                        action.Parameters[4] = destination_str.Value;
                        result_oper.AddAction(action);
                        for (int i = 2; i < complex_wait_condition_ph.Value.Count; i += 4)
                        {
                            if (complex_wait_condition_ph.Value[i].PhType == PhraseType.EoL)
                            { 
                                break; 
                            }
                            action = new Action();
                            wait_condition_ph = complex_wait_condition_ph.Value[i+1];
                            if (wait_condition_ph.Value.Exists(ph => ph.PhType == PhraseType.WaitTime))
                            {                                                                
                                action.Name = ActionName.Write_to_FTT;
                                Phrase wait_time_ph = wait_condition_ph.Value.Find(ph => ph.PhType == PhraseType.WaitTime);
                                Phrase wait_time_value_ph = wait_time_ph.Value.Find(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl);
                                action.Parameters.Add(wait_time_value_ph);
                                //action.Parameters.Add(true);//initiator                        
                                //action.Parameters.Add(false);//to the begining
                                //action.Parameters.Add(true);
                            }
                            if (wait_condition_ph.Value.Exists(ph => ph.PhType == PhraseType.WaitUntil))
                            {                      
                                action.Name = ActionName.Write_to_CT;
                                Phrase wait_until_ph = wait_condition_ph.Value.Find(ph => ph.PhType == PhraseType.WaitUntil);
                                Phrase wait_logic_expression_ph = wait_until_ph.Value.Find(ph => ph.PhType == PhraseType.LogicExpression);
                                action.Parameters.Add(wait_logic_expression_ph);//condition
                                //action.Parameters.Add(true);//initiator                        
                                //action.Parameters.Add(false);//to the begining                             
                                //action.Parameters.Add(true);                                
                            }
                            action.Parameters.Add(true);//initiator                        
                            action.Parameters.Add(false);//to the begining                           
                            transfer_oper_in_wait_ph = complex_wait_condition_ph.Value.Find(ph =>   (ph.PhType == PhraseType.TransferOperator) && 
                                                                                                    (complex_wait_condition_ph.Value.IndexOf(ph)>i));
                            destination_ph = transfer_oper_in_wait_ph.Value.Find(ph => ph.PhType == PhraseType.Destination);
                            destination_str = EjectStringDestination(destination_ph);
                            action.Parameters.Add(destination_str.Key);
                            action.Parameters.Add(destination_str.Value);
                            action.Parameters.Add(true);
                            result_oper.AddAction(action);
                        }
                    }
                    break;
                //конец ждать
            }
            result_oper.Start = concret_operator_ph.Start;
            result_oper.Length = concret_operator_ph.Length;
            return result_oper;
        }

        KeyValuePair<string, string> EjectStringDestination(Phrase destination_ph)
        {
            KeyValuePair<string, string> result;
            Word label_name_word = (Word)destination_ph.Value.Find(ph => ph.PhType == PhraseType.Name);

            result = new KeyValuePair<string, string>(label_name_word.LValue, this.ParentModel.ST_Cont.CurrentUnit.Name);
            Phrase ref_to_unit_ph = destination_ph.Value.Find(ph => ph.PhType == PhraseType.RefToUnit);

            if (ref_to_unit_ph.Value.Count > 0)
            {               
                result = new KeyValuePair<string,string>(label_name_word.LValue, EjectStringName(ref_to_unit_ph));
            }
            return result;
        }

        #endregion
    }
}
