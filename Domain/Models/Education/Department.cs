using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Education
{
    public class Department
    {
        public string Guid { get; set; }

        /// <summary>
        /// Название Направления обучения - УЦ / ШУ
        /// </summary>
        public string Title { get; set; }
    }
}
