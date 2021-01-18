using Microsoft.Extensions.Configuration;
using ParrotWingsApp.Data.DataLayer;
using ParrotWingsApp.Data.Model;
using ParrotWingsApp.Data.Services;
using System;
using System.IO;

namespace ParrotWingsApp.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var connStr = GetConnectionString();
            using var uService = new UsersService(connStr);
            uService.AddUser("kolyamba039@mail.ru", "password123", "Nikolay Rasskazov");
            uService.AddUser("misha111@yandex.ru", "asdfgh777", "Michail Pertov");

            var user1 = uService.GetUser("kolyamba039@mail.ru", "password123");
            var user2 = uService.GetUser("misha111@yandex.ru", "asdfgh777");

            using var tService = new TransactionsService(connStr);
            //tService.AddTransaction(user1, user2, 100);
        }

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config.GetConnectionString("ParrotWingsConnection");
        }
    }
}
