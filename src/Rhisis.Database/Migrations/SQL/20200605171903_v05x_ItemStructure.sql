DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

ALTER TABLE `characters` DROP FOREIGN KEY `FK_characters_users_UserId`;

ALTER TABLE `items` DROP FOREIGN KEY `FK_items_characters_CharacterId`;

ALTER TABLE `mails` DROP FOREIGN KEY `FK_mails_items_ItemId`;

ALTER TABLE `mails` DROP FOREIGN KEY `FK_mails_characters_ReceiverId`;

ALTER TABLE `mails` DROP FOREIGN KEY `FK_mails_characters_SenderId`;

ALTER TABLE `quests` DROP FOREIGN KEY `FK_quests_characters_CharacterId`;

ALTER TABLE `shortcuts` DROP FOREIGN KEY `FK_shortcuts_characters_CharacterId`;

ALTER TABLE `Skills` DROP FOREIGN KEY `FK_Skills_characters_CharacterId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'users');
ALTER TABLE `users` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'quests');
ALTER TABLE `quests` DROP PRIMARY KEY;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'items');
ALTER TABLE `items` DROP PRIMARY KEY;

ALTER TABLE `items` DROP INDEX `IX_items_CharacterId`;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'characters');
ALTER TABLE `characters` DROP PRIMARY KEY;

ALTER TABLE `users` RENAME `Users`;

ALTER TABLE `quests` RENAME `Quests`;

ALTER TABLE `items` RENAME `Items`;

ALTER TABLE `characters` RENAME `Characters`;

ALTER TABLE `Users` RENAME INDEX `IX_users_Username_Email` TO `IX_Users_Username_Email`;

ALTER TABLE `Quests` RENAME INDEX `IX_quests_QuestId_CharacterId` TO `IX_Quests_QuestId_CharacterId`;

ALTER TABLE `Quests` RENAME INDEX `IX_quests_CharacterId` TO `IX_Quests_CharacterId`;

ALTER TABLE `Characters` RENAME INDEX `IX_characters_UserId` TO `IX_Characters_UserId`;

ALTER TABLE `Users` MODIFY COLUMN `Username` NVARCHAR(32) NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `Password` VARCHAR(32) NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `LastConnectionTime` DATETIME NULL;

ALTER TABLE `Users` MODIFY COLUMN `IsDeleted` BIT NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `EmailConfirmed` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Users` MODIFY COLUMN `Email` VARCHAR(255) NOT NULL;

ALTER TABLE `Users` MODIFY COLUMN `Authority` TINYINT NOT NULL;

ALTER TABLE `Skills` MODIFY COLUMN `Level` TINYINT NOT NULL;

ALTER TABLE `Quests` MODIFY COLUMN `IsPatrolDone` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Quests` MODIFY COLUMN `IsDeleted` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Quests` MODIFY COLUMN `IsChecked` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Quests` MODIFY COLUMN `Finished` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Items` MODIFY COLUMN `Refine` TINYINT NULL;

ALTER TABLE `Items` MODIFY COLUMN `ElementRefine` TINYINT NULL;

ALTER TABLE `Items` MODIFY COLUMN `Element` TINYINT NULL;

ALTER TABLE `Items` ADD `GameItemId` int NOT NULL DEFAULT 0;

ALTER TABLE `Characters` MODIFY COLUMN `Strength` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `StatPoints` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `Stamina` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `SkinSetId` TINYINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `SkillPoints` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `PlayTime` BIGINT NOT NULL DEFAULT 0;

ALTER TABLE `Characters` MODIFY COLUMN `Name` NVARCHAR(32) NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `Level` int NOT NULL DEFAULT 1;

ALTER TABLE `Characters` MODIFY COLUMN `JobId` TINYINT NOT NULL DEFAULT 1;

ALTER TABLE `Characters` MODIFY COLUMN `IsDeleted` BIT NOT NULL DEFAULT 0;

ALTER TABLE `Characters` MODIFY COLUMN `Intelligence` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `HairId` TINYINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `Gender` TINYINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `FaceId` TINYINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `Experience` BIGINT NOT NULL DEFAULT 0;

ALTER TABLE `Characters` MODIFY COLUMN `Dexterity` SMALLINT NOT NULL;

ALTER TABLE `Characters` MODIFY COLUMN `Angle` float NOT NULL DEFAULT 0;

