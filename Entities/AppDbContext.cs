using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users => Set<Users>();
        public DbSet<Users_profiels> UserProf => Set<Users_profiels>();
        public DbSet<Tasks> Tasks => Set<Tasks>();

        public DbSet<User_password> User_Passwords => Set<User_password>();

        public DbSet<Task_comments> Task_Comments => Set<Task_comments>();
        public DbSet<Notification> Notification => Set<Notification>(); 
        public DbSet<Messages> Messages => Set<Messages>();

        public DbSet<Groups> Groups => Set<Groups>();

        public DbSet<Group_members> Group_members => Set<Group_members>();
        
        public DbSet<Channels> Channels => Set<Channels>();

        public AppDbContext() => Database.EnsureCreated();


    }
}
