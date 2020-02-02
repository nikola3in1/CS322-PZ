using System;
using System.Data.Entity;

namespace CS322_PZ.Models
{
    public class ChatContext : DbContext
    {
        public ChatContext() : base("mysqlConnection")
        {
        }

        public static ChatContext Create()
        {
            return new ChatContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
    }
}