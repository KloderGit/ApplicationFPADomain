using Mapster;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebPortalBuisenessLogic.Utils.Mapster
{
    public class RegisterMapsterConfig
    {
        public RegisterMapsterConfig()
        {
            Assembly AmoCrm = typeof(WebPortal_AmoCrm).GetTypeInfo().Assembly;

            TypeAdapterConfig.GlobalSettings.Scan(AmoCrm);
        }
    }
}
