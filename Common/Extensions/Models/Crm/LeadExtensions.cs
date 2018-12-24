using Common.Configuration.Crm;
using Domain.Models.Crm;
using Domain.Models.Crm.Fields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common.Extensions.Models.Crm
{
    public static class LeadExtensions
    {
        public static FieldValue Sources(this Lead lead) => lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Source)?.Values.FirstOrDefault();

        public static void Sources(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Source))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Source, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Source).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static FieldValue Seminars(this Lead lead) => lead.Fields.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Seminar)?.Values.FirstOrDefault();

        public static void Seminar(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Seminar))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Seminar, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Seminar).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static DateTime? SeminarDate(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.SeminarDate)?.Values.FirstOrDefault().Value.ToDateTime(dateSpliter: '-');
        }

        public static void SeminarDate(this Lead lead, DateTime value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != null || value != DateTime.MinValue)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.SeminarDate))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.SeminarDate, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.SeminarDate).Values = new List<FieldValue> { new FieldValue { Value = value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } };
                }
            };
        }

        public static int? StudentCount(this Lead lead)
        {
            var result = lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.StudentCount)?.Values.FirstOrDefault().Value;

            if (result == null) return null;

            return int.Parse(result);
        }

        public static void StudentCount(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) lead.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.StudentCount))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.StudentCount, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.StudentCount).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static DateTime? SeminarWishDate(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.SeminarWishDate)?.Values.FirstOrDefault().Value.ToDateTime();
        }

        public static void SeminarWishDate(this Lead lead, DateTime value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != null || value != DateTime.MinValue)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.SeminarWishDate))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.SeminarWishDate });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.SeminarWishDate).Values = new List<FieldValue> { new FieldValue { Value = value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } };
                }
            };
        }

        public static DateTime? ProgramStartDate(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.ProgramStartDate)?.Values.FirstOrDefault().Value.ToDateTime();
        }

        public static void ProgramStartDate(this Lead lead, DateTime value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != null || value != DateTime.MinValue)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.ProgramStartDate))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.ProgramStartDate });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.ProgramStartDate).Values = new List<FieldValue> { new FieldValue { Value = value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } };
                }
            };
        }

        public static DateTime? ContractExpireDate(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.ContractExpireDate)?.Values.FirstOrDefault().Value.ToDateTime();
        }

        public static void ContractExpireDate(this Lead lead, DateTime value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != null || value != DateTime.MinValue)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.ContractExpireDate))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.ContractExpireDate });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.ContractExpireDate).Values = new List<FieldValue> { new FieldValue { Value = value.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) } };
                }
            };
        }

        public static FieldValue Program(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Program)?.Values.FirstOrDefault();
        }

        public static void Program(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Program))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Program });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Program).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static bool LeadIsPaid(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.LeadIsPaid)?.Values.FirstOrDefault().Value == "1" ? true : false;
        }

        public static void LeadIsPaid(this Lead lead, bool value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (x.Fields == null) x.Fields = new List<Field>();

                if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.LeadIsPaid))
                {
                    x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.LeadIsPaid});
                }

                var digit = value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.LeadIsPaid).Values = new List<FieldValue> { new FieldValue { Value = digit } };
            };
        }

        public static bool SmsSent(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.SmsSent)?.Values.FirstOrDefault().Value == "1" ? true : false;

        }

        public static void SmsSent(this Lead lead, bool value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (x.Fields == null) x.Fields = new List<Field>();

                if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.SmsSent))
                {
                    x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.SmsSent });
                }

                var digit = value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.SmsSent).Values = new List<FieldValue> { new FieldValue { Value = digit } };
            };
        }

        public static FieldValue Teacher(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Teacher)?.Values.FirstOrDefault();
        }

        public static void Teacher(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Teacher))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Teacher, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Teacher).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static FieldValue DistantGroup(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.DistantGroup)?.Values.FirstOrDefault();
        }

        public static void DistantGroup(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.DistantGroup))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.DistantGroup, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.DistantGroup).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static string PersonalFormLink(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.PersonalFormLink)?.Values.FirstOrDefault().Value;
        }

        public static void PersonalFormLink(this Lead lead, string value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (x.Fields == null) x.Fields = new List<Field>();

                if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.PersonalFormLink))
                {
                    x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.PersonalFormLink });
                }

                x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.PersonalFormLink).Values = new List<FieldValue> { new FieldValue { Value = value } };
            };
        }

        public static bool ConfirmParcipate(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.ConfirmParcipate)?.Values.FirstOrDefault().Value == "1" ? true : false;
        }

        public static void ConfirmParcipate(this Lead lead, bool value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (x.Fields == null) x.Fields = new List<Field>();

                if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.ConfirmParcipate))
                {
                    x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.ConfirmParcipate });
                }

                var digit = value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.ConfirmParcipate).Values = new List<FieldValue> { new FieldValue { Value = digit } };
            };
        }

        public static string ParcipateForm(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.ParcipateForm)?.Values.FirstOrDefault().Value;
        }

        public static void ParcipateForm(this Lead lead, string value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (!String.IsNullOrEmpty(value))
                {
                    if (x.Fields == null) lead.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.ParcipateForm))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.ParcipateForm, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.ParcipateForm).Values = new List<FieldValue> { new FieldValue { Value = value } };
                }
            };
        }

        public static FieldValue EducationType(this Lead lead)
        {
            return lead.Fields.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.EducationType)?.Values.FirstOrDefault();
        }

        public static void EducationType(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.EducationType))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.EducationType, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.EducationType).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }

        public static string Guid(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Guid)?.Values.FirstOrDefault().Value;
        }

        public static void Guid(this Lead lead, string value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (!String.IsNullOrEmpty(value))
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Guid))
                    {
                        x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Guid, Values = new List<FieldValue>() });
                    }

                    x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Guid).Values = new List<FieldValue> { new FieldValue { Value = value } };
                }
            };
        }

        public static Dictionary<int, string> Promotions(this Lead lead) => lead.Fields?.FirstOrDefault(x => x.Id == (int)LeadFieldsEnum.Promotions)?.Values
            .ToDictionary(k => k.Enum.Value, v => v.Value);

        //public static void Promotions(this Lead lead, int value)
        //{
        //    lead.ChangeValueDelegate += delegate (Lead x)
        //    {
        //        if (value != 0)
        //        {
        //            if (x.Fields == null) x.Fields = new List<Field>();

        //            if (!x.Fields.Any(fl => fl.Id == (int)LeadFieldsEnum.Promotions))
        //            {
        //                x.Fields.Add(new Field { Id = (int)LeadFieldsEnum.Promotions, Values = new List<FieldValue>() });
        //                //{ new FieldValue { Value = value.ToString() } } });
        //            }

        //            x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Promotions).Values = new List<FieldValue>();

        //            foreach (var val in x.Promotions())
        //            {
        //                x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Promotions).Values.Add(new FieldValue { Enum = val.Key, Value = val.Value });
        //            }

        //            x.Fields.FirstOrDefault(fl => fl.Id == (int)LeadFieldsEnum.Promotions).Values.Add(new FieldValue { Enum = value });
        //        }
        //    };
        //}



        public static bool IsInService1C(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault( x => x.Id == (int)LeadFieldsEnum.IsInService1C )?.Values.FirstOrDefault().Value == "1" ? true : false;
        }

        public static void IsInService1C(this Lead lead, bool value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (x.Fields == null) x.Fields = new List<Field>();

                if (!x.Fields.Any( fl => fl.Id == (int)LeadFieldsEnum.IsInService1C ))
                {
                    x.Fields.Add( new Field { Id = (int)LeadFieldsEnum.IsInService1C } );
                }

                var digit = value.ToString().ToUpper() == "TRUE" ? "1" : "0";

                x.Fields.FirstOrDefault( fl => fl.Id == (int)LeadFieldsEnum.IsInService1C ).Values = new List<FieldValue> { new FieldValue { Value = digit } };
            };
        }



        public static FieldValue FullTimeGroup(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault( x => x.Id == (int)LeadFieldsEnum.FullTimeGroup )?.Values.FirstOrDefault();
        }

        public static void FullTimeGroup(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any( fl => fl.Id == (int)LeadFieldsEnum.FullTimeGroup ))
                    {
                        x.Fields.Add( new Field { Id = (int)LeadFieldsEnum.FullTimeGroup, Values = new List<FieldValue>() } );
                    }

                    x.Fields.FirstOrDefault( fl => fl.Id == (int)LeadFieldsEnum.FullTimeGroup ).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }


        public static FieldValue SubGroup(this Lead lead)
        {
            return lead.Fields?.FirstOrDefault( x => x.Id == (int)LeadFieldsEnum.SubGroup )?.Values.FirstOrDefault();
        }

        public static void SubGroup(this Lead lead, int value)
        {
            lead.ChangeValueDelegate += delegate (Lead x) {
                if (value != 0)
                {
                    if (x.Fields == null) x.Fields = new List<Field>();

                    if (!x.Fields.Any( fl => fl.Id == (int)LeadFieldsEnum.SubGroup ))
                    {
                        x.Fields.Add( new Field { Id = (int)LeadFieldsEnum.SubGroup, Values = new List<FieldValue>() } );
                    }

                    x.Fields.FirstOrDefault( fl => fl.Id == (int)LeadFieldsEnum.SubGroup ).Values = new List<FieldValue> { new FieldValue { Value = value.ToString() } };
                }
            };
        }
    }
}
