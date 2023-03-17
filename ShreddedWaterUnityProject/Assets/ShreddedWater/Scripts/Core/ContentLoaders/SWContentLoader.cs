using R2API.ScriptableObjects;
using RoR2;
using System;
using Moonstorm.Starstorm2;


namespace ShreddedWater
{
    public sealed class SWContentLoader : CommonContentLoader<SWContentLoader>
    {
        #region Reflection Generated Data

        // ReSharper disable UnassignedField.Global
        // ReSharper disable MemberCanBePrivate.Global

        public static class Artifacts
        {
            // public static ArtifactDef _;
        }


        public static class Items
        {
            // Tier 1
            public static ItemDef SandCannon;
            
            // Tier 2
            
            // Tier 3
            public static ItemDef LivingFortress;
            
            // Tier Boss
            
            // Tier Hidden
            
            // Tier Lunar
        }


        public static class Equipments
        {
            // public static EquipmentDef _;

            public static EquipmentDef ItemInjector;
        }


        public static class Buffs
        {
            // public static BuffDef _;
        }


        public static class Elites
        {
            //public static EliteDef _;
        }


        public static class Survivors
        {
            //public static SurvivorDef _;
        }


        public static class ItemTiers
        {
            // public static ItemTierDef _;
        }


        // ReSharper restore UnassignedField.Global
        // ReSharper restore MemberCanBePrivate.Global

        #endregion


        public override string identifier => SWPlugin.Instance.Info.Metadata.GUID;

        public override R2APISerializableContentPack SerializableContentPack { get; protected set; } =
            SWAssetsLoader.Instance.LoadAsset<R2APISerializableContentPack>("ContentPack", SWBundleEnum.Main);

        public override Action[] LoadDispatchers { get; protected set; }

        public override Action[] PopulateFieldsDispatchers { get; protected set; }

        public override void Init()
        {
            base.Init();

            LoadDispatchers = new Action[]
            {
                () => new SceneModule().Initialize(),
                () => new ItemTierModule().Initialize(),
                () => new ItemModule().Initialize(),
                () => new EquipmentModule().Initialize(),
                () => new BuffModule().Initialize(),
                () => new DamageTypeModule().Initialize(),
                () => new ProjectileModule().Initialize(),
                () => new EliteModule().Initialize(),
                DifficultyModule.Init,
                EventModule.Init,
                () => new CharacterModule().Initialize(),
                () => new ArtifactModule().Initialize(),
                () => new InteractableModule().Initialize(),
                () => new UnlockablesModule().Initialize(),
                EntityStateModule.Init,
                EffectModule.Init,
                () =>
                {
                    SS2Log.Info("Swapping material shaders");
                    SWAssetsLoader.Instance.SwapMaterialShaders();
                    SWAssetsLoader.Instance.FinalizeCopiedMaterials();
                }
            };

            PopulateFieldsDispatchers = new Action[]
            {
                () => PopulateTypeFields(typeof(Artifacts), ContentPack.artifactDefs),
                () => PopulateTypeFields(typeof(Items), ContentPack.itemDefs),
                () => PopulateTypeFields(typeof(Equipments), ContentPack.equipmentDefs),
                () => PopulateTypeFields(typeof(Buffs), ContentPack.buffDefs),
                () => PopulateTypeFields(typeof(Elites), ContentPack.eliteDefs),
                () => PopulateTypeFields(typeof(Survivors), ContentPack.survivorDefs),
                () => PopulateTypeFields(typeof(ItemTiers), ContentPack.itemTierDefs)
            };
        }
    }
}