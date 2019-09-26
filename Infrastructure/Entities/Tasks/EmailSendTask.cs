using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Infrastructure.Entities.Tasks
{
    public class EmailSendTask : MREntity, IMREntity
    {
        [BsonRepresentation(BsonType.String)]
        public EmailTaskBot Bot { get; set; }

        [BsonRepresentation(BsonType.String)]
        public EmailSendStatus Status { get; set; } = EmailSendStatus.New;

        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string FailMessage { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))] 
    public enum EmailTaskBot
    {
        MadRatBot = 0
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailSendStatus
    {
        New = 0,
        Canceled,
        InProgress,
        Delivered,
        Failed
    }
}
