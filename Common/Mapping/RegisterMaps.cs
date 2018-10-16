using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Mapping
{
    public class RegisterMaps
    {
        public RegisterMaps(TypeAdapterConfig config)
        {
            new AmoCRMtoDomain(config);
            new DomainTo1C( config );
        }
    }
}
