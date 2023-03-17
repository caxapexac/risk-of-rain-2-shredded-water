using Moonstorm;
using R2API;
using RoR2;
using RoR2.Items;


namespace ShreddedWater.Items
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class LivingFortress : ItemBase
    {
        public override ItemDef ItemDef { get; } = SWAssetsLoader.Instance.LoadAsset<ItemDef>("LivingFortress", SWBundleEnum.Main);

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SWContentLoader.Items.LivingFortress;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.armorAdd += 1.0f;
            }
        }
    }
}