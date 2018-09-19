using Common.DTO.Service1C;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Mapping
{
    public class DomainTo1C
    {
        public DomainTo1C(TypeAdapterConfig config)
        {
            config.NewConfig<Contact, CreateUserDTO>()
                .IgnoreNullValues(true)
                .Map(dest => dest.FIO, src => src.Name)
                .Map(dest => dest.Address, source: src => src.Location() ?? "")

            ;

        }
    }
}
