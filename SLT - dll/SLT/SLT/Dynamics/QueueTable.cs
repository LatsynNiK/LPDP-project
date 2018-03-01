using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SLT.Structure;

namespace SLT.Dynamics
{
    public class QueueTable
    {
        public List<Queue> Queues;
        Executor ParentExecutor;

        public QueueTable(Executor exec)
        {
            this.Queues = new List<Queue>();
            this.ParentExecutor = exec;
            foreach (Subprogram subp in this.ParentExecutor.ParentModel.ST_Cont.Tracks)
            {
                Queue q = new Queue(subp);
                this.Queues.Add(q);
            }
        }

        public void Update(SLT.Objects.Initiator init)
        {
            foreach (Queue q in this.Queues)
            {
                q.Items.Clear();
            }

            foreach (RecordCT rec in ParentExecutor.TC_Cont.ConditionsTable) 
            {
                this.ParentExecutor.SetInitiator(rec.Initiator);
                bool right_cond = this.ParentExecutor.ComputeLogicExpression(rec.Condition);
                if (right_cond)
                {
                    Queue que = this.Queues.Find(q => q.Place == rec.Subprogram); 
                    que.Add(rec.Initiator, Queue.DelayType.Ready);
                }
            }

            FutureTimesTable FTT = new FutureTimesTable();
            FTT.TimesTable = ParentExecutor.TC_Cont.FutureTimesTable.ToList();

            while (FTT.TimesTable.Count>0)
            {
                RecordFTT rec = FTT.FindNextMinTimeRecord();
                this.ParentExecutor.SetInitiator(rec.Initiator);
                Queue que = this.Queues.Find(q => q.Place == rec.Subprogram);
                que.Add(rec.Initiator, Queue.DelayType.WaitTime);
                FTT.TimesTable.Remove(rec);
            }

            foreach (RecordCT rec in ParentExecutor.TC_Cont.ConditionsTable) 
            {
                this.ParentExecutor.SetInitiator(rec.Initiator);
                bool right_cond = this.ParentExecutor.ComputeLogicExpression(rec.Condition);
                if (right_cond == false)
                {
                    Queue que = this.Queues.Find(q => q.Place == rec.Subprogram); 
                    que.Add(rec.Initiator, Queue.DelayType.Stopped);
                }
            }
            //для сохранения состояния
            this.ParentExecutor.SetInitiator(init);
        }
    }
}
