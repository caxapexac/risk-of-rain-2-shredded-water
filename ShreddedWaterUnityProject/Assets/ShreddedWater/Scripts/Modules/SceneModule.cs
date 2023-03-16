using R2API.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class SceneModule : SceneModuleBase
    {
        public static SceneModule Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info("Initializing Scenes.");
            GetSceneBases();
        }

        public override IEnumerable<SceneBase> GetSceneBases()
        {
            base.GetSceneBases()
                .ToList()
                .ForEach(scene => AddScene(scene));

            return null;
        }
    }
}