using BepInEx;
using BepInEx.Configuration;


namespace Moonstorm.Loaders
{
    public abstract class CommonConfigLoader<TSelf> : ConfigLoader<TSelf> where TSelf : ConfigLoader<TSelf>
    {
        public const string ConfigNameGeneral = "General";
        public const string ConfigNameCharacters = "Characters";
        
        public override bool CreateSubFolder => true;

        public ConfigFile ConfigGeneral;
        public ConfigFile ConfigCharacters;
        
        internal ConfigEntry<bool> EntryUnlockAll;

        protected readonly BaseUnityPlugin Plugin;

        public CommonConfigLoader(BaseUnityPlugin plugin)
        {
            Plugin = plugin;
        }

        public virtual void Init()
        {
            ConfigGeneral = CreateConfigFile(ConfigNameGeneral);
            ConfigCharacters = CreateConfigFile(ConfigNameCharacters);

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