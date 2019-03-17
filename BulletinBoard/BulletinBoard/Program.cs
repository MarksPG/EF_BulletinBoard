using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Category> Category { get; set; }
        //public DbSet<OrderItem> OrderItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=den1.mssql8.gear.host;Initial Catalog=efbulletinboard;Persist Security Info=True;User ID=efbulletinboard;Password=Qe9RK-mq!Ty6");
        }
    }

    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public override string ToString()
        {
            return Username;
        }
    }

    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Post 
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Topic { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public User User { get; set; }
        public int? Like { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Topic;

        }

    }

    public class Program
    {
        static AppDbContext database;
        static User loggedInUser;

        static void Main(string[] args)
        {
            StartMenu();
        }

        private static void StartMenu()
        {
            using (database = new AppDbContext())
            {
                while (true)
                {
                    string option = ShowMenu("What do you want to do?", new[] {
                        "Sign in",
                        "Create account",
                        "Quit"
                    });

                    if (option == "Sign in") SignIn();
                    else if (option == "Create account") CreateAccount();
                    else Environment.Exit(0);

                    Console.WriteLine();
                }
            }
        }

        private static void SignIn()
        {
            WriteUnderlined("Enter username and password to login");

            User user = new User()
            {
                Username = ReadString("Enter your username")
            };

            User[] users = database.User.ToArray();

            
            if (users.Select(u => u.Username).Contains(user.Username))
            {
                User selectedUser = users.First(u => u.Username == user.Username);
                user.Password = ReadString("Enter password");

                if (user.Password == selectedUser.Password)
                {
                    loggedInUser = selectedUser;
                    MainMenu();
                }
                else
                {
                    Console.WriteLine("Wrong password");
                }
            }
            else
            {
                Console.WriteLine("User doesn't exist. Enter an existing username or create an account.");
            }
            
        }

        private static void MainMenu()
        {
            using (database = new AppDbContext())
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"You are now logged in as {loggedInUser.Username}");
                    Console.WriteLine();
                    string option = ShowMenu("What do you want to do?", new[] {
                        "Most Recent Posts",
                        "Most Popular Posts",
                        "Posts by Category",
                        "Search",
                        "Create a Post",
                        "Delete a Post",
                        "Quit"
                    });

                    if (option == "Most Recent Posts") MostRecentPosts();
                    else if (option == "Most Popular Posts") MostPopularPosts();
                    else if (option == "Posts by Category") PostsByCategory();
                    else if (option == "Search") Search();
                    else if (option == "Create a Post") CreateAPost();
                    else if (option == "Delete a Post") DeleteAPost();
                    else Environment.Exit(0);

                    Console.WriteLine();
                }
            }
        }

        private static void DeleteAPost()
        {
            Console.Clear();
            var posts = database.Post.Include(p => p.User).Where(p => p.User.ID == loggedInUser.ID).ToArray();
            var postToDelete = (Post)ShowMenu2("Select which post to delete", posts);

            Console.WriteLine();
            WriteUnderlined("You have chosen to delete this post:");
            Console.WriteLine($"{postToDelete.Topic}");
            Console.WriteLine($"{postToDelete.Content}");
            Console.WriteLine($"Created by {postToDelete.User.Username} on {postToDelete.Date.Date}");
            Console.WriteLine("Are you sure you want to delete this post?(Y/N)");
            string userOption = Console.ReadLine().ToUpper();
            if (userOption == "Y")
            {
                database.Remove(postToDelete);
                database.SaveChanges();
                MainMenu();
            }
            else if (userOption == "N")
            {
                MainMenu();
            }
            else
            {
                Console.WriteLine("Come on, this was supposed to be a simple choice!");
                Console.ReadKey();
                MainMenu();
            }
        }

        private static void MostRecentPosts()
        {
            Console.Clear();
            Console.WriteLine("Hej!");
            Post[] allPosts = database.Post.Include(p => p.Category).Include(p => p.User).ToArray();
            var selectedPost = (Post)ShowMenu2("Most recent posts", allPosts);


            Console.WriteLine();

            WriteUnderlined($"{selectedPost.Topic}");
            Console.WriteLine();
            Console.WriteLine($"{selectedPost.Content}");
            Console.WriteLine();
            Console.WriteLine($"Posted by {selectedPost.User.Username} in {selectedPost.Category.Name} at {selectedPost.Date.Hour}:{selectedPost.Date.Minute}");
            Console.WriteLine();
            string option = ShowMenu("What do you want to do?", new[] {
                        "Like this post",
                        "Return to Main Menu"
                    });

            if (option == "Like this post") LikeAPost(selectedPost);
            else if (option == "Return to Main Menu") MainMenu();
            else Environment.Exit(0);


        }

        private static void LikeAPost(Post selectedPost)
        {

            Post post = new Post();
            post = selectedPost;
            post.Like++;
            database.Update(post);
            database.SaveChanges();
            StartMenu();

        }

        private static void MostPopularPosts()
        {
            throw new NotImplementedException();
        }

        private static void PostsByCategory()
        {
            Console.Clear();
            
            Console.WriteLine();
            var categories = database.Category.ToArray();
            var selectedCategory = (Category)ShowMenu2("Posts by category", categories);

            WriteUnderlined($"Posts by category + { selectedCategory.Name }");
            var posts = database.Post.Include(p => p.Category).Where(p => p.Category.Name == selectedCategory.Name).ToArray();
            foreach(Post post in posts)
            {
                Console.WriteLine($"- {post.Topic}");
            }
            Console.WriteLine();
            Console.ReadKey();
        }

        private static void Search()
        {
            throw new NotImplementedException();
        }

        private static void CreateAPost()
        {
            Console.Clear();
            WriteUnderlined("Create new post");
            Console.WriteLine();
            Category[] categories = database.Category.ToArray();
            object selectedCategory = ShowMenu2("Select category", categories);
            Post post = new Post();
            //Behöver kollas. Det är ingen referens till objektet. ShowMenu behöver returnera ett objekt.
            post.Category = (Category)selectedCategory;
            Console.WriteLine();
            post.Topic = ReadString("Enter a topic for your post");
            post.Content = ReadString("Write your post message");
            post.User = database.User.First(u => u.ID == loggedInUser.ID);
            post.Date = DateTime.Now;
            post.Like = 0;

            database.Add(post);
            database.SaveChanges();
        }

        private static void CreateAccount()
        {
            User user = new User();
            {
                user.Username = ReadString("Username:");
                user.Password = ReadString("Password:");
            }

            database.Add(user);
            database.SaveChanges();

            loggedInUser = user;
            MainMenu();
        }
         
        public bool UserExists(string userName)
        {
            string[] users = database.User.Select(u => u.Username).ToArray();

            try
            {
                users.Contains(userName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static string ShowMenu(string prompt, string[] options)
        {
            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and perform the selected action.
            Console.CursorVisible = true;
            Console.Clear();
            return options[selected];
        }

        static object ShowMenu2(string prompt, object[] options)
        {
            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option.ToString());
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and perform the selected action.
            Console.CursorVisible = true;
            Console.Clear();
            return options[selected];
        }

        static void WriteUnderlined(string line)
        {
            Console.WriteLine(line);
            Console.WriteLine(new String('-', line.Length));
        }

        static string ReadString(string prompt)
        {
            Console.Write(prompt + ": ");
            return Console.ReadLine();
        }

        static int ReadInt(string prompt)
        {
            Console.Write(prompt + ": ");
            int? number = null;
            while (number == null)
            {
                string input = Console.ReadLine();
                try
                {
                    number = int.Parse(input);
                }
                catch
                {
                    Console.Write("Please enter a valid integer: ");
                }
            }
            return (int)number;
        }
    }
}