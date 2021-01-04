
using System;
using System.Collections.Generic;
using System.Text;
using assigment2.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace assgiment2.Infrastructure.context
{
    public class DataContext : DbContext
    {
        
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Message> Messages { get; set; }

    }
}