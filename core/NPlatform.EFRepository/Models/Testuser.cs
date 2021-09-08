using System;
using System.Collections.Generic;

#nullable disable

namespace NPlatform.EFRepository.Models
{
    public partial class Testuser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
    }
}
