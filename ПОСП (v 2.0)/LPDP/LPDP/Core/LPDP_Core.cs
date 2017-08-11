using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    static class LPDP_Core
    {
        //Записи в таблицах
        public struct record_FTT
        {
            public int ID;
            public double ActiveTime;
            public int Initiator;
            public int Subprogram_Index;
        }
        public struct record_CT
        {
            public int ID;
            public string Condition;
            public string FromUnit;
            public int Initiator;
            public int Subprogram_Index;
        }
        public struct record_MARK
        {
            public string Name;
            public string Unit;
            public int Subprogram_Index;
        }
        public struct record_Queue
        {
            public enum condition { True, False, Time };
            public double Time;
            public condition Condition;
            public int Initiator;
            public int ID;
        }
        public struct POSP_Queue
        {
            public List<record_Queue> Queue;
        }

        // ТАБЛИЦЫ
        public static List<LPDP_Object> GlobalVar_Table = new List<LPDP_Object>();
        public static List<LocalArea> LocalArea_Table = new List<LocalArea>();

        public static List<record_FTT> FTT = new List<record_FTT>();
        public static List<record_CT> CT = new List<record_CT>();
        public static List<record_MARK> MARKS = new List<record_MARK>();
        public static List<POSP_Queue> Queues = new List<POSP_Queue>();

        // СПИСКИ ВСЕХ И КАЖДОЙ ПОДПРОГРАММЫ
        public struct operation
        {
            public string NameOperation;
            public List<string> Parameters;

            public int Unit_index;
            public int Line_index;
        }
        public struct SubProgram
        {
            //int Unit_Index;
            public List<operation> Operations;
        }
        public static List<SubProgram> POSP_Model = new List<SubProgram>();

        //ИДЕНТИФИКАТОРЫ
        public static double TIME = 0;
        public static int INITIATOR = -2;

        public static Random rand = new Random();

        // ОПЕРАТОРЫ
        static void Create_LPDP_Object(string Name, string Unit, string Type)
        {
            Type = Type.Replace(" ", "");
            int bracket1 = Type.IndexOf('(');
            int bracket2 = Type.LastIndexOf(')');
            string NameType = "";
            string Tree = "";

            if (bracket1 == -1) NameType = Type;
            else
            {
                NameType = Type.Substring(0, bracket1);
                Tree = Type.Substring(bracket1 + 1, bracket2 - bracket1 - 1);
            }
            GlobalVar_Table.RemoveAll(x => ((x.Name == Name) && (x.Unit == Unit)));

            switch (NameType)
            {
                case "скаляр":
                    Scalar New1 = new Scalar(Name, Unit);
                    LPDP_Core.GlobalVar_Table.Add(New1);
                    break;
                case "вектор":
                    Vector New2 = new Vector(Name, Unit, Tree);
                    LPDP_Core.GlobalVar_Table.Add(New2);
                    break;
                case "ссылка":
                    Link New3 = new Link(Name, Unit);
                    LPDP_Core.GlobalVar_Table.Add(New3);
                    break;
            }
        }
        static void Terminate_LPDP_Object(string Name, string Unit)
        {
            if (Name == "ИНИЦИАТОР") LPDP_Core.LocalArea_Table.RemoveAll(loc => loc.ID == INITIATOR);
            else LPDP_Core.GlobalVar_Table.RemoveAll(obj => ((obj.Name == Name) && (obj.Unit == Unit)));
        }
        static void SetValue(string ToObject, string Unit, string Value)
        {
            ToObject = ToObject.Replace(" ", "");
            int bracket1 = ToObject.IndexOf('(');
            int bracket2 = ToObject.LastIndexOf(')');
            string NameObj = "";
            string Tree = "";

            if (bracket1 == -1) NameObj = ToObject;
            else
            {
                NameObj = ToObject.Substring(0, bracket1);
                Tree = ToObject.Substring(bracket1 + 1, bracket2 - bracket1 - 1);
            }

            // для ИНИЦИАТОРа
            if (ToObject.Substring(0, ToObject.IndexOf('>') + 1) == "ИНИЦИАТОР->")
            {
                if (LPDP_Core.LocalArea_Table.Any<LocalArea>(loc => loc.ID == INITIATOR))
                {
                    int index = LPDP_Core.LocalArea_Table.FindIndex(loc => loc.ID == INITIATOR);
                    LPDP_Object.ObjectType type = LocalArea_Table[index].Value.Type;

                    switch (type)
                    {
                        case LPDP_Object.ObjectType.Scalar:
                            LPDP_Core.LocalArea_Table[index].Value.SetValue(Value);
                            break;

                        case LPDP_Object.ObjectType.Vector:
                            LPDP_Core.LocalArea_Table[index].Value.SetValue(Tree, Value);
                            break;
                    }
                }
                //else return "Ошибка нахождения значения в локальной среде";
            }

            // для объекта ПОСП
            if (LPDP_Core.GlobalVar_Table.Any<LPDP_Object>(obj => ((obj.Name == NameObj) && (obj.Unit == Unit))))
            {
                string ParentUnit = Unit;
                for (int i = 0; i < Pairs.Count; i++)
                {
                    if ((NameObj == Pairs[i].Name) && (Unit == Pairs[i].To))
                    {
                        ParentUnit = Pairs[i].From;
                        break;
                    }
                }
                int index = GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == ParentUnit)));
                //int index = POSP.GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == Unit)));
                LPDP_Object.ObjectType type = GlobalVar_Table[index].Type;
                switch (type)
                {
                    case LPDP_Object.ObjectType.Scalar:
                        GlobalVar_Table[index].SetValue(Value);
                        for (int i = 0; i < Pairs.Count; i++)
                        {
                            if ((NameObj == Pairs[i].Name) && (ParentUnit == Pairs[i].From))
                            {
                                index = GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == Pairs[i].To)));
                                GlobalVar_Table[index].SetValue(Value);
                            }
                        }
                        break;

                    case LPDP_Object.ObjectType.Vector:
                        GlobalVar_Table[index].SetValue(Tree, Value);
                        for (int i = 0; i < Pairs.Count; i++)
                        {
                            if ((NameObj == Pairs[i].Name) && (ParentUnit == Pairs[i].From))
                            {
                                index = GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == Pairs[i].To)));
                                GlobalVar_Table[index].SetValue(Tree, Value);
                            }
                        }
                        //POSP.GlobalVar_Table[index].SetValue(Tree, Value);
                        /////// для копирования "из блока"
                        //for (int i = 0; i < Pairs.Count; i++)
                        //{
                        //    if (((NameObj == Pairs[i].Name) && (Unit == Pairs[i].From)) || ((NameObj == Pairs[i].Name) && (Unit == Pairs[i].To)))
                        //    {
                        //        int newind = GlobalVar_Table.FindIndex(obj => ((obj.Name == Pairs[i].Name) && (obj.Unit == Pairs[i].To)));
                        //        GlobalVar_Table[newind].SetValue(Tree, Value);
                        //        int newind2 = GlobalVar_Table.FindIndex(obj => ((obj.Name == Pairs[i].Name) && (obj.Unit == Pairs[i].From)));
                        //        GlobalVar_Table[newind2].SetValue(Value);
                        //    }
                        //}
                        break;

                    case LPDP_Object.ObjectType.Link:
                        GlobalVar_Table[index].SetValue(Convert.ToInt32(Value));
                        for (int i = 0; i < Pairs.Count; i++)
                        {
                            if ((NameObj == Pairs[i].Name) && (ParentUnit == Pairs[i].From))
                            {
                                index = GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == Pairs[i].To)));
                                GlobalVar_Table[index].SetValue(Convert.ToInt32(Value));
                            }
                        }
                        break;
                }
            }
            //return "Неизвестное значение!";   


        }
        public static string GetValue(string FromVar, string Unit)
        {
            string result = "Ошибка!";
            FromVar = FromVar.Replace(" ", "");

            switch (FromVar)
            {
                case "ВРЕМЯ":
                    return result = Convert.ToString(TIME);

                case "RAND":
                    double r = rand.NextDouble();
                    r = SetPrecision(r, 4);
                    return result = Convert.ToString(r);

                default:
                    //if (FromVar.All<char>(x => (((int)x >= (int)'0') && (x <= (int)'9'))))
                    if (IsNumber(FromVar))
                    {
                        return result = FromVar;
                    }

                    if ((FromVar == "True") || (FromVar == "False")) return FromVar;

                    if ((FromVar[0] == '\"') && (FromVar[FromVar.Length - 1] == '\"'))
                    {
                        return result = FromVar;
                    }

                    int bracket1 = FromVar.IndexOf('(');
                    int bracket2 = FromVar.LastIndexOf(')');
                    string NameObj = "";
                    string Tree = "";

                    if (bracket1 == -1) NameObj = FromVar;
                    else
                    {
                        NameObj = FromVar.Substring(0, bracket1);
                        Tree = FromVar.Substring(bracket1 + 1, bracket2 - bracket1 - 1);
                    }

                    // для ИНИЦИАТОРа или ссылки
                    if (FromVar.IndexOf("->") != -1)
                    {
                        string linkName = FromVar.Substring(0, FromVar.IndexOf("->"));
                        if (linkName == "ИНИЦИАТОР")
                        {
                            if (LPDP_Core.LocalArea_Table.Any<LocalArea>(loc => loc.ID == INITIATOR))
                            {
                                int index = LPDP_Core.LocalArea_Table.FindIndex(loc => loc.ID == INITIATOR);
                                LPDP_Object.ObjectType type = LocalArea_Table[index].Value.Type;
                                switch (type)
                                {
                                    case LPDP_Object.ObjectType.Scalar:
                                        return result = LPDP_Core.LocalArea_Table[index].Value.GetValue();

                                    case LPDP_Object.ObjectType.Vector:
                                        return result = LPDP_Core.LocalArea_Table[index].Value.GetValue(Tree);
                                }
                            }
                            else return result + " Нет значения в локальной среде.";
                        }
                        else
                        {
                            if (LPDP_Core.LocalArea_Table.Any<LocalArea>(loc => loc.ID == Convert.ToInt32(GetValue(linkName, Unit))))
                            {
                                int index = LPDP_Core.LocalArea_Table.FindIndex(loc => loc.ID == Convert.ToInt32(GetValue(linkName, Unit)));
                                LPDP_Object.ObjectType type = LocalArea_Table[index].Value.Type;
                                switch (type)
                                {
                                    case LPDP_Object.ObjectType.Scalar:
                                        return result = LPDP_Core.LocalArea_Table[index].Value.GetValue();

                                    case LPDP_Object.ObjectType.Vector:
                                        return result = LPDP_Core.LocalArea_Table[index].Value.GetValue(Tree);
                                }
                            }
                            else return result + " Нет значения в локальной среде.";
                        }

                    }

                    // для объекта ПОСП
                    if (LPDP_Core.GlobalVar_Table.Any<LPDP_Object>(obj => ((obj.Name == NameObj) && (obj.Unit == Unit))))
                    {
                        int index = LPDP_Core.GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj) && (obj.Unit == Unit)));
                        LPDP_Object.ObjectType type = GlobalVar_Table[index].Type;
                        switch (type)
                        {
                            case LPDP_Object.ObjectType.Scalar:
                                return result = LPDP_Core.GlobalVar_Table[index].GetValue();

                            case LPDP_Object.ObjectType.Vector:
                                return result = LPDP_Core.GlobalVar_Table[index].GetValue(Tree);

                            case LPDP_Object.ObjectType.Link:
                                return result = LPDP_Core.GlobalVar_Table[index].GetValue();

                        }
                    }
                    if (FromVar.Contains("Ошибка!")) return FromVar;
                    return result + " Неизвестное значение: " + FromVar;
            }
        }
        static int LinkTo(string Name, string Unit)
        {
            LPDP_Object Obj = LPDP_Core.GlobalVar_Table.Find(obj => ((obj.Name == Name) && (obj.Unit == Unit)));
            if (Obj == null) return -1;
            else
            {
                LocalArea New = new LocalArea(Obj);
                LPDP_Core.LocalArea_Table.Add(New);
                return New.ID;
            }
        }


        // пары для объектов, повторяющихся в разных блоках
        public struct Pair
        {
            public string Name;
            public string From;
            public string To;
        }
        public static List<Pair> Pairs = new List<Pair>();
        public static void WriteTo_Pairs(string Name, string FromUnit, string ToUnit)
        {
            Pair New = new Pair();
            New.Name = Name; New.From = FromUnit; New.To = ToUnit;
            Pairs.Add(New);
            //New.Name = Name; New.From = ToUnit; New.To = FromUnit;
            //Pairs.Add(New);
        }


        // АРИФМЕТИКА и ЛОГИКА
        public static string[] Arithmetics = { "+", "-", "*", "/", "^", "log", "lg", "ln", /*"(", ")", */"[", "]", "ЦЕЛОЕ" };//арифметика
        public static string[] Logic = { "\\", "<", ">", "=", "!", "/"/*,  "(", ")"*/}; //логика
        /* удалены скобки!!!*/

        public static char[] SplitterSimbols = {' ','\t','\n',';','(',')','"',':','/','\\','*','*','+','-','>','<',
                                               '=','!','[',']','\'', '?', '.', '#', '@', '~', '|', '&' ,'%', '$',','};

        public static string[] SystemVar = { "ВРЕМЯ", "RAND", "ИНИЦИАТОР" }; // системные идентификаторы
        public static string[] FunctionWord = {"блока", "блок", "контроллер", "процессор","агрегат", "описание", "скаляр","вектор", "ссылка",
                                           "алгоритм",":=","создать","объект","типа", "активизировать","пассивизировать","инициатор","иначе",
                                           "уничтожить","направить","метку","ждать","если","то", "на","все", "из","в"}; // служебные слова     
        public static string[] OtherCharacters = { "\"", "'", "?", ".", "#", "@", "~", "|", "&", "%", "$", ":", ";", "," }; //другие символы                                                           

        public static string[] Errors = 
        {
            "Ошибка построения модели: ",
            "Ошибка выполнения моделирования: "
        };

        public static bool Here_Contains_Any_From(string str, string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (str.Contains(arr[i])) return true;
            }
            return false;
        }
        public static bool Here_Contains_NameOfVar(string exp, string Name)
        {
            exp = exp.Replace(" ", "");
            List<string> Splitted_Exp = new List<string>();

            //разбиваем на части
            string SubExp = "";
            for (int i = 0; i < exp.Length; i++)
            {
                if ((exp[i] == '(') || (exp[i] == ')') || (exp[i] == '=') || (exp[i] == '<') ||
                    (exp[i] == '>') || (exp[i] == '!') || (exp[i] == '/') || (exp[i] == '\\'))
                {
                    if (SubExp != "") Splitted_Exp.Add(SubExp);
                    SubExp = "";
                    Splitted_Exp.Add("" + exp[i]/*SubExp*/);
                    if (
                        ((exp[i] == '=') && (i > 0) && ((Splitted_Exp[Splitted_Exp.Count - 2] == "<") ||
                                                        (Splitted_Exp[Splitted_Exp.Count - 2] == ">") ||
                                                        (Splitted_Exp[Splitted_Exp.Count - 2] == "!")))//||
                        //(Splitted_Exp[Splitted_Exp.Count - 2] == "=")
                        || ((exp[i] == '/') && (i > 0) && (Splitted_Exp[Splitted_Exp.Count - 2] == "\\"))
                        || ((exp[i] == '\\') && (i > 0) && (Splitted_Exp[Splitted_Exp.Count - 2] == "/"))
                        )
                    {
                        Splitted_Exp[Splitted_Exp.Count - 2] += exp[i];
                        Splitted_Exp.RemoveAt(Splitted_Exp.Count - 1);
                    }
                }
                else
                {
                    SubExp += exp[i];
                }
            }
            if (SubExp != "") Splitted_Exp.Add(SubExp);
            SubExp = "";
            // разделили теперь находим ИНИЦИАТОР
            for (int i = 0; i < Splitted_Exp.Count - 2; i++)
            {
                if (Splitted_Exp[i] == "ИНИЦИАТОР-")
                {
                    if ((Splitted_Exp[i + 1] == ">") && ((Splitted_Exp[i + 2] == "вектор") || (Splitted_Exp[i + 2] == "скаляр")))
                    {
                        if (Splitted_Exp[i + 2] == "вектор")
                        {
                            bool exist_brackets = false;
                            int brackets = 0;
                            int j = i; //индекс конца выражения ИНИЦИАТОР->вектор(...(....))
                            while ((exist_brackets == false) || (brackets != 0))
                            {
                                j++;
                                if (Splitted_Exp[j] == "(") { brackets++; exist_brackets = true; }
                                if (Splitted_Exp[j] == ")") brackets--;
                            }
                            //нашли конец ИНИЦИАТОРа, теперь склеиваем
                            for (int k = i + 1; k <= j; k++) Splitted_Exp[i] += Splitted_Exp[k];
                            {
                                Splitted_Exp.RemoveRange(i + 1, j - i);
                                continue;
                            }

                        }
                        else
                        {
                            Splitted_Exp[i] += Splitted_Exp[i + 1];
                            Splitted_Exp[i] += Splitted_Exp[i + 2];
                            Splitted_Exp.RemoveRange(i + 1, 2);
                            //i -= 2; 
                            continue;
                        }
                    }
                    //else return "\"Ошибка! неправильно указан ИНИЦИАТОР\"";
                    else return false;
                }
            }

            //теперь раскладываем логические части на арифметические
            int savecount = Splitted_Exp.Count;
            for (int ii = 0; ii < savecount; ii++)
            {
                string exp1 = Splitted_Exp[ii];
                SubExp = "";
                for (int i = 0; i < exp1.Length; i++)
                {
                    if ((exp1[i] == '+') || (exp1[i] == '-') || (exp1[i] == '*') || (exp1[i] == '/') || (exp1[i] == '^') ||
                        (exp1[i] == '(') || (exp1[i] == ')') || (exp1[i] == '[') || (exp1[i] == ']'))
                    {
                        if (SubExp != "") Splitted_Exp.Add(SubExp);
                        SubExp = "" + exp1[i];
                        Splitted_Exp.Add(SubExp);
                        SubExp = "";
                    }
                    else
                    {
                        SubExp += exp1[i];
                        if ((SubExp == "log") || (SubExp == "lg") || (SubExp == "ln") || (SubExp == "ЦЕЛОЕ"))
                        {
                            Splitted_Exp.Add(SubExp);
                            SubExp = "";
                        }
                    }
                }
                if (SubExp != "") Splitted_Exp.Add(SubExp);
                SubExp = "";
            }
            Splitted_Exp.RemoveRange(0, savecount);
            // проверяем наличие имени
            for (int i = 0; i < Splitted_Exp.Count; i++)
            {
                if (Splitted_Exp[i] == Name) return true;
            }
            return false;
        }

        public static bool IsNumber(string str)
        {
            if (str == "") return false;
            if (str.All<char>(x => ((((int)x >= (int)'0') && ((int)x <= (int)'9')) || ((int)x == ',') || ((int)x == '-'))))
            {
                string substr = "";
                if (str[0] == '-') substr = str.Substring(1, str.Length - 1);
                else substr = str;
                if (substr.All<char>(x => ((((int)x >= (int)'0') && ((int)x <= (int)'9')) || ((int)x == ','))) && (substr.Length > 0)) return true;
            }
            return false;
        }
        public static bool IsString(string str)
        {
            if ((str[0] == '\"') && (str[str.Length - 1] == '\"')) return true;
            else return false;
        }
        public static bool IsValue(string str)
        {
            if (IsNumber(str) == true || IsString(str) == true) return true;
            else return false;
        }

        public static string IsCorrectName(string str)
        {
            if ((str.Contains("ИНИЦИАТОР->скаляр")) || (str.Contains("ИНИЦИАТОР->вектор"))) return "";
            if (str == "") return "Не указано имя";
            if (IsNumber(str)) return "Использовано числовое значение в качестве имени";
            if (Here_Contains_Any_From(str, Arithmetics))
                return "Использованы недопустимые арифметические символы в имени";
            if (Here_Contains_Any_From(str, Logic)) return "Использованы недопустимые логические символы в имени";
            for (int i = 0; i < SystemVar.Length; i++) if (SystemVar[i] == str) return "Использован системный идентификатор в качестве имени";
            //if (Here_Contains_Any_From(str, FunctionWord)) return "Использованы служебные слова в имени";
            if (Here_Contains_Any_From(str, OtherCharacters)) return "Использованы недопустимые символы в имени";
            return "";
        }

        public static double SetPrecision(double Value, int Precision)
        {
            int IntValue = (int)(Value * Math.Pow(10, Precision));
            Value = (double)IntValue / Math.Pow(10, Precision);
            return Value;
        }
        public static string Arithmetic_Expression(string exp, string Unit)
        {
            exp = exp.Replace(" ", "");
            List<string> Splitted_Exp = new List<string>();

            //разбиваем на части
            string SubExp = "";
            for (int i = 0; i < exp.Length; i++)
            {
                if ((exp[i] == '+') || (exp[i] == '-') || (exp[i] == '*') || (exp[i] == '/') || (exp[i] == '^') ||
                    (exp[i] == '(') || (exp[i] == ')') || (exp[i] == '[') || (exp[i] == ']'))
                {
                    if (SubExp != "") Splitted_Exp.Add(SubExp);
                    SubExp = "" + exp[i];
                    Splitted_Exp.Add(SubExp);
                    SubExp = "";
                }
                else
                {
                    SubExp += exp[i];
                    if ((SubExp == "log") || (SubExp == "lg") || (SubExp == "ln") || (SubExp == "ЦЕЛОЕ"))
                    {
                        Splitted_Exp.Add(SubExp);
                        SubExp = "";
                    }
                }
            }
            if (SubExp != "") Splitted_Exp.Add(SubExp);
            SubExp = "";
            // разделили теперь находим ИНИЦИАТОР
            for (int i = 0; i < Splitted_Exp.Count - 2; i++)
            {
                //if ( Splitted_Exp[i] == "ИНИЦИАТОР")
                //{
                if ((Splitted_Exp[i + 1] == "-") && ((Splitted_Exp[i + 2] == ">вектор") || (Splitted_Exp[i + 2] == ">скаляр")))
                {
                    if (Splitted_Exp[i + 2] == ">вектор")
                    {
                        bool exist_brackets = false;
                        int brackets = 0;
                        int j = i; //индекс конца выражения ИНИЦИАТОР->вектор(...(....))
                        while ((exist_brackets == false) || (brackets != 0))
                        {
                            j++;
                            if (Splitted_Exp[j] == "(") { brackets++; exist_brackets = true; }
                            if (Splitted_Exp[j] == ")") brackets--;
                        }
                        //нашли конец ИНИЦИАТОРа, теперь склеиваем
                        for (int k = i + 1; k <= j; k++)
                            Splitted_Exp[i] += Splitted_Exp[k];
                        Splitted_Exp.RemoveRange(i + 1, j - i);
                        continue;
                    }
                    else
                    {
                        Splitted_Exp[i] += Splitted_Exp[i + 1];
                        Splitted_Exp[i] += Splitted_Exp[i + 2];
                        Splitted_Exp.RemoveRange(i + 1, 2);
                        //i -= 2; 
                        continue;
                    }
                }
                //else return "\"Ошибка! неправильно указана ссылка\"";
                //}
            }
            // нашли ИНИЦИАТОР, теперь получаем значение всех переменных
            for (int i = 0; i < Splitted_Exp.Count; i++)
            {
                if ((Splitted_Exp[i] != "+") && (Splitted_Exp[i] != "-") && (Splitted_Exp[i] != "*") && (Splitted_Exp[i] != "/") &&
                    (Splitted_Exp[i] != "^") && (Splitted_Exp[i] != "log") && (Splitted_Exp[i] != "lg") && (Splitted_Exp[i] != "ln") && (Splitted_Exp[i] != "ЦЕЛОЕ") &&
                    (Splitted_Exp[i] != "(") && (Splitted_Exp[i] != ")") && (Splitted_Exp[i] != "[") && (Splitted_Exp[i] != "]"))
                {
                    if (LPDP_Core.GlobalVar_Table.Exists(var => (var.Name == Splitted_Exp[i]) && (var.Type == LPDP_Object.ObjectType.Vector)))
                    {
                        bool exist_brackets = false;
                        int brackets = 0;
                        int j = i; //индекс конца выражения Имя_вектора(...(....))
                        while ((exist_brackets == false) || (brackets != 0))
                        {
                            j++;
                            if (Splitted_Exp[j] == "(") { brackets++; exist_brackets = true; }
                            if (Splitted_Exp[j] == ")") brackets--;
                        }
                        //нашли конец вектора, теперь склеиваем
                        for (int k = i + 1; k <= j; k++)
                            Splitted_Exp[i] += Splitted_Exp[k];
                        Splitted_Exp.RemoveRange(i + 1, j - i);
                    }
                    Splitted_Exp[i] = LPDP_Core.GetValue(Splitted_Exp[i], Unit);
                    if (Splitted_Exp[i] == "") return "Ошибка! Переменной не присвоено значение";
                }
            }
            // подставили значения, теперь запускаем цикл решения выражения

            while (Splitted_Exp.Count > 1)
            {
                //проверка корректности вычислений
                for (int i = 0; i < Splitted_Exp.Count; i++)
                {
                    if (Splitted_Exp[i] == "NaN") return "Ошибка! Некорректное математическое выражение. Результат вычислений равен NaN.";
                }

                bool op = false;

                // вычисляем подвыражения
                int start_sub_exp = Splitted_Exp.IndexOf("(");
                int end_sub_exp = -1;
                int brackets = 1;
                for (int i = start_sub_exp + 1; i < Splitted_Exp.Count; i++)
                {
                    if (Splitted_Exp[i] == "(")
                    {
                        brackets++;
                        continue;
                    }
                    if (Splitted_Exp[i] == ")")
                    {
                        brackets--;
                        if (brackets == 0)
                        {
                            end_sub_exp = i;
                            break;
                        }
                    }
                }
                if ((start_sub_exp == -1) || (end_sub_exp == -1))
                { }
                else
                {
                    string sub_exp = "";
                    for (int i = start_sub_exp + 1; i < end_sub_exp; i++)
                        sub_exp += Splitted_Exp[i];
                    Splitted_Exp[start_sub_exp] = Arithmetic_Expression(sub_exp, Unit);
                    Splitted_Exp.RemoveRange(start_sub_exp + 1, end_sub_exp - start_sub_exp);
                    op = true;
                }
                if (op == true) continue;

                // ищем минусы
                for (int i = 0; i < Splitted_Exp.Count - 1; i++)
                {
                    if ((Splitted_Exp[i] == "-") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if (IsNumber(Splitted_Exp[i + 1]))
                        {
                            if (i == 0)
                            {
                                Splitted_Exp[i] = Convert.ToString(Convert.ToDouble(Splitted_Exp[i + 1]) * (-1));
                                Splitted_Exp.RemoveAt(i + 1);
                                op = true;
                                continue;
                            }
                            if ((Splitted_Exp[i - 1] == "(") || (Splitted_Exp[i - 1] == "[") || (Splitted_Exp[i - 1] == "]") ||
                                (Splitted_Exp[i - 1] == "lg") || (Splitted_Exp[i - 1] == "ln"))
                            {
                                Splitted_Exp[i] = Convert.ToString(Convert.ToDouble(Splitted_Exp[i + 1]) * (-1));
                                Splitted_Exp.RemoveAt(i + 1);
                                op = true;
                                continue;
                            }
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                }
                if (op == true) continue;


                ////ищем конструкцию "(" "12345" ")"
                //for (int i = 1; i < Splitted_Exp.Count - 1; i++ )
                //{
                //    if ((Splitted_Exp[i - 1] == "(") && (IsValue(Splitted_Exp[i]) == true) && (Splitted_Exp[i + 1] == ")"))
                //    {
                //        Splitted_Exp.RemoveAt(i + 1);
                //        Splitted_Exp.RemoveAt(i - 1);
                //        i--; op = true; continue;
                //    }
                //}
                //if (op == true) continue;

                //ищем логарифмы
                for (int i = 0; i < Splitted_Exp.Count - 1; i++)
                {
                    //конструкция "log" "[" "123" "]" "123"
                    if (Splitted_Exp[i] == "log")
                    {
                        if ((Splitted_Exp[i + 1] == "[") && (IsValue(Splitted_Exp[i + 2]) == true) &&
                        (Splitted_Exp[i + 3] == "]") && (IsValue(Splitted_Exp[i + 4]) == true))
                        {
                            if ((IsNumber(Splitted_Exp[i + 2]) == true) && (IsNumber(Splitted_Exp[i + 4]) == true))
                            {
                                Splitted_Exp[i] = Convert.ToString(SetPrecision(Math.Log(Convert.ToDouble(Splitted_Exp[i + 4]), Convert.ToDouble(Splitted_Exp[i + 2])), 4));
                                Splitted_Exp.RemoveRange(i + 1, 4);
                                op = true;
                                continue;
                            }
                            else return "Ошибка! Использование строк в числовом выражении (" +
                                GetValue(Splitted_Exp[i + 2], Unit) + ") или (" + GetValue(Splitted_Exp[i + 4], Unit) + ")";
                        }
                    }

                    //конструкция "lg" "123"
                    if ((Splitted_Exp[i] == "lg") && (IsValue(Splitted_Exp[i + 1]) == true))
                    {
                        if (IsNumber(Splitted_Exp[i + 1]) == true)
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Math.Log10(Convert.ToDouble(Splitted_Exp[i + 1])), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            op = true;
                            continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }

                    //конструкция "ln" "123"
                    if ((Splitted_Exp[i] == "ln") && (IsValue(Splitted_Exp[i + 1]) == true))
                    {
                        if (IsNumber(Splitted_Exp[i + 1]) == true)
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Math.Log(Convert.ToDouble(Splitted_Exp[i + 1])), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            op = true;
                            continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }

                    //конструкция "ЦЕЛОЕ" "123"
                    if ((Splitted_Exp[i] == "ЦЕЛОЕ") && (IsValue(Splitted_Exp[i + 1]) == true))
                    {
                        if (IsNumber(Splitted_Exp[i + 1]) == true)
                        {
                            Splitted_Exp[i] = Convert.ToString((int)(Convert.ToDouble(Splitted_Exp[i + 1])));
                            Splitted_Exp.RemoveAt(i + 1);
                            op = true;
                            continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                }// конец поиска логарифмов
                if (op == true) continue;

                //ищем конструкцию "123" "^" "123"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if ((IsValue(Splitted_Exp[i - 1])) && (Splitted_Exp[i] == "^") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if ((IsNumber(Splitted_Exp[i - 1])) && (IsNumber(Splitted_Exp[i + 1])))
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Math.Pow(Convert.ToDouble(Splitted_Exp[i - 1]), Convert.ToDouble(Splitted_Exp[i + 1])), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            Splitted_Exp.RemoveAt(i - 1);
                            op = true;
                            i--; continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" +
                            GetValue(Splitted_Exp[i - 1], Unit) + ") или (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                    if ((Splitted_Exp[i] == "]") || (Splitted_Exp[i] == "lg") || (Splitted_Exp[i] == "ln"))
                        i += 2;
                }
                if (op == true) continue;
                //ищем конструкцию "123" "*" "123" или  "123" "/" "123"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if ((IsValue(Splitted_Exp[i - 1])) && (Splitted_Exp[i] == "*") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if ((IsNumber(Splitted_Exp[i - 1])) && (IsNumber(Splitted_Exp[i + 1])))
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Convert.ToDouble(Splitted_Exp[i - 1]) * Convert.ToDouble(Splitted_Exp[i + 1]), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            Splitted_Exp.RemoveAt(i - 1);
                            op = true;
                            i--; continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" +
                            GetValue(Splitted_Exp[i - 1], Unit) + ") или (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                    if ((IsValue(Splitted_Exp[i - 1])) && (Splitted_Exp[i] == "/") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if ((IsNumber(Splitted_Exp[i - 1])) && (IsNumber(Splitted_Exp[i + 1])))
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Convert.ToDouble(Splitted_Exp[i - 1]) / Convert.ToDouble(Splitted_Exp[i + 1]), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            Splitted_Exp.RemoveAt(i - 1);
                            op = true;
                            i--; continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" +
                            GetValue(Splitted_Exp[i - 1], Unit) + ") или (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                    if ((Splitted_Exp[i] == "]") || (Splitted_Exp[i] == "lg") || (Splitted_Exp[i] == "ln"))
                        i += 2;
                }
                if (op == true) continue;
                //ищем конструкцию "123" "+" "123" или  "123" "-" "123"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if ((IsValue(Splitted_Exp[i - 1])) && (Splitted_Exp[i] == "+") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if ((IsNumber(Splitted_Exp[i - 1])) && (IsNumber(Splitted_Exp[i + 1])))
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Convert.ToDouble(Splitted_Exp[i - 1]) + Convert.ToDouble(Splitted_Exp[i + 1]), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            Splitted_Exp.RemoveAt(i - 1);
                            op = true;
                            i--; continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" +
                            GetValue(Splitted_Exp[i - 1], Unit) + ") или (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                    if ((IsValue(Splitted_Exp[i - 1])) && (Splitted_Exp[i] == "-") && (IsValue(Splitted_Exp[i + 1])))
                    {
                        if ((IsNumber(Splitted_Exp[i - 1])) && (IsNumber(Splitted_Exp[i + 1])))
                        {
                            Splitted_Exp[i] = Convert.ToString(SetPrecision(Convert.ToDouble(Splitted_Exp[i - 1]) - Convert.ToDouble(Splitted_Exp[i + 1]), 4));
                            Splitted_Exp.RemoveAt(i + 1);
                            Splitted_Exp.RemoveAt(i - 1);
                            op = true;
                            i--; continue;
                        }
                        else return "Ошибка! Использование строк в числовом выражении (" +
                            GetValue(Splitted_Exp[i - 1], Unit) + ") или (" + GetValue(Splitted_Exp[i + 1], Unit) + ")";
                    }
                    if ((Splitted_Exp[i] == "]") || (Splitted_Exp[i] == "lg") || (Splitted_Exp[i] == "ln"))
                        i += 2;
                }
                if (op == false)
                    return "Ошибка! Неправильное выражение"; ;
            }// конец цикла
            return GetValue(Splitted_Exp[0], Unit);

        }//конец ф-ции
        public static bool Logic_Expression(string exp, string Unit)
        {
            exp = exp.Replace(" ", "");
            List<string> Splitted_Exp = new List<string>();

            //разбиваем на части
            string SubExp = "";
            for (int i = 0; i < exp.Length; i++)
            {
                if ((exp[i] == '(') || (exp[i] == ')') || (exp[i] == '=') || (exp[i] == '<') ||
                    (exp[i] == '>') || (exp[i] == '!') || (exp[i] == '/') || (exp[i] == '\\'))
                {
                    if (SubExp != "") Splitted_Exp.Add(SubExp);
                    SubExp = "";
                    Splitted_Exp.Add("" + exp[i]/*SubExp*/);
                    if (
                        ((exp[i] == '=') && (i > 0) && ((Splitted_Exp[Splitted_Exp.Count - 2] == "<") ||
                                                        (Splitted_Exp[Splitted_Exp.Count - 2] == ">") ||
                                                        (Splitted_Exp[Splitted_Exp.Count - 2] == "!")))//||
                        //(Splitted_Exp[Splitted_Exp.Count - 2] == "=")
                        || ((exp[i] == '/') && (i > 0) && (Splitted_Exp[Splitted_Exp.Count - 2] == "\\"))
                        || ((exp[i] == '\\') && (i > 0) && (Splitted_Exp[Splitted_Exp.Count - 2] == "/"))
                        )
                    {
                        Splitted_Exp[Splitted_Exp.Count - 2] += exp[i];
                        Splitted_Exp.RemoveAt(Splitted_Exp.Count - 1);
                    }


                    //SubExp = "";
                }
                else
                {
                    SubExp += exp[i];
                    //if ((SubExp == "=") || (SubExp == "<=") || (SubExp == ">=") || (SubExp == "!=") || 
                    //    (SubExp == "/\\") || (SubExp == "\\/") || (SubExp == "<") || (SubExp == ">") || 
                    //    (SubExp == "(") || (SubExp == ")"))
                    //{
                    //    if ((i > 0) && (Splitted_Exp[i] == "=") && ((Splitted_Exp[i - 1] == "<") || (Splitted_Exp[i] == ">")))
                    //    {
                    //        Splitted_Exp[i - 1] += Splitted_Exp[i];
                    //        Splitted_Exp.RemoveAt(i);
                    //        i--;
                    //    }
                    //    Splitted_Exp.Add(SubExp);
                    //    SubExp = "";
                    //}
                }
            }
            if (SubExp != "") Splitted_Exp.Add(SubExp);
            SubExp = "";
            // разделили теперь находим ИНИЦИАТОР
            for (int i = 0; i < Splitted_Exp.Count - 2; i++)
            {
                if (Splitted_Exp[i] == "ИНИЦИАТОР-")
                {
                    if ((Splitted_Exp[i + 1] == ">") && ((Splitted_Exp[i + 2] == "вектор") || (Splitted_Exp[i + 2] == "скаляр")))
                    {
                        if (Splitted_Exp[i + 2] == "вектор")
                        {
                            bool exist_brackets = false;
                            int brackets = 0;
                            int j = i; //индекс конца выражения ИНИЦИАТОР->вектор(...(....))
                            while ((exist_brackets == false) || (brackets != 0))
                            {
                                j++;
                                if (Splitted_Exp[j] == "(") { brackets++; exist_brackets = true; }
                                if (Splitted_Exp[j] == ")") brackets--;
                            }
                            //нашли конец ИНИЦИАТОРа, теперь склеиваем
                            for (int k = i + 1; k <= j; k++)
                                Splitted_Exp[i] += Splitted_Exp[k];
                            Splitted_Exp.RemoveRange(i + 1, j - i);
                            continue;
                        }
                        else
                        {
                            Splitted_Exp[i] += Splitted_Exp[i + 1];
                            Splitted_Exp[i] += Splitted_Exp[i + 2];
                            Splitted_Exp.RemoveRange(i + 1, 2);
                            //i -= 2; 
                            continue;
                        }
                    }
                    //else return "\"Ошибка! неправильно указан ИНИЦИАТОР\"";
                    else return false;

                }
            }
            // нашли ИНИЦИАТОР, теперь получаем значение всех арифметических выражений
            for (int i = 0; i < Splitted_Exp.Count; i++)
            {
                if ((Splitted_Exp[i] != "(") && (Splitted_Exp[i] != ")") && (Splitted_Exp[i] != "\\/") && (Splitted_Exp[i] != "/\\") &&
                    (Splitted_Exp[i] != "=") && (Splitted_Exp[i] != "!=") && (Splitted_Exp[i] != "<=") && (Splitted_Exp[i] != ">=") && (Splitted_Exp[i] != "<") && (Splitted_Exp[i] != ">"))
                {
                    if (LPDP_Core.GlobalVar_Table.Exists(var => (var.Name == Splitted_Exp[i]) && (var.Type == LPDP_Object.ObjectType.Vector)))
                    {
                        bool exist_brackets = false;
                        int brackets = 0;
                        int j = i; //индекс конца выражения Имя_вектора(...(....))
                        while ((exist_brackets == false) || (brackets != 0))
                        {
                            j++;
                            if (Splitted_Exp[j] == "(") { brackets++; exist_brackets = true; }
                            if (Splitted_Exp[j] == ")") brackets--;
                        }
                        //нашли конец вектора, теперь склеиваем
                        for (int k = i + 1; k <= j; k++)
                            Splitted_Exp[i] += Splitted_Exp[k];
                        Splitted_Exp.RemoveRange(i + 1, j - i);
                    }

                    Splitted_Exp[i] = LPDP_Core.Arithmetic_Expression(Splitted_Exp[i], Unit);
                    if (Splitted_Exp[i].Contains("\"Ошибка!")) //return "\"Ошибка! Переменной не присвоено значение\"";
                        return false;
                }
            }
            // подставили значения, теперь запускаем цикл решения выражения

            while (Splitted_Exp.Count > 1)
            {
                bool op = false;

                //ищем конструкцию "(" "12345" ")"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if ((Splitted_Exp[i - 1] == "(") && ((Splitted_Exp[i] == Convert.ToString(true)) || (Splitted_Exp[i] == Convert.ToString(false))) && (Splitted_Exp[i + 1] == ")"))
                    {
                        Splitted_Exp.RemoveAt(i + 1);
                        Splitted_Exp.RemoveAt(i - 1);
                        i--; op = true; continue;
                    }
                    if ((Splitted_Exp[i - 1] == "(") && (IsValue(Splitted_Exp[i]) == true) && (Splitted_Exp[i + 1] == ")"))
                    {
                        Splitted_Exp.RemoveAt(i + 1);
                        Splitted_Exp.RemoveAt(i - 1);
                        i--; op = true; continue;
                    }
                }

                //ищем сравнения типа "123" "*" "123"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if (((Splitted_Exp[i] == "=") || (Splitted_Exp[i] == "!=") || (Splitted_Exp[i] == "<=") || (Splitted_Exp[i] == ">=") || (Splitted_Exp[i] == "<") || (Splitted_Exp[i] == ">"))
                        && (IsValue(Splitted_Exp[i - 1])) && (IsValue(Splitted_Exp[i + 1])))
                    {
                        bool b = false;
                        switch (Splitted_Exp[i])
                        {
                            case "=":
                                b = Splitted_Exp[i - 1] == Splitted_Exp[i + 1];
                                break;
                            case "!=":
                                b = Splitted_Exp[i - 1] != Splitted_Exp[i + 1];
                                break;
                            case "<=":
                                if (IsNumber(Splitted_Exp[i - 1]) && IsNumber(Splitted_Exp[i + 1]))
                                    b = Convert.ToDouble(Splitted_Exp[i - 1]) <= Convert.ToDouble(Splitted_Exp[i + 1]);
                                //else return "\"Ошибка! оператор '<=' не применим к строковым значениям\"";
                                else return false;
                                break;
                            case ">=":
                                if (IsNumber(Splitted_Exp[i - 1]) && IsNumber(Splitted_Exp[i + 1]))
                                    b = Convert.ToDouble(Splitted_Exp[i - 1]) >= Convert.ToDouble(Splitted_Exp[i + 1]);
                                //else return "\"Ошибка! оператор '>=' не применим к строковым значениям\"";
                                else return false;
                                break;
                            case "<":
                                if (IsNumber(Splitted_Exp[i - 1]) && IsNumber(Splitted_Exp[i + 1]))
                                    b = Convert.ToDouble(Splitted_Exp[i - 1]) < Convert.ToDouble(Splitted_Exp[i + 1]);
                                //else return "\"Ошибка! оператор '<' не применим к строковым значениям\"";
                                else return false;
                                break;
                            case ">":
                                if (IsNumber(Splitted_Exp[i - 1]) && IsNumber(Splitted_Exp[i + 1]))
                                    b = Convert.ToDouble(Splitted_Exp[i - 1]) > Convert.ToDouble(Splitted_Exp[i + 1]);
                                //else return "\"Ошибка! оператор '>' не применим к строковым значениям\"";
                                else return false;
                                break;
                            //default: return "\"Ошибка! неизвестный оператор\"";
                            default: return false;
                        }
                        Splitted_Exp[i] = Convert.ToString(b);
                        Splitted_Exp.RemoveAt(i + 1);
                        Splitted_Exp.RemoveAt(i - 1);
                        op = true;
                        i--;
                    }
                }
                if (op == true) continue;

                //ищем конструкцию "1" "/\" "1"  или  "1" "\/" "1"
                for (int i = 1; i < Splitted_Exp.Count - 1; i++)
                {
                    if (((Splitted_Exp[i - 1] == Convert.ToString(true)) || (Splitted_Exp[i - 1] == Convert.ToString(false))) &&
                         ((Splitted_Exp[i + 1] == Convert.ToString(true)) || (Splitted_Exp[i + 1] == Convert.ToString(false))) &&
                         ((Splitted_Exp[i] == "/\\") || (Splitted_Exp[i] == "\\/")))
                    {
                        bool b = true;
                        switch (Splitted_Exp[i])
                        {
                            case "/\\":
                                b = Convert.ToBoolean(Splitted_Exp[i - 1]) && Convert.ToBoolean(Splitted_Exp[i + 1]);
                                break;
                            case "\\/":
                                b = Convert.ToBoolean(Splitted_Exp[i - 1]) || Convert.ToBoolean(Splitted_Exp[i + 1]);
                                break;
                            //default: return "\"Ошибка! неизвестный оператор\"";
                            default: return false;
                        }
                        Splitted_Exp[i] = Convert.ToString(b);
                        Splitted_Exp.RemoveAt(i + 1);
                        Splitted_Exp.RemoveAt(i - 1);
                        op = true;
                        i--;
                    }
                }
                if (op == false) /*return "\"Ошибка! Неправильное выражение\""; ;*/return false;
            }// конец цикла
            try { return Convert.ToBoolean(Splitted_Exp[0]); }
            catch { return false; }

        }//конец ф-ции

        // Запись в ТАБЛИЦЫ
        public static void WriteTo_FTT(double time, int init, int mark, int id)
        {
            record_FTT rec = new record_FTT();
            rec.ActiveTime = time;
            rec.Initiator = init;
            rec.Subprogram_Index = mark;
            if (id == -1)
            {
                //if (init == -1)
                //{
                //    int ID = -1;
                //    while (((CT.FindIndex(record => record.ID == ID) == -1) && (FTT.FindIndex(record => record.ID == ID) == -1)) == false) ID--;
                //    rec.ID = ID;
                //}
                //else rec.ID = init;
                int ID = 0;
                while (((CT.FindIndex(record => record.ID == ID) == -1) && (FTT.FindIndex(record => record.ID == ID) == -1)) == false) ID++;
                rec.ID = ID;
            }////rec.ID = CT.Count;
            else { rec.ID = id; }
            FTT.Add(rec);
        }
        public static void WriteTo_CT(string cond, string fromUnit, int init, int mark, int id)
        {
            record_CT rec = new record_CT();
            rec.Condition = cond;
            rec.FromUnit = fromUnit;
            rec.Initiator = init;
            rec.Subprogram_Index = mark;
            if (id == -1)
            {
                //if (init == -1)
                //{
                //    int ID = -1;
                //    while (((CT.FindIndex(record => record.ID == ID) == -1) && (FTT.FindIndex(record => record.ID == ID) == -1)) == false) ID--;
                //    rec.ID = ID;
                //}
                //else rec.ID = init;
                int ID = 0;
                while (((CT.FindIndex(record => record.ID == ID) == -1) && (FTT.FindIndex(record => record.ID == ID) == -1)) == false) ID++;
                rec.ID = ID;
            }////rec.ID = CT.Count;
            else { rec.ID = id; }
            CT.Add(rec);
        }
        public static void WriteTo_MARKS(string name, string unit, int subp_ind)
        {
            record_MARK rec = new record_MARK();
            rec.Name = name;
            rec.Unit = unit;
            rec.Subprogram_Index = subp_ind;
            MARKS.Add(rec);
        }
        public static void WriteTo_Queue(int number, record_Queue.condition cond, double time, int init, int id)
        {
            record_Queue rec = new record_Queue();
            rec.Condition = cond;
            rec.Time = time;
            rec.Initiator = init;
            rec.ID = id;
            Queues[number].Queue.Add(rec);
        }

        //ф-ция для поиска слова, возвращающая индекс следующего символа после него
        public static int FindWord(string text, string word)
        {
            for (int i = 0; i < text.Length - word.Length + 1; i++)
            {
                if (text.Substring(i, word.Length) == word)
                    return i + word.Length;
            }
            return -1;
        }

        //построение модели из введенного текста
        public static bool Model_Is_Built = false; 

        public static string Build_POSP_Model(string code)
        {
            POSP_Model = new List<SubProgram>();
            LPDP_Code.Units = new List<LPDP_Code.Unit>();
            MARKS = new List<record_MARK>();
            Pairs = new List<Pair>();

            GlobalVar_Table = new List<LPDP_Object>();
            LocalArea_Table = new List<LocalArea>();
            CT = new List<record_CT>();
            FTT = new List<record_FTT>();
            TIME = 0;
            INITIATOR = -2;

            NEXT_SUBPROGRAMM = 0;
            Index_operation = 0;
            SubProg_IsBroken = false;

            // разбиваем весь текст на слова
            List<string> Words = new List<string>();
            code = code.Replace("ё", "е");

            string word = "";
            for (int i = 0; i < code.Length; i++)
            {
                if ((code[i] == ' ') || (code[i] == '\t') || (code[i] == '\n') || (code[i] == '\r') ||
                    Here_Contains_Any_From("" + code[i], Arithmetics) ||
                    Here_Contains_Any_From("" + code[i], Logic) ||
                    Here_Contains_Any_From("" + code[i], OtherCharacters) ||
                    (code[i] == '(') || (code[i] == ')'))
                {
                    if (word != "") Words.Add(word);
                    word = "" + code[i];
                    Words.Add(word);
                    word = "";
                }
                else
                    word += code[i];
            }
            if (word != "") Words.Add(word);

            //определение комментариев
            for (int i = 0; i < Words.Count - 1; i++)
            {
                if ((Words[i] == "/") && (Words[i + 1] == "/") ||
                    (Words[i] == "/") && (Words[i + 1] == "*") ||
                    (Words[i] == "*") && (Words[i + 1] == "/"))
                {
                    Words[i] += Words[i + 1];
                    Words.RemoveAt(i + 1);
                }
            }
            while (Words.Contains("/*"))
            {
                int start_comment = Words.IndexOf("/*");
                int end_comment = Words.IndexOf("*/", start_comment);
                if (end_comment == -1) end_comment = Words.Count - 1;

                for (int i = start_comment + 1; i <= end_comment; i++)
                    Words[start_comment] += Words[i];

                Words.RemoveRange(start_comment + 1, end_comment - start_comment);
            }
            if (Words.Contains("*/"))
            {
                int line = 1;
                for (int i = 0; i < Words.IndexOf("*/"); i++)
                    line += Words[i].Count(ch => ch == '\n');
                return Errors[0] + "Не указано начало комментария." + " (Строка " + line + ")";
            }
            while (Words.Contains("//"))
            {
                int start_comment = Words.IndexOf("//");
                int end_comment = Words.IndexOf("\n", start_comment) - 1;
                if (end_comment == -2) end_comment = Words.Count - 1;

                for (int i = start_comment + 1; i <= end_comment; i++)
                    Words[start_comment] += Words[i];
                Words.RemoveRange(start_comment + 1, end_comment - start_comment);
            }

            // склеиваем "все"
            while (Words.Contains("все"))
            {
                int index = Words.IndexOf("все");
                if (index > Words.Count - 2)
                {
                    int line = 1;
                    for (int i = 0; i < Words.IndexOf("все"); i++)
                        line += Words[i].Count(ch => ch == '\n');
                    return Errors[0] + "Ожидалось ключевое слово \"блок\", \"описание\" или \"алгорим\" после ключевого слова \"всё\"" + " (Строка " + line + ")";
                }
                if ((Words[index + 1] == " ") && ((Words[index + 2] == "блок") || (Words[index + 2] == "описание") || (Words[index + 2] == "алгоритм")))
                {
                    Words[index] = Words[index] + Words[index + 1] + Words[index + 2];
                    Words.RemoveRange(index + 1, 2);
                }
                else
                {
                    int line = 1;
                    for (int i = 0; i < Words.IndexOf("все"); i++)
                        line += Words[i].Count(ch => ch == '\n');
                    return Errors[0] + "Ожидалось ключевое слово \"блок\", \"описание\" или \"алгорим\" после ключевого слова \"всё\"" + " (Строка " + line + ")";
                }
            }

            // разбиваем всю кучу слов на блоки
            List<List<string>> Words_in_Units = new List<List<string>>();
            if (Words.Contains("блок") == false) return Errors[0] + "Не найдено начало блока.";
            while (Words.Contains("блок"))
            {
                List<string> Words_in_Unit = new List<string>();
                int start_unit = 0;
                int end_unit = Words.IndexOf("все блок", start_unit);
                if (end_unit == -1) return Errors[0] + "Не найден конец блока.";
                //{
                //    int line = 1;
                //    for (int u = 0; u < Words_in_Units.Count; u++)
                //        for (int i = 0; i < Words_in_Units[u].Count; i++)
                //            line += Words_in_Units[u][i].Count(ch => ch == '\n');
                //    for (int i = 0; i < Words.IndexOf("блок"); i++)
                //        line += Words[i].Count(ch => ch == '\n');
                //    return Errors[0] + "Не найден конец блока." + " (Строка " + line + ")";
                //}

                for (int i = start_unit; i <= end_unit; i++)
                    Words_in_Unit.Add(Words[i]);
                Words.RemoveRange(start_unit, end_unit - start_unit + 1);
                Words_in_Units.Add(Words_in_Unit);
            }

            if (Words.Contains("все блок")) return Errors[0] + "Не найдено начало блока.";
            for (int i = 0; i < Words.Count; i++)
            {
                if ((Words[i] != " ") && (Words[i] != "\t") && (Words[i] != "\n") && (Words[i] != "\r"))
                    if ((Words[i].Substring(0, 2) != "//") && (Words[i].Substring(0, 2) != "/*"))
                        return Errors[0] + "Код должен относиться к какому-либо блоку.";
                LPDP_Code.Comments_after_Units += Words[i];
            }

            LPDP_Code.Units = new List<LPDP_Code.Unit>();
            LPDP_Code.Comments_after_Units = "";
            //int Index_Of_Line = 1;

            // рассматриваем отдельно структуру каждого блока
            for (int u = 0; u < Words_in_Units.Count; u++)
            {
                LPDP_Code.Unit Unit = new LPDP_Code.Unit();
                Unit.Comments_before_Unit = "";
                Unit.Comments_for_Header = "";
                Unit.Comments_between_Description_and_Algorithm = "";
                Unit.Comments_for_End_of_Description = "";
                Unit.Comments_for_End_of_Algorithm = "";
                Unit.Comments_for_End_of_Unit = "";

                int start_unit = Words_in_Units[u].IndexOf("блок");
                //определение предблоковых комментариев
                for (int i = 0; i < start_unit; i++)
                {
                    if ((Words_in_Units[u][i] != " ") && (Words_in_Units[u][i] != "\t") && (Words_in_Units[u][i] != "\n") && (Words_in_Units[u][i] != "\r"))
                        if ((Words_in_Units[u][i].Contains("//") == false) && (Words_in_Units[u][i].Contains("/*") == false))
                            return Errors[0] + "Код должен относиться к какому-либо блоку.";
                        //if ((Words_in_Units[u][i].Contains("//")) || (Words_in_Units[u][i].Contains("/*")))
                        else
                            Unit.Comments_before_Unit += Words_in_Units[u][i];
                    else
                    {
                        Unit.Comments_before_Unit += Words_in_Units[u][i];
                    }
                    //else
                    //    if (Words_in_Units[u][i] == "\n") Index_Of_Line++; 
                }
                //Words_in_Units[u].RemoveRange(0, start_unit);

                //Index_Of_Line += Unit.Comments_before_Unit.Count(ch => ch == '\n');

                //определение заголовка
                int start_header = Words_in_Units[u].IndexOf("блок");
                int end_header = Words_in_Units[u].IndexOf("описание") - 1;
                if (end_header == -2) return Errors[0] + "Не найдено начало описания блока.";
                for (int i = start_header + 1; i <= end_header; i++)
                {
                    if ((Words_in_Units[u][i] == " ") || (Words_in_Units[u][i] == "\t") || (Words_in_Units[u][i] == "\n") || (Words_in_Units[u][i] == "\r"))
                    {
                        Unit.Comments_for_Header += Words_in_Units[u][i];
                        continue;
                    }
                    if ((Words_in_Units[u][i].Contains("//")) || (Words_in_Units[u][i].Contains("/*")))
                    {
                        Unit.Comments_for_Header += Words_in_Units[u][i];
                        continue;
                    }
                    if (IsCorrectName(Words_in_Units[u][i]) != "") return Errors[0] + "Использованы недопустимые символы или слова в заголовке блока";
                    if ((Words_in_Units[u][i] == "контроллер") || (Words_in_Units[u][i] == "агрегат") || (Words_in_Units[u][i] == "процессор"))
                        if (Unit.Type == null)
                        {
                            Unit.Type = Words_in_Units[u][i];
                            continue;
                        }
                        else return Errors[0] + "Тип блока уже был определён.";
                    //осталось только имя блока
                    for (int uu = 0; uu < u; uu++)
                        if (LPDP_Code.Units[uu].Name == Words_in_Units[u][i])
                            return Errors[0] + "Имя блока не должно повторятся!";
                    Unit.Name = Words_in_Units[u][i];
                }

                //Index_Of_Line += Unit.Comments_for_Header.Count(ch => ch == '\n');

                // определение структуры описания
                Unit.Description = new List<LPDP_Code.Line>();

                int start_description = Words_in_Units[u].IndexOf("описание") + 1;
                int end_description = Words_in_Units[u].IndexOf("все описание") - 1;
                if (end_description == -2) return Errors[0] + "Не найден конец описания блока.";

                LPDP_Code.Line Line = new LPDP_Code.Line();
                Line.Words = new List<string>();
                //Line.Comments = "";
                for (int i = start_description; i <= end_description; i++)
                {
                    if (Words_in_Units[u][i] == ";")
                    {
                        Line.Words.Add(Words_in_Units[u][i]);
                        Line = LPDP_Code.Correct_Line(Line);
                        //Index_Of_Line += Line.Comments.Count(ch => ch == '\n');
                        //Index_Of_Line++;
                        //Line.Index = Index_Of_Line;
                        Unit.Description.Add(Line);
                        Line = new LPDP_Code.Line();
                        Line.Words = new List<string>();
                        //Line.Comments = "";
                    }
                    else Line.Words.Add(Words_in_Units[u][i]);
                }
                //комментарий перед "все описание"
                for (int i = 0; i < Line.Words.Count; i++)
                    Unit.Comments_for_End_of_Description += Line.Words[i];


                // комментарий между описанием и алгоритмом
                int start_algorithm = Words_in_Units[u].IndexOf("алгоритм") + 1;
                for (int i = end_description + 2; i < start_algorithm - 1; i++)
                {
                    if ((Words_in_Units[u][i] != " ") && (Words_in_Units[u][i] != "\t") && (Words_in_Units[u][i] != "\n") && (Words_in_Units[u][i] != "\r"))
                        if ((Words_in_Units[u][i].Contains("//") == false) && (Words_in_Units[u][i].Contains("/*") == false))
                            return Errors[0] + "Код должен относиться к описанию или алгоритму.";
                        else
                            Unit.Comments_between_Description_and_Algorithm += Words_in_Units[u][i];
                    else
                        Unit.Comments_between_Description_and_Algorithm += Words_in_Units[u][i];

                    //if ((Words_in_Units[u][i].Contains("//")) || (Words_in_Units[u][i].Contains("/*")))
                    //{
                    //    Unit.Comments_between_Description_and_Algorithm += Words_in_Units[u][i];
                    //}

                    //else
                    //    if (Words_in_Units[u][i] == "\n") Index_Of_Line++;
                }

                //Index_Of_Line += Unit.Comments_between_Description_and_Algorithm.Count(ch => ch == '\n');

                // определение структуры алгоритма
                Unit.Algorithm = new List<LPDP_Code.Line>();

                int end_algorithm = Words_in_Units[u].IndexOf("все алгоритм") - 1;
                if (end_algorithm == -2) return Errors[0] + "Не найден конец алгоритма блока.";

                Line = new LPDP_Code.Line();
                Line.Words = new List<string>();
                //Line.Comments = "";
                for (int i = start_algorithm; i <= end_algorithm; i++)
                {
                    if (Words_in_Units[u][i] == ";")
                    {
                        Line.Words.Add(Words_in_Units[u][i]);
                        Line = LPDP_Code.Correct_Line(Line);
                        //Index_Of_Line += Line.Comments.Count(ch => ch == '\n');
                        //Index_Of_Line++;
                        //Line.Index = Index_Of_Line;
                        Unit.Algorithm.Add(Line);
                        Line = new LPDP_Code.Line();
                        Line.Words = new List<string>();
                        //Line.Comments = "";
                    }
                    else Line.Words.Add(Words_in_Units[u][i]);
                }
                //комментарий перед "все алгоритм"
                for (int i = 0; i < Line.Words.Count; i++)
                    Unit.Comments_for_End_of_Algorithm += Line.Words[i];

                //комментарий перед "все блок"
                for (int i = Words_in_Units[u].IndexOf("все алгоритм") + 1; i < Words_in_Units[u].IndexOf("все блок"); i++)
                    Unit.Comments_for_End_of_Unit += Words_in_Units[u][i];


                LPDP_Code.Units.Add(Unit);
            }//конец разбора структуры блока


            for (int u = 0; u < Words_in_Units.Count; u++)
            {
                LPDP_Code.Unit Unit = LPDP_Code.Units[u];
                string UnitName = Unit.Name;
                string UnitType = Unit.Type;
                // запись переменных из описания
                for (int d = 0; d < Unit.Description.Count; d++)
                {
                    //if (Unit.Description[d].Words[Unit.Description[d].Words.Count - 1] == ";") Unit.Description[d].Words.RemoveAt(Unit.Description[d].Words.Count - 1);
                    //else return "Строка описания должна заканчиваться символом \";\".";

                    List<string> before_dash = new List<string>();
                    List<string> after_dash = new List<string>();

                    for (int i = 0; i < Unit.Description[d].Words.IndexOf("-"); i++)
                        before_dash.Add(Unit.Description[d].Words[i]);

                    for (int i = Unit.Description[d].Words.IndexOf("-") + 1; i < Unit.Description[d].Words.Count; i++)
                        after_dash.Add(Unit.Description[d].Words[i]);

                    // оперделение типа будущих объектов
                    string Type;
                    string FromUnit = "";
                    if (after_dash.Count > 0) Type = after_dash[0];
                    else return Errors[0] + "Не указан тип объекта" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                    if ((Type != "скаляр") && (Type != "ссылка") && (Type != "вектор")) return Errors[0] + "Неизвестный тип объекта" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                    if (Type == "вектор")
                    {
                        for (int i = 1; i <= after_dash.LastIndexOf(")"); i++)
                            after_dash[0] += after_dash[i];
                        after_dash.RemoveRange(1, after_dash.LastIndexOf(")"));
                        Type = after_dash[0];
                    }
                    if (after_dash.Count > 2)
                        if (after_dash[1] != "блока") return Errors[0] + "Ожидалось ключевое слово \"блока\" или конец строки" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                        else
                            if (after_dash.Count < 3) return Errors[0] + "Не указано имя блока" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                            else
                                if (LPDP_Code.Units.Exists(unit => unit.Name == after_dash[2]))
                                    FromUnit = after_dash[2];
                                else return Errors[0] + "Нет блока с таким именем" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                    if (after_dash.Count > 4) return Errors[0] + "Ожидался конец строки" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";

                    // определение имен и значений
                    if (before_dash.Count == 0) return Errors[0] + "Не задано имя объекта" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                    for (int i = 0; i < before_dash.Count; i++)
                    {
                        string ObjName = before_dash[i];
                        string Value = "";
                        if (IsCorrectName(ObjName) != "") return Errors[0] + IsCorrectName(ObjName) + " объекта" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                        if (i + 1 != before_dash.Count)
                        {
                            i++;
                            if (before_dash[i] == ",") { ;}
                            else
                            {
                                if (before_dash[i] == "=")
                                {
                                    if (FromUnit != "") return Errors[0] + "Нельзя задать начальное значение для объекта из другого блока" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                                    int end_of_single = before_dash.IndexOf(",", i);
                                    if (end_of_single == -1) end_of_single = before_dash.Count;
                                    for (int ii = i + 1; ii < end_of_single; ii++)
                                        Value += before_dash[ii];
                                    if (Value == "") return Errors[0] + "Не задано значение объекта после символа \"=\"" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                                    i = end_of_single;
                                }
                                else return Errors[0] + "Имя объекта задано не корректно" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                            }
                        }

                        if (GlobalVar_Table.Exists(obj => (obj.Name == ObjName) && (obj.Unit == UnitName)))
                            return Errors[0] + "Объект с заданным именем уже есть в этом блоке" + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания).";
                        Create_LPDP_Object(ObjName, UnitName, Type);

                        if (Value != "")
                        {
                            Value = Arithmetic_Expression(Value, UnitName);
                            if (Value.Contains("Ошибка!")) return Errors[0] + Value + " (блок " + Unit.Name + ", " + (int)(d + 1) + " сторка описания)."; ;
                            SetValue(ObjName, UnitName, Value);
                        }
                        if (FromUnit != "") WriteTo_Pairs(ObjName, FromUnit, UnitName);
                    }
                }

                // разбор строк алгоритма
                bool first_subp = true; // для определения стартовых подпрограммм
                SubProgram SubP = new SubProgram();
                SubP.Operations = new List<operation>();

                for (int alg = 0; alg < Unit.Algorithm.Count; alg++)
                {
                    //если есть метка
                    if ((Unit.Algorithm[alg].Mark != "") && (Unit.Algorithm[alg].Mark.Contains('$') == false))
                    {
                        string NameMark = Unit.Algorithm[alg].Mark;
                        if ((IsCorrectName(NameMark) != "") &&
                            (IsCorrectName(NameMark) != "Использованы недопустимые символы в имени"))
                            return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        if (IsCorrectName(NameMark) == "Использованы недопустимые символы в имени")
                            if (IsCorrectName(NameMark.Remove(0, 1)) != "")
                                return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        //if (NameMark.Contains("$")) return "\"$\" - зарезервированное имя метки!";
                        if (MARKS.Any<record_MARK>(rec => ((rec.Name == NameMark) && (rec.Unit == UnitName)))) return Errors[0] + "Такая метка уже есть" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        // для меток, разрывающих подпрограмму на две
                        if (SubP.Operations.Count > 0)
                        {
                            operation NewOp = new operation();
                            NewOp.Parameters = new List<string>();
                            NewOp.NameOperation = "направить";
                            NewOp.Parameters.Add(NameMark); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(UnitName); //метка, из блока, в блок
                            NewOp.Unit_index = -1; NewOp.Line_index = -1;

                            SubP.Operations.Add(NewOp);
                            POSP_Model.Add(SubP);
                            if ((first_subp) && ((UnitType != "процессор")))
                            {
                                first_subp = false;
                                WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                            }
                            SubP = new SubProgram();
                            SubP.Operations = new List<operation>();
                        }
                        WriteTo_MARKS(NameMark, UnitName, POSP_Model.Count);
                    }

                    //если нет слов
                    if (Unit.Algorithm[alg].Words.Count == 0)
                        return Errors[0] + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";/*+ "Неизвестная ошибка построения модели."*/

                    //если есть только ";"
                    if (Unit.Algorithm[alg].Words[0] == ";")
                        return Errors[0] + "Строка не содержит операторов" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                    // если есть "создать объект"
                    if ((Unit.Algorithm[alg].Words.IndexOf("создать") == 0) &&
                        (Unit.Algorithm[alg].Words.IndexOf("объект") == 1))
                    {
                        if (Unit.Algorithm[alg].Words.IndexOf("типа") != 3) return Errors[0] + "Не указан тип создаваемого объекта" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string ObjName = Unit.Algorithm[alg].Words[2];
                        if (IsCorrectName(ObjName) != "") return Errors[0] + IsCorrectName(ObjName) + " создаваемого объекта" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string ObjType = Unit.Algorithm[alg].Words[4];
                        if ((ObjType == "") || ((ObjType != "скаляр") && (ObjType.Contains("вектор") == false) && (ObjType != "ссылка"))) return Errors[0] + "Неверный тип создаваемого объекта" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        if (ObjType == "вектор")
                        {
                            for (int i = 5; i < Unit.Algorithm[alg].Words.IndexOf(";"); i++)
                                ObjType += Unit.Algorithm[alg].Words[i];
                        }
                        operation NewOp = new operation();
                        NewOp.NameOperation = "создать";
                        NewOp.Parameters = new List<string>();
                        NewOp.Parameters.Add(ObjName); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(ObjType);//имя, блок, тип
                        NewOp.Unit_index = u; NewOp.Line_index = alg;
                        SubP.Operations.Add(NewOp);
                        continue;
                    }

                    // если есть ":="
                    if (Unit.Algorithm[alg].Words.Contains(":="))
                    {
                        string ObjName = "";
                        for (int i = 0; i < Unit.Algorithm[alg].Words.IndexOf(":="); i++)
                            ObjName += Unit.Algorithm[alg].Words[i];
                        if (IsCorrectName(ObjName) != "") return Errors[0] + IsCorrectName(ObjName) + " объекта" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        if ((Unit.Algorithm[alg].Words.IndexOf("ссылка") == Unit.Algorithm[alg].Words.IndexOf(":=") + 1) &&
                            (Unit.Algorithm[alg].Words.IndexOf("на") == Unit.Algorithm[alg].Words.IndexOf(":=") + 2))
                        {
                            string LinkToName = "";
                            for (int i = Unit.Algorithm[alg].Words.IndexOf("на") + 1; i < Unit.Algorithm[alg].Words.Count - 1; i++)
                                LinkToName += Unit.Algorithm[alg].Words[i];
                            if (IsCorrectName(LinkToName) != "") return Errors[0] + IsCorrectName(LinkToName) + " объекта" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                            operation NewOp = new operation();
                            NewOp.NameOperation = ":=ссылка";
                            NewOp.Parameters = new List<string>();
                            NewOp.Parameters.Add(ObjName); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(LinkToName);//имя, блок, имя на которое ссылаются
                            NewOp.Unit_index = u; NewOp.Line_index = alg;
                            SubP.Operations.Add(NewOp);
                            continue;
                        }
                        else
                        {
                            string exp = "";
                            for (int i = Unit.Algorithm[alg].Words.IndexOf(":=") + 1; i < Unit.Algorithm[alg].Words.Count - 1; i++)
                                exp += Unit.Algorithm[alg].Words[i];
                            operation NewOp = new operation();
                            NewOp.NameOperation = ":=";
                            NewOp.Parameters = new List<string>();
                            NewOp.Parameters.Add(ObjName); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(exp);//имя, блок, выражение
                            NewOp.Unit_index = u; NewOp.Line_index = alg;
                            SubP.Operations.Add(NewOp);
                            continue;
                        }
                    }

                    // если есть "активизировать"
                    if ((Unit.Algorithm[alg].Words.IndexOf("активизировать") == 0) &&
                        (Unit.Algorithm[alg].Words.IndexOf("инициатор") == 1) &&
                        (Unit.Algorithm[alg].Words.IndexOf("из") == 2))
                    {
                        if (((Unit.Algorithm[alg].Words.IndexOf("направить") == 4) &&
                            (Unit.Algorithm[alg].Words.IndexOf("на") == 5) &&
                            (Unit.Algorithm[alg].Words.IndexOf("метку") == 6)) == false) return Errors[0] + "Не указано направление активизации" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        string NameLink = Unit.Algorithm[alg].Words[3];
                        if (IsCorrectName(NameLink) != "") return Errors[0] + IsCorrectName(NameLink) + " ссылки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        if ((Unit.Algorithm[alg].Words.IndexOf("блока") != 8) || (Unit.Algorithm[alg].Words.Count < 11)) return Errors[0] + "Не указан блок метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string NameMark = Unit.Algorithm[alg].Words[7];
                        if (IsCorrectName(NameMark) != "") return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string ToUnit = Unit.Algorithm[alg].Words[9];
                        if (IsCorrectName(ToUnit) != "") return Errors[0] + IsCorrectName(ToUnit) + " блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        // если нет метки с таким именем ни в одном из блоков
                        if (LPDP_Code.Units.Find(unit => unit.Name == ToUnit).Algorithm.Exists(line => line.Mark == NameMark) == false)
                            return Errors[0] + "В указанном блоке нет такой метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        operation NewOp = new operation();
                        NewOp.NameOperation = "активизировать";
                        NewOp.Parameters = new List<string>();
                        NewOp.Parameters.Add(NameLink); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(NameMark); NewOp.Parameters.Add(ToUnit);//из имени ссылки, блок, на метку, в блок
                        NewOp.Unit_index = u; NewOp.Line_index = alg;
                        SubP.Operations.Add(NewOp);
                        continue;
                    }

                    // если есть "пассивизировать"
                    if ((Unit.Algorithm[alg].Words.IndexOf("пассивизировать") == 0) &&
                        (Unit.Algorithm[alg].Words.IndexOf("инициатор") == 1) &&
                        (Unit.Algorithm[alg].Words.IndexOf("в") == 2))
                    {
                        if (Unit.Algorithm[alg].Words.Count < 5) return Errors[0] + "Не указано направление пассивизации" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string ToNameLink = Unit.Algorithm[alg].Words[3];
                        if (IsCorrectName(ToNameLink) != "") return Errors[0] + IsCorrectName(ToNameLink) + " ссылки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        operation NewOp = new operation();
                        NewOp.NameOperation = "пассивизировать";
                        NewOp.Parameters = new List<string>();
                        NewOp.Parameters.Add(ToNameLink); NewOp.Parameters.Add(UnitName);//в имя ссылки, блок
                        NewOp.Unit_index = u; NewOp.Line_index = alg;
                        SubP.Operations.Add(NewOp);
                        continue;
                    }

                    if ((Unit.Algorithm[alg].Words.Contains("иначе")) && (Unit.Algorithm[alg].Words.Contains("если") == false)) return Errors[0] + "Найдено \"иначе\", но не найдено \"если\"" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                    // если есть "если"
                    if (Unit.Algorithm[alg].Words.Contains("если"))
                    {
                        bool first = true;
                        if (Unit.Algorithm[alg].Words.Contains("иначе") == false) return Errors[0] + "Не указано направление по умолчанию (\"иначе\")" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        List<string> full_line = new List<string>();
                        for (int i = 0; i < Unit.Algorithm[alg].Words.Count; i++)
                            full_line.Add(Unit.Algorithm[alg].Words[i]);

                        int first_if = Unit.Algorithm[alg].Words.IndexOf("если");
                        while ((full_line.Contains("если")) || (full_line.Contains("иначе")))
                        {
                            List<string> subline = new List<string>();
                            int Deleted_len = 0;
                            if (full_line.IndexOf("если") < full_line.LastIndexOf("если"))
                            {
                                //int first_if = Unit.Algorithm[alg].Words.IndexOf("если", last_if);
                                int second_if = Unit.Algorithm[alg].Words.IndexOf("если", first_if + 1);
                                for (int i = first_if; i < second_if; i++)
                                    subline.Add(Unit.Algorithm[alg].Words[i]);
                                Deleted_len = second_if - first_if;
                                //full_line.RemoveRange(0, second_if - first_if);
                                first_if = second_if;
                            }
                            if ((full_line.IndexOf("если") == full_line.LastIndexOf("если")) &&
                                (full_line.IndexOf("если") != -1))
                            {
                                for (int i = first_if; i < Unit.Algorithm[alg].Words.IndexOf("иначе"); i++)
                                    subline.Add(Unit.Algorithm[alg].Words[i]);
                                Deleted_len = Unit.Algorithm[alg].Words.IndexOf("иначе") - first_if;
                                //full_line.RemoveRange(0, Unit.Algorithm[alg].Words.IndexOf("иначе") - first_if);
                            }


                            if (full_line.IndexOf("если") == -1)
                            {
                                for (int i = full_line.IndexOf("иначе"); i < full_line.Count - 1; i++)
                                    subline.Add(full_line[i]);
                                Deleted_len = full_line.Count;
                                //full_line.RemoveRange(0,full_line.Count);
                            }
                            full_line.RemoveRange(0, Deleted_len);

                            int start_then = subline.IndexOf("то");
                            if (start_then == -1) start_then = subline.IndexOf("иначе");
                            if ((start_then == -1) ||
                                (subline.IndexOf("направить") != start_then + 1) ||
                                (subline.IndexOf("инициатор") != start_then + 2) ||
                                (subline.IndexOf("на") != start_then + 3) ||
                                (subline.IndexOf("метку") != start_then + 4))
                                return Errors[0] + "Не указано направление выполнения условия" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                            string Condition = "";
                            if (subline.Contains("если"))
                                for (int i = subline.IndexOf("если") + 1; i < subline.IndexOf("то"); i++)
                                    Condition += subline[i];
                            else Condition = "True";

                            if (subline.IndexOf("метку") == subline.Count - 1) return Errors[0] + "Не указана метка направления выполнения условия" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                            string NameMark = subline[subline.IndexOf("метку") + 1];
                            if (IsCorrectName(NameMark) != "") return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                            string ToUnit = "";
                            if (subline.Contains("блока"))
                            {
                                if (subline.IndexOf("блока") == subline.Count - 1) return Errors[0] + "Ожидалось указание блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                                ToUnit = subline[subline.IndexOf("блока") + 1];
                                if (IsCorrectName(ToUnit) != "") return Errors[0] + IsCorrectName(ToUnit) + " блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                            }
                            else
                                ToUnit = UnitName;

                            string ID = "";
                            if (first) ID = "-1";
                            else ID = "last";
                            operation NewOp = new operation();
                            NewOp.NameOperation = "если";
                            NewOp.Parameters = new List<string>();
                            NewOp.Parameters.Add(Condition); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(NameMark);
                            NewOp.Parameters.Add(ToUnit); NewOp.Parameters.Add(ID);//условие, из блока ,метка, в блок, id(-1 для нового/last для того как последний)
                            NewOp.Unit_index = u; NewOp.Line_index = alg;
                            SubP.Operations.Add(NewOp);
                            first = false;
                            continue;
                        }
                        POSP_Model.Add(SubP);
                        if ((first_subp) && (UnitType != "процессор"))
                        {
                            first_subp = false;
                            WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                        }
                        SubP = new SubProgram();
                        SubP.Operations = new List<operation>();
                        continue;
                    }

                    // если есть "ждать"
                    if (Unit.Algorithm[alg].Words.Contains("ждать"))
                    {
                        //string Condition = "";
                        string ToUnit = UnitName;
                        string NameMark = "";
                        string ID = "-1";

                        // одно ждать
                        if (Unit.Algorithm[alg].Words.IndexOf("ждать") == Unit.Algorithm[alg].Words.LastIndexOf("ждать"))
                        {
                            string Condition = "";
                            for (int i = 1; i < Unit.Algorithm[alg].Words.Count - 1; i++)
                                Condition += Unit.Algorithm[alg].Words[i];
                            //создаем метку
                            int j = 1;
                            NameMark = "$" + Convert.ToString(u + 1) + "_" + Convert.ToString(j);
                            while (MARKS.Any<record_MARK>(rec => ((rec.Name == NameMark) && (rec.Unit == ToUnit))))
                            {
                                j++;
                                NameMark = "$" + Convert.ToString(u + 1) + "_" + Convert.ToString(j);
                            }

                            bool newmark = false;
                            if (alg < Unit.Algorithm.Count - 1)
                            {
                                if (Unit.Algorithm[alg + 1].Mark == "")
                                {
                                    LPDP_Code.Line NewMark = Unit.Algorithm[alg + 1];
                                    NewMark.Mark = NameMark;
                                    Unit.Algorithm[alg + 1] = NewMark;
                                    newmark = true;
                                }
                                else
                                    NameMark = Unit.Algorithm[alg + 1].Mark;
                            }
                            else
                                return Errors[0] + "Неправильное завершение трека" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма)."; ;


                            operation NewOp = new operation();
                            NewOp.NameOperation = "ждать";
                            NewOp.Parameters = new List<string>();
                            NewOp.Parameters.Add(Condition); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(NameMark);
                            NewOp.Parameters.Add(ToUnit); NewOp.Parameters.Add(ID);//условие,  из блока, метка, в блок, id(-1 для нового/last для того как последний)
                            NewOp.Unit_index = u; NewOp.Line_index = alg;
                            SubP.Operations.Add(NewOp);
                            POSP_Model.Add(SubP);
                            if ((first_subp) && ((UnitType != "процессор") /*|| (first_mark)*/))
                            {
                                first_subp = false;
                                WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                            }
                            SubP = new SubProgram();
                            SubP.Operations = new List<operation>();

                            if (newmark)
                                WriteTo_MARKS(NameMark, ToUnit, POSP_Model.Count);
                            continue;
                        }
                        else
                        {
                            // много ждать
                            bool first = true;

                            List<string> full_line = new List<string>();
                            for (int i = 0; i < Unit.Algorithm[alg].Words.Count; i++)
                                full_line.Add(Unit.Algorithm[alg].Words[i]);

                            int first_if = Unit.Algorithm[alg].Words.IndexOf("ждать");
                            while (full_line.Contains("ждать"))
                            {
                                List<string> subline = new List<string>();

                                if (full_line.IndexOf("ждать") < full_line.LastIndexOf("ждать"))
                                {
                                    //int first_if = Unit.Algorithm[alg].Words.IndexOf("если", last_if);
                                    int second_if = Unit.Algorithm[alg].Words.IndexOf("ждать", first_if + 1);
                                    for (int i = first_if; i < second_if; i++)
                                        subline.Add(Unit.Algorithm[alg].Words[i]);
                                    full_line.RemoveRange(0, second_if - first_if);
                                    first_if = second_if;
                                }
                                else
                                {
                                    for (int i = full_line.IndexOf("ждать"); i < full_line.Count - 1; i++)
                                        subline.Add(full_line[i]);
                                    full_line.RemoveRange(0, full_line.Count);
                                }


                                int start_then = subline.IndexOf("то");
                                if ((subline.IndexOf("то") == -1) ||
                                    (subline.IndexOf("направить") != start_then + 1) ||
                                    (subline.IndexOf("инициатор") != start_then + 2) ||
                                    (subline.IndexOf("на") != start_then + 3) ||
                                    (subline.IndexOf("метку") != start_then + 4))
                                    return Errors[0] + "Не указано направление выполнения условия" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";


                                string Condition = "";
                                for (int i = subline.IndexOf("ждать") + 1; i < subline.IndexOf("то"); i++)
                                    Condition += subline[i];



                                if (subline.IndexOf("метку") == subline.Count - 1) return Errors[0] + "Не указана метка направления выполнения условия" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                                NameMark = subline[subline.IndexOf("метку") + 1];
                                if (IsCorrectName(NameMark) != "") return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                                ToUnit = "";
                                if (subline.Contains("блока"))
                                {
                                    if (subline.IndexOf("блока") == subline.Count - 1) return Errors[0] + "Ожидалось указание блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                                    ToUnit = subline[subline.IndexOf("блока") + 1];
                                    if (IsCorrectName(ToUnit) != "") return Errors[0] + IsCorrectName(ToUnit) + " блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                                }
                                else
                                    ToUnit = UnitName;

                                if (first) ID = "-1";
                                else ID = "last";
                                operation NewOp = new operation();
                                NewOp.NameOperation = "ждать";
                                NewOp.Parameters = new List<string>();
                                NewOp.Parameters.Add(Condition); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(NameMark);
                                NewOp.Parameters.Add(ToUnit); NewOp.Parameters.Add(ID);//условие, из блока, метка, в блок, id(-1 для нового/last для того как последний)
                                NewOp.Unit_index = u; NewOp.Line_index = alg;
                                SubP.Operations.Add(NewOp);
                                first = false;
                                continue;
                            }
                            POSP_Model.Add(SubP);
                            if ((first_subp) && ((UnitType != "процессор") /*|| (first_mark)*/))
                            {
                                first_subp = false;
                                WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                            }
                            SubP = new SubProgram();
                            SubP.Operations = new List<operation>();
                            continue;
                        }
                    }

                    // безусловный переход
                    if ((Unit.Algorithm[alg].Words.IndexOf("направить") == 0) &&
                        (Unit.Algorithm[alg].Words.IndexOf("инициатор") == 1) &&
                        (Unit.Algorithm[alg].Words.IndexOf("на") == 2) &&
                        (Unit.Algorithm[alg].Words.IndexOf("метку") == 3))
                    {
                        string ToUnit = "";
                        string NameMark = "";

                        if (Unit.Algorithm[alg].Words.Count < 5) return Errors[0] + "Не указано направление безусловного перехода" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        NameMark = Unit.Algorithm[alg].Words[4];
                        if (IsCorrectName(NameMark) != "") return Errors[0] + IsCorrectName(NameMark) + " метки" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                        if (Unit.Algorithm[alg].Words.Contains("блока"))
                        {
                            if (Unit.Algorithm[alg].Words.Count < 7) return Errors[0] + "Не указан блок направления безусловного перехода" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                            ToUnit = Unit.Algorithm[alg].Words[6];
                            if (IsCorrectName(ToUnit) != "") return Errors[0] + IsCorrectName(ToUnit) + " блока" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        }
                        else
                            ToUnit = UnitName;

                        operation NewOp = new operation();
                        NewOp.NameOperation = "направить";
                        NewOp.Parameters = new List<string>();
                        NewOp.Parameters.Add(NameMark); NewOp.Parameters.Add(UnitName); NewOp.Parameters.Add(ToUnit); //метка, из блока, в блок
                        NewOp.Unit_index = u; NewOp.Line_index = alg;
                        SubP.Operations.Add(NewOp);
                        POSP_Model.Add(SubP);
                        if ((first_subp) && ((UnitType != "процессор") /*|| (first_mark)*/))
                        {
                            first_subp = false;
                            WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                        }
                        SubP = new SubProgram();
                        SubP.Operations = new List<operation>();
                        continue;
                    }

                    // уничтожение
                    if (Unit.Algorithm[alg].Words.IndexOf("уничтожить") == 0)
                    {
                        if (Unit.Algorithm[alg].Words.Count < 3) return Errors[0] + "Не указан объект уничтожения" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        string Name = Unit.Algorithm[alg].Words[1];

                        if ((IsCorrectName(Name) != "") && (IsCorrectName(Name) != "Использован системный идентификатор в качестве имени"))
                            return Errors[0] + IsCorrectName(Name) + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";
                        operation NewOp = new operation();
                        NewOp.NameOperation = "уничтожить";
                        NewOp.Parameters = new List<string>();
                        NewOp.Parameters.Add(Name); NewOp.Parameters.Add(UnitName); //имя, в блоке
                        NewOp.Unit_index = u; NewOp.Line_index = alg;
                        SubP.Operations.Add(NewOp);
                        POSP_Model.Add(SubP);
                        if ((first_subp) && (UnitType != "процессор"))
                        {
                            first_subp = false;
                            WriteTo_CT("True", UnitName, -1, POSP_Model.Count - 1, -1);
                        }
                        SubP = new SubProgram();
                        SubP.Operations = new List<operation>();
                        continue;
                    }
                    return Errors[0] + "Неизвестный оператор" + " (блок " + Unit.Name + ", " + (int)(alg + 1) + " сторка алгоритма).";

                }// конец разбора строк алгоритма
                LPDP_Code.Units[u] = Unit;
            }
            //POSP_Model.Add(SubP);
            LPDP_Core.Model_Is_Built = true;
            return "Ошибок нет. Модель построена успешно.";
        }


        //выполнение одной подпрограммы
        static int lastID = 0; // для сложных условий в "ждать"
        static string Fulfill_SubProgramm(Mode mode)
        {
            for (int i = Index_operation; i < POSP_Model[NEXT_SUBPROGRAMM].Operations.Count; i++)
            {
                switch (POSP_Model[NEXT_SUBPROGRAMM].Operations[i].NameOperation)
                {
                    case "направить":
                        string NameMark = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        string ToUnit = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        int IndMark = MARKS.FindIndex(rec => ((rec.Name == NameMark) && (rec.Unit == ToUnit)));
                        if (IndMark == -1) return "Нет метки с таким именем!";
                        int NextSubp = MARKS[IndMark].Subprogram_Index;
                        WriteTo_CT("True", NameUnit, INITIATOR, NextSubp, -1);
                        break;

                    case "создать":
                        string NameObj2 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit2 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        string NameType2 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        Create_LPDP_Object(NameObj2, NameUnit2, NameType2);
                        break;

                    case ":=ссылка":
                        string NameLink3 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit3 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        if (GlobalVar_Table.FindIndex(obj => ((obj.Name == NameLink3) && (obj.Unit == NameUnit3) && (obj.Type == LPDP_Object.ObjectType.Link))) == -1)
                        {
                            return "Нет ссылки с таким именем!";
                        }
                        string LinkTo3 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        string LinkValue = Convert.ToString(LinkTo(LinkTo3, NameUnit3));
                        if (LinkValue.Contains("Ошибка!")) return LinkValue;
                        SetValue(NameLink3, NameUnit3, LinkValue);
                        break;

                    case ":=":
                        string NameObj4 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit4 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        if ((GlobalVar_Table.FindIndex(obj => ((obj.Name == NameObj4) && (obj.Unit == NameUnit4))) == -1) && (NameObj4.Contains("ИНИЦИАТОР->")) == false)
                        {
                            if (NameObj4.IndexOf('(') == -1) return "Нет объекта с таким именем!";
                            string VectorName = NameObj4.Substring(0, NameObj4.IndexOf('('));
                            if (GlobalVar_Table.FindIndex(obj => ((obj.Name == VectorName) && (obj.Unit == NameUnit4))) == -1)
                                return "Нет объекта с таким именем!";
                        }
                        string Exp = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        string Value = Arithmetic_Expression(Exp, NameUnit4);
                        if (Value.Contains("Ошибка!")) return Value;
                        SetValue(NameObj4, NameUnit4, Value);
                        break;

                    case "активизировать":
                        string FromNameLink5 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit5 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        int IndLink5 = GlobalVar_Table.FindIndex(obj => ((obj.Name == FromNameLink5) && (obj.Unit == NameUnit5) && (obj.Type == LPDP_Object.ObjectType.Link)));
                        if (IndLink5 == -1) return "Нет ссылки с таким именем!";
                        int initiat5 = Convert.ToInt32(GlobalVar_Table[IndLink5].GetValue());

                        string NameMark5 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        string ToUnit5 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[3];
                        int IndMark5 = MARKS.FindIndex(rec => ((rec.Name == NameMark5) && (rec.Unit == ToUnit5)));
                        if (IndMark5 == -1) return "Нет метки с таким именем!";
                        int NextSubp5 = MARKS[IndMark5].Subprogram_Index;

                        WriteTo_CT("True", NameUnit5, initiat5, NextSubp5, -1);
                        break;

                    case "пассивизировать":
                        string ToNameLink6 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string NameUnit6 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        int IndLink6 = GlobalVar_Table.FindIndex(obj => ((obj.Name == ToNameLink6) && (obj.Unit == NameUnit6) && (obj.Type == LPDP_Object.ObjectType.Link)));
                        if (IndLink6 == -1) return "Нет ссылки с таким именем!";
                        SetValue(ToNameLink6, NameUnit6, Convert.ToString(INITIATOR));
                        break;

                    case "если":
                        string Condition7 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string FromUnit7 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        string ToNameMark7 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        string ToNameUnit7 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[3];
                        string ID7 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[4]; //если -1 то с новым ID

                        int IndMark7 = MARKS.FindIndex(rec => ((rec.Name == ToNameMark7) && (rec.Unit == ToNameUnit7)));
                        if (IndMark7 == -1) return "Нет метки с таким именем!";
                        int NextSubp7 = MARKS[IndMark7].Subprogram_Index;

                        if (ID7 == Convert.ToString(-1))
                        {
                            WriteTo_CT(Condition7, FromUnit7, INITIATOR, NextSubp7, -1);
                            lastID = CT[CT.Count - 1].ID;
                        }
                        else
                        {
                            WriteTo_CT(Condition7, FromUnit7, INITIATOR, NextSubp7, lastID);
                        }
                        break;

                    case "ждать":
                        string Condition8 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string FromUnit8 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        string ToNameMark8 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[2];
                        string ToNameUnit8 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[3];
                        string ID8 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[4];

                        int IndMark8 = MARKS.FindIndex(rec => ((rec.Name == ToNameMark8) && (rec.Unit == ToNameUnit8)));
                        if (IndMark8 == -1) return "Нет метки с таким именем!";
                        int NextSubp8 = MARKS[IndMark8].Subprogram_Index;

                        int ID = 0;
                        if (ID8 == Convert.ToString(-1)) ID = -1;
                        else ID = lastID;

                        if (Condition8.Contains("ВРЕМЯ="))
                        {
                            double time = 0;
                            string TimeName = Condition8.Substring(FindWord(Condition8, "ВРЕМЯ="));
                            // если простое условие
                            if ((TimeName.Contains(")") == false) || (TimeName.Contains("вектор")) &&
                                ((IsCorrectName(TimeName) == "") || (IsCorrectName(TimeName) == "Использовано числовое значение в качестве имени")))
                            {
                                time = Convert.ToDouble(GetValue(TimeName, FromUnit8));
                                WriteTo_FTT(time, INITIATOR, NextSubp8, ID);
                                lastID = FTT[FTT.Count - 1].ID;
                            }
                            // если сложное условие
                            else
                            {
                                TimeName = TimeName.Substring(0, TimeName.IndexOf(')'));
                                time = Convert.ToDouble(GetValue(TimeName, FromUnit8));
                                WriteTo_FTT(time, INITIATOR, NextSubp8, ID);
                                lastID = FTT[FTT.Count - 1].ID;
                                WriteTo_CT(Condition8, FromUnit8, INITIATOR, NextSubp8, lastID);
                            }
                        }
                        // если нет условия времени
                        else WriteTo_CT(Condition8, FromUnit8, INITIATOR, NextSubp8, ID);

                        break;

                    case "уничтожить":
                        string Name9 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[0];
                        string Unit9 = POSP_Model[NEXT_SUBPROGRAMM].Operations[i].Parameters[1];
                        Terminate_LPDP_Object(Name9, Unit9);
                        break;

                    default: return "Неизвестная команда!";
                }

                if (i != POSP_Model[NEXT_SUBPROGRAMM].Operations.Count - 1)
                {
                    if ((POSP_Model[NEXT_SUBPROGRAMM].Operations[i + 1].Unit_index == -1) ||
                        (POSP_Model[NEXT_SUBPROGRAMM].Operations[i + 1].Line_index == -1))
                        continue;
                }

                if ((mode == Mode.step) && (i != POSP_Model[NEXT_SUBPROGRAMM].Operations.Count - 1))
                {
                    Index_operation++;
                    SubProg_IsBroken = true;
                    return "";
                }
            }
            Index_operation = 0;
            SubProg_IsBroken = false;
            return "";
        }

        // Запуск модели
        public static int NEXT_SUBPROGRAMM = 0;
        public static int Index_operation = 0;
        public static bool SubProg_IsBroken = false;
        public enum Mode { time, timestep, step };
        public static string Launch_POSP_Model(double ByTime, Mode mode)
        {
            //для определения положения в программе для временного шага
            double time_for_step = TIME;
            int subp_for_step = NEXT_SUBPROGRAMM;
            int oper_for_step = Index_operation;
            ////////////////////////////////////////

            string fulfillment_error = "";

            while (SubProg_IsBroken)
            {
                fulfillment_error = Fulfill_SubProgramm(mode);
                if (fulfillment_error != "")
                {
                    LPDP_Core.Model_Is_Built = false;
                    return Errors[1] + fulfillment_error;
                }
                else
                    if (mode == Mode.step) return "Шаг успешно выполнен.";
            }

            //NEXT_SUBPROGRAMM = 0;
            while ((CT.Count > 0) || (FTT.Count > 0))
            {
                //АПУ
                int SaveInitiatior = INITIATOR;
                int index;
                for (index = 0; index < CT.Count; index++)
                {
                    INITIATOR = CT[index].Initiator;
                    if (Logic_Expression(CT[index].Condition, CT[index].FromUnit) == true)
                        break;
                }
                INITIATOR = SaveInitiatior;
                if (index == CT.Count) index = -1;

                //int index = CT.FindIndex(rec => (Logic_Expression(rec.Condition, rec.FromUnit) == true));
                if (index != -1)
                {
                    INITIATOR = CT[index].Initiator;
                    NEXT_SUBPROGRAMM = CT[index].Subprogram_Index;
                    int id_for_remove = CT[index].ID;
                    CT.RemoveAll(rec => rec.ID == id_for_remove);
                    FTT.RemoveAll(rec => rec.ID == id_for_remove);

                    fulfillment_error = Fulfill_SubProgramm(mode);
                    if (fulfillment_error != "")
                    {
                        LPDP_Core.Model_Is_Built = false;
                        return Errors[1] + fulfillment_error;
                    }
                    else
                        if (mode == Mode.step) return "Шаг успешно выполнен.";
                }
                else
                {
                    if ((mode == Mode.timestep) &&
                        ((time_for_step != TIME) || (subp_for_step != NEXT_SUBPROGRAMM) || (oper_for_step != Index_operation)))
                        return "Выполнен КОС во времени: " + TIME;

                    //КАЛЕНДАРЬ
                    //ищем мин
                    int IndMin = 0;
                    for (int i = 0; i < FTT.Count; i++)
                    {
                        if (FTT[i].ActiveTime < FTT[IndMin].ActiveTime) IndMin = i;
                    }
                    if ((FTT[IndMin].ActiveTime >= ByTime) && (mode == Mode.time)) return "Модель остановлена. Время: " + Convert.ToString(TIME);
                    TIME = FTT[IndMin].ActiveTime;
                    INITIATOR = FTT[IndMin].Initiator;
                    NEXT_SUBPROGRAMM = FTT[IndMin].Subprogram_Index;
                    int id_for_remove = FTT[IndMin].ID;
                    CT.RemoveAll(rec => rec.ID == id_for_remove);
                    FTT.RemoveAll(rec => rec.ID == id_for_remove);
                    if (NEXT_SUBPROGRAMM == -1) continue;
                    else
                    {
                        fulfillment_error = Fulfill_SubProgramm(mode);
                        if (fulfillment_error != "")
                        {
                            LPDP_Core.Model_Is_Built = false;
                            return Errors[1] + fulfillment_error;
                        }
                        else
                            if (mode == Mode.step) return "Шаг успешно выполнен.";
                    }

                }
            }
            return "Модель выполнена до конца.";
        }

        //определение очередей
        public static void Rewrite_Queues()
        {
            Queues = new List<POSP_Queue>();

            List<record_FTT> NewFTT = new List<record_FTT>();
            NewFTT.AddRange(FTT);

            List<int> ListID = new List<int>();
            for (int ind_subp = 0; ind_subp < POSP_Model.Count; ind_subp++)
            {
                POSP_Queue Queue = new POSP_Queue();
                Queue.Queue = new List<record_Queue>();
                Queues.Add(Queue);
                // условие выполняется
                for (int ind_table = 0; ind_table < CT.Count; ind_table++)
                {
                    int SaveInitiatior = INITIATOR;
                    INITIATOR = CT[ind_table].Initiator;
                    if ((Logic_Expression(CT[ind_table].Condition, CT[ind_table].FromUnit) == true) &&
                            (CT[ind_table].Subprogram_Index == ind_subp))
                    {
                        ListID.Add(CT[ind_table].ID);
                        WriteTo_Queue(ind_subp, record_Queue.condition.True, 0, CT[ind_table].Initiator, CT[ind_table].ID);
                    }
                    INITIATOR = SaveInitiatior;
                }
                // временное условие
                while (NewFTT.Any<record_FTT>(rec => rec.Subprogram_Index == ind_subp))
                {
                    //любой с нужным инд подпрограммы
                    int IndMin = -1;
                    for (int ind_table = 0; ind_table < NewFTT.Count; ind_table++)
                    {
                        if (NewFTT[ind_table].Subprogram_Index == ind_subp)
                        {
                            IndMin = ind_table;
                            break;
                        }
                    }
                    //минимальный с нужным инд подпрограммы
                    for (int ind_table = 0; ind_table < NewFTT.Count; ind_table++)
                    {
                        if ((NewFTT[ind_table].ActiveTime < NewFTT[IndMin].ActiveTime) && (NewFTT[ind_table].Subprogram_Index == ind_subp)) IndMin = ind_table;
                    }
                    ListID.Add(NewFTT[IndMin].ID);
                    WriteTo_Queue(ind_subp, record_Queue.condition.Time, NewFTT[IndMin].ActiveTime, NewFTT[IndMin].Initiator, NewFTT[IndMin].ID);
                    NewFTT.RemoveAt(IndMin);
                }
                // условие не выполняется
                for (int ind_table = 0; ind_table < CT.Count; ind_table++)
                {
                    int SaveInitiatior = INITIATOR;
                    INITIATOR = CT[ind_table].Initiator;
                    if ((Logic_Expression(CT[ind_table].Condition, CT[ind_table].FromUnit) == false) &&
                            (CT[ind_table].Subprogram_Index == ind_subp))
                    {
                        ListID.Add(CT[ind_table].ID);
                        WriteTo_Queue(ind_subp, record_Queue.condition.False, 0, CT[ind_table].Initiator, CT[ind_table].ID);
                    }
                    INITIATOR = SaveInitiatior;
                }
            }
            //переписываю только повторяющиеся ID
            List<int> NewListID = new List<int>();
            for (int index_of_list = 0; index_of_list < ListID.Count; index_of_list++)
            {
                if (ListID.Count<int>(id => id == ListID[index_of_list]) > 1) NewListID.Add(ListID[index_of_list]);
            }

            for (int i = 0; i < Queues.Count; i++)
            {
                for (int j = 0; j < Queues[i].Queue.Count; j++)
                {
                    record_Queue NewRec = new record_Queue();
                    NewRec.Condition = Queues[i].Queue[j].Condition;
                    NewRec.Initiator = Queues[i].Queue[j].Initiator;
                    NewRec.Time = Queues[i].Queue[j].Time;
                    if (NewListID.Any<int>(id => id == Queues[i].Queue[j].ID)) NewRec.ID = 1;
                    else NewRec.ID = 0;

                    //убрать менее выгодные условия этого же инициатора
                    for (int jj = j + 1; jj < Queues[i].Queue.Count; jj++)
                    {
                        if (Queues[i].Queue[j].Initiator == Queues[i].Queue[jj].Initiator)
                        {
                            if (Queues[i].Queue[j].Condition == record_Queue.condition.True) Queues[i].Queue.RemoveAt(jj);
                            if (Queues[i].Queue[j].Condition == record_Queue.condition.Time)
                                if (Queues[i].Queue[jj].Condition == record_Queue.condition.False) Queues[i].Queue.RemoveAt(jj);
                                else Queues[i].Queue.RemoveAt(j);
                            if (Queues[i].Queue[j].Condition == record_Queue.condition.False) Queues[i].Queue.RemoveAt(j);
                        }
                    }

                    Queues[i].Queue[j] = NewRec;
                }
            }
        }

        //СБРОС
        public static void RESET()
        {
            TIME = 0;
            INITIATOR = -2;
            NEXT_SUBPROGRAMM = 0;
            Index_operation = 0;
            lastID = 0;
            GlobalVar_Table.Clear();
            LocalArea_Table.Clear();

            MARKS.Clear();
            Queues.Clear();
            FTT.Clear();
            CT.Clear();
            Pairs.Clear();

            LPDP_Graphics.GraphicModel.Clear();
            LPDP_Graphics.Connections.Clear();
        }
    }
}
