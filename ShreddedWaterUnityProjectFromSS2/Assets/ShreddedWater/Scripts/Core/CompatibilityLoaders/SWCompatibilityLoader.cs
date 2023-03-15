namespace ShreddedWater
{
    public class SWCompatibilityLoader
    {
        public static bool ScepterInstalled = false; // TODO generic singleton
        public static bool RiskyModInstalled = false; // TODO generic singleton
        
        public void Init()
        {
            ScepterInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            RiskyModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
        }
    }
}