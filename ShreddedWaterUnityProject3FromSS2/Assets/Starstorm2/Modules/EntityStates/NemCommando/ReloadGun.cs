﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonstorm;
using RoR2;
using UnityEngine.AddressableAssets;

namespace EntityStates.NemCommando
{
    public class ReloadGun : BaseState
    {
        private bool hasReloaded;
        public static float baseDuration;
        public static string reloadEffectMuzzleString;
        public static GameObject reloadEffectPrefab;

        public static string enterSoundString;
        public static string exitSoundString;

        public static float exitSoundpitch;
        public static float enterSoundPitch;

        private Animator animator;
        private bool hasEjectedMag = false;

        private float duration
        {
            get
            {
                return baseDuration / skillLocator.secondary.cooldownScale;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            animator = GetModelAnimator();

            PlayCrossfade("Gesture, Override, LeftArm", "LowerGun", "FireGun.playbackRate", duration, 0.03f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (animator.GetFloat("ejectMag") >= 0.1 && !hasEjectedMag)
            {
                hasEjectedMag = true;

                FindModelChild("magParticle").GetComponent<ParticleSystem>().Emit(1);
            }

            if (fixedAge >= duration / 1.6 && !hasReloaded)
            {
                Util.PlayAttackSpeedSound(enterSoundString, gameObject, enterSoundPitch);
                skillLocator.secondary.stock = skillLocator.secondary.maxStock;
                hasReloaded = true;
            }
            if (!isAuthority || fixedAge < duration)
            {
                return;
            }
            outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
