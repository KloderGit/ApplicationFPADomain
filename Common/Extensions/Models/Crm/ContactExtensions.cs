using Domain.Models.Crm;
using Common.Configuration.Crm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryAmoCRM.Configuration;
using Domain.Models.Crm.Fields;
using System.Globalization;

namespace Common.Extensions.Models.Crm
{
    public static class ContactExtensions
    {
        public static string Position(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Position))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Position).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Position(contact.Position()); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Position, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Position(contact.Position()); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Position).Values.FirstOrDefault().Value;
        }

        public static Dictionary<PhoneTypeEnum, string> Phones(this Contact contact, Dictionary<PhoneTypeEnum, string> value = null )
        {
            if (value != null)
            {
                if (contact.Fields == null) contact.Fields = new List<Field>();

                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Phone))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values = new List<FieldValue>();
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Phone, Values = new List<FieldValue>() });
                }

                foreach (var item in value)
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.Add(new FieldValue { Enum = (int)item.Key, Value = item.Value.LeaveJustDigits() });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Phones(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone)?.Values
                .ToDictionary(k => (PhoneTypeEnum)k.Enum.Value, v => v.Value);
        }

        public static Dictionary<PhoneTypeEnum, string> Phones(this Contact contact, PhoneTypeEnum type, string value)
        {
            if (value != null)
            {
                if (contact.Fields == null) contact.Fields = new List<Field>();
                if (!contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Phone))
                {
                    type = type == PhoneTypeEnum.NotSet ? PhoneTypeEnum.MOB : type;
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Phone, Values = new List<FieldValue> { new FieldValue { Enum = (int)type, Value = value.LeaveJustDigits() } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Phones(type,value); };
                }
                else
                {
                    var current = contact.Phones();
                    if (current.Any(x => x.Key == type))
                    {
                        current[type] = value.LeaveJustDigits();
                    }
                    else
                    {
                        current.Add(type, value.LeaveJustDigits());
                    }
                    contact.Phones(current);
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone)?.Values
                .ToDictionary(k => (PhoneTypeEnum)k.Enum.Value, v => v.Value);
        }

        public static Dictionary<EmailTypeEnum, string> Email(this Contact contact, EmailTypeEnum type = EmailTypeEnum.NotSet, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Email))
                {
                    if (type == EmailTypeEnum.NotSet)
                    {
                            var enumEmailTypes = Enum.GetValues(typeof(EmailTypeEnum)).Cast<EmailTypeEnum>().Select(i => (int)i);
                            var emailTypes = enumEmailTypes.Except(contact.Email().Select(e => (int)e.Key)).ToList();
                            emailTypes.Remove((int)EmailTypeEnum.NotSet);

                            type = (EmailTypeEnum)emailTypes.FirstOrDefault();

                        contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Add(new FieldValue { Enum = (int)type, Value = value.ClearEmail() });
                        contact.ChangeValueDelegate += delegate (Contact x) { x.Email(type, value.ClearEmail()); };
                    }
                    else
                    {
                        contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Add(new FieldValue { Enum = (int)type, Value = value });
                        contact.ChangeValueDelegate += delegate (Contact x) { x.Email(EmailTypeEnum.PRIV, value.ClearEmail()); };
                    }
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Email, Values = new List<FieldValue> { new FieldValue { Enum = (int)EmailTypeEnum.PRIV, Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Email(EmailTypeEnum.PRIV, value.ClearEmail()); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values
                .ToDictionary(k => (EmailTypeEnum)k.Enum.Value, v => v.Value);
        }

        public static Dictionary<MessengerTypeEnum, string> Messenger(this Contact contact, MessengerTypeEnum type = MessengerTypeEnum.NotSet, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Messanger))
                {
                    var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Messanger).Values.Any(x => x.Value.Replace(" ", "") == value.Replace(" ", ""));

                    if (!isAviable)
                    {
                        if (type == MessengerTypeEnum.NotSet) throw new ArgumentException("Не указан тип мессенжера");
                        contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Messanger).Values.Add(new FieldValue { Enum = (int)type, Value = value });
                        contact.ChangeValueDelegate += delegate (Contact x) { x.Messenger(type, value.Replace(" ", "")); };
                    }
                }
                else
                {
                    if (type == MessengerTypeEnum.NotSet) throw new ArgumentException("Не указан тип мессенжера");
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Messanger, Values = new List<FieldValue> { new FieldValue { Enum = (int)type, Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Messenger(type, value.Replace(" ", "")); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Messanger).Values
                .ToDictionary(k => (MessengerTypeEnum)k.Enum.Value, v => v.Value);
        }

        public static string City(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.City))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.City).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.City(contact.City()); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.City, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.City(contact.City()); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.City).Values.FirstOrDefault().Value;
        }

        public static bool? MailChimp(this Contact contact, bool? value = null)
        {
            if (value.HasValue && value != null)
            {
                var digit = value.Value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.MailChimp))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp).Values.FirstOrDefault().Value = digit;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.MailChimp(contact.MailChimp()); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.MailChimp, Values = new List<FieldValue> { new FieldValue { Value = digit } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.MailChimp(contact.Agreement()); };
                }
            }

            if (contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp) == null)
            {
                return null;
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp)?.Values.FirstOrDefault().Value == "1" ? true : false;
        }

        public static Dictionary<HowToKnowEnum, string> HowToKnow(this Contact contact, HowToKnowEnum value = HowToKnowEnum.NotSet)
        {
            if (value != HowToKnowEnum.NotSet)
            {
                var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Any(x => x.Enum == (int)value);

                if (!isAviable)
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Add(new FieldValue { Enum = (int)value });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.HowToKnow(value); };
                }

            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values
                .ToDictionary(k => (HowToKnowEnum)k.Enum.Value, v => v.Value);
        }

        public static DateTime Birthday(this Contact contact, DateTime? value = null)
        {
            if (value.HasValue && value.Value != null)
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Birthday))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Birthday).Values.FirstOrDefault().Value = value.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Birthday(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Birthday, Values = new List<FieldValue> { new FieldValue { Value = value.Value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Birthday).Values.FirstOrDefault().Value.ToDateTime();
        }

        public static string Education(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Education))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Education).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Education(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Education, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Education(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Education).Values.FirstOrDefault().Value;
        }

        public static string Experience(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Experience))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Experience).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Experience(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Experience, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Experience(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Experience).Values.FirstOrDefault().Value;
        }

        public static string GroupPart(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.GroupPart))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.GroupPart).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.GroupPart(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.GroupPart, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.GroupPart(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.GroupPart).Values.FirstOrDefault().Value;
        }

        public static string Location(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Location))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Location).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Location(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Location, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Location(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Location).Values.FirstOrDefault().Value;
        }

        public static string Guid(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Guid))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Guid).Values.FirstOrDefault().Value = value;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Guid(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Guid, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Guid(value); };
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Guid).Values.FirstOrDefault().Value;
        }

        public static bool? Agreement(this Contact contact, bool? value = null)
        {
            if (value.HasValue && value != null)
            {
                var digit = value.Value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Agreement))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement).Values.FirstOrDefault().Value = digit;
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Agreement(value); };
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Agreement, Values = new List<FieldValue> { new FieldValue { Value = digit } } });
                    contact.ChangeValueDelegate += delegate (Contact x) { x.Agreement(value); };
                }
            }

            if (contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement) == null)
            {
                return null;
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement)?.Values.FirstOrDefault().Value == "1" ? true : false;
        }
    }
}
