using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ParrotWingsApp.Data.Model
{
    public class Transaction : BaseEntity
    {
        [NotNull]
        public int Amount { get; set; }

        public Guid SenderId { get; set; }
        [NotNull]
        public virtual User Sender { get; set; }

        public Guid RecipientId { get; set; }
        [NotNull]
        public virtual User Recipient { get; set; }

        public DateTime DateTime { get; set; }

        public int SenderBalance { get; set; }

        public int RecipientBalance { get; set; }

    }
}
