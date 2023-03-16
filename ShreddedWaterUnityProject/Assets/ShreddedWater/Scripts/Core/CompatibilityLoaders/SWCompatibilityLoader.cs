namespace ShreddedWater
{
    public sealed class SWCompatibilityLoader
    {
        // TODO generic singleton
        // public static bool ScepterInstalled = false;
        // public static bool RiskyModInstalled = false;

        public void Init()
        {
            // ScepterInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            // RiskyModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
        }
    }
}