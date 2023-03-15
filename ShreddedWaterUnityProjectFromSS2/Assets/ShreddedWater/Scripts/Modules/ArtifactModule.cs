using R2API;
using R2API.ScriptableObjects;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using Moonstorm;
using Moonstorm.Starstorm2;
using UnityEngine;

namespace ShreddedWater
{
    public sealed class ArtifactModule : ArtifactModuleBase
    {
        public static ArtifactModule Instance { get; private set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = SWContentLoader.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            SS2Log.Info($"Initializing Artifacts");
            GetArtifactBases();

            // TODO this adds another shape to cycle button on 3x3 artifact field
            // var compound = SWAssetsLoader.LoadAsset<ArtifactCompoundDef>("acdStar", SWBundleEnum.Artifacts);
            // compound.decalMaterial.shader = Resources.Load<ArtifactCompoundDef>("artifactcompound/acdCircle").decalMaterial.shader;
            // ArtifactCodeAPI.AddCompound(compound);
        }

        protected override IEnumerable<ArtifactBase> GetArtifactBases()
        {
            base.GetArtifactBases()
                .ToList()
                .ForEach(artifact => AddArtifact(artifact));
            return null;
        }
    }
}