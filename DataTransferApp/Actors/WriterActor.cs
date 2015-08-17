using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferApp.Actors
{
    public class WriterActor : ReceiveActor
    {
        DataAccess target;
        private int count;
        public WriterActor()
        {
            target = DataAccess.Instance;
            Receive<IAData>(w => w != null, w =>
            {
                Save(w);
            });

            Receive<List<IAData>>(w => w != null, w =>
            {
                SaveBatch(w);
            });
        }

        private void Save(IAData tosave)
        {
            //Console.WriteLine(count++);
            target.Save(tosave);
        }

        private void SaveBatch(List<IAData> tosave)
        {
            Console.WriteLine(count++);
            target.SaveBatch(tosave);
        }
    }
}
