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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Position, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Position).Values.FirstOrDefault().Value;
        }

        public static Dictionary<PhoneTypeEnum, string> Phone(this Contact contact, PhoneTypeEnum type = PhoneTypeEnum.NotSet, string value = "" )
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Phone))
                {
                    var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.Any(x => x.Value.LeaveJustDigits() == value.LeaveJustDigits());

                    if (!isAviable)
                    {
                        var isMOBSet = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.Any(x => x.Enum == (int)PhoneTypeEnum.MOB);

                        if (isMOBSet)
                        {
                            if (type == PhoneTypeEnum.NotSet)
                            {
                                var enumPhoneTypes = Enum.GetValues(typeof(PhoneTypeEnum)).Cast<PhoneTypeEnum>().Select(i => (int)i);
                                var phoneTypes = enumPhoneTypes.Except(contact.Phone().Select(e => (int)e.Key)).ToList();
                                phoneTypes.Remove((int)PhoneTypeEnum.NotSet);

                                type = (PhoneTypeEnum)phoneTypes.FirstOrDefault();
                            }

                            contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.Add(new FieldValue { Enum = (int)type, Value = value.LeaveJustDigits() });
                        }
                        else
                        {
                            contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values.Add(new FieldValue { Enum = (int)PhoneTypeEnum.MOB, Value = value.LeaveJustDigits() });
                        }
                    }
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Phone, Values = new List<FieldValue> { new FieldValue { Enum = (int)PhoneTypeEnum.MOB, Value = value.LeaveJustDigits() } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Phone).Values
                .ToDictionary(k => (PhoneTypeEnum)k.Enum.Value, v => v.Value);
        }

        public static Dictionary<EmailTypeEnum, string> Email(this Contact contact, EmailTypeEnum type = EmailTypeEnum.NotSet, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Email))
                {
                    var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Any(x => x.Value.ClearEmail() == value.ClearEmail());

                    if (!isAviable)
                    {
                        var isPRIVSet = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Any(x => x.Enum == (int)EmailTypeEnum.PRIV);

                        if (isPRIVSet)
                        {
                            if (type == EmailTypeEnum.NotSet)
                            {
                                var enumEmailTypes = Enum.GetValues(typeof(EmailTypeEnum)).Cast<EmailTypeEnum>().Select(i => (int)i);
                                var emailTypes = enumEmailTypes.Except(contact.Email().Select(e => (int)e.Key)).ToList();
                                emailTypes.Remove((int)EmailTypeEnum.NotSet);

                                type = (EmailTypeEnum)emailTypes.FirstOrDefault();
                            }

                            contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Add(new FieldValue { Enum = (int)type, Value = value });
                        }
                        else
                        {
                            contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email).Values.Add(new FieldValue { Enum = (int)EmailTypeEnum.PRIV, Value = value });
                        }
                    }

                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Email, Values = new List<FieldValue> { new FieldValue { Enum = (int)EmailTypeEnum.PRIV, Value = value } } });
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
                    }
                }
                else
                {
                    if (type == MessengerTypeEnum.NotSet) throw new ArgumentException("Не указан тип мессенжера");
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Messanger, Values = new List<FieldValue> { new FieldValue { Enum = (int)type, Value = value } } });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.City, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.City).Values.FirstOrDefault().Value;
        }

        public static string MailChimp(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                var res = value.ToUpper() == "TRUE" ? "1" : "0";

                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.MailChimp))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp).Values.FirstOrDefault().Value = res;
                }
                else
                {
                    contact.Fields.Add( new Field { Id = (int)ContactFieldsEnum.MailChimp, Values = new List<FieldValue> { new FieldValue { Value = res } } } );
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp)?.Values.FirstOrDefault().Value;
        }

        public static Dictionary<HowToKnowEnum, string> HowToKnow(this Contact contact, HowToKnowEnum value = HowToKnowEnum.NotSet)
        {
            if (value != HowToKnowEnum.NotSet)
            {
                var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Any(x => x.Enum == (int)value);

                if (!isAviable) contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Add(new FieldValue { Enum = (int)value });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Education, Values = new List<FieldValue> { new FieldValue { Value = value } } });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Experience, Values = new List<FieldValue> { new FieldValue { Value = value } } });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.GroupPart, Values = new List<FieldValue> { new FieldValue { Value = value } } });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Location, Values = new List<FieldValue> { new FieldValue { Value = value } } });
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
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Guid, Values = new List<FieldValue> { new FieldValue { Value = value } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Guid).Values.FirstOrDefault().Value;
        }

        public static string Agreement(this Contact contact, string value = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                var res = value.ToUpper() == "TRUE" ? "1" : "0";

                if (contact.Fields.Any(x => x.Id == (int)ContactFieldsEnum.Agreement))
                {
                    contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement).Values.FirstOrDefault().Value = res;
                }
                else
                {
                    contact.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Agreement, Values = new List<FieldValue> { new FieldValue { Value = res } } });
                }
            }

            return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement)?.Values.FirstOrDefault().Value;
        }
    }
}
