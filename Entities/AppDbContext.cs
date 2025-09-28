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
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<User_profile> UserProfiles { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<User_password> UserPasswords { get; set; }
        public DbSet<Task_comment> TaskComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Group_member> GroupMembers { get; set; }
        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Установка схемы
            modelBuilder.HasDefaultSchema("app");
            
            // Настройка таблиц
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserProfile>().ToTable("user_profiles");
            modelBuilder.Entity<Task>().ToTable("tasks");
            modelBuilder.Entity<UserPassword>().ToTable("user_passwords");
            modelBuilder.Entity<TaskComment>().ToTable("task_comments");
            modelBuilder.Entity<Notification>().ToTable("notifications");
            modelBuilder.Entity<Message>().ToTable("messages");
            modelBuilder.Entity<Group>().ToTable("groups");
            modelBuilder.Entity<GroupMember>().ToTable("group_members");
            modelBuilder.Entity<Channel>().ToTable("channels");

        }
    }
}
