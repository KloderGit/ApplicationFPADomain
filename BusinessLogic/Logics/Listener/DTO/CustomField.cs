using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class CustomField
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonConverter(typeof(IntOrArrayJsonConverter))]
        [JsonProperty(PropertyName = "values")]
        public IEnumerable<Field> Values { get; set; }
    }
}
