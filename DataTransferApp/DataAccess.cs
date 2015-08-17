using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace DataTransferApp
{
    public class DataAccess
    {
        private MongoServer server;
        private MongoServer writeServer;
        private MongoDatabase database;
        private MongoDatabase writeDatabase;
        private MongoCollection<IAData> readColl;
        private MongoCollection<IAData> writeColl;

        private static DataAccess instance;

        private DataAccess()
        {
            Initialise();
        }

        private void Initialise()
        {
            MongoServerSettings settings = MongoServerSettings.FromUrl(new MongoUrl("mongodb://localhost:27017"));
            server = new MongoServer(settings);
            MongoServerSettings writeSettings = MongoServerSettings.FromUrl(new MongoUrl("mongodb://localhost:27017"));
            writeServer = new MongoServer(writeSettings);

            var dataSettings = new MongoDatabaseSettings();
            database = new MongoDatabase(server, "iadata20110409", dataSettings);
            var dataWriteSettings = new MongoDatabaseSettings();
            dataWriteSettings.WriteConcern = WriteConcern.Unacknowledged;
            writeDatabase = new MongoDatabase(server, "citablerefs", dataWriteSettings);

            readColl = database.GetCollection<IAData>("InformationAsset");
            writeColl = writeDatabase.GetCollection<IAData>("CitableRefs");
        }

        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataAccess();
                }
                return instance;
            }
        }

        public IAData Read(string iaid)
        {
            var query = Query.EQ("IAID", iaid);
            var result = readColl.FindOneAs<IAData>(query);

            return result;
        }

        public bool Save(IAData data)
        {
            if (data == null) throw new NullReferenceException("data");

            if (writeColl.FindOne(Query.EQ("_id", ObjectId.Parse(data._id.ToString()))) != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exists::::::::::::::::::::::::::" + data.IAID);
                Console.ResetColor();
            }

            var result = writeColl.Insert(data);

            return true;
        }

        public bool SaveBatch(List<IAData> data)
        {
            if (data == null) throw new NullReferenceException("data");

            var result = writeColl.InsertBatch(data);
            
            return true;
        }

        public IEnumerable<IAData> ReadAll()
        {
            var result = readColl.FindAll().AsEnumerable();
            return result;
        }
    }
}
