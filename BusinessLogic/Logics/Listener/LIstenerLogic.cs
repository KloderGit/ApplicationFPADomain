using System;
using System.Collections.Generic;
using System.Text;
using WebApiBusinessLogic.Logics.Listener.DTO;

namespace WebApiBusinessLogic.Logics.Listener
{
    public class LIstenerLogic
    {
        public void EventsHandle(IEnumerable<CrmEventDTO> crmEvents)
        {
            foreach (var @event in crmEvents)
            {
                Console.WriteLine(@event.Event);
            }
        }
    }
}
