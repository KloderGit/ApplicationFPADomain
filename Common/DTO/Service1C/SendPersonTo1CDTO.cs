using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTO.Service1C
{
    public class SendPersonTo1CDTO : IValidatableObject
    {
        public string FIO { get; set; } = "";
        public string City { get; set; } = "";
        public string Email { get; set; } = "";
        public string Position { get; set; } = "";
        public DateTime BirthDay { get; set; } = DateTime.MinValue;
        public string Education { get; set; } = "";
        public string Expirience { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.FIO))
                errors.Add(new ValidationResult("Не указано имя пользователя"));

            if ((this.Phone.Trim() + this.Email.Trim()).Length == 0 )
                errors.Add(new ValidationResult("Для внесения пользователя в 1С нужно указать email или телефон"));

            return errors;
        }

        public IEnumerable<ValidationResult> GetValidateErrors()
        {
            var validateModelResults = new List<ValidationResult>();
            var context = new ValidationContext(validateModelResults);
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