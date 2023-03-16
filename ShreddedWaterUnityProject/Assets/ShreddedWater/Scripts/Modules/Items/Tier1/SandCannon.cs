using Moonstorm;
using R2API;
using RoR2;
using RoR2.Items;

namespace ShreddedWater
{
    public sealed class SandCannon : ItemBase
    {
        public override ItemDef ItemDef { get; } = SWAssetsLoader.Instance.LoadAsset<ItemDef>("SandCannon", SWBundleEnum.Main);

        [ConfigurableField(ConfigDesc = "Damage multiplier for sand cannon")]
        [TokenModifier("SW_ITEM_SANDCANNON_DESC", StatTypes.Default, 0)]
        public static float damageBonus = 2.0f;

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SWContentLoader.Items.SandCannon;
            
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.baseDamageAdd += damageBonus * stack;
            }
        }
    }
}