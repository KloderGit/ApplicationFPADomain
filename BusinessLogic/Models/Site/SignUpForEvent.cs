using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Models.Site
{
    public class SignUpForEvent
    {
        public string ContactName { get; set; }
        public IEnumerable<string> ContactPhones { get; set; }
        public IEnumerable<string> ContactEmails { get; set; }
        public string ContactCity { get; set; }
        public string LeadName { get; set; }
        public DateTime? LeadDate { get; set; }
        public string LeadType { get; set; }
        public string EventType { get; set; }
        public int LeadPrice { get; set; }
        public string LeadGuid { get; set; }
    }
}
