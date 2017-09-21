using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPDP.Objects
{
    public class ObjectController
    {
        
        //int Initiator_Number_Counter;
        LPDP.Structure.Model ParentModel;
        Object CurrentObject;

        public ObjectController(LPDP.Structure.Model model)
        {
            //this.ID_Object_Counter = 0;
            //this.Initiator_Number_Counter = 0;
            this.ParentModel = model;
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

        public void AddObject(Object obj)
        {
            this.ParentModel.Memory.Add(obj);

            if (obj.Type == ObjectType.Macro)
            {
                this.ParentModel.MT.Add((Macro)obj);
            }
            else
            {
                this.ParentModel.GVT.AddToUnit(obj, this.ParentModel.ST_Cont.CurrentUnit.Name);
            }
        }
    }
}
