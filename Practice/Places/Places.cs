using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Google.Keep
{
    public class GoogleContext : DbContext
    {
        public DbSet<Keep> Google { get; set; }
        public DbSet<LabelNote> Label { get; set; }
        public DbSet<Checklist> CheckList {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=Note;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Keep>().HasMany(n => n.CheckList).WithOne().HasForeignKey(c => c.KeepId);
            modelBuilder.Entity<Keep>().HasMany(n => n.Lable).WithOne().HasForeignKey(c => c.KeepId);
        } 
    }

    public class Keep
    {
        [Key]
        public int KeepId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Checklist> CheckList {get; set;}
        public List<LabelNote> Lable { get; set; }
        public bool Pinned {get; set; }
    }

    public class LabelNote
    {
        [Key]
        public int Id { get; set; }
        public string items { get; set; }
        public int KeepId { get; set; }
    }
    public class Checklist
    {
        [Key]
        public int ChecklistID {get; set;}
        public string ChecklistText {get; set;}
        public int KeepId {get; set;}
    }
}