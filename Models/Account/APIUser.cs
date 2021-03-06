﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLEXAPI.Models.Account
{
    public class APIUser
    {

        public int Id { get; set; }

        [StringLength(128)]
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [StringLength(255)]
        public string EmailAddress { get; set; }

    }
}
