using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Login
    {
        [Required]
        public String Username{get; set;}
        [Required, DataType(DataType.Password)]
        public String Password{get; set;}
    }
}