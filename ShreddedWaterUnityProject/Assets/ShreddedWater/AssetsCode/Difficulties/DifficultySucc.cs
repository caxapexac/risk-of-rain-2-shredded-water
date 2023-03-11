using R2API;
using R2API.Utils;
using RoR2;
using ShreddedWater;


namespace ShreddedWater
{
    public static class DifficultySucc
    {
        public static R2API.ScriptableObjects.SerializableDifficultyDef SuccDef { get; private set; }
        public static DifficultyIndex SuccIndex { get => SuccDef.DifficultyIndex; }

        private static int defMonsterCap;

        internal static void Init()
        {
            SuccDef = SWAssetsLoader.LoadAsset<R2API.ScriptableObjects.SerializableDifficultyDef>("Succ", SWBundleEnum.Main);
            DifficultyAPI.AddDifficulty(SuccDef);
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
        }

        private static void Run_onRunStartGlobal(Run run)
        {
            defMonsterCap = TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit;
            if (run.selectedDifficulty == SuccIndex)
            {
                foreach (CharacterMaster cm in run.userMasters.Values)
                    cm.inventory.GiveItem(RoR2Content.Items.MonsoonPlayerHelper.itemIndex);
                if (SWConfigLoader.TyphoonIncreaseSpawnCap.Value)
                    TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit *= 2;
            }
        }

        private static void Run_onRunDestroyGlobal(Run run)
        {
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = defMonsterCap;
        }
    }
}
