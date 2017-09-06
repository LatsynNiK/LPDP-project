using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP
{
    abstract class LPDP_Object //: Var
    {
        public string Name;
        public string Unit;
        public ObjectType Type;

        public enum ObjectType { Scalar, Link, Vector }

        // virtual string Value {get;set;}

        public LPDP_Object(string Name, string Unit, LPDP_Object.ObjectType Type)
        {
            this.Name = Name;
            this.Unit = Unit;
            this.Type = Type;
        }

        public LPDP_Object(string Name, string Unit)
        {
            this.Name = Name;
            this.Unit = Unit;
        }

        public LPDP_Object() { }

        virtual public string GetValue() { return ""; }
        virtual public string GetValue(string tree) { return ""; }

        virtual public void SetValue(string val) { }
        virtual public void SetValue(string tree, string val) { }
        virtual public void SetValue(int val) { }
    }

    class Scalar : LPDP_Object
    {
         string Value;

        //конструкторы
        public Scalar(string Name, string Unit, string Value)
            : base(Name, Unit)
        {
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Scalar;
            this.Value = Value;
        }

        public Scalar(string Name, string Unit, int Value)
            : base(Name, Unit)
        {
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Scalar;
            this.Value = Convert.ToString(Value);
        }
        public Scalar(string Name, string Unit, double Value)
            : base(Name, Unit)
        {
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Scalar;
            this.Value = Convert.ToString(Value);
        }
        public Scalar(string Name, string Unit)
            : base(Name, Unit)
        {
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Scalar;
            this.Value = "";
        }

        override public string GetValue()
        {
            string result = this.Value;
            return result;
        }
        override public void SetValue(string val)
        {
            this.Value = val;
        }
    }

    class Link : LPDP_Object
    {
        int Value;
        public Link(string Name, string Unit/*, int ID*/)
            : base(Name, Unit)
        {
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Link;
            this.Value = -2;//ID;
        }

        override public string GetValue()
        {
            string result;
            if
                (this.Value == -2) result = "---";
            else
                result = Convert.ToString(this.Value);
            return result;
        }
        override public void SetValue(int val)
        {
            this.Value = val;
        }
    }

    class Vector : LPDP_Object
    {
        public List<LPDP_Object> Value = new List<LPDP_Object>();

        public Vector(string Name, string Unit, string Tree)
            : base(Name, Unit)
        {
            Tree = Tree.Replace(" ", "");
            base.Name = Name;
            base.Unit = Unit;
            base.Type = LPDP_Object.ObjectType.Vector;

            int i = 0;
            while (Tree.IndexOf('-') != -1)
            {
                if (Tree.Substring(Tree.IndexOf('-') + 1, 6) == "скаляр")
                {
                    Value.Add(new Scalar(Name + "." + Convert.ToString(i + 1), Unit));
                }

                int end_flag = 0;
                int j = 0;
                if (Tree.Substring(Tree.IndexOf('-') + 1, 6) == "вектор")
                {
                    //определение индекса конца подстроки
                    int count = 0;
                    for (j = 0; j < Tree.Length; j++)
                    {
                        if (Tree[j] == '(')
                            count++;
                        if ((Tree[j] == ')') && (count == 1))
                            break;
                        if ((Tree[j] == ')') && (count > 1))
                            count--;
                    }

                    string SubTree = Tree.Substring(Tree.IndexOf('(') + 1, j - Tree.IndexOf('(') - 1);
                    Value.Add(new Vector(Name + "." + Convert.ToString(i + 1), Unit, SubTree));
                }
                i++;
                end_flag = Tree.IndexOf(',', j);
                if (end_flag == -1) Tree = "";
                else Tree = Tree.Substring(end_flag + 1);
            }
        }
        
        override public string GetValue(string Tree)
        {
            string result = "\"Ошибка!\"";
            if (Tree == "") return "пустое древо";
            //определение подстроки для индекса
            int len = Tree.IndexOf('(');
            if (len == -1) len = Tree.Length;
            int index = Convert.ToInt32(Tree.Substring(0, len)) - 1;
            if (index >= this.Value.Count) return "\"Ошибка! Элемента не существует! (на указанном уровне нет этого индекса)\"";

            if (this.Value[index].GetType() == typeof(Vector))
            {
                string SubTree = Tree.Substring(Tree.IndexOf('(') + 1, Tree.LastIndexOf(')') - Tree.IndexOf('(') - 1);
                result = this.Value[index].GetValue(SubTree);
            }
            if (this.Value[index].GetType() == typeof(Scalar))
            {
                if (Tree.Any<char>(c => ((c == '(')) || (c == ')'))) return "\"Ошибка! Элемента не существует!(элемент на указанном уровне не является вектором)\"";
                result = this.Value[index].GetValue();
            }
            return result;
        }

        override public string GetValue()
        {
            string result = "(";
            for (int i = 0; i < this.Value.Count; i++)
            {
                if (this.Value[i].Type == ObjectType.Vector)
                {
                    Vector NewVector = (Vector)this.Value[i];
                    result += NewVector.GetValue();
                }
                else
                    result += this.Value[i].GetValue();

                if (i < this.Value.Count - 1) // если не последний
                    result += "; ";
            }
            result += ")";
            return result;
        }

        override public void SetValue(string Tree, string val)
        {
            //определение подстроки для индекса
            int len = Tree.IndexOf('(');
            if (len == -1) len = Tree.Length;
            int index = Convert.ToInt32(Tree.Substring(0, len)) - 1;

            if (this.Value[index].GetType() == typeof(Vector))
            {
                string SubTree = Tree.Substring(Tree.IndexOf('(') + 1, Tree.LastIndexOf(')') - Tree.IndexOf('(') - 1);
                this.Value[index].SetValue(SubTree, val);
            }
            if (this.Value[index].GetType() == typeof(Scalar))
            {
                this.Value[index].SetValue(val);
            }
        }

        public string GetTree()
        {
            string result = "(";
            for (int i = 0; i < this.Value.Count; i++)
            {
                if (this.Value[i].Type == ObjectType.Vector)
                {
                    Vector NewVector = (Vector)this.Value[i];
                    result += NewVector.GetTree();
                }
                else
                    result += this.Value[i].Name.Substring(this.Value[i].Name.LastIndexOf('.') + 1);

                if (i < this.Value.Count - 1) // если не последний
                    result += ", ";
            }
            result += ")";
            return result;
        }
    }

    class LocalArea
    {
        public int ID;
        public LPDP_Object Value;

        public LocalArea(LPDP_Object obj)
        {
            int id;
            if (LPDP_Core.LocalArea_Table.Count > 0)
                id = LPDP_Core.LocalArea_Table[LPDP_Core.LocalArea_Table.Count - 1].ID + 1;
            else
                id = 0;

            this.ID = id;
            this.Value = obj;
        }
    }
}
