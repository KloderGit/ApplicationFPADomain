using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configuration.Crm
{
    public enum ContactFieldsEnum
    {
        Phone = 54667,
        Email = 54669,
        Messanger = 54673,
        Position = 54665,
        City = 72337,
        MailChimp = 482589,
        HowToKnow = 548465,
        Birthday = 565515,
        Education = 565517,
        Experience = 565519,
        GroupPart = 565521,
        Location = 565525,
        Guid = 571611,
        Agreement = 573355
    }

    public enum LeadFieldsEnum
    {
        Source = 66339,
        Seminar = 66349,
        SeminarDate = 72333,
        StudentCount = 72411,
        SeminarWishDate = 72413,
        ProgramStartDate = 72417,
        ContractExpireDate = 72419,
        Program = 227457,
        LeadIsPaid = 497267,
        SmsSent = 549619,
        Teacher = 551795,
        DistantGroup = 554029,
        PersonalFormLink = 565513,
        ConfirmParcipate = 566027,
        ParcipateForm = 566891,
        EducationType = 566897,
        Guid = 570933,
        Promotions = 574701,
        IsInService1C = 579855,
        FullTimeGroup = 579887,
        SubGroup = 579889
    }

    public enum EducationTypeEnum
    {
        Default = (int)ResponsibleUserEnum.Анастасия_Столовая,
        ОТКРЫТОЕ = (int)ResponsibleUserEnum.Ирина_Моисеева,
        ОТКРЫТАЯ = (int)ResponsibleUserEnum.Ирина_Моисеева,
        КОРПОРАТИВНОЕ = (int)ResponsibleUserEnum.Лина_Серрие,
        КОРПОРАТИВНАЯ = (int)ResponsibleUserEnum.Лина_Серрие,
        ОЧНОЕ = (int)ResponsibleUserEnum.Лина_Серрие,
        ОЧНАЯ = (int)ResponsibleUserEnum.Лина_Серрие,
        ДИСТАНЦИОННАЯ = (int)ResponsibleUserEnum.Анастасия_Шатилова,
        ДИСТАНЦИОННОЕ = (int)ResponsibleUserEnum.Анастасия_Шатилова,
    }

    public enum PipelineStartStatusEnum
    {
        Default = 18664336,
        ОТКРЫТОЕ = 17769205,
        ОТКРЫТАЯ = 17769205,
        КОРПОРАТИВНОЕ = 17793877,
        ОЧНОЕ = 17793886,
        ОЧНАЯ = 17793886,
        ДИСТАНЦИОННАЯ = 18855163,
        ДИСТАНЦИОННОЕ = 18855163
    }

    public enum ResponsibleUserEnum
    {
        Robot = 2076025,
        Лина_Серрие = 2079676,
        Анастасия_Столовая = 2079679,
        Ирина_Моисеева = 2079682,
        Евгения_Ковалева = 2079688,
        Наталья_Бердникова = 2079706,
        Мила_Маминова = 2079712,
        Илья_Иджян = 2079718,
        Анастасия_Шатилова = 2267437,
        Филатова_Елена = 2950174,
        Шевченко_Екатерина = 2950210,
        Анна_Скорюпина =2864566
    }
}
