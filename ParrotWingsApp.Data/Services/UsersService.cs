using Microsoft.EntityFrameworkCore.Query.Internal;
using ParrotWingsApp.Data.DataLayer;
using ParrotWingsApp.Data.Helpers;
using ParrotWingsApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParrotWingsApp.Data.Services
{
    public class UsersService : IDisposable
    {
        private readonly ParrotWingsDataProvider _dataProvider;
        private readonly Repository<User> _userRepository;

        public UsersService(string connectionString)
        {
            _dataProvider = new ParrotWingsDataProvider(connectionString);
            _userRepository = _dataProvider.GetRepository<User>();
        }

        public void AddUser(string email, string password, string name)
        {
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"{nameof(email)} or {nameof(name)} or {nameof(password)}");

            if (IsUserExists(email))
                throw new ParrotWingsModelException($"Пользователь с email '{email}' уже зарегистрирован в системе!");

            _userRepository.Add(new User() { Balance = 500, Email = email, Name = name, PasswordHash = PasswordHashHelper.GetMd5Hash(password) });
            _userRepository.SaveChanges();
        }

        public bool IsUserExists(string email)
        {
            return _userRepository
                .Get(u => u.Email.Equals(email))
                .FirstOrDefault() != null;
        }

        public UserDto GetUser(string email, string password)
        {
            var existedUser = _userRepository
                .Get(u => u.Email.Equals(email))
                .FirstOrDefault();

            if (existedUser == null || !existedUser.PasswordHash.Equals(PasswordHashHelper.GetMd5Hash(password)))
                throw new ParrotWingsModelException($"Пользователь с email '{email}' не зарегистрирован в системе либо пароль не верный!");

            return MapUserDto(existedUser);
        }

        public UserDto GetUser(Guid id)
        {
            var existedUser = _userRepository
                .Get(u => u.Id == id)
                .FirstOrDefault();

            return MapUserDto(existedUser);
        }

        public IEnumerable<UserDto> GetUsersList()
        {
            var users = _userRepository.Get().ToList();

            return users.Select(MapUserDto);
        }

        private UserDto MapUserDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Balance = user.Balance,
                Name = user.Name
            };
        }

        public void Dispose()
        {
            _dataProvider?.Dispose();
        }
    }
}
