using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LPDP.Structure;
using LPDP.Objects;

namespace LPDP.Dynamics
{
    public class Queue
    {
        public enum DelayType
        {
            Ready,
            WaitTime,
            Stopped
        }
        public struct QueueItem
        {
            public Initiator Initiator;
            public DelayType Delay;
            public QueueItem(Initiator init, DelayType delay)
            {
                this.Initiator = init;
                this.Delay = delay;
            }
        }
        public Subprogram Place;
        public List<QueueItem> Items;

        //[FlagsAttribute]
        public enum ArrowType
        {
            None = 0,
            Full = 10,
            Half = 20,
            Ready = 1,
            WaitTime = 2,
            Stopped = 3,
            Several = 100,
        }

        public Queue(Subprogram subp)
        {
            this.Place = subp;
            this.Items = new List<QueueItem>();
        }

        public void Add(Initiator init, DelayType delay)
        {
            this.Items.Add(new QueueItem(init, delay));
        }
    }
}
