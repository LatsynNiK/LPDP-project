using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.TextAnalysis;

namespace LPDP.Objects
{
    public class ObjectController
    {
        
        //int Initiator_Number_Counter;
        LPDP.Structure.Model ParentModel;

        public InitiatorsTable IT;
        public GlobalVarsTable GVT;
        Object CurrentObject;

        public ObjectController(LPDP.Structure.Model model)
        {
            //this.ID_Object_Counter = 0;
            //this.Initiator_Number_Counter = 0;
            this.ParentModel = model;
            this.IT = new InitiatorsTable();
            this.GVT = new GlobalVarsTable();
        }

        //public void CreateObject(ObjectType type)
        //{
        //    Object NewObject;
        //    switch (type)
        //    {
        //        //case ObjectType.Scalar:
        //        //    NewObject = new Scalar(this.ID_Object_Counter);
        //        //    break;
        //        //case ObjectType.Link:
        //        //    NewObject = new Link(this.ID_Object_Counter);
        //        //    break;
        //        //case ObjectType.Vector:
        //        //    NewObject = new Vector(this.ID_Object_Counter);
        //        //    break;
        //        //case ObjectType.Macro:
        //        //    NewObject = new Macro(this.ID_Object_Counter);
        //        //    break;
        //        //default:
        //        //    NewObject = new Scalar(this.ID_Object_Counter);
        //        //    break;
        //    }
        //    //this.ID_Object_Counter++;

        //    //Object NewObject = new Object(ID_Object_Counter);
        //    //this.ParentModel.Units.Add(NewObject);
        //    //this.CurrentObject = NewObject;

        //}

        public void AddObject(Object obj, string unit)
        {
            obj = this.ParentModel.Memory.AddSingletonObject(obj);

            if (obj.Type == ObjectType.Macro)
            {
                this.ParentModel.MT.Add((Macro)obj);
            }
            else
            {
                this.GVT.AddToUnit(obj, unit);
            }
        }

        public void DeleteObjectByID(int id)
        {
            this.ParentModel.Memory.DeleteObject(id);
            this.GVT.DeleteObjectByID(id);
        }
        public void DeleteInitiatorByID(int id)
        {
            this.ParentModel.Memory.DeleteObject(id);
            this.IT.Delete(id);
        }

        public void SetValueToScalar(string name, string unit, object value) 
        {
            LPDP.Objects.Scalar updated_scalar = (Scalar)this.GVT.Vars[unit].Find(o => o.Name == name);
            updated_scalar.SetValue(value);
        }
        public void SetValueToVector(Phrase path, string unit, object value) 
        {
            string vector_name = ((Word)(path.Value[0])).LValue;
            Phrase inner_node = path.Value[2];
            Vector parent_vector = (Vector)this.GVT.Vars[unit].Find(v => v.Name == vector_name);

            //LPDP.Objects.Scalar updated_scalar = (Scalar)this.ParentModel.GVT.Vars[unit].Find(o => o.Name == name);

            LPDP.Objects.Object updated_obj = parent_vector.FindNode(inner_node);//Scalar or Link
            updated_obj.SetValue(value);
        }

//GET
        public LPDP.Objects.Object GetObjectByID(int id)
        {
            //LPDP.Objects.Link finded_link = (Link)this.ParentModel.GVT.Vars[unit].Find(o => o.Name == name);
            LPDP.Objects.Object result = this.ParentModel.Memory.GetObjectByID(id);

            return result;
        }

        public LPDP.Objects.Object GetObjectByName(string name, string unit)
        {
            return this.GVT.Vars[unit].Find(o => o.Name == name);
        }

        public int GetLinkValue(string link_name, string link_unit)
        {
            LPDP.Objects.Link link = (Link)this.GVT.Vars[link_unit].Find(l => l.Name == link_name);
            int result = (int)link.GetValue();
            return result;
        }

        //public Scalar GetScalar(string name, string unit)
        //{
        //    return (Scalar)this.GVT.Vars[unit].Find(o => o.Name == name);
        //}
        //public Link GetLink(string name, string unit)
        //{
        //    return (Link)this.GVT.Vars[unit].Find(o => o.Name == name);
        //}

        public Object GetVectorNode(Phrase path, string unit)
        {
            Object result;
            string vector_name = ((Word)(path.Value[0])).LValue;
            Phrase inner_node = path.Value[2];
            Vector parent_vector = (Vector)this.GVT.Vars[unit].Find(v => v.Name == vector_name);

            //LPDP.Objects.Scalar updated_scalar = (Scalar)this.ParentModel.GVT.Vars[unit].Find(o => o.Name == name);

            result = parent_vector.FindNode(inner_node);//Scalar or Link
            return result;
            //updated_obj.SetValue(value);
        }




        //INITIAORS
        public Initiator CreateInitiator(string unit_name)
        {
            Initiator init = new Initiator(InitiatorType.Aggregate);

            Object obj = new Scalar ("Инициатор блока",unit_name,unit_name);
            obj = this.ParentModel.Memory.AddSingletonObject(obj);
            init.ID_of_MemoryCell = obj.ID;
            this.IT.Add(init);
            return init;
        }

        public Initiator ActivateFromLink(string link_name, string link_unit)
        {
            Initiator init = new Initiator(InitiatorType.Flow);

            Object obj = GetObjectFromLink(link_name,link_unit);
            //obj = this.ParentModel.Memory.AddSingletonObject(obj);
            init.ID_of_MemoryCell = obj.ID;
            this.IT.Add(init);
            return init;
        }

        public Object GetObjectFromInitiator(Initiator init)
        {
            int cell_id = init.ID_of_MemoryCell;
            Object finded_obj = this.ParentModel.Memory.GetObjectByID(cell_id);
            return finded_obj;
        }

        public Object GetObjectFromLink(string link_name, string link_unit)
        {
            int link_value = GetLinkValue(link_name, link_unit);
            Object result = GetObjectByID(link_value);
            return result;
        }

        //public Vector FindVector
    }
}
