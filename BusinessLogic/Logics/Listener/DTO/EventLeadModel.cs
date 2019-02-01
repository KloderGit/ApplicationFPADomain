using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class EventLeadModel : CrmEventEntityBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "modified_user_id")]
        public int ModifiedUserId { get; set; }

        [JsonProperty(PropertyName = "status_id")]
        public int StatusId { get; set; }

        [JsonProperty(PropertyName = "old_status_id")]
        public int OldStatusId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }

        [JsonProperty(PropertyName = "pipeline_id")]
        public int PipelineId { get; set; }

        [JsonProperty(PropertyName = "custom_fields")]
        public IEnumerable<CustomField> CustomFields { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<Tag> Tags { get; set; }
    }
}
