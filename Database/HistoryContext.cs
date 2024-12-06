using Microsoft.EntityFrameworkCore;
using qui_test_api.Models;
using System;

namespace qui_test_api.Database
{
    public class HistoryContext: DbContext
    {
        public HistoryContext(DbContextOptions<HistoryContext> options) : base(options) { }
        public DbSet<HistoryRecord> HistoryRecords { get; set; }
    }
}
