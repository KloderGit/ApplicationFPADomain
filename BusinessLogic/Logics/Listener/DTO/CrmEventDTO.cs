using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.DTO
{
    public class CrmEventDTO
    {
        public string Event { get; set; }
        public List<CrmEventEntityBase> Entities { get; set; }
    }
}
