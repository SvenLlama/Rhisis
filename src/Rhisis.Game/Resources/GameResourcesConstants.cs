using Rhisis.Game.Common.Resources;
using Rhisis.Game.Resources.Loaders;
using System.IO;

namespace Rhisis.Game.Resources
{
    public class GameResourcesConstants
    {
        // Cache keys
        public const string Defines = nameof(Defines);
        public const string Texts = nameof(Texts);
        public const string Movers = nameof(MoverData);
        public const string Items = nameof(ItemData);
        public const string Dialogs = nameof(DialogLoader);
        public const string Shops = nameof(ShopLoader);
        public const string Jobs = nameof(JobData);
        public const string ExpTables = nameof(ExpTableLoader);
        public const string PenalityData = nameof(PenalityLoader);
        public const string Npcs = nameof(NpcLoader);
        public const string Quests = nameof(Quests);
        public const string Skills = nameof(SkillLoader);

        public class Errors
        {
            public const string UnableLoadMapMessage = "Unable to load map '{0}'. Reason: {1}.";
            public const string UnableLoadMessage = "Unable to load {0}. Reason: {1}";
            public const string ObjectIgnoredMessage = "{0} id '{1}' was ignored. Reason: {2}.";
            public const string ObjectOverridedMessage = "{0} id '{1}' was overrided. Reason: {2}.";
            public const string ObjectErrorMessage = "{0} with id '{1}' has an error. Reason: {2}";
        }

        public class Paths
        {
            public static string WorkingDirectoryPath { get; set; } = Directory.GetCurrentDirectory();
            public static string DataPath => Path.Combine(WorkingDirectoryPath, "data");
            public static string DialogsPath => Path.Combine(DataPath, "dialogs");
            public static string ResourcePath => Path.Combine(DataPath, "res");
            public static string MapsPath => Path.Combine(DataPath, "maps");
            public static string ShopsPath => Path.Combine(DataPath, "shops");
            public static string QuestsPath => Path.Combine(DataPath, "quests");
            public static string DataSub0Path => Path.Combine(ResourcePath, "data");
            public static string DataSub1Path => Path.Combine(ResourcePath, "dataSub1");
            public static string DataSub2Path => Path.Combine(ResourcePath, "dataSub2");
            public static string MoversPropPath => Path.Combine(DataSub0Path, "propMover.txt");
            public static string MoversPropExPath => Path.Combine(DataSub0Path, "propMoverEx.inc");
            public static string ItemsPropPath => Path.Combine(DataSub2Path, "propItem.txt");
            public static string WorldScriptPath => Path.Combine(DataSub0Path, "World.inc");
            public static string JobPropPath => Path.Combine(DataSub1Path, "propJob.inc");
            public static string TextClientPath => Path.Combine(DataSub1Path, "textClient.inc");
            public static string ExpTablePath => Path.Combine(DataSub0Path, "expTable.inc");
            public static string SkillPropPath => Path.Combine(DataSub0Path, "propSkill.txt");
            public static string SkillPropAddPath => Path.Combine(DataSub0Path, "propSkillAdd.csv");

            // Rhisis related
            public static string DeathPenalityPath => Path.Combine(ResourcePath, "deathPenality.json");
            public static string JobsDefinitionsPath => Path.Combine(ResourcePath, "jobsDefinitions.json");
        }

        public class QuestInstructions
        {
            public const string SetTitle = "SetTitle";
            public const string SetCharacter = "SetCharacter";
            public const string SetEndCharacter = "SetEndCondCharacter";
            public const string SetBeginLevel = "SetBeginCondLevel";
            public const string SetBeginPreviousQuest = "SetBeginCondPreviousQuest";
            public const string SetBeginJob = "SetBeginCondJob";
        }
    }
}
