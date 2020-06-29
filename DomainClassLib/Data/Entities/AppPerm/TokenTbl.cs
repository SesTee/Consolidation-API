using System;
using System.ComponentModel.DataAnnotations;

namespace DomainClassLib.Data.Entities.AppPerm
{
    public class TokenTbl
    {

        [Required]
        public string Token { get; set; }

        [Required]
        public string Refresh { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
