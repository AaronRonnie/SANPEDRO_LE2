using BlogDataLibrary.Model;
using BlogDataLibrary.SQL;
using System;

namespace BlogTestUI
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=BlogDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

            var db = new SqlDataAccess(connectionString);

            bool done = false;
            while (!done)
            {
                Console.WriteLine("\n=== BLOG TEST MENU ===");
                Console.WriteLine("1 - List Users");
                Console.WriteLine("2 - Add User");
                Console.WriteLine("3 - List Posts");
                Console.WriteLine("4 - Add Post");
                Console.WriteLine("0 - Exit");
                Console.Write("Choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListUsers(db); break;
                    case "2": AddUser(db); break;
                    case "3": ListPosts(db); break;
                    case "4": AddPost(db); break;
                    case "0": done = true; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        static void ListUsers(SqlDataAccess db)
        {
            var sql = "SELECT * FROM dbo.Users";
            var users = db.LoadData<UserModel>(sql, new { });
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id}: {u.UserName} ({u.FirstName} {u.LastName})");
            }
        }

        static void AddUser(SqlDataAccess db)
        {
            Console.Write("Username: ");
            string userName = Console.ReadLine();

            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            var sql = "INSERT INTO dbo.Users (UserName, FirstName, LastName, Password) VALUES (@UserName, @FirstName, @LastName, @Password)";
            db.SaveData(sql, new { UserName = userName, FirstName = firstName, LastName = lastName, Password = password });

            Console.WriteLine("User added successfully!");
        }

        static void ListPosts(SqlDataAccess db)
        {
            var sql = "SELECT p.Id, p.Title, p.Body, p.DateCreated, u.UserName " +
                      "FROM dbo.Posts p JOIN dbo.Users u ON p.UserId = u.Id";
            var posts = db.LoadData<dynamic>(sql, new { });

            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Id}: {post.Title} by {post.UserName}");
                Console.WriteLine($"{post.Body} [{post.DateCreated}]");
                Console.WriteLine("------");
            }
        }

        static void AddPost(SqlDataAccess db)
        {
            Console.Write("UserId (author): ");
            int userId = int.Parse(Console.ReadLine());

            Console.Write("Post Title: ");
            string title = Console.ReadLine();

            Console.Write("Post Body: ");
            string body = Console.ReadLine();

            var sql = "INSERT INTO dbo.Posts (UserId, Title, Body, DateCreated) VALUES (@UserId, @Title, @Body, @DateCreated)";
            db.SaveData(sql, new { UserId = userId, Title = title, Body = body, DateCreated = DateTime.Now });

            Console.WriteLine("Post added successfully!");
        }
    }

}
