using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferApp.Actors
{
    public class ProcessorActor : ReceiveActor
    {
        List<IAData> list = new List<IAData>();

        IActorRef writer;
        
        public ProcessorActor()
        {
            Receive<IAData>(d => d != null, d =>
                {
                    //ProcessMessage(d);
                    ProcessBatch(d);
                }
                );
        }

        private void ProcessBatch(IAData d)
        {
            list.Add(d);
            if (list.Count == 1000)
            {
                List<IAData> tosend = new List<IAData>(list);
                writer.Ask(tosend);
                list.Clear();
            }

            if(list.Count>1000)
            {
                Console.WriteLine("List count: " + list.Count);
            }
        }

        private void ProcessMessage(IAData data)
        {
            writer.Tell(data);
        }

        protected override void PreStart()
        {
            writer = Context.ActorOf(Props.Create(() => new WriterActor()));
            base.PreStart();
        }
    }
}
