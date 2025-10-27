using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<User_profile> UserProfiles { get; set; }
        public DbSet<Problem> Problem { get; set; }
        public DbSet<User_password> UserPasswords { get; set; }
        // public DbSet<Task_comment> TaskComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Group_member> GroupMembers { get; set; }
        public DbSet<Channel> Channels { get; set; }

    }
}
