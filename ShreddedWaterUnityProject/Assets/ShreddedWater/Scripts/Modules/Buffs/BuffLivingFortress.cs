using Moonstorm;
using Moonstorm.Components;
using R2API;
using RoR2;
using UnityEngine;

namespace ShreddedWater.Buffs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BuffLivingFortress : BuffBase
    {
        public override BuffDef BuffDef { get; } = SWAssetsLoader.Instance.LoadAsset<BuffDef>("bdLivingFortress", SWBundleEnum.Main);

        public override Material OverlayMaterial { get; } = null;

        public sealed class Behavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation]
            private static BuffDef GetBuffDef() => SWContentLoader.Buffs.bdLivingFortress;
            
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.damageMultAdd += 0.1f * buffStacks;
            }
        }
    }
}