ALTER TABLE `Characters` ADD `ClusterId` TINYINT NOT NULL DEFAULT 0;

ALTER TABLE `Users` ADD CONSTRAINT `PK_Users` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Users', 'Id');

ALTER TABLE `Quests` ADD CONSTRAINT `PK_Quests` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Quests', 'Id');

ALTER TABLE `Items` ADD CONSTRAINT `PK_Items` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Items', 'Id');

ALTER TABLE `Characters` ADD CONSTRAINT `PK_Characters` PRIMARY KEY (`Id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Characters', 'Id');

CREATE TABLE `Attributes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(20) NULL,
    CONSTRAINT `PK_Attributes` PRIMARY KEY (`Id`)
);

CREATE TABLE `ItemsStorage` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `StorageTypeId` int NOT NULL,
    `CharacterId` int NOT NULL,
    `ItemId` int NOT NULL,
    `Slot` SMALLINT NOT NULL,
    `Quantity` SMALLINT NOT NULL,
    `Updated` DATETIME NOT NULL DEFAULT NOW(),
    `IsDeleted` BIT NOT NULL DEFAULT 0,
    CONSTRAINT `PK_ItemsStorage` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ItemsStorage_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `Characters` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ItemsStorage_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ItemsStorage_ItemsStorage_StorageTypeId` FOREIGN KEY (`StorageTypeId`) REFERENCES `ItemsStorage` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `ItemStorageTypes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(20) NOT NULL,
    CONSTRAINT `PK_ItemStorageTypes` PRIMARY KEY (`Id`)
);

INSERT INTO `ItemStorageTypes` (`Id`, `Name`)
VALUES (2, 'ExtraBag');
INSERT INTO `ItemStorageTypes` (`Id`, `Name`)
VALUES (3, 'Bank');
INSERT INTO `ItemStorageTypes` (`Id`, `Name`)
VALUES (1, 'Inventory');
INSERT INTO `ItemStorageTypes` (`Id`, `Name`)
VALUES (4, 'GuildBank');


                DELETE FROM Items WHERE ItemSlot = -1;
				UPDATE Items SET GameItemId = ItemId;
                INSERT INTO ItemsStorage (CharacterId, StorageTypeId, ItemId, Quantity, Slot) 
                SELECT CharacterId, 1, Id, ItemCount, ItemSlot FROM Items WHERE IsDeleted = 0;
            

ALTER TABLE `items` DROP COLUMN `CharacterId`;

ALTER TABLE `items` DROP COLUMN `ItemCount`;

ALTER TABLE `items` DROP COLUMN `ItemId`;

ALTER TABLE `items` DROP COLUMN `ItemSlot`;

CREATE TABLE `ItemAttributes` (
    `ItemId` int NOT NULL,
    `AttributeId` int NOT NULL,
    `Value` int NOT NULL,
    CONSTRAINT `PK_ItemAttributes` PRIMARY KEY (`ItemId`, `AttributeId`),
    CONSTRAINT `FK_ItemAttributes_Attributes_AttributeId` FOREIGN KEY (`AttributeId`) REFERENCES `Attributes` (`Id`),
    CONSTRAINT `FK_ItemAttributes_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`)
);

INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (1, 'STR');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (85, 'ONEHANDMASTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (83, 'ATKPOWER');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (82, 'HEAL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (81, 'MELEE_STEALHP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (80, 'PVP_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (79, 'MONSTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (78, 'SKILL_LEVEL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (77, 'CRITICAL_BONUS');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (76, 'CAST_CRITICAL_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (75, 'SPELL_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (74, 'FP_DEC_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (86, 'TWOHANDMASTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (73, 'MP_DEC_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (71, 'RECOVERY_EXP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (70, 'CHR_CHANCEBLEEDING');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (69, 'CHR_CHANCESTEALHP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (68, 'JUMPING');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (67, 'EXPERIENCE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (66, 'ATKPOWER_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (65, 'PARRY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (64, 'CHRSTATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (63, 'CHR_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (62, 'ADDMAGIC');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (61, 'IMMUNITY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (72, 'ADJDEF_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (60, 'CHR_CHANCEPOISON');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (87, 'YOYOMASTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (89, 'KNUCKLEMASTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10019, 'ALL_DEC_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10018, 'KILL_ALL_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10017, 'KILL_FP_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10016, 'KILL_MP_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10015, 'KILL_HP_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10014, 'KILL_ALL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10013, 'ALL_RECOVERY_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10012, 'ALL_RECOVERY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10011, 'MASTRY_ALL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10010, 'LOCOMOTION');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10009, 'FP_RECOVERY_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (88, 'BOWMASTER_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10008, 'MP_RECOVERY_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10006, 'CURECHR');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10005, 'DEFHITRATE_DOWN');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10004, 'HPDMG_UP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10003, 'STAT_ALLUP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10002, 'RESIST_ALL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10001, 'PXP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10000, 'GOLD');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (93, 'MAX_ADJPARAMARY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (92, 'GIFTBOX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (91, 'RESIST_MAGIC_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (90, 'HAWKEYE_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10007, 'HP_RECOVERY_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (58, 'AUTOHP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (59, 'CHR_CHANCEDARK');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (28, 'RESIST_ELECTRICITY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (27, 'RESIST_MAGIC');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (26, 'ADJDEF');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (25, 'SWD_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (24, 'ATTACKSPEED');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (22, 'PVP_DMG_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (21, 'KNUCKLE_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (20, 'MASTRY_WIND');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (19, 'MASTRY_ELECTRICITY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (18, 'MASTRY_WATER');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (17, 'MASTRY_FIRE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (16, 'STOP_MOVEMENT');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (57, 'CHR_CHANCESTUN');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (15, 'MASTRY_EARTH');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (13, 'ABILITY_MAX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (12, 'ABILITY_MIN');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (11, 'SPEED');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (10, 'CHR_BLEEDING');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (9, 'CHR_CHANCECRITICAL');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (8, 'BLOCK_RANGE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (7, 'CHR_RANGE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (6, 'BOW_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (5, 'YOY_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (4, 'STA');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (3, 'INT');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (14, 'BLOCK_MELEE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (2, 'DEX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (29, 'REFLECT_DAMAGE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (31, 'RESIST_WIND');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (56, 'CHR_STEALHP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (55, 'CHR_WEAEATKCHANGE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (54, 'FP_MAX_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (53, 'MP_MAX_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (52, 'HP_MAX_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (51, 'ATTACKSPEED_RATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (50, 'CHR_STEALHP_IMM');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (49, 'CLEARBUFF');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (47, 'ADJ_HITRATE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (46, 'KILL_FP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (45, 'KILL_MP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (30, 'RESIST_FIRE');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (44, 'KILL_HP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (42, 'MP_RECOVERY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (41, 'HP_RECOVERY');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (40, 'FP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (39, 'MP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (38, 'HP');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (37, 'FP_MAX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (36, 'MP_MAX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (35, 'HP_MAX');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (34, 'AXE_DMG');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (33, 'RESIST_EARTH');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (32, 'RESIST_WATER');
INSERT INTO `Attributes` (`Id`, `Name`)
VALUES (43, 'FP_RECOVERY');

CREATE INDEX `IX_ItemAttributes_AttributeId` ON `ItemAttributes` (`AttributeId`);

CREATE UNIQUE INDEX `IX_ItemAttributes_ItemId_AttributeId` ON `ItemAttributes` (`ItemId`, `AttributeId`);

CREATE INDEX `IX_ItemsStorage_ItemId` ON `ItemsStorage` (`ItemId`);

CREATE INDEX `IX_ItemsStorage_StorageTypeId` ON `ItemsStorage` (`StorageTypeId`);

ALTER TABLE `Characters` ADD CONSTRAINT `FK_Characters_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`);

ALTER TABLE `mails` ADD CONSTRAINT `FK_mails_Items_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `Items` (`Id`) ON DELETE RESTRICT;

ALTER TABLE `mails` ADD CONSTRAINT `FK_mails_Characters_ReceiverId` FOREIGN KEY (`ReceiverId`) REFERENCES `Characters` (`Id`) ON DELETE CASCADE;

ALTER TABLE `mails` ADD CONSTRAINT `FK_mails_Characters_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `Characters` (`Id`) ON DELETE CASCADE;

ALTER TABLE `Quests` ADD CONSTRAINT `FK_Quests_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `Characters` (`Id`);

ALTER TABLE `shortcuts` ADD CONSTRAINT `FK_shortcuts_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `Characters` (`Id`) ON DELETE RESTRICT;

ALTER TABLE `Skills` ADD CONSTRAINT `FK_Skills_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `Characters` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200704112456_v05x_ItemStructure', '3.1.3');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;


