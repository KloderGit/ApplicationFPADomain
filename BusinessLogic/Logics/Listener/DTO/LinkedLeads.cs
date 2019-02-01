using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class LinkedLeads
    {
        [JsonProperty(PropertyName = "ID")]
        public string Id { get; set; }
    }
}
