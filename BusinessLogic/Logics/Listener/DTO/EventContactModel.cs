using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class EventContactModel : CrmEventEntityBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "modified_user_id")]
        public int ModifiedUserId { get; set; }

        [JsonProperty(PropertyName = "custom_fields")]
        public IEnumerable<CustomField> CustomFields { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<Tag> Tags { get; set; }

        [JsonProperty(PropertyName = "linked_leads_id")]
        public IEnumerable<LinkedLeads> LinkedLeads { get; set; }
    }
}
