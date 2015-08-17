using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DataTransferApp
{
    [BsonIgnoreExtraElements]
    public class IAData
    {
        public object _id { get; set; }
        public int IAID { get; set; }
        public string Reference { get; set; }
        public int ParentIAID { get; set; }
    }
}
