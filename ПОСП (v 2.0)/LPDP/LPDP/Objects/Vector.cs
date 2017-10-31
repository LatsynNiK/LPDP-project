using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Objects
{
    class Vector:Object
    {
        List<Object> Value;

        public Vector(string name, string unit, List<Object> value)
            : base(name, unit)
        {
            base.Type = ObjectType.Vector;
            this.Value = value;
        }

        public Vector(int id, string name, string unit)
            : base(id, name, unit)
        {
            base.Type = ObjectType.Vector;
            this.Value = new List<Object>();
        }

        public Vector(int id)
            : base(id)
        {
            base.Type = ObjectType.Vector;
            this.Value = new List<Object>();
        }
        ////
        public override void SetValue(object node_and_value)
        {
            //KeyValuePair<Phrase, object> node_and_value_pair = (KeyValuePair<Phrase, object>)node_and_value;
            //Phrase node = node_and_value_pair.Key;
            //object value = node_and_value_pair.Value;

            //string name = 

            //if (path.Value.Exists(ph => ph.PhType == PhraseType.VectorNode))
            //{
            //    Vector inner_vector = 

            //    Phrase inner_path = path.Value.Find(ph => ph.PhType == PhraseType.VectorNode);
            //    this.Value.
            //}
            ////this.Value = Convert.ToString(value);
        }

        public override List<Object> GetValue()
        {
            return this.Value;
        }

        public LPDP.Objects.Object FindNode(Phrase path)
        {
            LPDP.Objects.Object finded_node;
            if (path.PhType == PhraseType.Name)
            {
                string node_name = ((Word)path).LValue;
                finded_node = this.Value.Find(obj => obj.Name == node_name);
            }
            else
            {
                Phrase inner_node = path.Value[2];
                finded_node = this.FindNode(inner_node);
            }
            return finded_node;
        }

    }
}
