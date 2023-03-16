using RoR2;


namespace ShreddedWater
{
    public sealed class SWSoundLoader
    {
        private readonly SWPlugin _plugin;

        public SWSoundLoader(SWPlugin plugin)
        {
            _plugin = plugin;
        }

        // ReSharper disable once UnusedMember.Global
        public string SoundBankDirectory
        {
            get => System.IO.Path.Combine(_plugin.AssemblyDir, "soundbanks");
        }

        public void Init()
        {
#if Wwise
            //LogCore.LogE(AkSoundEngine.ClearBanks().ToString());
            AkSoundEngine.AddBasePath(SoundBankDirectory);
            AkSoundEngine.LoadFilePackage("Starstorm2.pck", out var packageID/*, -1*/);
            AkSoundEngine.LoadBank("Starstorm2", /*-1,*/ out var bank);
            AkSoundEngine.LoadBank("SS2Init", /*-1,*/ out var bitch);
#endif
        }

        [SystemInitializer(dependencies: typeof(MusicTrackCatalog))]
        public static void MusicInit()
        {
#if Wwise
            AkSoundEngine.LoadBank("SS2Music", /*-1,*/ out var bank);
            GameObject.Instantiate(SS2Assets.Instance.MainAssetBundle.LoadAsset("SS2MusicInitializer"));
#endif
        }
    }
}