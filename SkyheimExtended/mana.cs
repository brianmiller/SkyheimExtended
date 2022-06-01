using HarmonyLib;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SkyheimExtended.Mana
{
    [HarmonyPatch(typeof(skyheim.SkyheimMana), nameof(skyheim.SkyheimMana.UpdateMana))]
    public class SkyheimMana_Patch
    {
        public static float skillLevel = 1;
        public static float regenScaled = 1;
        public static float manaScaled = 1;
        public static double regenScaleFactor = 1;
        public static double manaScaleFactor = 1;
        public static string skillFlavor;
        public static float skyheimExtended_maxMana;
        public static float skyheimExtended_manaRegen;
        public static float skyheimExtended_currentMana;
        public static float skyheimExtended_currentMana_test;
        public static float skyheimExtended_manaDrain;

        public static void Postfix(ref float ____manaRegen, ref float ____maxMana, ref float ____currentMana)
        {
            //only run code when a player is active
            if ((Object)(object)Player.m_localPlayer != (Object)null)
            {   
                //only run code when a weapon is equipped. If no weapon is equipped, do not run.
                if (!Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab)
                {
                    return;
                }
                //currently equipped weapon      
                ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();

                //frost magic
                if (currentWeapon.m_dropPrefab.name == "rune_frostbolt" || currentWeapon.m_dropPrefab.name == "rune_glacial_spike")
                {
                    skillFlavor = "FrostMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.frostRegenScaleFactor.Value;
                    manaScaleFactor = SkyheimExtended.frostManaScaleFactor.Value;
                }
                //fire magic
                else if (currentWeapon.m_dropPrefab.name == "rune_firebolt" || currentWeapon.m_dropPrefab.name == "rune_immolate" || currentWeapon.m_dropPrefab.name == "rune_warmth")
                {
                    skillFlavor = "FireMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.fireRegenScaleFactor.Value;
                    manaScaleFactor = SkyheimExtended.fireManaScaleFactor.Value;
                }
                //holy magic
                else if (currentWeapon.m_dropPrefab.name == "rune_light")
                {
                    skillFlavor = "HolyMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.HolyMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.holyRegenScaleFactor.Value;
                    manaScaleFactor = SkyheimExtended.holyManaScaleFactor.Value;
                }
                //nature magic
                else if (currentWeapon.m_dropPrefab.name == "rune_heal" || currentWeapon.m_dropPrefab.name == "rune_chain_lighting")
                {
                    skillFlavor = "NatureMagic";
                    skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                    regenScaleFactor = SkyheimExtended.natureRegenScaleFactor.Value;
                    manaScaleFactor = SkyheimExtended.natureManaScaleFactor.Value;
                }

                //scale it
                regenScaled = (float)(skillLevel * regenScaleFactor + 3);
                manaScaled = (float)(skillLevel * manaScaleFactor + 100);


                //if a weapon is equipped, do it
                if (currentWeapon != null && (Object)(object)currentWeapon.m_dropPrefab != (Object)null)
                {
                    if (SkyheimExtended.scaleWithLevel.Value)
                    {
                        ____manaRegen = regenScaled;
                        ____maxMana = manaScaled;
                    }

                    else if (!SkyheimExtended.scaleWithLevel.Value)
                    {
                        ____manaRegen = SkyheimExtended.staticManaRegen.Value;
                        ____maxMana = SkyheimExtended.staticMaxMana.Value;
                    }

                    else
                    {
                        ____manaRegen = 3f;
                        ____maxMana = 100f;
                    }


                    //void ManaDrain(SkyheimItemData itemData)
                    //{
                    //skyheimExtended_manaDrain = (float)SkyheimItemData.FindObjectOfType<ManaUsed>();
                    //}



                    skyheimExtended_manaRegen = ____manaRegen;
                    skyheimExtended_maxMana = ____maxMana;
                    skyheimExtended_currentMana = ____currentMana;

                }
            }
        }
    }
}
