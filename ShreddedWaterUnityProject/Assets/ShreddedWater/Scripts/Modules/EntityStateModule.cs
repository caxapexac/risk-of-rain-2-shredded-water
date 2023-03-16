using System.Linq;
using Moonstorm.Starstorm2;
using R2API.ScriptableObjects;
using RoR2;


namespace ShreddedWater
{
    public sealed class EntityStateModule
    {
        private static R2APISerializableContentPack SerializableContentPack => SWContentLoader.Instance.SerializableContentPack;

        public static void Init()
        {
            SS2Log.Info("Populating entity state array");
            typeof(EntityStateModule)
                .Assembly.GetTypes()
                .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                .ToList()
                .ForEach(state => HG.ArrayUtils.ArrayAppend(ref SerializableContentPack.entityStateTypes, new EntityStates.SerializableEntityStateType(state)));

            SS2Log.Info("Populating EntityStateConfigurations");
            SerializableContentPack.entityStateConfigurations = SWAssetsLoader.Instance.LoadAllAssetsByTypeFromAnyBundle<EntityStateConfiguration>();
        }
    }
}