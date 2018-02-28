using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace LPDP.TextAnalysis
{
    /// <summary>
    /// Типы лексем
    /// </summary>
    public enum LexemeType
    {
        Anything,

        Word,
        SetOperator_Word,    // :=
        Number,

        Enter,                  // \n
        Empty,                  // " " \t
        EoL,                    // End of Line ";"

        Round_Bracket_Open,     // (
        Round_Bracket_Close,    // )
        Square_Bracket_Open,    // [
        Square_Bracket_Close,   // ]

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
        //Logic_Operator,         //  /\ \/ 
        Comparison_Operator,    //= != > < >= <=
        Ref_Operator,           // ->

        Comment_Bracket_Open,   // /*
        Comment_Bracket_Close,  // */
        Comment_Slash,           // //

        Comment,
        String,

        Unknown_Symbol


    }
    public enum PhraseType
    {
        // Неопределенная лексема
        Error,
        UnknownLexeme,
        Anything,

        //незначимые
        Comment,
        Empty,                  // " " \t \т

        // 0 LEVEL
        Name,
        Number,

        ModelBracket_Word,
        UnitBracket_Word,
        UnitType_Word,
        DescriptionBracket_Word,
        AlgorithmBracket_Word,

        //[Description("всё")]
        ThatIsAll_Word,

        ScalarVarType_Word,
        VectorVarType_Word,
        LinkVarType_Word,
        MacroVarType_Word,
        RefToUnit_Word,

        LabelSeparator,          // :

        //[Description("--")]
        TypeSeparator,          // --

        Round_Bracket_Open,     // (
        Round_Bracket_Close,    // )
        Square_Bracket_Open,    // [
        Square_Bracket_Close,   // ]

        EoL,                    // End of Line ";"
        Comma,                  // ,

        //Word       
        SetOperator_Word,
        AssignOperator_Word,
        String,
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
        //AnotherUnit,
        UnitHeader,
        UnitEnding,

        Description,
        DescriptionEnding,
        DescriptionLine,
        AnotherDescriptionLine,
        DescriptionLines,
        Vars,
        InitialVar,
        AnotherVars,
        InitialValue,
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
        //Expression,
        ArithmeticExpression_1lvl,
        ArithmeticExpression_2lvl,
        ArithmeticExpression_3lvl,
        AnotherArithmeticExpression_1lvl,
        AnotherArithmeticExpression_2lvl,
        AnotherArithmeticExpression_3lvl,

        FinalValue,
        Var,
        ValueFromLink,
        //Path,
        VectorNode,
        ArithmeticFunction,
        Parameters,
        AnotherParameters,

        Destination,
        IfConditions,
        IfCondition,
        AlternativeCondition,
        LogicExpression,
        AnotherLogicExpression,
        ComparisonExpression,
        WaitConditions,
        WaitUntil,
        WaitTime,
        WaitCondition,
        ComplexWaitCondition,
        AnotherWaitCondition,

        //нет при построении
        True,
        False
    }
    public enum WordType
    {
        KeyWord,
        SystemWord,
        ArithmeticFunction,
        //LogicOperator,
        Name      
    }

    public static class ModelTextRules
    {
        public static void SetRules()
        {
            ModelTextRules.InitializeLexicalTemplates();
            ModelTextRules.InitializeWordTypes();
            ModelTextRules.InitializePrimaryPhraseTypes();
            ModelTextRules.InitializePhraseTypeCommonNames();
            ModelTextRules.InitializeSyntacticalTemplates();
            //ModelTextRules.InitializeErrorTypes();
        }


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
            if (ch == '_')
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
            if (ch == '[')
                return LexemeType.Square_Bracket_Open;
            if (ch == ']')
                return LexemeType.Square_Bracket_Close;

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


            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Slash, LexemeType.Enter));
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Slash, LexemeType.Comment_Slash, LexemeType.Anything));
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Close));
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Open, LexemeType.Anything));            
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.String, LexemeType.Quotes, LexemeType.Quotes));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Quotes, LexemeType.Quotes, LexemeType.Anything));

            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Empty, LexemeType.Enter));

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.SetOperator_Word, LexemeType.LabelSeparator, LexemeType.Equality)); //:=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.TypeSeparator, LexemeType.Minus, LexemeType.Minus)); //--
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Exclamation_Point, LexemeType.Equality)); //!=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comparison_Operator, LexemeType.Comparison, LexemeType.Equality)); //>=
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Ref_Operator, LexemeType.Minus, LexemeType.Comparison)); //->

            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Logic_Operator, LexemeType.Slash, LexemeType.Back_Slash)); // /\
            //LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Logic_Operator, LexemeType.Back_Slash, LexemeType.Slash)); // \/

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Open, LexemeType.Slash, LexemeType.Star)); // /*
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Close, LexemeType.Star, LexemeType.Slash)); // */
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Slash, LexemeType.Slash, LexemeType.Slash)); // //

            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Slash, LexemeType.Enter));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Slash, LexemeType.Comment_Slash, LexemeType.Anything));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment, LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Close));
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Comment_Bracket_Open, LexemeType.Comment_Bracket_Open, LexemeType.Anything, LexemeType.Anything));
            
            LexicalTemplates.Add(new LexemeTypeTemplate(LexemeType.Empty, LexemeType.Enter));
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
                {"модель",PhraseType.ModelBracket_Word},
                {"блок",PhraseType.UnitBracket_Word},
                {"описание",PhraseType.DescriptionBracket_Word},
                {"алгоритм",PhraseType.AlgorithmBracket_Word},
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
                {"присвоить",PhraseType.AssignOperator_Word},
                {":=",PhraseType.SetOperator_Word},

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
                {"И",PhraseType.LogicOperator},
                {"ИЛИ",PhraseType.LogicOperator},
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

        //словарь терминалов/нетерминалов
        public static Dictionary<PhraseType, bool> PrimaryPhraseTypes;
        public static void InitializePrimaryPhraseTypes()
        {
            PrimaryPhraseTypes = new Dictionary<PhraseType, bool>();
            foreach (PhraseType pt in Enum.GetValues(typeof(PhraseType)))
            {
                PrimaryPhraseTypes.Add(pt, false);
            }
            // Неопределенная лексема
            PrimaryPhraseTypes[PhraseType.Error] = true;
            PrimaryPhraseTypes[PhraseType.UnknownLexeme] = true;
            PrimaryPhraseTypes[PhraseType.Anything] = true;
            //незначимые
            PrimaryPhraseTypes[PhraseType.Comment] = true;
            PrimaryPhraseTypes[PhraseType.Empty] = true;

            // 0 LEVEL
            PrimaryPhraseTypes[PhraseType.Name] = true;
            PrimaryPhraseTypes[PhraseType.Number] = true;

            PrimaryPhraseTypes[PhraseType.ModelBracket_Word] = true;
            PrimaryPhraseTypes[PhraseType.UnitBracket_Word] = true;
            PrimaryPhraseTypes[PhraseType.UnitType_Word] = true;
            PrimaryPhraseTypes[PhraseType.DescriptionBracket_Word] = true;
            PrimaryPhraseTypes[PhraseType.AlgorithmBracket_Word] = true;
            PrimaryPhraseTypes[PhraseType.ThatIsAll_Word] = true;

            PrimaryPhraseTypes[PhraseType.ScalarVarType_Word] = true;
            PrimaryPhraseTypes[PhraseType.VectorVarType_Word] = true;
            PrimaryPhraseTypes[PhraseType.LinkVarType_Word] = true;
            PrimaryPhraseTypes[PhraseType.MacroVarType_Word] = true;
            PrimaryPhraseTypes[PhraseType.RefToUnit_Word] = true;

            PrimaryPhraseTypes[PhraseType.LabelSeparator] = true;          // :
            PrimaryPhraseTypes[PhraseType.TypeSeparator] = true;          // --

            PrimaryPhraseTypes[PhraseType.Round_Bracket_Open] = true;    // (
            PrimaryPhraseTypes[PhraseType.Round_Bracket_Close] = true; // )
            PrimaryPhraseTypes[PhraseType.Square_Bracket_Open] = true;    // [
            PrimaryPhraseTypes[PhraseType.Square_Bracket_Close] = true; // ]

            PrimaryPhraseTypes[PhraseType.EoL] = true;                   // End of Line ";"
            PrimaryPhraseTypes[PhraseType.Comma] = true;             // ,

            //Word       
            PrimaryPhraseTypes[PhraseType.AssignOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.SetOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.String] = true;
            PrimaryPhraseTypes[PhraseType.ArithmeticOperator_1lvl] = true;    // ^
            PrimaryPhraseTypes[PhraseType.ArithmeticOperator_2lvl] = true; // * /
            PrimaryPhraseTypes[PhraseType.ArithmeticOperator_3lvl] = true;// + -

            PrimaryPhraseTypes[PhraseType.Time_Word] = true;
            PrimaryPhraseTypes[PhraseType.Rand_Word] = true;
            PrimaryPhraseTypes[PhraseType.Initiator_Word] = true;
            PrimaryPhraseTypes[PhraseType.Ref_Operator] = true;          // ->

            PrimaryPhraseTypes[PhraseType.TransferOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.InitiatorOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.ToOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.LabelOperator_Word] = true;

            PrimaryPhraseTypes[PhraseType.CreateOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.ObjectOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.RefToTypeOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.ActivateOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.FromOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.PassivateOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.IntoOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.TerminateOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.IfOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.ThenOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.ElseOperator_Word] = true;
            PrimaryPhraseTypes[PhraseType.WaitOperator_Word] = true;

            PrimaryPhraseTypes[PhraseType.ArithmeticFunction_Word] = true;    // log lg ln ЦЕЛОЕ
            PrimaryPhraseTypes[PhraseType.LogicOperator] = true;         // /\ \/ 
            PrimaryPhraseTypes[PhraseType.ComparisonOperator] = true;     // = != > < >= <=          
        }

        //словарь Пользовательских названий фраз
        public static Dictionary<PhraseType, string> PhraseTypeCommonNames;
        public static void InitializePhraseTypeCommonNames()
        {
            PhraseTypeCommonNames = new Dictionary<PhraseType, string>();
            foreach (PhraseType pt in Enum.GetValues(typeof(PhraseType)))
            {
                PhraseTypeCommonNames.Add(pt, "Синтаксическое выражение");
            }            
            PhraseTypeCommonNames[PhraseType.ActivateOperator] = "Оператор активизации";
            PhraseTypeCommonNames[PhraseType.ActivateOperator_Word] = "Ключевое слово \"активизировать\"";
            PhraseTypeCommonNames[PhraseType.Algorithm] = "Алгоритм";
            PhraseTypeCommonNames[PhraseType.AlgorithmBracket_Word] = "Ключевое слово \"алгоритм\"";
            PhraseTypeCommonNames[PhraseType.AlgorithmEnding] = "Завершение алгоритма";
            PhraseTypeCommonNames[PhraseType.AlgorithmLine] = "Строка алгоритма";
            PhraseTypeCommonNames[PhraseType.AlgorithmLines] = "Строка алгоритма";
            PhraseTypeCommonNames[PhraseType.AlternativeCondition] = "Альтернативное решение";
            PhraseTypeCommonNames[PhraseType.ArithmeticExpression_1lvl] = "Арифметическое выражение";
            PhraseTypeCommonNames[PhraseType.ArithmeticExpression_2lvl] = "Арифметическое выражение";
            PhraseTypeCommonNames[PhraseType.ArithmeticExpression_3lvl] = "Арифметическое выражение";
            PhraseTypeCommonNames[PhraseType.ArithmeticFunction] = "Арифметическая функция";
            PhraseTypeCommonNames[PhraseType.ArithmeticFunction_Word] = "Название арифметической функции";
            PhraseTypeCommonNames[PhraseType.ArithmeticOperator_1lvl] = "Арифметический оператор";
            PhraseTypeCommonNames[PhraseType.ArithmeticOperator_2lvl] = "Арифметический оператор";
            PhraseTypeCommonNames[PhraseType.ArithmeticOperator_3lvl] = "Арифметический оператор";
            PhraseTypeCommonNames[PhraseType.AssignOperator] = "Оператор присваивания";
            PhraseTypeCommonNames[PhraseType.AssignOperator_Word] = "Ключевое слово \"присвоить\"";

            PhraseTypeCommonNames[PhraseType.Comma] = "Символ \",\"";
            PhraseTypeCommonNames[PhraseType.Comment] = "Комментарий";
            PhraseTypeCommonNames[PhraseType.ComparisonExpression] = "Выражение сравнения";
            PhraseTypeCommonNames[PhraseType.ComparisonOperator] = "Оператор сравнения";
            PhraseTypeCommonNames[PhraseType.ComplexWaitCondition] = "Ветвящийся оператор ожидания";
            PhraseTypeCommonNames[PhraseType.CreateOperator] = "Оператор создания";
            PhraseTypeCommonNames[PhraseType.CreateOperator_Word] = "Ключевое слово \"создать\"";

            PhraseTypeCommonNames[PhraseType.DeleteOperator] = "Оператор удаления";
            PhraseTypeCommonNames[PhraseType.Description] = "Описание блока";
            PhraseTypeCommonNames[PhraseType.DescriptionBracket_Word] = "Ключевое слово \"описание\"";
            PhraseTypeCommonNames[PhraseType.DescriptionEnding] = "Завершение описания";
            PhraseTypeCommonNames[PhraseType.DescriptionLine] = "Строка описания";
            PhraseTypeCommonNames[PhraseType.DescriptionLines] = "Строка описания";
            PhraseTypeCommonNames[PhraseType.Destination] = "Место назначения";

            PhraseTypeCommonNames[PhraseType.ElseOperator_Word] = "Ключевое слово \"иначе\"";
            //PhraseTypeCommonNames[PhraseType.Empty] = "";
            PhraseTypeCommonNames[PhraseType.EoL] = "Символ \";\"";
            //PhraseTypeCommonNames[PhraseType.Error] = "";
            //PhraseTypeCommonNames[PhraseType.Expression] = "";

            //PhraseTypeCommonNames[PhraseType.False] = "";
            PhraseTypeCommonNames[PhraseType.FinalValue] = "Значение";
            PhraseTypeCommonNames[PhraseType.FromOperator_Word] = "Ключевое слово \"из\"";

            PhraseTypeCommonNames[PhraseType.IfCondition] = "Условие";
            PhraseTypeCommonNames[PhraseType.IfConditions] = "Условие";
            PhraseTypeCommonNames[PhraseType.IfOperator] = "Оператор \"если\"";
            PhraseTypeCommonNames[PhraseType.IfOperator_Word] = "Ключевое слово \"если\"";
            PhraseTypeCommonNames[PhraseType.InitialValue] = "Начальное значение";
            PhraseTypeCommonNames[PhraseType.InitialVar] = "Инициализируемый объект";
            PhraseTypeCommonNames[PhraseType.Initiator_Word] = "Ключевое слово \"ИНИЦИАТОР\"";
            PhraseTypeCommonNames[PhraseType.InitiatorOperator_Word] = "Ключевое слово \"инициатор\"";
            PhraseTypeCommonNames[PhraseType.IntoOperator_Word] = "Ключевое слово \"в\"";

            PhraseTypeCommonNames[PhraseType.Label] = "Метка";
            PhraseTypeCommonNames[PhraseType.LabelOperator_Word] = "Ключевое слово \"метку\"";
            PhraseTypeCommonNames[PhraseType.LabelSeparator] = "Символ \":\"";
            PhraseTypeCommonNames[PhraseType.LinkVarType_Word] = "Ключевое слово \"ссылка\"";
            PhraseTypeCommonNames[PhraseType.LogicExpression] = "Логическое выражение";
            PhraseTypeCommonNames[PhraseType.LogicOperator] = "Логический оператор";

            PhraseTypeCommonNames[PhraseType.MacroVarType_Word] = "Ключевое слово \"макрос\"";
            PhraseTypeCommonNames[PhraseType.Model] = "Модель";
            PhraseTypeCommonNames[PhraseType.ModelBracket_Word] = "Ключевое слово \"модель\"";
            PhraseTypeCommonNames[PhraseType.ModelEnding] = "Завершение модели";

            PhraseTypeCommonNames[PhraseType.Name] = "Наименование";
            PhraseTypeCommonNames[PhraseType.Names] = "Подставляемое имя объекта";
            PhraseTypeCommonNames[PhraseType.Number] = "Число";

            PhraseTypeCommonNames[PhraseType.ObjectOperator_Word] = "Ключевое слово \"объект\"";
            PhraseTypeCommonNames[PhraseType.Operator] = "Оператор";

            PhraseTypeCommonNames[PhraseType.Parameters] = "Аргументы функции";
            PhraseTypeCommonNames[PhraseType.PassivateOperator] = "Оператор пассивмзации";
            PhraseTypeCommonNames[PhraseType.PassivateOperator_Word] = "Ключевое слово \"пассивизировать\"";

            PhraseTypeCommonNames[PhraseType.Rand_Word] = "Ключевое слово \"RAND\"";
            PhraseTypeCommonNames[PhraseType.Ref_Operator] = "оператор \"->\"";
            PhraseTypeCommonNames[PhraseType.RefToTypeOperator_Word] = "Ключевое слово \"типа\"";
            PhraseTypeCommonNames[PhraseType.RefToUnit] = "Ссылка на блок";
            PhraseTypeCommonNames[PhraseType.RefToUnit_Word] = "Ключевое слово \"блока\"";
            PhraseTypeCommonNames[PhraseType.Round_Bracket_Close] = "Символ \")\"";
            PhraseTypeCommonNames[PhraseType.Round_Bracket_Open] = "Символ \"(\"";

            PhraseTypeCommonNames[PhraseType.ScalarVarType_Word] = "Ключевое слово \"скаляр\"";
            PhraseTypeCommonNames[PhraseType.SetOperator_Word] = "Оператор \":=\"";
            PhraseTypeCommonNames[PhraseType.Square_Bracket_Close] = "Символ \"]\"";
            PhraseTypeCommonNames[PhraseType.Square_Bracket_Open] = "Символ \"[\"";
            PhraseTypeCommonNames[PhraseType.String] = "Строковое значение";

            PhraseTypeCommonNames[PhraseType.TerminateOperator] = "Оператор уничтожения";
            PhraseTypeCommonNames[PhraseType.TerminateOperator_Word] = "Ключевое слово \"уничтожить\"";
            PhraseTypeCommonNames[PhraseType.ThatIsAll_Word] = "Ключевое слово \"всё\"";
            PhraseTypeCommonNames[PhraseType.ThenOperator_Word] = "Ключевое слово \"то\"";
            PhraseTypeCommonNames[PhraseType.Time_Word] = "Ключевое слово \"ВРЕМЯ\"";
            PhraseTypeCommonNames[PhraseType.ToOperator_Word] = "Ключевое слово \"на\"";
            PhraseTypeCommonNames[PhraseType.TransferOperator] = "Оператор перенаправления";
            PhraseTypeCommonNames[PhraseType.TransferOperator_Word] = "Ключевое слово \"направить\"";
            PhraseTypeCommonNames[PhraseType.TypeSeparator] = "Оператор \"--\"";

            PhraseTypeCommonNames[PhraseType.Unit] = "Блок";
            PhraseTypeCommonNames[PhraseType.UnitBracket_Word] = "Ключевое слово \"блок\"";
            PhraseTypeCommonNames[PhraseType.UnitEnding] = "Завершение блока";
            PhraseTypeCommonNames[PhraseType.UnitHeader] = "Заголовок блока";
            PhraseTypeCommonNames[PhraseType.Units] = "Блок";
            PhraseTypeCommonNames[PhraseType.UnitType_Word] = "Тип блока";
            
            PhraseTypeCommonNames[PhraseType.Value] = "Значение";
            PhraseTypeCommonNames[PhraseType.ValueFromLink] = "Значение из ссылки";
            PhraseTypeCommonNames[PhraseType.Var] = "Переменная";
            PhraseTypeCommonNames[PhraseType.VarDescription] = "Описание переменной";
            PhraseTypeCommonNames[PhraseType.Vars] = "Переменная";
            PhraseTypeCommonNames[PhraseType.VarType] = "Тип переменной";
            PhraseTypeCommonNames[PhraseType.VectorNode] = "Векторный узел";
            PhraseTypeCommonNames[PhraseType.VectorVarType_Word] = "Ключевое слово \"вектор\"";

            PhraseTypeCommonNames[PhraseType.WaitCondition] = "Условие";
            PhraseTypeCommonNames[PhraseType.WaitConditions] = "Условие";
            PhraseTypeCommonNames[PhraseType.WaitOperator] = "Оператор ожидания";
            PhraseTypeCommonNames[PhraseType.WaitOperator_Word] = "Ключевое слово \"ждать\"";
            PhraseTypeCommonNames[PhraseType.WaitTime] = "Оператор ожидания времени";
            PhraseTypeCommonNames[PhraseType.WaitUntil] = "Оператор ожидания условия";
            
        }

        public static PhraseType DeterminePhrase(Lexeme lexeme)
        {
            switch (lexeme.LType)
            {
                case LexemeType.Word:
                    return DetermineWord(lexeme);
                case LexemeType.SetOperator_Word:
                    return PhraseType.SetOperator_Word;
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
                case LexemeType.Square_Bracket_Open:
                    return PhraseType.Square_Bracket_Open;
                case LexemeType.Square_Bracket_Close:
                    return PhraseType.Square_Bracket_Close;

                case LexemeType.Comma:
                    return PhraseType.Comma;

                case LexemeType.LabelSeparator:
                    return PhraseType.LabelSeparator;
                case LexemeType.TypeSeparator:
                    return PhraseType.TypeSeparator;
                case LexemeType.String:
                    return PhraseType.String;
                case LexemeType.Quotes:
                    return PhraseType.String;

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
                //case LexemeType.Logic_Operator:
                //    return PhraseType.LogicOperator;

                case LexemeType.Ref_Operator:
                    return PhraseType.Ref_Operator;

                case LexemeType.Comment:
                    return PhraseType.Comment;
                case LexemeType.Comment_Slash:
                    return PhraseType.Comment;
                case LexemeType.Comment_Bracket_Open:
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
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Model, PhraseType.ModelBracket_Word, PhraseType.Name, PhraseType.Units));

            //блок
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Units, PhraseType.Unit, PhraseType.Units));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Units, PhraseType.ModelEnding));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Unit, PhraseType.UnitHeader, PhraseType.Description, PhraseType.Algorithm, PhraseType.UnitEnding));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.UnitHeader, PhraseType.UnitBracket_Word, PhraseType.UnitType_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.UnitEnding, PhraseType.ThatIsAll_Word, PhraseType.UnitBracket_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ModelEnding, PhraseType.ThatIsAll_Word, PhraseType.ModelBracket_Word));

            //описание
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Description, PhraseType.DescriptionBracket_Word, PhraseType.DescriptionLines/*, PhraseType.DescriptionEnding*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionEnding, PhraseType.ThatIsAll_Word, PhraseType.DescriptionBracket_Word));

            //строка описания
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.EoL, PhraseType.DescriptionLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionEnding));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription, PhraseType.Comma, PhraseType.DescriptionLine));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription, PhraseType.AnotherDescriptionLine));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherDescriptionLine,PhraseType.Comma, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription, PhraseType.AnotherDescriptionLine));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherDescriptionLine));

            
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLine, PhraseType.Vars, PhraseType.TypeSeparator, PhraseType.VarDescription));

            //для вектора
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.Comma, PhraseType.DescriptionLines));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DescriptionLines, PhraseType.DescriptionLine, PhraseType.Comma, PhraseType.DescriptionLine));

            //Var
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.Var));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.InitialVar, PhraseType.AnotherVars));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherVars, PhraseType.Comma, PhraseType.InitialVar, PhraseType.AnotherVars));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherVars));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.AssignOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.InitialVar, PhraseType.Name, PhraseType.InitialValue));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.InitialValue,PhraseType.SetOperator_Word, PhraseType.Value));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.InitialValue));

            //Var
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.Var, PhraseType.Comma, PhraseType.Vars));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Vars, PhraseType.Var, PhraseType.AnotherVars));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherVars, PhraseType.Comma, PhraseType.Vars));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.AssignOperator));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.Name));

            //VarDescription
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarDescription, PhraseType.VarType, PhraseType.RefToUnit));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarDescription, PhraseType.VarType));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.RefToUnit, PhraseType.RefToUnit_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.RefToUnit));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.VectorVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.DescriptionLine, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.MacroVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.Names, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.MacroVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.ScalarVarType_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VarType, PhraseType.LinkVarType_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Names, PhraseType.Name, PhraseType.Comma, PhraseType.Names));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Names, PhraseType.Name));

            //алгоритм
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Algorithm, PhraseType.AlgorithmBracket_Word, PhraseType.AlgorithmLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmEnding, PhraseType.ThatIsAll_Word, PhraseType.AlgorithmBracket_Word));

            //строка алгоритма
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLines, PhraseType.AlgorithmLine, PhraseType.AlgorithmLines));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLines, PhraseType.AlgorithmEnding));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Label, PhraseType.Operator/*, PhraseType.EoL*/));
            
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Operator/*, PhraseType.EoL*/));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AlgorithmLine, PhraseType.Label, PhraseType.AlgorithmLine/*, PhraseType.EoL*/));

            //метка
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Label, PhraseType.Name, PhraseType.LabelSeparator));

            //ОПЕРАТОРЫ
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.TransferOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.CreateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.ActivateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.PassivateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.TerminateOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.DeleteOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.AssignOperator, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.IfOperator));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Operator, PhraseType.WaitOperator));

            //AssignOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.AssignOperator_Word, PhraseType.Var, PhraseType.SetOperator_Word, PhraseType.Value));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.AssignOperator_Word, PhraseType.VectorNode, PhraseType.SetOperator_Word, PhraseType.Value));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.AssignOperator_Word, PhraseType.ValueFromLink, PhraseType.SetOperator_Word, PhraseType.Value));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AssignOperator, PhraseType.Name, PhraseType.AssignOperator_Word, PhraseType.Algorithm));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.String));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.ArithmeticExpression_3lvl));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.True));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.False));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Value, PhraseType.Name));

            //ArithmeticExpression
            
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Round_Bracket_Open, PhraseType.Expression, PhraseType.Round_Bracket_Close));
            //!!!SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Expression, PhraseType.ArithmeticExpression_3lvl));
            //!!!SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Expression, PhraseType.LogicExpression));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticOperator_3lvl, PhraseType.ArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.ArithmeticExpression_2lvl, PhraseType.AnotherArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_3lvl,PhraseType.ArithmeticOperator_3lvl, PhraseType.ArithmeticExpression_3lvl, PhraseType.AnotherArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_3lvl));


            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticExpression_1lvl, PhraseType.ArithmeticOperator_2lvl, PhraseType.ArithmeticExpression_2lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.ArithmeticExpression_1lvl, PhraseType.AnotherArithmeticExpression_2lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_2lvl, PhraseType.ArithmeticOperator_2lvl, PhraseType.ArithmeticExpression_2lvl, PhraseType.AnotherArithmeticExpression_2lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_2lvl));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.DigitalValue, PhraseType.ArithmeticOperator_1lvl, PhraseType.ArithmeticExpression_1lvl));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.DigitalValue));
            
            //!!!SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.FinalValue, PhraseType.AnotherArithmeticExpression_1lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_1lvl, PhraseType.ArithmeticOperator_1lvl, PhraseType.ArithmeticExpression_1lvl, PhraseType.AnotherArithmeticExpression_1lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherArithmeticExpression_1lvl));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            

            //функции
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.ArithmeticFunction));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticFunction, PhraseType.ArithmeticFunction_Word, PhraseType.Round_Bracket_Open, PhraseType.Parameters, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Parameters, PhraseType.ArithmeticExpression_3lvl, PhraseType.AnotherParameters));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherParameters, PhraseType.Comma, PhraseType.ArithmeticExpression_3lvl, PhraseType.AnotherParameters));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherParameters));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_2lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ArithmeticExpression_1lvl, PhraseType.Round_Bracket_Open, PhraseType.ArithmeticExpression_3lvl, PhraseType.Round_Bracket_Close));

            //значения
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.Rand_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.Time_Word));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.Number));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.String));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.LinkVarType_Word, PhraseType.ToOperator_Word, PhraseType.Name));
            
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.FinalValue, PhraseType.Var));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.Name, PhraseType.VectorNode));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.Name, PhraseType.ValueFromLink));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Var, PhraseType.Initiator_Word, PhraseType.ValueFromLink));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Round_Bracket_Open, PhraseType.Name, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ValueFromLink, PhraseType.Ref_Operator, PhraseType.Var));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ValueFromLink, PhraseType.Ref_Operator, PhraseType.Path));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.VectorVarType_Word, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.ScalarVarType_Word));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.VectorNode));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.Path, PhraseType.Name));

            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name, PhraseType.Round_Bracket_Open, PhraseType.VectorNode, PhraseType.Round_Bracket_Close));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.VectorNode, PhraseType.Name));





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
            //!!!SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Round_Bracket_Open, PhraseType.LogicExpression, PhraseType.Round_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.ComparisonExpression, PhraseType.AnotherLogicExpression));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherLogicExpression, PhraseType.LogicOperator, PhraseType.LogicExpression, PhraseType.AnotherLogicExpression));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherLogicExpression));

            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ComparisonExpression, PhraseType.Value, PhraseType.ComparisonOperator, PhraseType.Value));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.ComparisonExpression));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.Square_Bracket_Open, PhraseType.LogicExpression, PhraseType.Square_Bracket_Close));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ComparisonExpression, PhraseType.Square_Bracket_Open, PhraseType.ComparisonExpression, PhraseType.Square_Bracket_Close));
            
            
            
           // SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.LogicExpression, PhraseType.LogicExpression, PhraseType.LogicOperator, PhraseType.LogicExpression));//!!!

            //WaitOperator
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitCondition, PhraseType.WaitConditions));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitConditions));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitOperator, PhraseType.WaitOperator_Word, PhraseType.WaitCondition, PhraseType.ComplexWaitCondition));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitTime));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitUntil));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ComplexWaitCondition, PhraseType.ThenOperator_Word, PhraseType.TransferOperator, PhraseType.AnotherWaitCondition));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ComplexWaitCondition, PhraseType.EoL));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherWaitCondition, PhraseType.WaitOperator_Word, PhraseType.WaitCondition, PhraseType.ThenOperator_Word, PhraseType.TransferOperator, PhraseType.AnotherWaitCondition));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.AnotherWaitCondition, PhraseType.EoL));



            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions, PhraseType.ThenOperator_Word, PhraseType.TransferOperator, PhraseType.WaitCondition));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions));




            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitTime, PhraseType.Time_Word, PhraseType.ComparisonOperator, PhraseType.ArithmeticExpression_3lvl));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitUntil, PhraseType.LogicExpression));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions, PhraseType.WaitCondition, PhraseType.WaitConditions));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitConditions, PhraseType.WaitCondition, PhraseType.EoL));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitTime, PhraseType.ThenOperator_Word, PhraseType.TransferOperator));
            //SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.WaitCondition, PhraseType.WaitUntil, PhraseType.ThenOperator_Word, PhraseType.TransferOperator));

            //ActivateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.ActivateOperator, PhraseType.ActivateOperator_Word, PhraseType.InitiatorOperator_Word, PhraseType.FromOperator_Word, PhraseType.Name, PhraseType.TransferOperator));
            //PassivateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.PassivateOperator, PhraseType.PassivateOperator_Word, PhraseType.InitiatorOperator_Word, PhraseType.IntoOperator_Word, PhraseType.Name));
            //TerminateOperator
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.DeleteOperator, PhraseType.TerminateOperator_Word, PhraseType.Name));
            SyntacticalTemplates.Add(new PhraseTypeTemplate(PhraseType.TerminateOperator, PhraseType.TerminateOperator_Word, PhraseType.Initiator_Word));
        }     
        #endregion
    }
}
