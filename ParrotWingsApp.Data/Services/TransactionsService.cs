using ParrotWingsApp.Data.DataLayer;
using ParrotWingsApp.Data.Helpers;
using ParrotWingsApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParrotWingsApp.Data.Services
{
    public class TransactionsService : IDisposable
    {
        private readonly ParrotWingsDataProvider _dataProvider;
        private readonly Repository<User> _userRepository;
        private readonly Repository<Transaction> _transactionRepository;


        public TransactionsService(string connectionString)
        {
            _dataProvider = new ParrotWingsDataProvider(connectionString);
            _userRepository = _dataProvider.GetRepository<User>();
            _transactionRepository = _dataProvider.GetRepository<Transaction>();
        }

        public IEnumerable<TransactionDto> GetTransactionHistory(User user)
        {
            List<TransactionDto> dtos = new List<TransactionDto>();
            var income = user.IncomeTransactions;
            var outcome = user.OutcomeTransactions;
            
            if (income != null)
                foreach (var transaction in income)
                {
                    var dto = MapTransaction(transaction, TransactionDtoType.Входящая);
                    dtos.Add(dto);
                }

            if (outcome != null)
                foreach (var transaction in outcome)
                {
                    var dto = MapTransaction(transaction, TransactionDtoType.Исходящая);
                    dtos.Add(dto);
                }

            return dtos.OrderByDescending(t => t.DateTime);
        }

        public IEnumerable<TransactionDto> GetTransactionHistory(Guid userId)
        {
            var user = _userRepository.Get(u => u.Id == userId).SingleOrDefault();
            if (user == null) return null;
            return GetTransactionHistory(user);
        }

        public void AddTransaction(Guid senderId, Guid recipientId, int amount)
        {
            var sender = _userRepository.Get(u => u.Id == senderId).SingleOrDefault();
            var recipient = _userRepository.Get(u => u.Id == recipientId).SingleOrDefault();

            if (amount < 0) throw new ParrotWingsModelException("Размер транзакции должен быть положительным числом!");
            if (sender.Balance < amount) throw new ParrotWingsModelException("Недостаточно средств на счете!");

            var dbTransaction = _transactionRepository.CreateDbTransaction();

            sender.Balance = sender.Balance - amount;
            recipient.Balance = recipient.Balance + amount;
            var newTransaction = new Transaction()
            {
                Amount = amount,
                DateTime = DateTime.Now,
                RecipientBalance = recipient.Balance,
                SenderBalance = sender.Balance
            };

            sender.OutcomeTransactions.Add(newTransaction);
            recipient.IncomeTransactions.Add(newTransaction);

            try
            {
                _transactionRepository.Add(newTransaction);
                _transactionRepository.SaveChanges();
                _userRepository.Update(sender);
                _userRepository.Update(recipient);
                _userRepository.SaveChanges();
                dbTransaction.Commit();
            }
            catch (Exception e)
            {
                throw new ParrotWingsModelException("Ошибка при выполнении транзакции!", e);
            }

        }

        private TransactionDto MapTransaction(Transaction transaction, TransactionDtoType type)
        {
            return new TransactionDto()
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                DateTime = transaction.DateTime,
                RecipientName = transaction.Recipient?.Name,
                RecipientId = transaction.Recipient?.Id ?? Guid.Empty,
                SenderName = transaction.Sender.Name,
                Type = type,
                Balance = type == TransactionDtoType.Входящая ? transaction.RecipientBalance : transaction.SenderBalance
            };
        }

        public void Dispose()
        {
            _dataProvider?.Dispose();
        }
    }
}
