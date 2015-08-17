using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using DataTransferApp.Actors;

namespace DataTransferApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //var result = DataAccess.Instance.ReadAll();
            var dataTransferSystem = ActorSystem.Create("DataTransferSystem");

            var reader = dataTransferSystem.ActorOf(Props.Create(() => new ReaderActor()));

            reader.Tell(ReaderActor.ReadMessage);
            reader.Tell(ReaderActor.ProcessAll);


            dataTransferSystem.AwaitTermination();
        }
    }
}
