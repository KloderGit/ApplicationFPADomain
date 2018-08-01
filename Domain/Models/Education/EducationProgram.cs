using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Education
{
    public class EducationProgram
    {
        public string Guid { get; set; }

        /// <summary>
        /// Активность
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Название программы
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Тип - Обучение / Стажировка
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Вид проведения - Курс / Семинар
        /// </summary>
        public string Variant { get; set; }

        /// <summary>
        /// Форма проведения - Очно / Дистанционно
        /// </summary>
        public EducationForm EducationForm { get; set; }

        /// <summary>
        /// Название Направления обучения - УЦ / ШУ
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Название Направления обучения - УЦ / ШУ
        /// </summary>
        public IEnumerable<Subject> Subjects { get; set; }

        /// <summary>
        /// Дата утверждения
        /// </summary>
        public DateTime Accepted { get; set; }
    }
}
