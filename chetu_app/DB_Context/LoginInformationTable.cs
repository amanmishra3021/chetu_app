﻿using System;
using System.Collections.Generic;

#nullable disable

namespace chetu_app.DB_Context
{
    public partial class LoginInformationTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
