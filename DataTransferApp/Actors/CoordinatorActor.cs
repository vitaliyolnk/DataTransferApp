using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferApp.Actors
{
    public class CoordinatorActor : ReceiveActor
    {
        IActorRef _processorActor;

        protected override void PreStart()
        {
            _processorActor = Context.ActorOf(Props.Create(() =>
                new ProcessorActor()).WithRouter(new RandomPool(10)));
        }

        public CoordinatorActor()
        {
            Receive<IAData>(w => w != null, w =>
            {
                _processorActor.Tell(w);
            });
        }
    }
}
