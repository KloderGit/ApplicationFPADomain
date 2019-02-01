using Common.Converters;
using Newtonsoft.Json;
using System;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class CrmEventEntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "responsible_user_id")]
        public int ResponsibleUserId { get; set; }

        [JsonProperty(PropertyName = "old_responsible_user_id")]
        public int OldResponsibleUserId { get; set; }

        [JsonConverter(typeof(UnixTimeStampToDateTime))]
        [JsonProperty(PropertyName = "date_create")]
        public DateTime DateCreate { get; set; }

        [JsonConverter(typeof(UnixTimeStampToDateTime))]
        [JsonProperty(PropertyName = "last_modified")]
        public DateTime LastModified { get; set; }

        [JsonProperty(PropertyName = "created_user_id")]
        public int CreatedUserId { get; set; }

        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }
    }
}
