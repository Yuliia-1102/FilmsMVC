﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FilmsInfrastructure.Models
{
	public class User : IdentityUser
	{
        public string Name { get; set; }
	}
}

