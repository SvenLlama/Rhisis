﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rhisis.Database;

namespace Rhisis.Database.Migrations
{
    [DbContext(typeof(RhisisDatabaseContext))]
    partial class RhisisDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Rhisis.Database.Entities.DbAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Attributes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "STR"
                        },
                        new
                        {
                            Id = 2,
                            Name = "DEX"
                        },
                        new
                        {
                            Id = 3,
                            Name = "INT"
                        },
                        new
                        {
                            Id = 4,
                            Name = "STA"
                        },
                        new
                        {
                            Id = 5,
                            Name = "YOY_DMG"
                        },
                        new
                        {
                            Id = 6,
                            Name = "BOW_DMG"
                        },
                        new
                        {
                            Id = 7,
                            Name = "CHR_RANGE"
                        },
                        new
                        {
                            Id = 8,
                            Name = "BLOCK_RANGE"
                        },
                        new
                        {
                            Id = 9,
                            Name = "CHR_CHANCECRITICAL"
                        },
                        new
                        {
                            Id = 10,
                            Name = "CHR_BLEEDING"
                        },
                        new
                        {
                            Id = 11,
                            Name = "SPEED"
                        },
                        new
                        {
                            Id = 12,
                            Name = "ABILITY_MIN"
                        },
                        new
                        {
                            Id = 13,
                            Name = "ABILITY_MAX"
                        },
                        new
                        {
                            Id = 14,
                            Name = "BLOCK_MELEE"
                        },
                        new
                        {
                            Id = 15,
                            Name = "MASTRY_EARTH"
                        },
                        new
                        {
                            Id = 16,
                            Name = "STOP_MOVEMENT"
                        },
                        new
                        {
                            Id = 17,
                            Name = "MASTRY_FIRE"
                        },
                        new
                        {
                            Id = 18,
                            Name = "MASTRY_WATER"
                        },
                        new
                        {
                            Id = 19,
                            Name = "MASTRY_ELECTRICITY"
                        },
                        new
                        {
                            Id = 20,
                            Name = "MASTRY_WIND"
                        },
                        new
                        {
                            Id = 21,
                            Name = "KNUCKLE_DMG"
                        },
                        new
                        {
                            Id = 22,
                            Name = "PVP_DMG_RATE"
                        },
                        new
                        {
                            Id = 24,
                            Name = "ATTACKSPEED"
                        },
                        new
                        {
                            Id = 25,
                            Name = "SWD_DMG"
                        },
                        new
                        {
                            Id = 26,
                            Name = "ADJDEF"
                        },
                        new
                        {
                            Id = 27,
                            Name = "RESIST_MAGIC"
                        },
                        new
                        {
                            Id = 28,
                            Name = "RESIST_ELECTRICITY"
                        },
                        new
                        {
                            Id = 29,
                            Name = "REFLECT_DAMAGE"
                        },
                        new
                        {
                            Id = 30,
                            Name = "RESIST_FIRE"
                        },
                        new
                        {
                            Id = 31,
                            Name = "RESIST_WIND"
                        },
                        new
                        {
                            Id = 32,
                            Name = "RESIST_WATER"
                        },
                        new
                        {
                            Id = 33,
                            Name = "RESIST_EARTH"
                        },
                        new
                        {
                            Id = 34,
                            Name = "AXE_DMG"
                        },
                        new
                        {
                            Id = 35,
                            Name = "HP_MAX"
                        },
                        new
                        {
                            Id = 36,
                            Name = "MP_MAX"
                        },
                        new
                        {
                            Id = 37,
                            Name = "FP_MAX"
                        },
                        new
                        {
                            Id = 38,
                            Name = "HP"
                        },
                        new
                        {
                            Id = 39,
                            Name = "MP"
                        },
                        new
                        {
                            Id = 40,
                            Name = "FP"
                        },
                        new
                        {
                            Id = 41,
                            Name = "HP_RECOVERY"
                        },
                        new
                        {
                            Id = 42,
                            Name = "MP_RECOVERY"
                        },
                        new
                        {
                            Id = 43,
                            Name = "FP_RECOVERY"
                        },
                        new
                        {
                            Id = 44,
                            Name = "KILL_HP"
                        },
                        new
                        {
                            Id = 45,
                            Name = "KILL_MP"
                        },
                        new
                        {
                            Id = 46,
                            Name = "KILL_FP"
                        },
                        new
                        {
                            Id = 47,
                            Name = "ADJ_HITRATE"
                        },
                        new
                        {
                            Id = 49,
                            Name = "CLEARBUFF"
                        },
                        new
                        {
                            Id = 50,
                            Name = "CHR_STEALHP_IMM"
                        },
                        new
                        {
                            Id = 51,
                            Name = "ATTACKSPEED_RATE"
                        },
                        new
                        {
                            Id = 52,
                            Name = "HP_MAX_RATE"
                        },
                        new
                        {
                            Id = 53,
                            Name = "MP_MAX_RATE"
                        },
                        new
                        {
                            Id = 54,
                            Name = "FP_MAX_RATE"
                        },
                        new
                        {
                            Id = 55,
                            Name = "CHR_WEAEATKCHANGE"
                        },
                        new
                        {
                            Id = 56,
                            Name = "CHR_STEALHP"
                        },
                        new
                        {
                            Id = 57,
                            Name = "CHR_CHANCESTUN"
                        },
                        new
                        {
                            Id = 58,
                            Name = "AUTOHP"
                        },
                        new
                        {
                            Id = 59,
                            Name = "CHR_CHANCEDARK"
                        },
                        new
                        {
                            Id = 60,
                            Name = "CHR_CHANCEPOISON"
                        },
                        new
                        {
                            Id = 61,
                            Name = "IMMUNITY"
                        },
                        new
                        {
                            Id = 62,
                            Name = "ADDMAGIC"
                        },
                        new
                        {
                            Id = 63,
                            Name = "CHR_DMG"
                        },
                        new
                        {
                            Id = 64,
                            Name = "CHRSTATE"
                        },
                        new
                        {
                            Id = 65,
                            Name = "PARRY"
                        },
                        new
                        {
                            Id = 66,
                            Name = "ATKPOWER_RATE"
                        },
                        new
                        {
                            Id = 67,
                            Name = "EXPERIENCE"
                        },
                        new
                        {
                            Id = 68,
                            Name = "JUMPING"
                        },
                        new
                        {
                            Id = 69,
                            Name = "CHR_CHANCESTEALHP"
                        },
                        new
                        {
                            Id = 70,
                            Name = "CHR_CHANCEBLEEDING"
                        },
                        new
                        {
                            Id = 71,
                            Name = "RECOVERY_EXP"
                        },
                        new
                        {
                            Id = 72,
                            Name = "ADJDEF_RATE"
                        },
                        new
                        {
                            Id = 73,
                            Name = "MP_DEC_RATE"
                        },
                        new
                        {
                            Id = 74,
                            Name = "FP_DEC_RATE"
                        },
                        new
                        {
                            Id = 75,
                            Name = "SPELL_RATE"
                        },
                        new
                        {
                            Id = 76,
                            Name = "CAST_CRITICAL_RATE"
                        },
                        new
                        {
                            Id = 77,
                            Name = "CRITICAL_BONUS"
                        },
                        new
                        {
                            Id = 78,
                            Name = "SKILL_LEVEL"
                        },
                        new
                        {
                            Id = 79,
                            Name = "MONSTER_DMG"
                        },
                        new
                        {
                            Id = 80,
                            Name = "PVP_DMG"
                        },
                        new
                        {
                            Id = 81,
                            Name = "MELEE_STEALHP"
                        },
                        new
                        {
                            Id = 82,
                            Name = "HEAL"
                        },
                        new
                        {
                            Id = 83,
                            Name = "ATKPOWER"
                        },
                        new
                        {
                            Id = 85,
                            Name = "ONEHANDMASTER_DMG"
                        },
                        new
                        {
                            Id = 86,
                            Name = "TWOHANDMASTER_DMG"
                        },
                        new
                        {
                            Id = 87,
                            Name = "YOYOMASTER_DMG"
                        },
                        new
                        {
                            Id = 88,
                            Name = "BOWMASTER_DMG"
                        },
                        new
                        {
                            Id = 89,
                            Name = "KNUCKLEMASTER_DMG"
                        },
                        new
                        {
                            Id = 90,
                            Name = "HAWKEYE_RATE"
                        },
                        new
                        {
                            Id = 91,
                            Name = "RESIST_MAGIC_RATE"
                        },
                        new
                        {
                            Id = 92,
                            Name = "GIFTBOX"
                        },
                        new
                        {
                            Id = 93,
                            Name = "MAX_ADJPARAMARY"
                        },
                        new
                        {
                            Id = 10000,
                            Name = "GOLD"
                        },
                        new
                        {
                            Id = 10001,
                            Name = "PXP"
                        },
                        new
                        {
                            Id = 10002,
                            Name = "RESIST_ALL"
                        },
                        new
                        {
                            Id = 10003,
                            Name = "STAT_ALLUP"
                        },
                        new
                        {
                            Id = 10004,
                            Name = "HPDMG_UP"
                        },
                        new
                        {
                            Id = 10005,
                            Name = "DEFHITRATE_DOWN"
                        },
                        new
                        {
                            Id = 10006,
                            Name = "CURECHR"
                        },
                        new
                        {
                            Id = 10007,
                            Name = "HP_RECOVERY_RATE"
                        },
                        new
                        {
                            Id = 10008,
                            Name = "MP_RECOVERY_RATE"
                        },
                        new
                        {
                            Id = 10009,
                            Name = "FP_RECOVERY_RATE"
                        },
                        new
                        {
                            Id = 10010,
                            Name = "LOCOMOTION"
                        },
                        new
                        {
                            Id = 10011,
                            Name = "MASTRY_ALL"
                        },
                        new
                        {
                            Id = 10012,
                            Name = "ALL_RECOVERY"
                        },
                        new
                        {
                            Id = 10013,
                            Name = "ALL_RECOVERY_RATE"
                        },
                        new
                        {
                            Id = 10014,
                            Name = "KILL_ALL"
                        },
                        new
                        {
                            Id = 10015,
                            Name = "KILL_HP_RATE"
                        },
                        new
                        {
                            Id = 10016,
                            Name = "KILL_MP_RATE"
                        },
                        new
                        {
                            Id = 10017,
                            Name = "KILL_FP_RATE"
                        },
                        new
                        {
                            Id = 10018,
                            Name = "KILL_ALL_RATE"
                        },
                        new
                        {
                            Id = 10019,
                            Name = "ALL_DEC_RATE"
                        });
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbCharacter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Angle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0f);

                    b.Property<short>("BankCode")
                        .HasColumnType("SMALLINT(4)");

                    b.Property<sbyte>("ClusterId")
                        .HasColumnType("TINYINT");

                    b.Property<short>("Dexterity")
                        .HasColumnType("SMALLINT");

                    b.Property<long>("Experience")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIGINT")
                        .HasDefaultValue(0L);

                    b.Property<sbyte>("FaceId")
                        .HasColumnType("TINYINT");

                    b.Property<int>("Fp")
                        .HasColumnType("int");

                    b.Property<sbyte>("Gender")
                        .HasColumnType("TINYINT");

                    b.Property<int>("Gold")
                        .HasColumnType("int");

                    b.Property<int>("HairColor")
                        .HasColumnType("int");

                    b.Property<sbyte>("HairId")
                        .HasColumnType("TINYINT");

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<short>("Intelligence")
                        .HasColumnType("SMALLINT");

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<sbyte>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((sbyte)1);

                    b.Property<DateTime>("LastConnectionTime")
                        .HasColumnType("DATETIME");

                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<int>("MapId")
                        .HasColumnType("int");

                    b.Property<int>("MapLayerId")
                        .HasColumnType("int");

                    b.Property<int>("Mp")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(32)")
                        .HasMaxLength(32);

                    b.Property<long>("PlayTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIGINT")
                        .HasDefaultValue(0L);

                    b.Property<float>("PosX")
                        .HasColumnType("float");

                    b.Property<float>("PosY")
                        .HasColumnType("float");

                    b.Property<float>("PosZ")
                        .HasColumnType("float");

                    b.Property<short>("SkillPoints")
                        .HasColumnType("SMALLINT");

                    b.Property<sbyte>("SkinSetId")
                        .HasColumnType("TINYINT");

                    b.Property<sbyte>("Slot")
                        .HasColumnType("TINYINT");

                    b.Property<short>("Stamina")
                        .HasColumnType("SMALLINT");

                    b.Property<short>("StatPoints")
                        .HasColumnType("SMALLINT");

                    b.Property<short>("Strength")
                        .HasColumnType("SMALLINT");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<sbyte?>("Element")
                        .HasColumnType("TINYINT");

                    b.Property<sbyte?>("ElementRefine")
                        .HasColumnType("TINYINT");

                    b.Property<int>("GameItemId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<sbyte?>("Refine")
                        .HasColumnType("TINYINT");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItemAttributes", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("ItemId", "AttributeId");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ItemId", "AttributeId")
                        .IsUnique();

                    b.ToTable("ItemAttributes");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItemStorage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<short>("Quantity")
                        .HasColumnType("SMALLINT");

                    b.Property<short>("Slot")
                        .HasColumnType("SMALLINT");

                    b.Property<int>("StorageTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("DATETIME");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StorageTypeId");

                    b.ToTable("ItemsStorage");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItemStorageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("ItemStorageTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Inventory"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ExtraBag"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bank"
                        },
                        new
                        {
                            Id = 4,
                            Name = "GuildBank"
                        });
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbMail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("Gold")
                        .HasColumnType("BIGINT");

                    b.Property<bool>("HasBeenRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("HasReceivedGold")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("HasReceivedItem")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<short>("ItemQuantity")
                        .HasColumnType("smallint");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("mails");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbQuest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<ulong>("Finished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<ulong>("IsChecked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<ulong>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<ulong>("IsPatrolDone")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<sbyte>("MonsterKilled1")
                        .HasColumnType("TINYINT");

                    b.Property<sbyte>("MonsterKilled2")
                        .HasColumnType("TINYINT");

                    b.Property<int>("QuestId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("DATETIME");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("QuestId", "CharacterId")
                        .IsUnique();

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbShortcut", b =>
                {
                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<sbyte>("Slot")
                        .HasColumnType("TINYINT");

                    b.Property<short>("SlotLevelIndex")
                        .HasColumnType("SMALLINT");

                    b.Property<short>("ObjectData")
                        .HasColumnType("SMALLINT");

                    b.Property<sbyte>("ObjectIndex")
                        .HasColumnType("TINYINT");

                    b.Property<short?>("ObjectItemSlot")
                        .HasColumnType("SMALLINT");

                    b.Property<sbyte>("ObjectType")
                        .HasColumnType("TINYINT");

                    b.Property<sbyte>("TargetTaskbar")
                        .HasColumnType("TINYINT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<sbyte>("Type")
                        .HasColumnType("TINYINT");

                    b.Property<short>("UserId")
                        .HasColumnType("SMALLINT");

                    b.HasKey("CharacterId", "Slot", "SlotLevelIndex");

                    b.HasIndex("CharacterId", "Slot", "SlotLevelIndex")
                        .IsUnique();

                    b.ToTable("shortcuts");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<sbyte>("Level")
                        .HasColumnType("TINYINT");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("SkillId", "CharacterId")
                        .IsUnique();

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<sbyte>("Authority")
                        .HasColumnType("TINYINT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(255)")
                        .HasMaxLength(255);

                    b.Property<ulong>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIT")
                        .HasDefaultValue(0ul);

                    b.Property<ulong>("IsDeleted")
                        .HasColumnType("BIT");

                    b.Property<DateTime?>("LastConnectionTime")
                        .HasColumnType("DATETIME");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbCharacter", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbUser", "User")
                        .WithMany("Characters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItemAttributes", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbAttribute", "Attribute")
                        .WithMany()
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Rhisis.Database.Entities.DbItem", "Item")
                        .WithMany("ItemAttributes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbItemStorage", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Character")
                        .WithMany("Items")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Rhisis.Database.Entities.DbItem", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Rhisis.Database.Entities.DbItemStorage", "StorageType")
                        .WithMany()
                        .HasForeignKey("StorageTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbMail", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbItem", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Receiver")
                        .WithMany("ReceivedMails")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Sender")
                        .WithMany("SentMails")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbQuest", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbShortcut", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Character")
                        .WithMany("TaskbarShortcuts")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Rhisis.Database.Entities.DbSkill", b =>
                {
                    b.HasOne("Rhisis.Database.Entities.DbCharacter", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
