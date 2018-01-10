using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.Structure;
using LPDP.TextAnalysis;
using LPDP.Objects;
using LPDP.Dynamics;

namespace LPDP
{
    public class Executor
    {
        public Model ParentModel;

        Subprogram SUBPROGRAM;
        Operator NEXT_OPERATOR;
        Initiator INITIATOR;
        double TIME;

        //public FutureTimesTable FTT;
        //public ConditionsTable CT;
        public TimeAndConditionController TC_Cont;

        //public InitiatorsTable IT;
        Random RAND;

        public QueueTable QT;
        
        
        public Executor(Model model) 
        {
            this.ParentModel = model;

            //this.CT = new ConditionsTable();
            //this.FTT = new FutureTimesTable();
            this.TC_Cont = new TimeAndConditionController(this);

            

            this.TIME = 0;
            this.SUBPROGRAM = null;
            this.NEXT_OPERATOR = null;
            this.INITIATOR = null;
            this.RAND = new Random();
        }

        public void StartUntil(double time)
        {
            while (this.TIME < time)
            {
                this.ExecuteSubprogram(this.SUBPROGRAM);
            }
        }

        public void StartStep()
        {
            this.ExecuteOperator(this.NEXT_OPERATOR);
        }

        public void StartSEC()
        {
            //while(this.SUBPROGRAM.)
        }

        public void Stop()
        {
            this.ParentModel.Built = false;
            this.SetCurrentUnit(this.ParentModel.Units[0]);
        }

        void SetState()
        {
            RecordEvent next_event = this.TC_Cont.FindNextEvent();
            if (next_event != null)
            {
                this.TC_Cont.DeleteRecords(next_event.ID);
                this.INITIATOR = next_event.Initiator;
                this.SUBPROGRAM = next_event.Subprogram;
                this.NEXT_OPERATOR = this.SUBPROGRAM.Operators[0];

                this.INITIATOR.Position = this.SUBPROGRAM;
                if (next_event.GetType() == typeof(RecordFTT))
                {
                    double active_time = ((RecordFTT)next_event).ActiveTime;
                    this.TIME = active_time;
                }
            }
            else
            {
                //модель выполнилась до конца
            }
        }

        public void SetCurrentUnit(Unit unit)
        {
            this.INITIATOR = new Initiator(InitiatorType.Description);
            this.INITIATOR.Position = new Subprogram();
            this.INITIATOR.Position.Unit = unit;
            //this.SUBPROGRAM = new Subprogram();
            //this.SUBPROGRAM.Unit = unit;
        }

        public void SetInitiator(Initiator init)
        {
            this.INITIATOR = init;
        }

        public Initiator GetInitiator()
        {
            return this.INITIATOR;
        }

        int ExecuteSubprogram(Subprogram subprogram)
        {
            this.SUBPROGRAM = subprogram;

            foreach (Operator oper in subprogram.Operators)
            {
                ExecuteOperator(oper);
            }
            //RecordEvent next_event = this.TC_Cont.FindNextEvent();
            //this.INITIATOR = next_event.Initiator;
            //this.SUBPROGRAM = this.TC_Cont.FindNextEvent().Subprogram;
            //this.NEXT_OPERATOR = this.SUBPROGRAM.Operators[0];
            this.SetState();



            //this.NEXT_OPERATOR = null;
            return 1;
        }

        int ExecuteOperator(Operator oper)
        {
            foreach (Structure.Action action in oper.Actions)
            {
                ExecuteAction(action);
            }
            int next_oper_index = oper.ParentSubprogram.Operators.IndexOf(oper)+1;            
            if (oper.ParentSubprogram.Operators.Count == next_oper_index)
            {
                this.SetState();
            }
            else
            {
                this.NEXT_OPERATOR = oper.ParentSubprogram.Operators[next_oper_index];
            }
            //Очереди            
            this.QT.Update(this.INITIATOR);
            return 1;
        }

