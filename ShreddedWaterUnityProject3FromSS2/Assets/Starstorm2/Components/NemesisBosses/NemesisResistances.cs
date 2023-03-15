﻿using R2API.Utils;
using RoR2;
using UnityEngine;

namespace Moonstorm.Starstorm2.Components
{
    public class NemesisResistances : MonoBehaviour
    {
        static NemesisResistances()
        {
            On.RoR2.HealthComponent.Suicide += ResistVoid;
            On.RoR2.MapZone.TryZoneStart += TPBack;
        }

        private static void ResistVoid(On.RoR2.HealthComponent.orig_Suicide orig, RoR2.HealthComponent self, GameObject killerOverride, GameObject inflictorOverride, DamageType damageType)
        {
            var body = self.body;
            if (body)
            {
                var master = body.master;
                if (master)
                {
                    if (damageType == DamageType.VoidDeath && master.GetComponent<NemesisResistances>())
                    {
                        //ChatMessage.SendColored("He laughs in the face of the void.", ColorCatalog.ColorIndex.VoidItem);

                        int rng = Run.instance.runRNG.RangeInt(1, 4);
                        switch (rng)
                        {
                            case 1:
                                ChatMessage.Send(body.GetDisplayName() + ": <link=\"textShaky\">Not like that</link>.");
                                break;
                            case 2:
                                ChatMessage.Send(body.GetDisplayName() + ": <link=\"textShaky\">It's not over yet</link>.");
                                break;
                            case 3:
                                ChatMessage.Send(body.GetDisplayName() + ": <link=\"textShaky\">Old trick</link>.");
                                break;
                        }

                        return;
                    }
                }
            }
            orig(self, killerOverride, inflictorOverride, damageType);
        }

        private static void TPBack(On.RoR2.MapZone.orig_TryZoneStart orig, RoR2.MapZone self, Collider other)
        {
            if (other.gameObject)
            {
                var body = other.GetComponent<CharacterBody>();
                if (body.master.GetComponent<NemesisResistances>())
                {
                    var teamComponent = body.teamComponent;
                    if (teamComponent)
                    {
                        if (teamComponent.teamIndex != TeamIndex.Player)
                        {
                            teamComponent.teamIndex = TeamIndex.Player;
                            orig(self, other);
                            teamComponent.teamIndex = TeamIndex.Monster;
                            return;
                        }
                    }
                }
            }
            orig(self, other);
        }
    }
}
