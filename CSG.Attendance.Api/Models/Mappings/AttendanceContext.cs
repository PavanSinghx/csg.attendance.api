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
        public virtual DbSet<TbDailyClassListGrade> TbDailyClassListGrade { get; set; }
        public virtual DbSet<TbLearner> TbLearner { get; set; }
        public virtual DbSet<TbTeacher> TbTeacher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbClass>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PK__tb_Class__CB1927C018C4EF6C");

                entity.ToTable("tb_Class");

                entity.Property(e => e.ClassDescription)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TbClass)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_Class__Teache__3F115E1A");
            });

            modelBuilder.Entity<TbClassList>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.LearnerId })
                    .HasName("PK__tb_Class__7D63980DE60B4B3F");

                entity.ToTable("tb_ClassList");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TbClassList)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ClassL__Class__41EDCAC5");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.TbClassList)
                    .HasForeignKey(d => d.LearnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ClassL__Learn__42E1EEFE");
            });

            modelBuilder.Entity<TbDailyClassListGrade>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.LearnerId, e.DayStart })
                    .HasName("PK__tb_Daily__0BE5A9C62FD24710");

                entity.ToTable("tb_DailyClassListGrade");

                entity.Property(e => e.DayStart).HasColumnType("datetime");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TbDailyClassListGrade)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_DailyC__Class__489AC854");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.TbDailyClassListGrade)
                    .HasForeignKey(d => d.LearnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_DailyC__Learn__498EEC8D");
            });

            modelBuilder.Entity<TbLearner>(entity =>
            {
                entity.HasKey(e => e.LearnerId)
                    .HasName("PK__tb_Learn__67ABFCDA9270F599");

                entity.ToTable("tb_Learner");

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
                    .HasName("PK__tb_Teach__EDF25964AFAD9081");

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
