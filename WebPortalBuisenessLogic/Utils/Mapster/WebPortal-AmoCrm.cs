using Domain.Models.Crm;
using LibraryAmoCRM.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebPortalBuisenessLogic.Models.Crm;

namespace WebPortalBuisenessLogic.Utils.Mapster
{
    public class WebPortal_AmoCrm : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ContactDTO, Contact>()
                .Ignore(x => x.Tags)
                .Map(dest => dest.Leads, src => src.Leads.IDs != null ? src.Leads.IDs.Select(c => new Lead { Id = c }) : null)
                .Map(dest => dest.Company, src => src.Company.Id != null ? new Company { Id = src.Company.Id.Value } : null)
                .Map(dest => dest.Fields, src => src.CustomFields != null ? src.CustomFields : null)
           ;

            config.NewConfig<LeadDTO, Lead>()
                .Map(dest => dest.Company, src => src.Company!= null ? new Company { Id = (int)src.Company.Id } : null)
                .Map(dest => dest.Contacts, src => src.Contacts != null ? src.Contacts.IDs.Select(c => new Contact { Id = c }) : null)
                .Map(dest => dest.MainContact, src => src.MainContact != null ? new Contact { Id = (int)src.MainContact.Id } : null)
                .Map(dest => dest.CustomFields, src => src.CustomFields != null ? src.CustomFields : null)
            ;

            config.NewConfig<Lead, UpdateFormDTO>()
              .IgnoreNullValues(true)
              .Map(dest => dest.LeadId, src => src.Id)
              .Map(dest => dest.ContactId, src => src.MainContact.Id)
              .Map(dest => dest.Name, src => src.MainContact != null ? src.MainContact.Name : null)
              .Map(dest => dest.Phone, src => src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54667) != null ?
                                                    src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54667).Values.FirstOrDefault().Value : null)
              .Map(dest => dest.Email, src => src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54669) != null ?
                                                    src.MainContact.Fields.FirstOrDefault(pr => pr.Id == 54669).Values.FirstOrDefault().Value : null)
              ;
        }
    }
}
