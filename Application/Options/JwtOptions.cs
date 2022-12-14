﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Options
{
    public class JwtOptions
    {
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public List<string> Audiences { get; set; }
    }
}
