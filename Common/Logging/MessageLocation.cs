using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging
{
    public class MessageLocation
    {
        public string Assambly { get; set; }
        public string Controller { get; set; }
        public string Metod { get; set; }
        public string Route { get; set; }

        public MessageLocation(Object controller)
        {
            Assambly = controller.GetType().Assembly.FullName;
            Controller = controller.GetType().Name;
        }
    }
}
