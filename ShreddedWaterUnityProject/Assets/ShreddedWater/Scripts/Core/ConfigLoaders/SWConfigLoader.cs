using BepInEx;
using BepInEx.Configuration;
using Moonstorm.Loaders;
using UnityEngine;


namespace ShreddedWater
{
    public sealed class SWConfigLoader : CommonConfigLoader<SWConfigLoader>
    {
        public const string ConfigNameItems = "Items";
        public const string ConfigNameEquips = "Equips";
        public const string ConfigNameInteractables = "Interactables";

        public override BaseUnityPlugin MainClass => SWPlugin.Instance;

        public ConfigFile ConfigItems;
        public ConfigFile ConfigEquips;
        public ConfigFile ConfigInteractables;

        internal ConfigEntry<bool> EntryEnableEvents;
        internal ConfigEntry<KeyCode> EntryRestKeyCode;
        internal static KeyCode CachedRestKeyCode;
        internal ConfigEntry<KeyCode> EntryTauntKeyCode;
        internal static KeyCode CachedTauntKeyCode;

        // internal static List<ConfigEntry<bool>> ItemToggles; // TODO auto toggles in config

        public SWConfigLoader(BaseUnityPlugin plugin) : base(plugin)
        {
        }

        public override void Init()
        {
            base.Init();

            ConfigItems = CreateConfigFile(ConfigNameItems);
            ConfigEquips = CreateConfigFile(ConfigNameEquips);
            ConfigInteractables = CreateConfigFile(ConfigNameInteractables);

            EntryEnableEvents = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: Events",
                "Enabled",
                true,
                $"Enables {Plugin.Info.Metadata.Name}'s random events");

            EntryRestKeyCode = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: KeyBinds",
                "Rest Emote",
                KeyCode.Alpha1,
                "KeyCode used for the Rest emote");
            CachedRestKeyCode = EntryRestKeyCode.Value;

            EntryTauntKeyCode = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: KeyBinds",
                "Taunt Emote",
                KeyCode.Alpha2,
                "KeyCode used for the Taunt emote");
            CachedTauntKeyCode = EntryTauntKeyCode.Value;
        }
    }
}