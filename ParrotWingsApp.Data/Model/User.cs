using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParrotWingsApp.Data.Model
{
    public class User : BaseEntity
    {
        public User()
        {
        }

        [StringLength(128)]
        [Required]
        public string Name { get; set; }

        [StringLength(128)]
        [Required]
        public string Email { get; set; }

        [StringLength(128)]
        [Required]
        public string PasswordHash { get; set; }

        [NotNull]
        public int Balance { get; set; }

        public virtual ICollection<Transaction> IncomeTransactions { get; set; } 
        public virtual ICollection<Transaction> OutcomeTransactions { get; set; } 
    }
}
