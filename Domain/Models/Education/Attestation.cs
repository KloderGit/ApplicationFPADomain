using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Education
{
    public class Attestation
    {
        public string Guid { get; set; }

        /// <summary>
        /// Название Аттестации Экзамен \ Зачет
        /// </summary>
        public string Title { get; set; }
    }
}
