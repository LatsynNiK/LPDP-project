﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLT
{
    class Executor
    {
        public Model ParentModel;

        Initiator INITIATOR;
        double TIME;
        public TimeAndConditionController TC_Cont;        
        Random RAND;

        public QueueTable QT;
        
        
        public Executor(Model model) 
        {
            this.ParentModel = model;            
            this.TC_Cont = new TimeAndConditionController(this);           
            this.TIME = 0;
            this.INITIATOR = null;
            this.RAND = new Random();
        }

        public void StartUntil(double time)
        {
            try
            {
                while (this.TIME <= time)
                {
                    this.ExecuteSubprogram(this.INITIATOR.NextOperator.ParentSubprogram);
                }
                this.ParentModel.StatusInfo = "Выполнено моделирование до времени: " + this.TIME + ".";
            }
            catch (UserError e)
            {
                UserError Err = new UserError(e);
                this.INITIATOR.Type = InitiatorType.RunTimeError;
                this.ParentModel.Analyzer.Selections.AddError(Err);
                this.ParentModel.StatusInfo = Err.GetErrorStack();
            }
        }

        public void StartStep()
        {
            try
            {
                this.ExecuteOperator(this.INITIATOR.NextOperator);
                this.ParentModel.StatusInfo = "Выполнен шаг.";
            }
            catch (UserError e)
            {
                UserError Err = new UserError(e);
                this.INITIATOR.Type = InitiatorType.RunTimeError;
                this.ParentModel.Analyzer.Selections.AddError(Err);
                this.ParentModel.StatusInfo = Err.GetErrorStack();
            }
        }

        public void StartSEC()
        {            
            try
            {
                this.StartUntil(this.TIME);
                this.ParentModel.StatusInfo = "Выполнены все одновременные события во момент времени: " + this.TIME + ".";
            }
            catch (UserError e)
            {
                UserError Err = new UserError(e);
                this.INITIATOR.Type = InitiatorType.RunTimeError;
                this.ParentModel.Analyzer.Selections.AddError(Err);
                this.ParentModel.StatusInfo = Err.GetErrorStack();
            }
        }

        public void Stop()
        {
            this.ParentModel.Built = false;
            this.ParentModel.StatusInfo = "Модель остановлена.";
            this.SetCurrentUnit(this.ParentModel.ST_Cont.Units[0]);
            this.QT = new QueueTable(this);
            this.TIME = 0;
        }

        void SetState()
        {
            RecordEvent next_event = this.TC_Cont.FindNextEvent();
            if (next_event != null)
            {
                this.TC_Cont.DeleteRecords(next_event.ID);
                this.INITIATOR = next_event.Initiator;
                this.INITIATOR.NextOperator = next_event.Subprogram.Operators[0];                
                if (next_event.GetType() == typeof(RecordFTT))
                {
                    double active_time = ((RecordFTT)next_event).ActiveTime;
                    this.TIME = active_time;
                }
            }
            else
            {
                this.ParentModel.StatusInfo = "Модель выполнилась до конца.";
            }
        }

        public void SetCurrentUnit(Unit unit)
        {
            this.INITIATOR = new Initiator(InitiatorType.Description);
            this.INITIATOR.NextOperator = new Operator();
            this.INITIATOR.NextOperator.ParentSubprogram = new Subprogram();
            this.INITIATOR.NextOperator.ParentSubprogram.Unit = unit;
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
            foreach (Operator oper in subprogram.Operators)
            {
                ExecuteOperator(oper);
            }
            return 1;
        }

        int ExecuteOperator(Operator oper)
        {
            foreach (Action action in oper.Actions)
            {
                try
                {
                    ExecuteAction(action);
                }
                catch (RunTimeError e)
                {
                    throw new RunTimeError(e);
                }
            }
            int next_oper_index = oper.ParentSubprogram.Operators.IndexOf(oper)+1;            
            if (oper.ParentSubprogram.Operators.Count == next_oper_index)
            {
                this.SetState();
            }
            else
            {
                this.INITIATOR.NextOperator = oper.ParentSubprogram.Operators[next_oper_index];
            }
            //Очереди            
            this.QT.Update(this.INITIATOR);
            return 1;
        }

        int ExecuteAction(Action action)
        {
            string unit_name = action.ParentOperator.ParentSubprogram.Unit.Name;
            string label;
            string unit;
            bool islast;
            switch (action.Name)
            { 
                case ActionName.Assign:
                    Phrase var_ph = (Phrase)action.Parameters[0];
                    SLT.Object var = this.GetObject(var_ph);
                    
                    Phrase value_ph = (Phrase)action.Parameters[1];
                    object value_obj;// = ConvertValueToObject(value_ph);
                    if (value_ph.PhType == PhraseType.Initiator_Word)
                    {
                        //пассивизация инициатора
                        value_obj = GetObjectFromLink(value_ph).ID;                        
                    }
                    else
                    {
                        value_obj = ConvertValueToObject(value_ph);
                    }
                    var.SetValue(value_obj);
                    break;
                case ActionName.Create:
                    string var_name = (string)action.Parameters[0];
                    Phrase vartype_ph = (Phrase)action.Parameters[1];

                    SLT.Object new_obj = new SLT.Scalar("", "");//заглушка

                    //для скаляра
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.ScalarVarType_Word))
                    {
                        new_obj = new SLT.Scalar(var_name, unit_name);
                    }
                    //для вектора
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.VectorVarType_Word))
                    {
                        Phrase description_line_ph = vartype_ph.Value.Find(ph => ph.PhType == PhraseType.DescriptionLine);
                        new_obj = new SLT.Vector(var_name, unit_name, this.ParentModel.Analyzer.CreateObjectsFromDesciptionLine(description_line_ph,this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name));
                    }
                    //для ссылки
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.LinkVarType_Word))
                    {
                        new_obj = new SLT.Link(var_name,unit_name);
                    }
                    //для макроса
                    if (vartype_ph.Value.Exists(ph => ph.PhType == PhraseType.MacroVarType_Word))
                    {

                    }
                    //this.ParentModel.O_Cont.CreateObject(new_obj,this.INITIATOR.NextOperator.ParentSubprogram SUBPROGRAM.Unit.Name);
                    this.ParentModel.O_Cont.CreateObject(new_obj,this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);

                    //list.Add(new_obj);
                    break;
                case ActionName.Delete:
                    string deleted_name = Convert.ToString(action.Parameters[0]);
                    if (deleted_name == "True")
                    {
                        int deleted_id = this.INITIATOR.ID_of_MemoryCell;                        
                        this.ParentModel.O_Cont.IT.Delete(deleted_id);
                    }
                    else
                    {
                        //Objects.Object del_obj= this.ParentModel.O_Cont.GetObjectByName(deleted_name,this.SUBPROGRAM.Unit.Name);
                        SLT.Object del_obj = this.ParentModel.O_Cont.GetObjectByName(deleted_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                        //if (del_obj.Unit == this.SUBPROGRAM.Unit.Name)
                        if (del_obj.Unit == this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name)
                        {
                            this.ParentModel.O_Cont.DeleteObjectByID(del_obj.ID);
                        }
                    }
                    break;
                case ActionName.Terminate:
                    string terminated_name = Convert.ToString(action.Parameters[0]);
                    int terminated_id = this.INITIATOR.ID_of_MemoryCell;
                    this.ParentModel.O_Cont.DeleteInitiatorByID(terminated_id);
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
                    Subprogram subp;
                    try
                    {
                        subp = this.ParentModel.ST_Cont.FindSubprogramByLabelAndUnit(label, unit);
                    }
                    catch (RunTimeError e)
                    {
                        throw new RunTimeError(e);
                    }

                    if (link_name == "True")
                    {
                        init = this.INITIATOR;
                    }
                    else 
                    {
                        //init = this.ParentModel.O_Cont.ActivateFromLink(link_name, this.SUBPROGRAM.Unit.Name);
                        init = this.ParentModel.O_Cont.ActivateFromLink(link_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                    }
                    //init.NextOperator = subp;
                    init.NextOperator = subp.Operators[0];

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
                    label = (string)action.Parameters[3];
                    unit = (string)action.Parameters[4];
                    islast = (bool)action.Parameters[5];

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

            //double result = 0;
            Phrase first_exp_ph;
            Phrase last_exp_ph;
            Phrase oper_ph;

            double first_num;
            double last_num;

            switch (ar_exp.PhType)
            {
                case PhraseType.ArithmeticExpression_3lvl:
                case PhraseType.ArithmeticExpression_2lvl:
                case PhraseType.ArithmeticExpression_1lvl:
                case PhraseType.Value:                   
                    string result = "";
                    if (ar_exp.Value[0].PhType == PhraseType.Round_Bracket_Open)
                    {
                        first_exp_ph = ar_exp.Value[1];
                        result = Convert.ToString(this.ComputeArithmeticExpression(first_exp_ph));
                        return result;
                    }
                    first_exp_ph = ar_exp.Value[0];
                    result = this.ComputeArithmeticExpression(first_exp_ph);
                    for (int i = 1; i<ar_exp.Value.Count; i+=2)
                    {
                        first_num = Convert.ToDouble(result);
                        oper_ph = ar_exp.Value[i];
                        last_exp_ph = ar_exp.Value[i+1];
                        last_num = Convert.ToDouble(this.ComputeArithmeticExpression(last_exp_ph));
                        switch (((Lexeme)oper_ph).LValue)
                        {
                            case "+":
                                first_num += last_num;
                                break;
                            case "-":
                                first_num -= last_num;
                                break;
                            case "*":
                                first_num = first_num * last_num;
                                break;
                            case "/":
                                first_num = first_num / last_num;
                                break;
                            case "^":
                                first_num = Math.Pow(first_num, last_num);
                                break;
                        }
                        result = Convert.ToString(first_num);
                    }
                    return result;
                    
                case PhraseType.FinalValue:
                    #region FinalValue
                    first_exp_ph = ar_exp.Value[0];
                    switch (first_exp_ph.PhType)
                    {
                        case PhraseType.Var:
                            return Convert.ToString(this.GetObject(first_exp_ph).GetValue());
                        case PhraseType.LinkVarType_Word:
                            last_exp_ph = ar_exp.Value[2];//ссылка на Name
                            return Convert.ToString(this.GetObject(last_exp_ph).ID);
                            //break;
                        case PhraseType.Rand_Word:
                            return Convert.ToString(this.RAND.NextDouble());
                            //break;
                        case PhraseType.Time_Word:
                            return Convert.ToString(this.TIME);
                            //break;
                        case PhraseType.Number:
                            string double_str = ((Lexeme)first_exp_ph).LValue;
                            double_str = double_str.Replace('.', ',');
                            return Convert.ToString(double_str);
                            //break;     
                        case PhraseType.String:
                            return ((Lexeme)first_exp_ph).LValue;

                        default:
                            return "";
                    }
                    #endregion
                    //break;

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

                    string p1, p2;//, p3;
                    switch (func_type)
                    {
                        case "ЦЕЛОЕ":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString(Convert.ToInt32(Math.Truncate(Convert.ToDouble(p1))));
                            //break;
                        case "ln":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString( Math.Log(Convert.ToDouble(p1)));
                            //break;
                        case "log":
                            if (param_values.Count < 2)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            p2 = param_values[1];
                            return Convert.ToString(Math.Log(Convert.ToDouble(p1),Convert.ToDouble(p2)));
                            //break;
                        case "lg":
                            if (param_values.Count < 1)
                            {
                                //error
                            }
                            p1 = param_values[0];
                            return Convert.ToString(Math.Log10(Convert.ToDouble(p1)));
                            //break;
                        default:
                            return "";
                    }
                    #endregion
                    //break;
                default:
                    return "";
            }
        }

        public bool ComputeLogicExpression(Phrase logic_exp)
        {
            bool result;
            if (logic_exp.PhType == PhraseType.True)
            {
                return true;
            }
            if (logic_exp.PhType == PhraseType.False)
            {
                return false;
            }
            if (logic_exp.Value[0].PhType == PhraseType.Square_Bracket_Open)
            {
                return this.ComputeLogicExpression(logic_exp.Value[1]);
            }
            result = this.ComputeComparisonExpression(logic_exp.Value[0]);
            for (int i = 1; i < logic_exp.Value.Count; i += 2)
            {
                Phrase logic_oper_ph = logic_exp.Value[i];
                Phrase last_value_ph = logic_exp.Value[i + 1];
                bool last_value = this.ComputeLogicExpression(last_value_ph);
                switch (((Lexeme)logic_oper_ph).LValue)
                {
                    case "И":
                        result = result && last_value;
                        break;
                    case "ИЛИ":
                        result = result || last_value;
                        break;
                    default:
                        //error
                        result = false;
                        break;
                }
            }
            return result;
        }

        public bool ComputeComparisonExpression(Phrase comp_exp)
        {
            if (comp_exp.Value[0].PhType == PhraseType.Square_Bracket_Open)
            {
                return this.ComputeComparisonExpression(comp_exp.Value[1]);
            }
            Phrase first_value_ph = comp_exp.Value[0];
            Phrase comparison_oper_ph = comp_exp.Value[1];
            Phrase last_value_ph = comp_exp.Value[2];
            
            string first_value;
            string last_value;

            first_value = this.ComputeArithmeticExpression(first_value_ph);
            last_value = this.ComputeArithmeticExpression(last_value_ph);

            bool result;

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
            return result;
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
        public SLT.Object GetObject(Phrase var)
        {
            try
            {
                //заглушка
                SLT.Object result = new SLT.Scalar("", "");
                //
                string var_name;// = ((Lexeme)var_name_ph).LValue;
                if (var.PhType == PhraseType.Name)
                {
                    var_name = ((Lexeme)var).LValue;
                    result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                    //if (result == null)
                    //{
                    //    throw new NameNotFound(var.Start, var.Length, var.Line, var_name);
                    //}
                    return result;
                }

                Phrase var_name_ph = var.Value[0];
                Phrase next_ph = var.Value[1];

                var_name = ((Lexeme)var_name_ph).LValue;
                if (var_name_ph.PhType == PhraseType.Name)
                {
                    result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                    //if (result == null)
                    //{
                    //    throw new NameNotFound(var.Start, var.Length, var.Line, var_name);
                    //}
                }

                switch (next_ph.PhType)
                {
                    case PhraseType.VectorNode:
                        //result = this.ParentModel.O_Cont.GetVectorNode(var, this.SUBPROGRAM.Unit.Name);

                        //result = this.ParentModel.O_Cont.GetVectorNode(var, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                        result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
                        if (next_ph.Value.Count == 0)
                        {
                            //var_name = ((Lexeme)var_name_ph).LValue;
                            //result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);                        
                        }
                        else
                        {
                            //string var_name = ((Lexeme)var_name_ph).LValue;
                            //result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);                        
                            string inner_name = ((Lexeme)next_ph.Value[1]).LValue;
                            Phrase inner_ph = next_ph.Value[2];
                            result = ((Vector)result).FindNode(inner_name, inner_ph);
                        }
                        break;
                    case PhraseType.ValueFromLink:
                        result = this.GetObjectFromLink(var_name_ph);
                        next_ph = next_ph.Value[1].Value[1];
                        if (next_ph.Value.Count == 0)
                        {
                            //var_name = ((Lexeme)var_name_ph).LValue;
                            //result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);                        
                        }
                        else
                        {
                            //string var_name = ((Lexeme)var_name_ph).LValue;
                            //result = this.ParentModel.O_Cont.GetObjectByName(var_name, this.INITIATOR.NextOperator.ParentSubprogram.Unit.Name);                        
                            string inner_name = ((Lexeme)next_ph.Value[1]).LValue;
                            Phrase inner_ph = next_ph.Value[2];
                            result = ((Vector)result).FindNode(inner_name, inner_ph);
                        }
                        break;
                }
                if (result == null)
                {
                    throw new ObjectNotFoundError(var.Start, var.Length, var.Line, var_name);
                }
                return result;
            }
            catch (NameNotFound e)
            {
                throw new ObjectNotFoundError(var.Start, var.Length, var.Line, e);
                //throw new ObjectDoesNotExistError(e);
            }
        }

        public SLT.Object GetObjectFromLink(Phrase link_name)
        {
            SLT.Object result;
            int cell_id;
            if (link_name.PhType == PhraseType.Initiator_Word)
            {
                cell_id = this.INITIATOR.GetID();
            }
            else
            {
                //cell_id = this.ParentModel.O_Cont.GetLinkValue((((Lexeme)link_name).LValue), this.SUBPROGRAM.Unit.Name);
                cell_id = this.ParentModel.O_Cont.GetLinkValue((((Lexeme)link_name).LValue), this .INITIATOR.NextOperator.ParentSubprogram.Unit.Name);
            }
            result = this.ParentModel.O_Cont.GetObjectByID(cell_id);
            return result;
        }


        //GET 
        public double GetTIME()
        {
            return this.TIME;
        }

    }
}
