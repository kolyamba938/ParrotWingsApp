using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWingsApp.Data.Model
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }

        public string SenderName { get; set; }

        public string RecipientName { get; set; }
        public Guid RecipientId { get; set; }

        public DateTime DateTime { get; set; }

        public int Balance { get; set; }

        public TransactionDtoType Type { get; set; }
        public string TransactionTypeName => Enum.GetName(typeof(TransactionDtoType),Type);
    }

    public enum TransactionDtoType
    {
        Входящая,
        Исходящая
    }
}
