using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.Service1C
{
    public class SendLeadto1CDTo : IValidatableObject
    {
        public string ProgramGuid { get; set; } = "";
        public string UserGuid { get; set; } = "";
        public string ContractTitle { get; set; } = "";
        public int ContractPrice { get; set; } = 0;
        public string ContractGroup { get; set; } = "";
        public string ContractSubGroup { get; set; } = "";
        public DateTime ContractExpire { get; set; }
        public DateTime ContractEducationStart { get; set; }
        public DateTime ContractEducationEnd { get; set; }
        public string DecreeTitle { get; set; } = "";


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.ProgramGuid))
                errors.Add(new ValidationResult("Не указан Guid мероприятия."));

            if (string.IsNullOrWhiteSpace(this.UserGuid))
                errors.Add(new ValidationResult("Не указан Guid пользователя."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указано название договора."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указана группа обучния."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указана дата начала обучения."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указана дата окончания обучения."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указана дата дейсвия договора."));

            if (string.IsNullOrWhiteSpace(this.ContractTitle))
                errors.Add(new ValidationResult("Не указано название приказа."));

            return errors;
        }

        public IEnumerable<ValidationResult> GetValidateErrors()
        {
            var validateModelResults = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (!Validator.TryValidateObject(this, context, validateModelResults, true))
            {
                return validateModelResults;
            }

            return null;
        }

        public bool isValid
        {
            get { return GetValidateErrors() != null ? false : true; }
        }


    }
}
