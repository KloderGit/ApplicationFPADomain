using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Education
{
    public class Subject
    {
        public string Guid { get; set; }

        /// <summary>
        /// Название дисциплины
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Продолжительность
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Аттестация
        /// </summary>
        public Attestation Attestation { get; set; }
    }
}
