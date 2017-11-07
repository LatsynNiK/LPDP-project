using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.TextAnalysis
{
    /// <summary>
    /// Типы лексем
    /// </summary>
    public enum LexemeType
    {
        Anything,

        Word,
        AssignOperator_Word,    // :=
        Number,

        Enter,                  // \n
        Empty,                  // " " \t
        EoL,                    // End of Line ";"

        Round_Bracket_Open,     // (
        Round_Bracket_Close,    // )
        //Square_Bracket_Open,    // [
        //Square_Bracket_Close,   // ]

        Digit_Point,            // .
        Comma,                  // ,

        LabelSeparator,          // :
        TypeSeparator,          // --
        Quotes,                 // "
        Star,                   // *

        Slash,                  // /
        Back_Slash,             // \

        Exclamation_Point,      // !
        Minus,                  // -
        Equality,               // =
        Comparison,             // > <

        Arithmetic_Operator_1lvl,    // ^
        Arithmetic_Operator_2lvl,    // * /
        Arithmetic_Operator_3lvl,    // + -

        Arithmetic_Function,    // log lg ln ЦЕЛОЕ
        Logic_Operator,         //  /\ \/ 
        Comparison_Operator,    //= != > < >= <=
        Ref_Operator,           // ->

        Comment_Bracket_Open,   // /*
        Comment_Bracket_Close,  // */
        Comment_Slash,           // //

        Comment,
        StringValue,

        Unknown_Symbol


    }
    public enum PhraseType
    {
        // Неопределенная лексема
        UnknownLexeme,
        Anything,

        //незначимые
        Comment,
        Empty,                  // " " \t \т

        // 0 LEVEL
        Name,
        Number,

        ModelBracketOpen_Word,
        UnitBracketOpen_Word,
        UnitType_Word,
        DescriptionBracketOpen_Word,
        AlgorithmBracketOpen_Word,
        ThatIsAll_Word,

        ScalarVarType_Word,
        VectorVarType_Word,
        LinkVarType_Word,
        MacroVarType_Word,
        RefToUnit_Word,

        LabelSeparator,          // :
        TypeSeparator,          // --

        Round_Bracket_Open,     // (
        Round_Bracket_Close,    // )
        //Square_Bracket_Open,    // [
        //Square_Bracket_Close,   // ]

        EoL,                    // End of Line ";"
        Comma,                  // ,

        //Word       
        AssignOperator_Word,
        StringValue,
        ArithmeticOperator_1lvl,    // ^
        ArithmeticOperator_2lvl,    // * /
        ArithmeticOperator_3lvl,    // + -

        Time_Word,
        Rand_Word,
        Initiator_Word,
        Ref_Operator,           // ->


        TransferOperator_Word,
        InitiatorOperator_Word,
        ToOperator_Word,
        LabelOperator_Word,

        CreateOperator_Word,
        ObjectOperator_Word,
        RefToTypeOperator_Word,
        ActivateOperator_Word,
        FromOperator_Word,
        PassivateOperator_Word,
        IntoOperator_Word,
        TerminateOperator_Word,
        IfOperator_Word,
        ThenOperator_Word,
        ElseOperator_Word,
        WaitOperator_Word,

        ArithmeticFunction_Word,    // log lg ln ЦЕЛОЕ
        LogicOperator,         // /\ \/ 
        ComparisonOperator,     // = != > < >= <=

        // Составные
        Model,
        ModelEnding,

        Units,
        Unit,
        UnitHeader,
        UnitEnding,

        Description,
        DescriptionEnding,
        DescriptionLine,
        DescriptionLines,
        Vars,
        Var,
        VarDescription,
        RefToUnit,
        VarType,
        Names,

        Algorithm,
        AlgorithmEnding,
        AlgorithmLine,
        AlgorithmLines,
        Label,
        Operator,

        TerminateOperator,
        DeleteOperator,
        IfOperator,
        ActivateOperator,
        PassivateOperator,
        WaitOperator,
        TransferOperator,
        CreateOperator,
        AssignOperator,

        Value,
        ArithmeticExpression_1lvl,
        ArithmeticExpression_2lvl,
        ArithmeticExpression_3lvl,
        DigitalValue,
        ValueFromLink,
        Path,
        VectorNode,
        ArithmeticFunction,
        Parameters,

        Destination,
        IfConditions,
        IfCondition,
        AlternativeCondition,
        LogicExpression,
        WaitConditions,
        WaitUntil,
        WaitTime,
        WaitCondition,

        //нет при построении
        True,
        False
    }
    public enum WordType
    {
        KeyWord,
        SystemVar,
        ArithmeticFunction,
        Name
    }
    public enum ErrorType
    {
        EmptyText,
        UnknownSimbol,
        UnknownLexeme,

        ExpectedPhrase,

        Replacing,


        //UnknownPhrase
    }

    class ModelTextRules
    {
        //ЛЕКСИЧЕСКИЙ АНАЛИЗ
        #region Lexis
        /// <summary>
        /// Определяет символ как лексему
        /// </summary>
        /// <param name="ch">Рассматриваемый символ</param>
        /// <returns>Тип лексемы для символа</returns>
        public static LexemeType DetermineSymbol(char ch)
        {
            if (Char.IsLetter(ch))
                return LexemeType.Word;
            if (Char.IsNumber(ch))
                return LexemeType.Number;
            if ((ch == ' ') || (ch == '\t'))
                return LexemeType.Empty;
            if (ch == ';')
                return LexemeType.EoL;
            if (ch == '\n')
                return LexemeType.Enter;

            if (ch == '(')
                return LexemeType.Round_Bracket_Open;
            if (ch == ')')
                return LexemeType.Round_Bracket_Close;
            //if (ch == '[')
            //    return LexemeType.Square_Bracket_Open;
            //if (ch == ']')
            //    return LexemeType.Square_Bracket_Close;

            if (ch == '.')
                return LexemeType.Digit_Point;
            if (ch == ',')
                return LexemeType.Comma;

            if (ch == '-')
                return LexemeType.Minus;
            if (ch == ':')
                return LexemeType.LabelSeparator;
            if (ch == '"')
                return LexemeType.Quotes;
            if (ch == '*')
                return LexemeType.Star;
            if (ch == '!')
                return LexemeType.Exclamation_Point;
            if (ch == '/')
                return LexemeType.Slash;
            if (ch == '\\')
                return LexemeType.Back_Slash;
            if (ch == '=')
                return LexemeType.Equality;
            if ((ch == '<') || (ch == '>'))
                return LexemeType.Comparison;

            if (ch == '+')
                return LexemeType.Arithmetic_Operator_3lvl;
            if (ch == '^')
                return LexemeType.Arithmetic_Operator_1lvl;

            return LexemeType.Unknown_Symbol;
        }

        /// <summary>
        /// Список лексических шаблонов объединения
        /// </summary>
        public static List<LexemeTypeTemplate> LexicalTemplates;
        public static void InitializeLexicalTemplates()
        {
            LexicalTemplates = new List<LexemeTypeTemplate>();

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Word, LexemeType.Word, LexemeType.Word));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Word, LexemeType.Word, LexemeType.Number));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Word, LexemeType.Number, LexemeType.Word));

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Number, LexemeType.Number, LexemeType.Number));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Number, LexemeType.Number, LexemeType.Digit_Point, LexemeType.Number));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Empty, LexemeType.Empty, LexemeType.Empty));


            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Slash, LexemeType.Enter));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Slash, LexemeType.Comment_Slash, LexemeType.Anything));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Open, LexemeType.Anything));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Close));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.StringValue, LexemeType.Quotes, LexemeType.Quotes));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Quotes, LexemeType.Quotes, LexemeType.Anything));


            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Empty, LexemeType.Enter));


            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.AssignOperator_Word, LexemeType.LabelSeparator, LexemeType.Equality)); //:=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.TypeSeparator, LexemeType.Minus, LexemeType.Minus)); //--
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Exclamation_Point, LexemeType.Equality)); //!=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Comparison, LexemeType.Equality)); //>=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Ref_Operator, LexemeType.Minus, LexemeType.Comparison)); //->

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Logic_Operator, LexemeType.Slash, LexemeType.Back_Slash)); // /\
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Logic_Operator, LexemeType.Back_Slash, LexemeType.Slash)); // \/

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Open, LexemeType.Slash, LexemeType.Star)); // /*
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Close, LexemeType.Star, LexemeType.Slash)); // */
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Slash, LexemeType.Slash, LexemeType.Slash)); // //

            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Arithmetic_Operator_2lvl, LexemeType.Star)); // *
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Arithmetic_Operator_2lvl, LexemeType.Slash)); // /
            ////LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Arithmetic_Operator_3lvl, LexemeType.Minus)); // -
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Comparison)); //> or <
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Equality)); // =
        }
        #endregion

        //СИНТАКСИЧЕСКИЙ АНАЛИЗ
        #region Syntax
        

        /// <summary>
        /// Типы слов
        /// </summary>
        public static Dictionary<string, PhraseType> WordTypes;
        /// <summary>
        /// Инициализация словаря ключевых слов
        /// </summary>
        public static void InitializeWordTypes()
        {
            WordTypes = new Dictionary<string, PhraseType>()
            {
                {"модель",PhraseType.ModelBracketOpen_Word},
                {"блок",PhraseType.UnitBracketOpen_Word},
                {"описание",PhraseType.DescriptionBracketOpen_Word},
                {"алгоритм",PhraseType.AlgorithmBracketOpen_Word},
                {"все",PhraseType.ThatIsAll_Word},

                {"контроллер",PhraseType.UnitType_Word},
                {"агрегат",PhraseType.UnitType_Word},
                {"процессор",PhraseType.UnitType_Word},

                {"скаляр",PhraseType.ScalarVarType_Word},
                {"вектор",PhraseType.VectorVarType_Word},
                {"ссылка",PhraseType.LinkVarType_Word},
                {"макрос",PhraseType.MacroVarType_Word},

                {"блока",PhraseType.RefToUnit_Word},
                {"создать",PhraseType.CreateOperator_Word},
                {":=",PhraseType.AssignOperator_Word},

                {"объект",PhraseType.ObjectOperator_Word},
                {"типа",PhraseType.RefToTypeOperator_Word},
                {"активизировать",PhraseType.ActivateOperator_Word},
                {"из",PhraseType.FromOperator_Word},
                {"пассивизировать",PhraseType.PassivateOperator_Word},
                {"в",PhraseType.IntoOperator_Word},
                {"инициатор",PhraseType.InitiatorOperator_Word},
                {"если",PhraseType.IfOperator_Word},
                {"то",PhraseType.ThenOperator_Word},

                {"иначе",PhraseType.ElseOperator_Word},
                {"направить",PhraseType.TransferOperator_Word},
                {"на",PhraseType.ToOperator_Word},
                {"метку",PhraseType.LabelOperator_Word},
                {"ждать",PhraseType.WaitOperator_Word},
                {"уничтожить",PhraseType.TerminateOperator_Word},

                {"log",PhraseType.ArithmeticFunction_Word},
                {"lg",PhraseType.ArithmeticFunction_Word},
                {"ln",PhraseType.ArithmeticFunction_Word},
                {"ЦЕЛОЕ",PhraseType.ArithmeticFunction_Word},

                {"ВРЕМЯ",PhraseType.Time_Word},
                {"RAND",PhraseType.Rand_Word},
                {"ИНИЦИАТОР",PhraseType.Initiator_Word},
            };
        }
        public static PhraseType DetermineWord(Lexeme word)
        {
            foreach (KeyValuePair<string, PhraseType> word_str in WordTypes)
            {
                if (word.LValue == word_str.Key)
                    return word_str.Value;
            }
            return PhraseType.Name;
        }

        public static Dictionary<PhraseType, bool> PrimaryPhraseTypes;
        public static void InitializePrimaryPhraseTypes()
        {
            PrimaryPhraseTypes = new Dictionary<PhraseType, bool>()
            {
                                // Неопределенная лексема
                {PhraseType.UnknownLexeme, true},
                {PhraseType.Anything,true},
                //незначимые
                {PhraseType.Comment,true},
                {PhraseType.Empty,   true},
                
                // 0 LEVEL
                {PhraseType. Name,true},
                {PhraseType.Number,true},
                
                {PhraseType.ModelBracketOpen_Word,true},
                {PhraseType.UnitBracketOpen_Word,true},
                {PhraseType.UnitType_Word,true},
                {PhraseType.DescriptionBracketOpen_Word,true},
                {PhraseType.AlgorithmBracketOpen_Word,true},
                {PhraseType.ThatIsAll_Word,true},
                
                {PhraseType.ScalarVarType_Word,true},
                {PhraseType.VectorVarType_Word,true},
                {PhraseType.LinkVarType_Word,true},
                {PhraseType.MacroVarType_Word,true},
                {PhraseType.RefToUnit_Word,true},
                
                {PhraseType.LabelSeparator,true},          // :
                {PhraseType.TypeSeparator,true},          // --
                
                {PhraseType.Round_Bracket_Open, true},    // (
                {PhraseType.Round_Bracket_Close,   true}, // )
                //Square_Bracket_Open,    // [
                //Square_Bracket_Close,   // ]
                {PhraseType.EoL, true},                   // End of Line ";"
                {PhraseType.Comma,     true},             // ,
                
                //Word       
                {PhraseType.AssignOperator_Word,true},   
                {PhraseType.StringValue,true},
                {PhraseType.ArithmeticOperator_1lvl,true},    // ^
                {PhraseType.ArithmeticOperator_2lvl,   true}, // * /
                {PhraseType.ArithmeticOperator_3lvl,    true},// + -
                
                {PhraseType.Time_Word,true},
                {PhraseType.Rand_Word,true},
                {PhraseType.Initiator_Word,true},
                {PhraseType.Ref_Operator, true},          // ->
                
                {PhraseType.TransferOperator_Word,true},
                {PhraseType.InitiatorOperator_Word,true},
                {PhraseType.ToOperator_Word,true},
                {PhraseType.LabelOperator_Word,true},
                
                {PhraseType.CreateOperator_Word,true},
                {PhraseType.ObjectOperator_Word,true},
                {PhraseType.RefToTypeOperator_Word,true},
                {PhraseType.ActivateOperator_Word,true},
                {PhraseType.FromOperator_Word,true},
                {PhraseType.PassivateOperator_Word,true},
                {PhraseType.IntoOperator_Word,true},
                {PhraseType.TerminateOperator_Word,true},
                {PhraseType.IfOperator_Word,true},
                {PhraseType.ThenOperator_Word,true},
                {PhraseType.ElseOperator_Word,true},
                {PhraseType.WaitOperator_Word,true},
                
                {PhraseType.ArithmeticFunction_Word,true},    // log lg ln ЦЕЛОЕ
                {PhraseType.LogicOperator,true},         // /\ \/ 
                {PhraseType.ComparisonOperator,true},     // = != > < >= <=
                
                
                // Составные
                {PhraseType.Model,false},
                {PhraseType.ModelEnding,false},

                {PhraseType.Units,false},
                {PhraseType.Unit,false},
                {PhraseType.UnitHeader,false},
                {PhraseType.UnitEnding,false},
                
                {PhraseType.Description,false},
                {PhraseType.DescriptionEnding,false},
                {PhraseType.DescriptionLine,false},
                {PhraseType.DescriptionLines,false},
                {PhraseType.Vars,false},
                {PhraseType.Var,false},
                {PhraseType.VarDescription,false},
                {PhraseType.RefToUnit,false},
                {PhraseType.VarType,false},
                {PhraseType.Names,false},
                
                {PhraseType.Algorithm,false},
                {PhraseType.AlgorithmEnding,false},
                {PhraseType.AlgorithmLine,false},
                {PhraseType.AlgorithmLines,false},
                {PhraseType.Label,false},
                {PhraseType.Operator,false},
                
                {PhraseType.TerminateOperator,false},
                {PhraseType.IfOperator,false},
                {PhraseType.ActivateOperator,false},
                {PhraseType.PassivateOperator,false},
                {PhraseType.WaitOperator,false},
                {PhraseType.TransferOperator,false},
                {PhraseType.CreateOperator,false},
                {PhraseType.AssignOperator,false},
                
                {PhraseType.Value,false},
                {PhraseType.ArithmeticExpression_1lvl,false},
                {PhraseType.ArithmeticExpression_2lvl,false},
                {PhraseType.ArithmeticExpression_3lvl,false},
                {PhraseType.DigitalValue,false},
                {PhraseType.ValueFromLink,false},
                {PhraseType.Path,false},
                {PhraseType.VectorNode,false},
                {PhraseType.ArithmeticFunction,false},
                {PhraseType.Parameters,false},
                
                {PhraseType.Destination,false},
                {PhraseType.IfConditions,false},
                {PhraseType.IfCondition,false},
                {PhraseType.AlternativeCondition,false},
                {PhraseType.LogicExpression,false},
                {PhraseType.WaitConditions,false},
                {PhraseType.WaitUntil,false},
                {PhraseType.WaitTime,false},
                {PhraseType.WaitCondition, false}
            };

        }

        public static PhraseType DeterminePhrase(Lexeme lexeme)
        {
            switch (lexeme.LType)
            {
                case LexemeType.Word:
                    return DetermineWord(lexeme);
                case LexemeType.AssignOperator_Word:
                    return PhraseType.AssignOperator_Word;
                case LexemeType.Number:
                    return PhraseType.Number;

                //case LexemeType.Enter:
                //    return PhraseType.Enter;
                case LexemeType.Empty:
                    return PhraseType.Empty;
                case LexemeType.EoL:
                    return PhraseType.EoL;

                case LexemeType.Round_Bracket_Open:
                    return PhraseType.Round_Bracket_Open;
                case LexemeType.Round_Bracket_Close:
                    return PhraseType.Round_Bracket_Close;
                //case LexemeType.Square_Bracket_Open:
                //    return PhraseType.Square_Bracket_Open;
                //case LexemeType.Square_Bracket_Close:
                //    return PhraseType.Square_Bracket_Close;

                case LexemeType.Comma:
                    return PhraseType.Comma;

                case LexemeType.LabelSeparator:
                    return PhraseType.LabelSeparator;
                case LexemeType.TypeSeparator:
                    return PhraseType.TypeSeparator;
                case LexemeType.StringValue:
                    return PhraseType.StringValue;

                case LexemeType.Arithmetic_Operator_1lvl:
                    return PhraseType.ArithmeticOperator_1lvl;
                case LexemeType.Arithmetic_Operator_2lvl:
                    return PhraseType.ArithmeticOperator_2lvl;
                case LexemeType.Arithmetic_Operator_3lvl:
                    return PhraseType.ArithmeticOperator_3lvl;
                case LexemeType.Minus:
                    return PhraseType.ArithmeticOperator_3lvl;
                case LexemeType.Star:
                    return PhraseType.ArithmeticOperator_2lvl;
                case LexemeType.Slash:
                    return PhraseType.ArithmeticOperator_2lvl;
                case LexemeType.Comparison:
                    return PhraseType.ComparisonOperator;
                case LexemeType.Comparison_Operator:
                    return PhraseType.ComparisonOperator;
                case LexemeType.Equality:
                    return PhraseType.ComparisonOperator;

                case LexemeType.Arithmetic_Function:
                    return PhraseType.ArithmeticFunction_Word;
                case LexemeType.Logic_Operator:
                    return PhraseType.LogicOperator;

                case LexemeType.Ref_Operator:
                    return PhraseType.Ref_Operator;

                case LexemeType.Comment:
                    return PhraseType.Comment;

                default:
                    return PhraseType.UnknownLexeme;
            }

        }

        public static List<PhraseTypeTemplate> SyntacticalTemplates;
        public static void InitializeSyntacticalTemplates()
        {
            SyntacticalTemplates = new List<PhraseTypeTemplate>();

            //заголовки и хвосты
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Model, PhraseType.ModelBracketOpen_Word, PhraseType.Name, PhraseType.Units));

            //блок
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Units, PhraseType.Unit, PhraseType.Units));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Units, PhraseType.ModelEnding));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Unit, PhraseType.UnitHeader, PhraseType.Description, PhraseType.Algorithm, PhraseType.UnitEnding));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.UnitHeader, PhraseType.UnitBracketOpen_Word, PhraseType.UnitType_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.UnitEnding, PhraseType.ThatIsAll_Word, PhraseType.UnitBracketOpen_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ModelEnding, PhraseType.ThatIsAll_Word, PhraseType.ModelBracketOpen_Word));

            //описание
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Description, PhraseType.DescriptionBracketOpen_Word, PhraseType.DescriptionLines/*, PhraseType.DescriptionEnding*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionEnding, PhraseType.ThatIsAll_Word, PhraseType.DescriptionBracketOpen_Word));

            //строка описания
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.EoL, PhraseType.DescriptionLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionEnding));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription, PhraseType.Comma, PhraseType.DescriptionLine));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription));

            
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription));

            //для вектора
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.Comma, PhraseType.DescriptionLines));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.Comma, PhraseType.DescriptionLine));

            //Var
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.Var, PhraseType.Comma, PhraseType.Vars));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.Var));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.AssignOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.Name));

            //VarDescription
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarDescription, PhraseType.VarType, PhraseType.RefToUnit));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarDescription, PhraseType.VarType));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.RefToUnit, PhraseType.RefToUnit_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.VectorVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.DescriptionLine, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.MacroVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.Names, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.MacroVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.ScalarVarType_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.LinkVarType_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Names, PhraseType.Name, PhraseType.Comma, PhraseType.Names));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Names, PhraseType.Name));

            //алгоритм
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Algorithm, PhraseType.AlgorithmBracketOpen_Word, PhraseType.AlgorithmLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmEnding, PhraseType.ThatIsAll_Word, PhraseType.AlgorithmBracketOpen_Word));

            //строка алгоритма
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLines, PhraseType.AlgorithmLine, PhraseType.AlgorithmLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLines, PhraseType.AlgorithmEnding));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Label, PhraseType.Operator/*, PhraseType.EoL*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Label, PhraseType.AlgorithmLine/*, PhraseType.EoL*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Operator/*, PhraseType.EoL*/));

            //метка
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Label, PhraseType.Name, PhraseType.LabelSeparator));

            //ОПЕРАТОРЫ
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.TransferOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.CreateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.ActivateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.PassivateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.TerminateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.AssignOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.IfOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.WaitOperator));

            //AssignOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.Name, PhraseType.AssignOperator_Word, PhraseType.Value));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.VectorNode, PhraseType.AssignOperator_Word, PhraseType.Value));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.ValueFromLink, PhraseType.AssignOperator_Word, PhraseType.Value));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.Name, PhraseType.AssignOperator_Word, PhraseType.Algorithm));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name, PhraseType.Round_Bracket_Open, PhraseType.Name, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.StringValue));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.ArithmeticExpression_3lvl));

            //ArithmeticExpression
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticOperator_3lvl, PhraseType.ArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.ArithmeticExpression_2lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticExpression_1lvl, PhraseType.ArithmeticOperator_2lvl, PhraseType.ArithmeticExpression_2lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticExpression_1lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.DigitalValue, PhraseType.ArithmeticOperator_1lvl, PhraseType.ArithmeticExpression_1lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.DigitalValue));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));

            //функции
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.ArithmeticFunction));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticFunction, PhraseType.ArithmeticFunction_Word, PhraseType.Round_Bracket_Open, PhraseType.Parameters, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Parameters, PhraseType.ArithmeticExpression_3lvl, PhraseType.Comma, PhraseType.Parameters));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Parameters, PhraseType.ArithmeticExpression_3lvl));

            //ссылки
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.ValueFromLink));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.VectorNode));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.LinkVarType_Word, PhraseType.ToOperator_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ValueFromLink, PhraseType.Initiator_Word, PhraseType.Ref_Operator, PhraseType.Path));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ValueFromLink, PhraseType.Name, PhraseType.Ref_Operator, PhraseType.Path));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.VectorVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.ScalarVarType_Word));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.VectorNode));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.Name));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name));



            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.Rand_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.Time_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.Number));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DigitalValue, PhraseType.Name));

            //TransferOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.TransferOperator, PhraseType.TransferOperator_Word, PhraseType.InitiatorOperator_Word, PhraseType.ToOperator_Word, PhraseType.Destination));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Destination, PhraseType.LabelOperator_Word, PhraseType.Name, PhraseType.RefToUnit));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Destination, PhraseType.LabelOperator_Word, PhraseType.Name));

            //CreateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.CreateOperator, PhraseType.CreateOperator_Word, PhraseType.ObjectOperator_Word, PhraseType.Name, PhraseType.RefToTypeOperator_Word, PhraseType.VarType));

            //IfOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.IfOperator, PhraseType.IfConditions/*, PhraseType.AlternativeCondition*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.IfConditions, PhraseType.IfCondition, PhraseType.IfConditions));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.IfConditions, PhraseType.AlternativeCondition, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlternativeCondition, PhraseType.ElseOperator_Word, PhraseType.TransferOperator));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.IfCondition, PhraseType.IfOperator_Word, PhraseType.LogicExpression, PhraseType.ThenOperator_Word, PhraseType.TransferOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Round_Bracket_Open, PhraseType.Value, PhraseType.ComparisonOperator, PhraseType.Value, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Round_Bracket_Open, PhraseType.LogicExpression, PhraseType.LogicOperator, PhraseType.LogicExpression, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Round_Bracket_Open, PhraseType.LogicExpression, PhraseType.Round_Bracket_Close));

            //WaitOperator
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitCondition, PhraseType.WaitConditions));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitConditions));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitTime, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitUntil, PhraseType.EoL));


            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitTime, PhraseType.WaitOperator_Word, PhraseType.Time_Word, PhraseType.ComparisonOperator, PhraseType.ArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitUntil, PhraseType.WaitOperator_Word, PhraseType.LogicExpression));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions, PhraseType.WaitCondition, PhraseType.WaitConditions));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions, PhraseType.WaitCondition, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitTime, PhraseType.ThenOperator_Word, PhraseType.TransferOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitUntil, PhraseType.ThenOperator_Word, PhraseType.TransferOperator));

            //ActivateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ActivateOperator, PhraseType.ActivateOperator_Word, PhraseType.InitiatorOperator_Word, PhraseType.FromOperator_Word, PhraseType.Name, PhraseType.TransferOperator));
            //PassivateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.PassivateOperator, PhraseType.PassivateOperator_Word, PhraseType.InitiatorOperator_Word, PhraseType.IntoOperator_Word, PhraseType.Name));
            //TerminateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DeleteOperator, PhraseType.TerminateOperator_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.TerminateOperator, PhraseType.TerminateOperator_Word, PhraseType.Initiator_Word));
        }
        #endregion

        //ОШИБКИ
        #region Errors
        /// <summary>
        /// Типы ошибок
        /// </summary>
        public static Dictionary<ErrorType, string> ErrorTypes;
        /// <summary>
        /// Инициализация типов ошибок
        /// </summary>
        public static void InitializeErrorTypes()
        {
            ErrorTypes = new Dictionary<ErrorType, string>()
            {
                {ErrorType.EmptyText, "Не найден текст модели."},
                {ErrorType.UnknownSimbol, "Неизвестный символ: "},
                {ErrorType.UnknownLexeme, "Неизвестнная лексема: "},

                {ErrorType.ExpectedPhrase, "Ожидаемая фраза: "},

                {ErrorType.Replacing, "На этом месте ожидалось: "},

                //{ErrorType.UnknownPhrase, "Неизвестнная конструкция: "}
            };
        }
        #endregion
    }
}
