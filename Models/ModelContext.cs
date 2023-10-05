using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AboutUs> AboutUs { get; set; }

    public virtual DbSet<Bank> Bank { get; set; }

    public virtual DbSet<Beneficiaries> Beneficiaries { get; set; }

    public virtual DbSet<ContactUs> ContactUs { get; set; }

    public virtual DbSet<HomePage> HomePage { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Subscriptions> Subscriptions { get; set; }

    public virtual DbSet<Testimonials> Testimonials { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1522))(CONNECT_DATA=(SID=xe)));User Id=C##Amir_DB;Password=toor;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##AMIR_DB")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<AboutUs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009079");

            entity.ToTable("ABOUT_US");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.FooterComponent1)
                .IsUnicode(false)
                .HasColumnName("FOOTER_COMPONENT1");
            entity.Property(e => e.FooterComponent2)
                .IsUnicode(false)
                .HasColumnName("FOOTER_COMPONENT2");
            entity.Property(e => e.HeaderComponent1)
                .IsUnicode(false)
                .HasColumnName("HEADER_COMPONENT1");
            entity.Property(e => e.HeaderComponent2)
                .IsUnicode(false)
                .HasColumnName("HEADER_COMPONENT2");
            entity.Property(e => e.ImagePath1)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH1");
            entity.Property(e => e.ImagePath2)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH2");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("LOGO_PATH");
            entity.Property(e => e.Text1)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.CardNo).HasName("SYS_C009092");

            entity.ToTable("BANK");

            entity.Property(e => e.CardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CARD_NO");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CARD_HOLDER_NAME");
            entity.Property(e => e.Cvv)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_METHOD");
        });

        modelBuilder.Entity<Beneficiaries>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009074");

            entity.ToTable("BENEFICIARIES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.BeneficiaryCreationDate)
                .HasColumnType("DATE")
                .HasColumnName("BENEFICIARY_CREATION_DATE");
            entity.Property(e => e.BeneficiaryImagePath)
                .IsUnicode(false)
                .HasColumnName("BENEFICIARY_IMAGE_PATH");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("DATE")
                .HasColumnName("DATE_OF_BIRTH");
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.RelationshipToSubscriber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("RELATIONSHIP_TO_SUBSCRIBER");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Subscriptionid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTIONID");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.Subscriptionid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BENEFICIARY_SUBSCRIPTION");
        });

        modelBuilder.Entity<ContactUs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009081");

            entity.ToTable("CONTACT_US");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("LOGO_PATH");
            entity.Property(e => e.Message)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Subject)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<HomePage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009077");

            entity.ToTable("HOME_PAGE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.FooterComponent1)
                .IsUnicode(false)
                .HasColumnName("FOOTER_COMPONENT1");
            entity.Property(e => e.FooterComponent2)
                .IsUnicode(false)
                .HasColumnName("FOOTER_COMPONENT2");
            entity.Property(e => e.HeaderComponent1)
                .IsUnicode(false)
                .HasColumnName("HEADER_COMPONENT1");
            entity.Property(e => e.HeaderComponent2)
                .IsUnicode(false)
                .HasColumnName("HEADER_COMPONENT2");
            entity.Property(e => e.ImagePath1)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH1");
            entity.Property(e => e.ImagePath2)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH2");
            entity.Property(e => e.LogoPath)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("LOGO_PATH");
            entity.Property(e => e.Text1)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009054");

            entity.ToTable("ROLES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Subscriptions>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009067");

            entity.ToTable("SUBSCRIPTIONS");

            entity.HasIndex(e => e.Userid, "SYS_C009068").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(12,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.StartDate)
                .HasColumnType("DATE")
                .HasColumnName("START_DATE");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithOne(p => p.Subscription)
                .HasForeignKey<Subscriptions>(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SUBSCRIPTION_USER");
        });

        modelBuilder.Entity<Testimonials>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009086");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Commentt)
                .IsUnicode(false)
                .HasColumnName("COMMENTT");
            entity.Property(e => e.Rating)
                .HasColumnType("NUMBER(12,5)")
                .HasColumnName("RATING");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.SubmissionDate)
                .HasColumnType("DATE")
                .HasColumnName("SUBMISSION_DATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TESTIMONIAL_USER");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C009060");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Username, "SYS_C009061").IsUnique();

            entity.HasIndex(e => e.Email, "SYS_C009062").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("PROFILE_PICTURE_URL");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("DATE")
                .HasColumnName("REGISTRATION_DATE");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
