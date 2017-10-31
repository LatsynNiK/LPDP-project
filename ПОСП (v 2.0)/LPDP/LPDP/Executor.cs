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
        Model ParentModel;

        Subprogram SUBPROGRAM;
        Operator NEXT_OPERATOR;
        Initiator INITIATOR;
        double TIME;

        //public FutureTimesTable FTT;
        //public ConditionsTable CT;
        public TimeAndConditionController TC_Cont;

        public InitiatorsTable IT;
        Random RAND;
        
        
        public Executor(Model model) 
        {
            this.ParentModel = model;

            //this.CT = new ConditionsTable();
            //this.FTT = new FutureTimesTable();
            this.TC_Cont = new TimeAndConditionController(this);

            this.IT = new InitiatorsTable();

            this.TIME = 0;
            this.SUBPROGRAM = null;
            this.NEXT_OPERATOR = null;
            this.INITIATOR = null;
            this.RAND = new Random();
            
        }

        public 

        int ExecuteSubprogram(Subprogram subprogram)
        {
            this.SUBPROGRAM = subprogram;

            foreach (Operator oper in subprogram.Operators)
            {
                ExecuteOperator(oper);
            }
            return 1;
        }

        int ExecuteOperator(Operator oper)
        {
            foreach (Structure.Action action in oper.Actions)
            {
                ExecuteAction(action);
            }
            return 1;
        }

        int ExecuteAction(Structure.Action action)
        {
            switch (action.Name)
            { 
                case ActionName.Assign:
                    Phrase var_ph = (Phrase)action.Parameters[0];
                    Objects.Object var = this.GetObject(var_ph);
                    string unit_name = action.ParentOperator.ParentSubprogram.Unit.Name;

                    Phrase value_ph = (Phrase)action.Parameters[1];
                    object value_obj;
                    if (value_ph.Value[0].PhType == PhraseType.StringValue)
                    {
                        value_obj = ((Word)value_ph.Value[0]).LValue;
                    }
                    else
                    {
                        value_obj = ComputeArithmeticExpression(value_ph.Value[0]);
                    }
                    var.SetValue(value_obj);


                    

                    //this.ParentModel.O_Cont.SetValueToVar(action.Parameters
                    break;
                case ActionName.Create:
                    break;
                case ActionName.Delete:
                    break;
                case ActionName.Write_to_CT:
                    break;
                case ActionName.Write_to_FTT:
                    break;
            }
            return 1;
        }

        #region computing region

        double ComputeArithmeticExpression(Phrase ar_exp)
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

                        first_num = this.ComputeArithmeticExpression(first_exp_ph);
                        last_num = this.ComputeArithmeticExpression(last_exp_ph);
                        switch (((Word)oper_ph).LValue)
                        {
                            case "+":
                                result = first_num + last_num;
                                break;
                            case "-":
                                result = first_num - last_num;
                                break;
                        }
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_2lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
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

                        first_num = this.ComputeArithmeticExpression(first_exp_ph);
                        last_num = this.ComputeArithmeticExpression(last_exp_ph);
                        switch (((Word)oper_ph).LValue)
                        {
                            case "*":
                                result = first_num * last_num;
                                break;
                            case "/":
                                result = first_num / last_num;
                                break;
                        }
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_1lvl))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
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

                        first_num = this.ComputeArithmeticExpression(first_exp_ph);
                        last_num = this.ComputeArithmeticExpression(last_exp_ph);

                        switch (((Word)oper_ph).LValue)
                        {
                            case "^":
                                result = Math.Pow(first_num,last_num);
                                break;
                        }
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.DigitalValue))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticExpression_3lvl))
                    {
                        first_exp_ph = ar_exp.Value[1]; // ( exp )
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
                    }
                    if (ar_exp.Value.Exists(ph => ph.PhType == PhraseType.ArithmeticFunction))
                    {
                        first_exp_ph = ar_exp.Value[0];
                        result = this.ComputeArithmeticExpression(first_exp_ph);
                        return result;
                    }
                    #endregion
                    break;
                case PhraseType.DigitalValue:
                    #region DigitalValue
                    first_exp_ph = ar_exp.Value[0];
                    switch (first_exp_ph.PhType)
                    {
                        case PhraseType.ValueFromLink:
                            result = Convert.ToDouble(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.VectorNode:
                            result = Convert.ToDouble(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.Name:
                            result = Convert.ToDouble(this.GetObject(first_exp_ph).GetValue());
                            break;
                        case PhraseType.LinkVarType_Word:
                            last_exp_ph = ar_exp.Value[2];//ссылка на Name
                            result = Convert.ToDouble(this.GetObject(first_exp_ph).ID);
                            break;
                        case PhraseType.Rand_Word:
                            result = this.RAND.NextDouble();
                            break;
                        case PhraseType.Time_Word:
                            result = this.TIME;
                            break;
                        case PhraseType.Number:
                            result = Convert.ToDouble(((Word)first_exp_ph).LValue);
                            break;                        
                    }
                    #endregion
                    break;
            }




            return result;
        }

        bool ComputeLogicExpression(Phrase logic_exp)
        {
            bool result;
            if (logic_exp.Value.Exists(ph => ph.PhType == PhraseType.ComparisonOperator))
            {
                #region comparison
                Phrase first_value_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.Value).Value[0];
                Phrase last_value_ph = logic_exp.Value.FindLast(ph => ph.PhType == PhraseType.Value).Value[0];
                Phrase comparison_oper_ph = logic_exp.Value.Find(ph => ph.PhType == PhraseType.ComparisonOperator);

                object first_value;
                object last_value;

                if (first_value_ph.PhType == PhraseType.ArithmeticExpression_3lvl)
                {
                    first_value = this.ComputeArithmeticExpression(first_value_ph);
                }
                else
                {
                    first_value = ((Word)first_value_ph).LValue;
                }
                if (last_value_ph.PhType == PhraseType.ArithmeticExpression_3lvl)
                {
                    last_value = this.ComputeArithmeticExpression(last_value_ph);
                }
                else
                {
                    last_value = ((Word)last_value_ph).LValue;
                }

                switch (((Word)comparison_oper_ph).LValue)
                {
                    case "=":
                        result = first_value == last_value;
                        break;
                    case "!=":
                        result = first_value != last_value;
                        break;
                    case ">":
                        result = (double)first_value > (double)last_value;
                        break;
                    case "<":
                        result = (double)first_value > (double)last_value;
                        break;
                    case ">=":
                        result = (double)first_value >= (double)last_value;
                        break;
                    case "<=":
                        result = (double)first_value <= (double)last_value;
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
                switch (((Word)logic_oper_ph).LValue)
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


        #endregion

        public LPDP.Objects.Object GetObject(Phrase var)
        {
            //заглушка
            LPDP.Objects.Object result = this.ParentModel.O_Cont.GetScalar(((Word)var).LValue, this.SUBPROGRAM.Unit.Name);
            //

            switch (var.PhType)
            {
                case PhraseType.Name:
                    string var_name = ((Word)var).LValue;
                    result = this.ParentModel.O_Cont.GetScalar(var_name, this.SUBPROGRAM.Unit.Name);
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
                            if (var_from_link.Name != ((Word)path).LValue)
                            { //error
                            }
                            result = var_from_link;
                            break;
                        case ObjectType.Vector:
                            if (var_from_link.Name != ((Word)path.Value[0]).LValue)
                            { //error
                            }
                            Phrase inner_node = path.Value[2];
                            LPDP.Objects.Vector vector_from_link = (LPDP.Objects.Vector)var_from_link;
                            result = vector_from_link.FindNode(inner_node);
                            break;
                    }
                    break;
                default:
                    //error
                    var_name = ((Word)var).LValue;
                    result = this.ParentModel.O_Cont.GetScalar(var_name, this.SUBPROGRAM.Unit.Name);
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
                cell_id = this.ParentModel.O_Cont.GetLinkValue((((Word)link_name).LValue), this.SUBPROGRAM.Unit.Name);
            }
            result = this.ParentModel.O_Cont.GetObjectByID(cell_id);
            return result;
        }
    }
}
