using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.Mongo
{
    public class LogService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string LogType { get; set; }

        public string AppName { get; set; }

        public string AppType { get; set; }

        public string IP { get; set; }

        public string User { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}