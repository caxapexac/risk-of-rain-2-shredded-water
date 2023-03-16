using BepInEx;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    #region R2API

    [BepInDependency("com.bepis.r2api.artifactcode")]
    [BepInDependency("com.bepis.r2api.colors")]
    [BepInDependency("com.bepis.r2api.commandhelper")]
    [BepInDependency("com.bepis.r2api.content_management")]
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.bepis.r2api.damagetype")]
    [BepInDependency("com.bepis.r2api.deployable")]
    [BepInDependency("com.bepis.r2api.difficulty")]
    [BepInDependency("com.bepis.r2api.director")]
    [BepInDependency("com.bepis.r2api.dot")]
    [BepInDependency("com.bepis.r2api.elites")]
    [BepInDependency("com.bepis.r2api.items")]
    [BepInDependency("com.bepis.r2api.language")]
    [BepInDependency("com.bepis.r2api.loadout")]
    [BepInDependency("com.bepis.r2api.lobbyconfig")]
    [BepInDependency("com.bepis.r2api.networking")]
    [BepInDependency("com.bepis.r2api.orb")]
    [BepInDependency("com.bepis.r2api.prefab")]
    [BepInDependency("com.bepis.r2api.recalculatestats")]
    [BepInDependency("com.bepis.r2api.rules")]
    [BepInDependency("com.bepis.r2api.sceneasset")]
    [BepInDependency("com.bepis.r2api.sound")]
    [BepInDependency("com.bepis.r2api.tempvisualeffect")]
    [BepInDependency("com.bepis.r2api.unlockable")]

    #endregion

    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils")]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.RiskyLives.RiskyMod", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(Guid, ModName, Version)]
    public sealed class SWPlugin : CommonUnityPlugin<SWPlugin>
    {
        private const string Guid = "com.caxapexac.ShreddedWater";
        private const string ModName = "Shredded Water";
        private const string Version = "0.0.1";

        private SWAssetsLoader _assetsLoader;
        private SWConfigLoader _configLoader;
        private SWContentLoader _contentLoader;
        private SWLanguageLoader _languageLoader;
        private SWSoundLoader _soundLoader;
        private SWCompatibilityLoader _compatibilityLoader;

        protected override void OnAwake()
        {
            SS2Log.logger = Logger;
            SS2Log.Info("SW Awakening");

#if DEBUG
            SS2Log.Warning("SW Actually Debug");
#endif
            gameObject.AddComponent<SWDebugUtil>(); // TODO https://discord.com/channels/786037647263924224/850009338021019688/1086022838818975788

            _assetsLoader = new SWAssetsLoader();
            _assetsLoader.Init();
            _configLoader = new SWConfigLoader(this);
            _configLoader.Init();
            _contentLoader = new SWContentLoader();
            _contentLoader.Init();
            _languageLoader = new SWLanguageLoader();
            _languageLoader.Init();
            ConfigurableFieldManager.AddMod(this);

            //N: i have no idea if SystemInitializer would be too late for this, so it stays here for now.
            // R2API.Networking.NetworkingAPI.RegisterMessageType<ScriptableObjects.NemesisSpawnCard.SyncBaseStats>();
            SS2Log.Info("Awaken");
        }

        protected override void OnStart()
        {
            _soundLoader = new SWSoundLoader(this);
            _soundLoader.Init();
            _compatibilityLoader = new SWCompatibilityLoader();
            _compatibilityLoader.Init();
        }
    }
}