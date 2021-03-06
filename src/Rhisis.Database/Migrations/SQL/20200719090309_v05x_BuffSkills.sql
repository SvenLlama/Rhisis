ALTER TABLE `ItemsStorage` MODIFY COLUMN `Updated` DATETIME NOT NULL DEFAULT NOW();

CREATE TABLE `SkillBuffs` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CharacterId` int NOT NULL,
    `SkillId` int NOT NULL,
    `SkillLevel` TINYINT NOT NULL,
    `RemainingTime` int NOT NULL,
    CONSTRAINT `PK_SkillBuffs` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_SkillBuffs_Characters_CharacterId` FOREIGN KEY (`CharacterId`) REFERENCES `Characters` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `SkillBuffAttributes` (
    `SkillBuffId` int NOT NULL,
    `AttributeId` int NOT NULL,
    `Value` int NOT NULL,
    CONSTRAINT `PK_SkillBuffAttributes` PRIMARY KEY (`SkillBuffId`, `AttributeId`),
    CONSTRAINT `FK_SkillBuffAttributes_Attributes_AttributeId` FOREIGN KEY (`AttributeId`) REFERENCES `Attributes` (`Id`),
    CONSTRAINT `FK_SkillBuffAttributes_SkillBuffs_SkillBuffId` FOREIGN KEY (`SkillBuffId`) REFERENCES `SkillBuffs` (`Id`)
);

CREATE INDEX `IX_ItemsStorage_CharacterId` ON `ItemsStorage` (`CharacterId`);

CREATE INDEX `IX_SkillBuffAttributes_AttributeId` ON `SkillBuffAttributes` (`AttributeId`);

CREATE INDEX `IX_SkillBuffAttributes_SkillBuffId_AttributeId` ON `SkillBuffAttributes` (`SkillBuffId`, `AttributeId`);

CREATE UNIQUE INDEX `IX_SkillBuffs_CharacterId_SkillId` ON `SkillBuffs` (`CharacterId`, `SkillId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200719090309_v05x_BuffSkills', '3.1.3');


