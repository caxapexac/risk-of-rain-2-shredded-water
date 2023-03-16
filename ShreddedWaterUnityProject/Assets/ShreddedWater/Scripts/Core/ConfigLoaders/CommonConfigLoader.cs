using BepInEx;
using BepInEx.Configuration;
using Moonstorm.Loaders;


namespace ShreddedWater
{
    public abstract class CommonConfigLoader<TSelf> : ConfigLoader<TSelf> where TSelf : ConfigLoader<TSelf>
    {
        private const string ConfigNameGeneral = "General";
        private const string ConfigNameCharacters = "Characters";
        private const string ConfigNameItems = "Items";

        public override bool CreateSubFolder => true;

        public ConfigFile ConfigGeneral;
        public ConfigFile ConfigCharacters;
        public ConfigFile ConfigItems;

        internal ConfigEntry<bool> EntryUnlockAll;

        protected readonly BaseUnityPlugin Plugin;

        protected CommonConfigLoader(BaseUnityPlugin plugin)
        {
            Plugin = plugin;
        }

        public virtual void Init()
        {
            ConfigGeneral = CreateConfigFile(ConfigNameGeneral);
            ConfigCharacters = CreateConfigFile(ConfigNameCharacters);
            ConfigItems = CreateConfigFile(ConfigNameItems);

            EntryUnlockAll = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: Unlock All",
                "Enabled",
                false,
                $"Setting this to true unlocks all the content in {Plugin.Info.Metadata.Name}, excluding skin unlocks.");
        }

        // This helper automatically makes config entries for enabling/disabling survivors
        internal ConfigEntry<bool> CharacterEnableConfig(string characterName, string displayName)
        {
            return ConfigCharacters.Bind(
                $"{Plugin.Info.Metadata.Name} :: {characterName}",
                "Enabled",
                true,
                $"Enables {Plugin.Info.Metadata.Name}'s {displayName} survivor");
        }
    }
}