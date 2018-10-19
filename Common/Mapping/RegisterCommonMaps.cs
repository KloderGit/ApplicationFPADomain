using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Mapping
{
    public class RegisterCommonMaps
    {
        public RegisterCommonMaps(TypeAdapterConfig config)
        {
            new Domain_AmoCRM( config);
            new Domain_1C( config );
            new Domain_Neo( config );
        }
    }
}
