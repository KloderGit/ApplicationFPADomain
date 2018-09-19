using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Service1C
{
    public class CreateUserDTO
    {
        public string FIO { get; set; }
        public string City { get; set; } = "";
        public string Email { get; set; }
        public string Position { get; set; } = "";
        public DateTime BirthDay { get; set; } = DateTime.MinValue;
        public string Education { get; set; } = "";
        public string Expirience { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; }
    }
}
