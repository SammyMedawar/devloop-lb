﻿using Microsoft.EntityFrameworkCore;

namespace DevLoopLB.Models;

public partial class DevLoopLbContext : DbContext
{
    public DevLoopLbContext()
    {
    }

    public DevLoopLbContext(DbContextOptions<DevLoopLbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<ImageAsset> ImageAssets { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Academy> Academies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DevLoopLB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__7944C87018F922A5");

            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.EventDateEnd).HasColumnName("event_date_end");
            entity.Property(e => e.EventDateStart).HasColumnName("event_date_start");
            entity.Property(e => e.Longdescription).HasColumnName("longdescription");
            entity.Property(e => e.Metadescription).HasColumnName("metadescription");
            entity.Property(e => e.Metatitle)
                .HasMaxLength(255)
                .HasColumnName("metatitle");
            entity.Property(e => e.Poster)
                .HasMaxLength(512)
                .HasColumnName("poster");
            entity.Property(e => e.Shortdescription).HasColumnName("shortdescription");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasMany(d => d.Tags).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "Eventtag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__eventtags__TagID__3B75D760"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("Eventid")
                        .HasConstraintName("FK__eventtags__event__3A81B327"),
                    j =>
                    {
                        j.HasKey("Eventid", "TagId").HasName("PK_event_tags");
                        j.ToTable("eventtags");
                        j.IndexerProperty<int>("Eventid").HasColumnName("eventid");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<ImageAsset>(entity =>
        {
            entity.HasKey(e => e.ImageAssetId).HasName("PK__ImageAss__16EED56D4AD05A54");

            entity.ToTable("ImageAsset");

            entity.Property(e => e.ImageAssetId).HasColumnName("ImageAssetID");
            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.HasOne(d => d.Event).WithMany(p => p.ImageAssets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImageAsse__Event__267ABA7A");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__tag__657CFA4CA59FA867");

            entity.ToTable("tag");

            entity.HasIndex(e => e.Name, "UQ__tag__737584F6B274B69D").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.HashedPassword)
                .HasColumnType("VARBINARY(64)")
                .IsRequired();

            entity.Property(e => e.IsAdmin)
                .HasColumnType("BIT")
                .IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Academy>(entity =>
        {
            entity.HasKey(e => e.AcademyId).HasName("PK__Academy__AcademyID");

            entity.ToTable("Academy");

            entity.Property(e => e.AcademyId).HasColumnName("AcademyID");
            entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
            entity.Property(e => e.ShortDescription).HasMaxLength(255).IsRequired();
            entity.Property(e => e.LongDescription).HasColumnType("TEXT");
            entity.Property(e => e.MetaTitle).HasMaxLength(255);
            entity.Property(e => e.MetaDescription).HasMaxLength(255);
            entity.Property(e => e.PosterLink).HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasColumnType("DATETIME");
            entity.Property(e => e.ReadMoreLink).HasMaxLength(255);
            entity.Property(e => e.ReadMoreText).HasMaxLength(255);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
