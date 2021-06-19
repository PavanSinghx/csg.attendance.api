using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CSG.Attendance.Api.Models.Mappings
{
    public partial class AttendanceContext : DbContext
    {
        public AttendanceContext()
        {
        }

        public AttendanceContext(DbContextOptions<AttendanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbClass> TbClass { get; set; }
        public virtual DbSet<TbClassList> TbClassList { get; set; }
        public virtual DbSet<TbLearner> TbLearner { get; set; }
        public virtual DbSet<TbTeacher> TbTeacher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=attendancedb;Data Source=DESKTOP-9FRBPQ0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbClass>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PK__tb_Class__CB1927C093A6AC6F");

                entity.ToTable("tb_Class");

                entity.Property(e => e.ClassDescription)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TbClass)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_Class__Teache__75A278F5");
            });

            modelBuilder.Entity<TbClassList>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.LearnerId })
                    .HasName("PK__tb_Class__7D63980D71ABF603");

                entity.ToTable("tb_ClassList");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TbClassList)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ClassL__Class__787EE5A0");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.TbClassList)
                    .HasForeignKey(d => d.LearnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ClassL__Learn__797309D9");
            });

            modelBuilder.Entity<TbLearner>(entity =>
            {
                entity.HasKey(e => e.LearnerId)
                    .HasName("PK__tb_Learn__67ABFCDA97E84269");

                entity.ToTable("tb_Learner");

                entity.Property(e => e.LearnerId).ValueGeneratedNever();

                entity.Property(e => e.Firstnames)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbTeacher>(entity =>
            {
                entity.HasKey(e => e.TeacherId)
                    .HasName("PK__tb_Teach__EDF25964F2544A00");

                entity.ToTable("tb_Teacher");

                entity.Property(e => e.FirebaseUid)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Firstnames)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
