using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Models.Crm
{
    public class ChangedParam
    {
        public string @Event { get; set; }
        public string OldValue { get; set; }
        public string CurrentValue { get; set; }
    }
}
