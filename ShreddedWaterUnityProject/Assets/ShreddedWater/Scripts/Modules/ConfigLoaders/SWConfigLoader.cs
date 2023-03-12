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

        public override BaseUnityPlugin MainClass => SWPlugin.Instance;
        
        public static ConfigFile ConfigItems;
        public static ConfigFile ConfigEquips;

        internal static ConfigEntry<KeyCode> RestKeyCode;
        internal static KeyCode RestKeyCodeCached;
        internal static ConfigEntry<KeyCode> TauntKeyCode;
        internal static KeyCode TauntKeyCodeCached;

        // internal static List<ConfigEntry<bool>> ItemToggles; // TODO auto toggles in config

        public SWConfigLoader(BaseUnityPlugin plugin) : base(plugin)
        {
        }
        
        public override void Init() 
        {
            base.Init();
            
            ConfigItems = CreateConfigFile(ConfigNameItems);
            ConfigEquips = CreateConfigFile(ConfigNameEquips);

            RestKeyCode = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: KeyBinds",
                "Rest Emote",
                KeyCode.Alpha1,
                "KeyCode used for the Rest emote.");
            RestKeyCodeCached = RestKeyCode.Value;

            TauntKeyCode = ConfigGeneral.Bind(
                $"{Plugin.Info.Metadata.Name} :: KeyBinds",
                "Taunt Emote",
                KeyCode.Alpha2,
                "KeyCode used for the Taunt emote.");
            TauntKeyCodeCached = TauntKeyCode.Value;
        }
    }
}