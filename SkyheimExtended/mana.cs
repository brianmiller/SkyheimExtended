using HarmonyLib;
using UnityEngine;

namespace SkyheimExtended.Mana
{
    [HarmonyPatch(typeof(skyheim.SkyheimMana), nameof(skyheim.SkyheimMana.UpdateMana))]
    public class SkyheimMana_Patch
    {
        public static float skillLevel = 1;
        public static float regenScaled = 1;
        public static float manaScaled = 1;
        public static float manaStaticDrain = 1;
        public static double regenScaleFactor = 1;
        public static double manaScaleFactor = 1;
        public static string skillFlavor;
        public static float skyheimExtended_maxMana;
        public static float skyheimExtended_manaRegen;
        public static float skyheimExtended_manaDrain;

        public static void Postfix(ref float ____manaRegen, ref float ____maxMana)
        {
            //only run code when a player is active
            if ((Object)(object)Player.m_localPlayer != (Object)null)
            {

                //currently equipped weapon      
                ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();

                //get component object of currentWeapon (used for manaDrain stuff)
                SkyheimItemData itemComponent = CommonUtils.GetItemComponent<SkyheimItemData>(currentWeapon);

                //only run code when a weapon is equipped. If no weapon is equipped, do not run.
                if (!Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab)
                {
                    //we set regen and mana values to whatever the travel rune is configured to
                    //use in our config file. This is because of the way the travel rune works.
                    //If the travel rune is equipped, but not armed (on back), the mana needs to
                    //regenerate at the same rate and capacity that exists in the config file.
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaled = (float)(skillLevel * SkyheimExtended.scalingTravelRegenFactor.Value + 3);
                    manaScaled = (float)(skillLevel * SkyheimExtended.scalingTravelManaFactor.Value + 100);
                    ____manaRegen = regenScaled;
                    ____maxMana = manaScaled;
                    return;
                }

                //rune condtionals
                if (currentWeapon.m_dropPrefab.name == "rune_firebolt")
                {
                    skillFlavor = "FireMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingFireBoltRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingFireBoltManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticFireBoltManaDrain.Value;
                }
                if (currentWeapon.m_dropPrefab.name == "rune_immolate")
                {
                    skillFlavor = "FireMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingImmolateRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingImmolateManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticImmolateManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_travel")
                {
                    skillFlavor = "FireMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingTravelRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingTravelManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticTravelManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_warmth")
                {
                    skillFlavor = "FireMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingWarmthRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingWarmthManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticWarmthManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_frostbolt")
                {
                    skillFlavor = "FrostMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingFrostBoltRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingFrostBoltManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticFrostBoltManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_glacial_spike")
                {
                    skillFlavor = "FrostMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingGlacialSpikeRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingGlacialSpikeManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticGlacialSpikeManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_invigorate")
                {
                    skillFlavor = "FrostMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingInvigorateRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingInvigorateManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticInvigorateManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_recall")
                {
                    skillFlavor = "FrostMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingRecallRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingRecallManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticRecallManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_light")
                {
                    skillFlavor = "HolyMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.HolyMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingLightRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingLightManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticLightManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_blink")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingBlinkRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingBlinkManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticBlinkManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_chain_lightning")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingChainLightningRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingChainLightningManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticChainLightningManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_force")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingForceRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingForceManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticForceManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_frostnova")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingFrostNovaRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingFrostNovaManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticFrostNovaManaDrain.Value;
                }
                else if (currentWeapon.m_dropPrefab.name == "rune_heal")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.scalingHealRegenFactor.Value;
                    manaScaleFactor = SkyheimExtended.scalingHealManaFactor.Value;
                    manaStaticDrain = SkyheimExtended.staticHealManaDrain.Value;
                }

                //scale it
                regenScaled = (float)(skillLevel * regenScaleFactor + 3);
                manaScaled = (float)(skillLevel * manaScaleFactor + 100);

                //treat the travel rune differently
                if (currentWeapon.m_dropPrefab.name == "rune_travel")
                {
                    //do not regenerate mana when travel rune is in use
                    regenScaled = 0.001F;
                    //scale mana
                    ____manaRegen = regenScaled;
                    //drain mana
                    itemComponent.ManaDrain = manaStaticDrain;
                }

                else if (SkyheimExtended.scaleWithSkill.Value)
                {
                    ____manaRegen = regenScaled;
                    ____maxMana = manaScaled;

                }
                //custom statics from config

                else if (!SkyheimExtended.scaleWithSkill.Value)
                {
                    ____manaRegen = SkyheimExtended.staticManaRegen.Value;
                    ____maxMana = SkyheimExtended.staticMaxMana.Value;
                }

                else
                {
                    //skyheim's defaults
                    ____manaRegen = 3f;
                    ____maxMana = 100f;
                }

                //adjust debug output when travel rune is selected (different drain mechanics)
                if (currentWeapon.m_dropPrefab.name == "rune_travel")
                {
                    skyheimExtended_manaRegen = (float)(skillLevel * SkyheimExtended.scalingTravelRegenFactor.Value + 3);
                    skyheimExtended_maxMana = ____maxMana;
                    skyheimExtended_manaDrain = manaStaticDrain;
                }
                else
                {
                    skyheimExtended_manaRegen = ____manaRegen;
                    skyheimExtended_maxMana = ____maxMana;
                    skyheimExtended_manaDrain = manaStaticDrain;
                }
            }
        }
    }
}
