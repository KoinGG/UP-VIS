using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SimpleAuthApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать!");

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Регистрация");
                Console.WriteLine("2. Авторизация");
                Console.WriteLine("3. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        AuthenticateUser();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор! Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void RegisterUser()
        {
            Console.WriteLine("\n<<< Регистрация пользователя >>>");

            Console.Write("Введите имя пользователя: ");
            var username = Console.ReadLine();

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();

            var encryptedPassword = EncryptString(password);

            // Сохраняем данные пользователя в файл
            var userData = $"{username}:{encryptedPassword}";
            File.AppendAllText("users.txt", userData + Environment.NewLine);

            Console.WriteLine("\nПользователь успешно зарегистрирован!");
        }

        static void AuthenticateUser()
        {
            Console.WriteLine("\n<<< Авторизация пользователя >>>");

            Console.Write("Введите имя пользователя: ");
            var username = Console.ReadLine();

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();

            var encryptedPassword = EncryptString(password);

            // Проверяем наличие данных пользователя в файле
            var users = File.ReadAllLines("users.txt");
            foreach (var user in users)
            {
                var userData = user.Split(':');
                var storedUsername = userData[0];
                var storedEncryptedPassword = userData[1];

                if (username == storedUsername && encryptedPassword == storedEncryptedPassword)
                {
                    Console.WriteLine("\nАвторизация успешна!");
                    return;
                }
            }

            Console.WriteLine("\nНеверное имя пользователя или пароль.");
        }

        static string EncryptString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(inputBytes);
                var sb = new StringBuilder();

                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}