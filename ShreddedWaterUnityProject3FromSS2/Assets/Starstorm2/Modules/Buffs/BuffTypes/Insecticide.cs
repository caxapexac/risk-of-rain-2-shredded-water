﻿using Moonstorm.Components;
using Moonstorm.Starstorm2.Items;
using R2API;
using RoR2;

namespace Moonstorm.Starstorm2.Buffs
{
    public sealed class Insecticide : BuffBase
    {
        public override BuffDef BuffDef { get; } = SS2Assets.LoadAsset<BuffDef>("BuffInsecticide", SS2Bundle.Items);
        public static DotController.DotIndex index;

        public override void Initialize()
        {
            index = DotAPI.RegisterDotDef(0.25f, 0.15f, DamageColorIndex.DeathMark, BuffDef);
        }
    }
}
