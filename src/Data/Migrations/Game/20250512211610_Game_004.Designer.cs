﻿// <auto-generated />
using System;
using Iso.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20250512211610_Game_004")]
    partial class Game_004
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Iso.Data.Models.RoomModel.Group", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<int>("GroupMode")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.GroupMember", b =>
                {
                    b.Property<string>("GroupId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("GroupId", "UserId");

                    b.ToTable("GroupMember");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("GroupId")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlayersLimit")
                        .HasColumnType("integer");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId")
                        .IsUnique();

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomBan", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("RoomId", "UserId");

                    b.ToTable("RoomBans");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomBannedWord", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<string>("BannedWord")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)");

                    b.HasKey("RoomId");

                    b.ToTable("RoomBannedWord");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomRight", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("RoomId", "UserId");

                    b.ToTable("RoomRights");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomTag", b =>
                {
                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("RoomId");

                    b.ToTable("RoomTag");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomTemplate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RoomTemplates");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.GroupMember", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.Room", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Group", "Group")
                        .WithOne("Room")
                        .HasForeignKey("Iso.Data.Models.RoomModel.Room", "GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomBan", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Room", "Room")
                        .WithMany("RoomBans")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomBannedWord", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Room", "Room")
                        .WithMany("RoomBannedWords")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomRight", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Room", "Room")
                        .WithMany("RoomRights")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.RoomTag", b =>
                {
                    b.HasOne("Iso.Data.Models.RoomModel.Room", "Room")
                        .WithMany("RoomTags")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.Group", b =>
                {
                    b.Navigation("GroupMembers");

                    b.Navigation("Room")
                        .IsRequired();
                });

            modelBuilder.Entity("Iso.Data.Models.RoomModel.Room", b =>
                {
                    b.Navigation("RoomBannedWords");

                    b.Navigation("RoomBans");

                    b.Navigation("RoomRights");

                    b.Navigation("RoomTags");
                });
#pragma warning restore 612, 618
        }
    }
}
