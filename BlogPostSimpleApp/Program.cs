using BlogPostSimpleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        TestDatabase();
    }

    
    static void ListAllUsers()
    {
        using var context = new AppDbContext();
        var users = context.Users.ToList();

        Console.WriteLine("Current Users ");
        if (!users.Any())
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.UserId}, Name: {user.UserName}, Email: {user.Email}, Phone: {user.PhoneNumber}");
        }
    }

    
    static void AddUser(string name, string email, string phone)
    {
        using var context = new AppDbContext();
        var user = new User
        {
            UserName = name,
            Email = email,
            PhoneNumber = phone
        };

        context.Users.Add(user);
        context.SaveChanges();
        Console.WriteLine($"Added user: {name}");
    }

    
    static void UpdateUser(int userId, string newName)
    {
        using var context = new AppDbContext();
        var user = context.Users.FirstOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            Console.WriteLine($" No user found with ID {userId}");
            return;
        }

        user.UserName = newName;
        context.SaveChanges();
        Console.WriteLine($" Updated user ID {userId} to new name: {newName}");
    }

    
    static void DeleteUser(int userId)
    {
        using var context = new AppDbContext();
        var user = context.Users.FirstOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            Console.WriteLine($" No user found with ID {userId} to delete.");
            return;
        }

        context.Users.Remove(user);
        context.SaveChanges();
        Console.WriteLine($" Deleted user ID {userId}");
    }

    
    static void TestDatabase()
    {
        Console.WriteLine(" CRUD Tests...");

        
        using (var context = new AppDbContext())
        {
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Users', RESEED, 0)");

            
            var users = new List<User>
            {
                new User { UserName = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890" },
                new User { UserName = "Bob", Email = "bob@example.com", PhoneNumber = "2345678901" }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        ListAllUsers();

         
        AddUser("Charlie", "charlie@example.com", "3456789012");
        ListAllUsers();

       
        UpdateUser(2, "Bobby");
        ListAllUsers();

        
        DeleteUser(1);
        ListAllUsers();
    }
}
