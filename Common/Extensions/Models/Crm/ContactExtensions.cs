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
        public static string Position(this Contact contact) => contact.Fields?.FirstOrDefault(x => x.Id == (int) ContactFieldsEnum.Position)?.Values.FirstOrDefault().Value;

        public static void Position(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Position)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Position });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Position).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static Dictionary<PhoneTypeEnum, string> Phones(this Contact contact) => contact.Fields?.FirstOrDefault(x => x.Id == (int) ContactFieldsEnum.Phone)?.Values?
                 .ToDictionary(k => (PhoneTypeEnum) k.Enum.Value, v => v.Value);

        public static void Phones(this Contact contact, PhoneTypeEnum type, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Phone)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Phone, Values = new List<FieldValue>() });

                var current = x.Phones();
                if (current != null && current.Any(p => p.Key == type))
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Phone).Values.FirstOrDefault( p=>p.Enum == (int)type ).Value = value.LeaveJustDigits();
                }
                else
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Phone).Values.Add( new FieldValue { Enum = (int)type, Value = value.LeaveJustDigits() } );
                }
            };
        }

        public static Dictionary<EmailTypeEnum, string> Email(this Contact contact) => contact.Fields?.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Email)?.Values?
                 .ToDictionary(k => (EmailTypeEnum)k.Enum.Value, v => v.Value);


        public static void Email(this Contact contact, EmailTypeEnum type, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Email)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Email, Values = new List<FieldValue>() });

                var current = x.Email();
                if (current != null && current.Any(p => p.Key == type))
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Email).Values.FirstOrDefault(p => p.Enum == (int)type).Value = value.ClearEmail();
                }
                else
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Email).Values.Add(new FieldValue { Enum = (int)type, Value = value.ClearEmail() });
                }
            };
        }

        public static Dictionary<MessengerTypeEnum, string> Messenger(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int) ContactFieldsEnum.Messanger)?
            .Values?
            .ToDictionary(k => (MessengerTypeEnum) k.Enum.Value, v => v.Value);


        public static void Messenger(this Contact contact, MessengerTypeEnum type, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Messanger)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Messanger, Values = new List<FieldValue>() });

                var current = x.Messenger();
                if (current != null && current.Any(p => p.Key == type))
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Messanger).Values.FirstOrDefault(p => p.Enum == (int)type).Value = value.ClearEmail();
                }
                else
                {
                    x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Messanger).Values.Add(new FieldValue { Enum = (int)type, Value = value.ClearEmail() });
                }
            };
        }

        public static string City(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int) ContactFieldsEnum.City)?
            .Values?.FirstOrDefault().Value;


        public static void City(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.City)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.City });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.City).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static bool MailChimp(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.MailChimp)?
            .Values?
            .FirstOrDefault().Value == "1" ? true : false;


        public static void MailChimp(this Contact contact, bool? value)
        {
            if (!value.HasValue || value==null) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.MailChimp)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.MailChimp });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.MailChimp).Values = new List<FieldValue> { new FieldValue { Value = value == true? "1": "0" } };
            };
        }

        public static Dictionary<int, string> HowToKnow(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow)?
            .Values?
            .ToDictionary(k => k.Enum.Value, v => v.Value);

        //public static Dictionary<int, string> HowToKnow(this Contact contact, int value = 0)
        //{
        //    if (value != 0)
        //    {
        //        if (contact.Fields == null) contact.Fields = new List<Field>();

        //        var isAviable = contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Any(x => x.Enum == value);

        //        if (!isAviable)
        //        {
        //            contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values.Add(new FieldValue { Enum = value });
        //            contact.ChangeValueDelegate += delegate (Contact x) { x.HowToKnow(value); };
        //        }

        //    }

        //    return contact.Fields.FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.HowToKnow).Values
        //        .ToDictionary(k => (int)k.Enum.Value, v => v.Value);
        //}

        public static DateTime? Birthday(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Birthday)?
            .Values?
            .FirstOrDefault().Value.ToDateTime('/');


        public static void Birthday(this Contact contact, DateTime value)
        {
            if (value == null || value == DateTime.MinValue) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Birthday)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Birthday });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Birthday).Values = new List<FieldValue> { new FieldValue { Value = value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } };
            };
        }

        public static string Education(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Education)?
            .Values?
            .FirstOrDefault().Value;

        public static void Education(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Education)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Education });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Education).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static string Experience(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Experience)?
            .Values?
            .FirstOrDefault().Value;

        public static void Experience(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Experience)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Experience });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Experience).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static string GroupPart(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.GroupPart)?
            .Values?
            .FirstOrDefault().Value;

        public static void GroupPart(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.GroupPart)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.GroupPart });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.GroupPart).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static string Location(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Location)?
            .Values?
            .FirstOrDefault().Value;

        public static void Location(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Location)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Location });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Location).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static string Guid(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Guid)?
            .Values?
            .FirstOrDefault().Value;


        public static void Guid(this Contact contact, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Guid)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Guid });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Guid).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static bool Agreement(this Contact contact) => contact.Fields?
            .FirstOrDefault(x => x.Id == (int)ContactFieldsEnum.Agreement)?
            .Values?
            .FirstOrDefault().Value == "1" ? true : false;


        public static void Agreement(this Contact contact, bool? value)
        {
            if (!value.HasValue || value == null) return;

            contact.ChangeValueDelegate += delegate (Contact x) {
                x.Fields = x.Fields ?? new List<Field>();
                if (!x.Fields.Any(fl => fl.Id == (int)ContactFieldsEnum.Agreement)) x.Fields.Add(new Field { Id = (int)ContactFieldsEnum.Agreement });
                x.Fields.FirstOrDefault(fl => fl.Id == (int)ContactFieldsEnum.Agreement).Values = new List<FieldValue> { new FieldValue { Value = value == true ? "1" : "0" } };
            };
        }
    }
}
