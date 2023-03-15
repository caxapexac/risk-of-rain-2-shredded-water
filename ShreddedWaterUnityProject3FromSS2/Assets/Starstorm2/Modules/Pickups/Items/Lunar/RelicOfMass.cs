﻿using R2API;
using RoR2;
using RoR2.Items;

namespace Moonstorm.Starstorm2.Items
{
    public sealed class RelicOfMass : ItemBase
    {
        public override ItemDef ItemDef { get; } = SS2Assets.LoadAsset<ItemDef>("RelicOfMass", SS2Bundle.Items);

        [ConfigurableField(ConfigDesc = "Amount of which acceleration is divided by.")]
        public static float acclMult = 8f;

        public sealed class Behavior : BaseItemBodyBehavior, IBodyStatArgModifier, IStatItemBehavior
        {
            [ItemDefAssociation]
            private static ItemDef GetItemDef() => SS2Content.Items.RelicOfMass;
            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.baseHealthAdd += (body.baseMaxHealth + (body.levelMaxHealth * (body.level - 1))) * stack;
                //args.moveSpeedMultAdd += stack / 2;
            }
            public void RecalculateStatsStart()
            {

            }

            public void RecalculateStatsEnd()
            {
                body.acceleration = body.baseAcceleration / (stack * acclMult);
            }
        }
    }
}
