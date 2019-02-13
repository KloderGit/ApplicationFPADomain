using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiBusinessLogic.Logics.Listener.DTO;

namespace WebApiBusinessLogic.Logics.Listener
{
    public class ListenerLogic
    {
        CrmEventHandler events = new CrmEventHandler();

        public ListenerLogic()
        {

        }

        public void EventsHandle(IEnumerable<EventDTO> crmEvents)
        {

            
        }

    }
}
