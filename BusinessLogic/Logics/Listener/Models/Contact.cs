using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiBusinessLogic.Logics.Listener.Models
{
    public class Contact : Domain.Models.Crm.Contact
    {
        public int OldResponsibleUserId { get; set; }
    }
}