        int ExecuteAction(Structure.Action action)
        {
            string unit_name = action.ParentOperator.ParentSubprogram.Unit.Name;
            string label;
            string unit;
            bool islast;
            switch (action.Name)
            { 
                case ActionName.Assign:
                    Phrase var_ph = (Phrase)action.Parameters[0];
                    Objects.Object var = this.GetObject(var_ph);
                    
                    Phrase value_ph = (Phrase)action.Parameters[1];
                    object value_obj = ConvertValueToObject(value_ph);
                    var.SetValue(value_obj);
                    break;
                case ActionName.Create:
                    string var_name = (string)action.Parameters[0];
                    Phrase vartype_ph = (Phrase)action.Parameters[1];

                    Objects.Object new_obj = new Objects.Scalar("", "");//заглушка

                    //для скаляра
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.ScalarVarType_Word))
                    {
                        new_obj = new Objects.Scalar(var_name, unit_name);
                    }
                    //для вектора
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.VectorVarType_Word))
                    {
                        Phrase description_line_ph = vartype_ph.Value.Find(ph => ph.PhType == PhraseType.DescriptionLine);
                        new_obj = new Objects.Vector(var_name, unit_name, this.ParentModel.Analysis.CreateObjectsFromDesciptionLine(description_line_ph,this.SUBPROGRAM.Unit.Name));
                    }
                    //для ссылки
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.LinkVarType_Word))
                    {
                        new_obj = new Objects.Link(var_name,unit_name);
                    }
                    //для макроса
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.MacroVarType_Word))
                    {

                    }
                    this.ParentModel.O_Cont.CreateObject(new_obj,this.SUBPROGRAM.Unit.Name);

                    //list.Add(new_obj);
                    break;
                case ActionName.Delete:
                    string deleted_name = Convert.ToString(action.Parameters[0]);
                    if (deleted_name == "True")
                    {
                        int deleted_id = this.INITIATOR.ID_of_MemoryCell;
                        this.ParentModel.O_Cont.DeleteInitiatorByID(deleted_id);
                    }
                    else
                    {
                        Objects.Object del_obj= this.ParentModel.O_Cont.GetObjectByName(deleted_name,this.SUBPROGRAM.Unit.Name);
                        if (del_obj.Unit == this.SUBPROGRAM.Unit.Name)
                        {
                            this.ParentModel.O_Cont.DeleteObjectByID(del_obj.ID);
                        }
                    }
                    break;
                case ActionName.Write_to_CT:
                    Phrase condition;
                    if (typeof(bool) == action.Parameters[0].GetType())
                    {
                        if ((bool)action.Parameters[0]) 
                            condition = new Phrase(PhraseType.True);
                        else
                            condition = new Phrase(PhraseType.False);
                    }
                    else 
                    {
                        condition = (Phrase)action.Parameters[0];
                    }
                    string link_name = Convert.ToString(action.Parameters[1]);
                    bool to_the_begining = (bool)action.Parameters[2];
                    label = (string)action.Parameters[3];
                    unit = (string)action.Parameters[4];
                    islast = (bool)action.Parameters[5];

                    Initiator init;
                    Subprogram subp = this.ParentModel.ST_Cont.FindSubprogramByLabelAndUnit(label,unit);
                    if (link_name == "True")
                    {
                        init = this.INITIATOR;
                    }
                    else 
                    {
                        init = this.ParentModel.O_Cont.ActivateFromLink(link_name, this.SUBPROGRAM.Unit.Name);
                    }
                    init.Position = subp;

                    if (to_the_begining)
                    {
                        this.TC_Cont.InsertConditionRecord(condition,init,subp,islast);
                    }
                    else
                    {
                        this.TC_Cont.AddConditionRecord(condition,init,subp,islast);
                    }
                    break;
                case ActionName.Write_to_FTT:
                    Phrase time_exp = (Phrase)action.Parameters[0];
                    double time = Convert.ToDouble( this.ComputeArithmeticExpression(time_exp));
                    label = (string)action.Parameters[1];
                    unit = (string)action.Parameters[2];
                    islast = (bool)action.Parameters[3];

                    init = this.INITIATOR;
                    subp = this.ParentModel.ST_Cont.FindSubprogramByLabelAndUnit(label, unit);
                    this.TC_Cont.AddTimeRecord(time, init, subp, islast);
                    break;
            }
            return 1;
        }

        public void Preparetion()
        {
            this.QT = new QueueTable(this);
            this.ParentModel.Executor.SetState();
            this.QT.Update(this.INITIATOR);
        }

        #region computing region

        public string ComputeArithmeticExpression(Phrase ar_exp)
        {
            double result = 0;
            Phrase first_exp_ph;
            Phrase last_exp_ph;
            Phrase oper_ph;

            double first_num;
            double last_num;

            switch (ar_exp.PhType)
            {
                case PhraseType.ArithmeticExpression_3lvl:
                    #region ArithmeticExpression_3lvl
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticOperator_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        oper_ph = ar_exp.Value[1];
                        last_exp_ph = ar_exp.Value[2];

                        first_num = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        last_num = Convert.ToDouble(this.ComputeArithmeticExpression(last_exp_ph));
                        switch (((Lexeme)oper_ph).LValue)
                        {
                            case "+":
                                result = first_num + last_num;
                                break;
                            case "-":
                                result = first_num - last_num;
                                break;
                        }
                        return Convert.ToString(result);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_2lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        //result = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        //result = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    #endregion
                    break;
                case PhraseType.ArithmeticExpression_2lvl:
                    #region ArithmeticExpression_2lvl
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticOperator_2lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        oper_ph = ar_exp.Value[1];
                        last_exp_ph = ar_exp.Value[2];

                        first_num = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        last_num = Convert.ToDouble(this.ComputeArithmeticExpression(last_exp_ph));
                        switch (((Lexeme)oper_ph).LValue)
                        {
                            case "*":
                                result = first_num * last_num;
                                break;
                            case "/":
                                result = first_num / last_num;
                                break;
                        }
                        return Convert.ToString(result);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_1lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        //result =Convert.ToDouble( this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        //result = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    #endregion
                    break;
                case PhraseType.ArithmeticExpression_1lvl:
                    #region ArithmeticExpression_1lvl
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticOperator_1lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        oper_ph = ar_exp.Value[1];
                        last_exp_ph = ar_exp.Value[2];

                        first_num = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        last_num = Convert.ToDouble(this.ComputeArithmeticExpression(last_exp_ph));

                        switch (((Lexeme)oper_ph).LValue)
                        {
                            case "^":
                                result = Math.Pow(first_num,last_num);
                                break;
                        }
                        return Convert.ToString(result);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.DigitalValue))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        //result = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        //result = Convert.ToDouble(this.ComputeArithmeticExpression(first_exp_ph));
                        //return Convert.ToString(result);
                        return this.ComputeArithmeticExpression(first_exp_ph);
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticFunction))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        return this.ComputeArithmeticExpression(first_exp_ph);
                        //return Convert.ToString(result);
                    }
                    #endregion
                    break;
                case PhraseType.DigitalValue:
                    #region DigitalValue
                    first_exp_ph = ar_exp.Value[0];
                    switch (first_exp_ph.PhType)
                    {
                        case PhraseType.ValueFromLink:
                            //result = Convert.ToDouble(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.VectorNode:
                            return Convert.ToString(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.Name:
                            return Convert.ToString(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.LinkVarType_Word:
                            last_exp_ph = ar_exp.Value[2];//ссылка на Name
                            return Convert.ToString(this.GetObject(last_exp_ph).ID);
                            break;
                        case PhraseType.Rand_Word:
                            return Convert.ToString(this.RAND.NextDouble());
                            break;
                        case PhraseType.Time_Word:
                            return Convert.ToString(this.TIME);
                            break;
                        case PhraseType.Number:
                            string double_str = ((Lexeme)first_exp_ph).LValue;
                            double_str = double_str.Replace('.', ',');
                            return Convert.ToString(double_str);
                            break;     
                        case PhraseType.String:
                            return ((Lexeme)first_exp_ph).LValue;

                    }
                    #endregion
                    break;

                    //
                case PhraseType.ArithmeticFunction:
                    #region ArithmeticFunction
                    string func_type = ((Word)ar_exp.Value[0]).LValue;
                    
                    List<string> param_values = new List<string>();
                    Phrase Params = ar_exp.Value.Find(ph => ph.PhType == PhraseType.Parameters);
                    foreach (Phrase ph in Params.Value)
                    {
                        if (ph.PhType != PhraseType.Comma)
                        {
                            string value = this.ComputeArithmeticExpression(ph);
                            param_values.Add(value);
                        }
                    }

                    string p1, p2, p3;
                    switch (func_type)
                    {
                        case "ЦЕЛОЕ":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString(Convert.ToInt32(Convert.ToDouble(p1)));
                            break;
                        case "ln":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString( Math.Log(Convert.ToDouble(p1)));
                            break;
                        case "log":
                            if (param_values.Count < 2)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            p2 = param_values[1];
                            return Convert.ToString(Math.Log(Convert.ToDouble(p1),Convert.ToDouble(p2)));
                            break;
                        case "lg":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString(Math.Log10(Convert.ToDouble(p1)));
                            break;
                    }
                    #endregion
                    break;
            }




            return Convert.ToString(result);
        }

        public bool ComputeLogicExpression(Phrase logic_exp)
        {
            bool result;
            if (logic_exp.PhType == PhraseType.True)
            {
                return true;
            }
            if (logic_exp.Value.Exists(ph => ph.PhType == PhraseType.ComparisonOperator))
            {
                #region comparison
                Phrase first_value_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.Value).Value[0];
                Phrase last_value_ph = logic_exp.Value.FindLast(ph => ph.PhType == PhraseType.Value).Value[0];
                Phrase comparison_oper_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.ComparisonOperator);

                string first_value;
                string last_value;

                if (first_value_ph.PhType == PhraseType.ArithmeticExpression_3lvl)
                {
                    first_value = this.ComputeArithmeticExpression(first_value_ph);
                }
                else
                {
                    first_value = ((Lexeme)first_value_ph).LValue;
                }
                if (last_value_ph.PhType == PhraseType.ArithmeticExpression_3lvl)
                {
                    last_value = this.ComputeArithmeticExpression(last_value_ph);
                }
                else
                {
                    last_value = ((Lexeme)last_value_ph).LValue;
                }

                switch (((Lexeme)comparison_oper_ph).LValue)
                {
                    case "=":
                        result = first_value == last_value;
                        break;
                    case "!=":
                        result = first_value != last_value;
                        break;
                    case ">":
                        result = Convert.ToDouble(first_value) > Convert.ToDouble(last_value);
                        break;
                    case "<":
                        result = Convert.ToDouble(first_value) < Convert.ToDouble(last_value);
                        break;
                    case ">=":
                        result = Convert.ToDouble(first_value) >= Convert.ToDouble(last_value);
                        break;
                    case "<=":
                        result = Convert.ToDouble(first_value) <= Convert.ToDouble(last_value);
                        break;
                    default:
                        //error
                        result = false;
                        break;
                }
                #endregion
                return result;
            }
            if (logic_exp.Value.Exists(ph => ph.PhType == PhraseType.LogicOperator))
            {
                #region logic operator
                Phrase first_exp_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.LogicExpression);
                Phrase last_exp_ph = logic_exp.Value.FindLast(ph => ph.PhType == PhraseType.LogicExpression);
                Phrase logic_oper_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.LogicOperator);

                bool first_bool = this.ComputeLogicExpression(first_exp_ph);
                bool last_bool = this.ComputeLogicExpression(last_exp_ph);
                switch (((Lexeme)logic_oper_ph).LValue)
                {
                    case "/\\":
                        result = first_bool && last_bool;
                        break;
                    case "\\/":
                        result = first_bool || last_bool;
                        break;
                    default:
                        //error
                        result = false;
                        break;
                }
                #endregion
                return result;
            }
            if (logic_exp.Value.Count(ph => ph.PhType == PhraseType.LogicExpression) == 1)
            {
                Phrase inner_logic_exp = logic_exp.Value[1];
                result = this.ComputeLogicExpression(inner_logic_exp);
                return result;
            }
            //error
            return false;
        }

        public object ConvertValueToObject(Phrase value_ph)
        {
            object value_obj;
            switch (value_ph.Value[0].PhType)
            {
                case PhraseType.String:
                    value_obj = ((Lexeme)value_ph.Value[0]).LValue;
                    break;
                case PhraseType.Name:
                    value_obj = this.GetObject(value_ph).GetValue();
                    break;
                case PhraseType.ArithmeticExpression_3lvl:
                    value_obj = ComputeArithmeticExpression(value_ph.Value[0]);
                    break;
                default:
                    value_obj = ComputeArithmeticExpression(value_ph.Value[0]);
                    break;
            }
            return value_obj;
        }

        #endregion

        // GET OBJECT
        public LPDP.Objects.Object GetObject(Phrase var)
        {
            //заглушка
            LPDP.Objects.Object result = new LPDP.Objects.Scalar("","");
            //

            switch (var.PhType)
            {
                case PhraseType.Name:
                    string var_name = ((Lexeme)var).LValue;

                    result = this.ParentModel.O_Cont.GetObjectByName (var_name, this.INITIATOR.Position.Unit.Name);

                    
                    //if (this.ParentModel.O_Cont.GVT.Vars[this.SUBPROGRAM.Unit.Name].
                    //    Exists(v => (v.Name == var_name) && (v.Type == Objects.ObjectType.Scalar)))
                    //{
                    //    result = this.ParentModel.O_Cont.GetScalar(var_name, this.SUBPROGRAM.Unit.Name);
                    //}
                    //if (this.ParentModel.O_Cont.GVT.Vars[this.SUBPROGRAM.Unit.Name].
                    //    Exists(v => (v.Name == var_name) && (v.Type == Objects.ObjectType.Link)))
                    //{
                    //    result = this.ParentModel.O_Cont.GetLink(var_name, this.SUBPROGRAM.Unit.Name);
                    //}
                    //if (this.ParentModel.O_Cont.GVT.Vars[this.SUBPROGRAM.Unit.Name].
                    //    Exists(v => (v.Name == var_name) && (v.Type == Objects.ObjectType.Scalar)))
                    //{
                    //    result = this.ParentModel.O_Cont.GetScalar(var_name, this.SUBPROGRAM.Unit.Name);
                    //}
                    //if (this.ParentModel.O_Cont.GVT.Vars[this.SUBPROGRAM.Unit.Name].
                    //    Exists(v => (v.Name == var_name) && (v.Type == Objects.ObjectType.Vector)))
                    //{
                    //    result = this.ParentModel.O_Cont.Get(var_name, this.SUBPROGRAM.Unit.Name);
                    //}
                    //this.ParentModel.O_Cont.SetValueToScalar(var_name, unit_name, value_obj);

                    break;
                case PhraseType.VectorNode:
                    //this.ParentModel.O_Cont.SetValueToVector(var, unit_name, value_obj);
                    result = this.ParentModel.O_Cont.GetVectorNode(var, this.SUBPROGRAM.Unit.Name);
                    break;
                case PhraseType.ValueFromLink:
                    Objects.Object var_from_link = this.GetObjectFromLink(var.Value[0]);
                    Phrase path = var.Value[2];
                    switch (var_from_link.Type)
                    {
                        case ObjectType.Scalar:
                            if (var_from_link.Name != ((Lexeme)path.Value[0]).LValue)
                            { //error
                            }
                            result = var_from_link;
                            break;
                        case ObjectType.Vector:
                            if (var_from_link.Name != ((Lexeme)path.Value[0].Value[0]).LValue)
                            { //error
                            }
                            Phrase inner_node = path.Value[0].Value[2];
                            LPDP.Objects.Vector vector_from_link = (LPDP.Objects.Vector)var_from_link;
                            result = vector_from_link.FindNode(inner_node);
                            break;
                    }
                    break;
                default:
                    //error
                    var_name = ((Lexeme)var).LValue;
                    result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.SUBPROGRAM.Unit.Name);
                    break;
            }
            return result;

        }

        public LPDP.Objects.Object GetObjectFromLink(Phrase link_name)
        {
            LPDP.Objects.Object result;
            int cell_id;
            if (link_name.PhType == PhraseType.Initiator_Word)
            {
                cell_id = this.INITIATOR.GetID();
            }
            else
            {
                cell_id = this.ParentModel.O_Cont.GetLinkValue((((Lexeme)link_name).LValue), this.SUBPROGRAM.Unit.Name);
            }
            result = this.ParentModel.O_Cont.GetObjectByID(cell_id);
            return result;
        }


        //GET 
        public double GetTIME()
        {
            return this.TIME;
        }

        public LPDP.DataSets.Selection GetNextOperatorPosition()
        {
            return this.NEXT_OPERATOR.Position;
        }

        public InitiatorType GetInitiatorType()
        {
            return this.INITIATOR.Type;
        }
    }
}
