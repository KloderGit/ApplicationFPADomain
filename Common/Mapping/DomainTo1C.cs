using Common.DTO.Service1C;
using Common.Extensions;
using Common.Extensions.Models.Crm;
using Domain.Models.Crm;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mapping
{
    public class DomainTo1C
    {
        public DomainTo1C(TypeAdapterConfig config)
        {
            config.NewConfig<Contact, SendPersonTo1CDTO>()
                .Map(dest => dest.FIO, src => src.Name)
                .Map(
                    dest => dest.Phone, 
                    src => src.Phones() != null ? src.Phones().FirstOrDefault().Value.LeaveJustDigits() : "")
                .Map(
                    dest => dest.Email,
                    src => src.Email() != null ? src.Email().FirstOrDefault().Value.ClearEmail() : "")
                .Map(dest => dest.City, src => src.City() ?? "")
                .Map(dest => dest.Address, src => src.Location() ?? "")
                .Map(dest => dest.Education, src => src.Education() ?? "")
                .Map(dest => dest.Expirience, src => src.Experience() ?? "")
                .Map(dest => dest.Position, src => src.Position() ?? "")
                .Map(dest => dest.BirthDay, src => src.Birthday() ?? DateTime.MinValue)
            ;

        }
    }
}