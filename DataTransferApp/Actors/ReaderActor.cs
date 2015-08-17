using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferApp.Actors
{
    public class ReaderActor : ReceiveActor
    {
        public const string ReadMessage = "readall";
        public const string ProcessAll = "processall";

        DataAccess source = DataAccess.Instance;
        IEnumerable<IAData> dataToProcess;
        IActorRef _coordinator;

        public ReaderActor()
        {
            Receive<string>(s => string.Equals(s, ReadMessage), s =>
            {
                dataToProcess = source.ReadAll();
            });

            Receive<string>(s => string.Equals(s, ProcessAll), s =>
            {
                foreach (var item in dataToProcess)
                {
                    _coordinator.Tell(item);
                }
              
            });
        }

        protected override void PreStart()
        {
            var c1 = Context.ActorOf(Props.Create(() => new CoordinatorActor()), "CoordinatorActor");

            _coordinator = Context.ActorOf(Props.Empty.WithRouter(new BroadcastGroup(new IActorRef[]{c1})));
            base.PreStart();
        }

        private IEnumerable<IEnumerable<IAData>> Split()
        {
            if (dataToProcess != null)
            {
                return dataToProcess.Batch(10);
            }

            return null;
        }

    }
}
