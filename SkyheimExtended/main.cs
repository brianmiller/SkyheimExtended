using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using System.Collections.Generic;


namespace SkyheimExtended
{
    [BepInPlugin("posixone.SkyheimExtended", "SkyheimExtended", "1.0.3")]
    [BepInProcess("valheim.exe")]
    public class SkyheimExtended : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("posixone.SkyheimExtended");

        public static ConfigEntry<bool> skyheimExtendedEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<bool> scaleWithLevel;
        public static ConfigEntry<double> manaScaleFactor;
        public static ConfigEntry<double> regenScaleFactor;
        public static ConfigEntry<float> manaRegen;
        public static ConfigEntry<float> maxMana;


        void Awake()
        { 
            skyheimExtendedEnabled = Config.Bind(new ConfigDefinition("Global", "skyheimExtendedEnabled"), true, new ConfigDescription("Set this to true to enable and false to disable this mod."));
            isDebug = Config.Bind(new ConfigDefinition("Global", "isDebug"), false, new ConfigDescription("Set this to true to enable and false to disable debug mode. This spams the console and will impact performance."));
            scaleWithLevel = Config.Bind(new ConfigDefinition("Scaling", "scaleWithLevel"), true, new ConfigDescription("Set this to true to automatically scale maximum mana and mana regen rates with player level. See scale factors below for formulas."));
            manaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "manaScaleFactor"), 1.25, new ConfigDescription("Max Mana scale factor--formula: playerLevel * manaScaleFactor + 100. scaleWithLevel must be set to true."));
            regenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "regenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor--formula: playerLevel * regenScaleFactor + 3. scaleWithLevel must be set to true."));
            manaRegen = Config.Bind(new ConfigDefinition("Static", "manaRegen"), 7f, new ConfigDescription("Statically set Mana Regen rate. Skyheim's default is 3. scaleWithLevel must be set to false."));
            maxMana = Config.Bind(new ConfigDefinition("Static", "maxMana"), 125f, new ConfigDescription("Statically set Max Mana. Skyheim's default is 100. scaleWithLevel must be set to false."));

            //if this mod isn't enabled, don't run
            if (!skyheimExtendedEnabled.Value)
            {
                return;
            }

            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(skyheim.SkyheimMana), nameof(skyheim.SkyheimMana.UpdateMana))]

        public class SkyheimMana_Patch : BaseUnityPlugin
        {

            static void Postfix(ref float ____manaRegen, ref float ____maxMana)
            {
                //only run code when weapon is equipped
                if ((Object)(object)Player.m_localPlayer != (Object)null)
                {
                    //currently equipped weapon      
                    ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();
                                       
                    float playerLevel = 1;

                    //frost magic
                    if (currentWeapon.m_dropPrefab.name == "rune_frostbolt" || currentWeapon.m_dropPrefab.name == "rune_glacial_spike")
                    {                       
                        playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                        //playerLevel = Player.m_localPlayer.GetSkillFactor((Skills.SkillType)203) * 100f + 0.000001f;
                    } 
                    //fire magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_firebolt" || currentWeapon.m_dropPrefab.name == "rune_immolate" || currentWeapon.m_dropPrefab.name == "rune_warmth")
                    {
                        playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                        //playerLevel = Player.m_localPlayer.GetSkillFactor((Skills.SkillType)202) * 100f + 0.000001f;
                    }
                    //holy magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_light")
                    {
                        playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.HolyMagic) * 100f + 0.000001f;
                        //playerLevel = Player.m_localPlayer.GetSkillFactor((Skills.SkillType)201) * 100f + 0.000001f;
                    }
                    //nature magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_heal" || currentWeapon.m_dropPrefab.name == "rune_chain_lighting" )
                    {
                        playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                        //playerLevel = Player.m_localPlayer.GetSkillFactor((Skills.SkillType)200) * 100f + 0.000001f;
                    }

                    
                    //float playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType.Run)) * 100f + 0.000001f;


                    //scale it
                    float regenScaled = (float)(playerLevel * regenScaleFactor.Value + 3);
                    float manaScaled = (float)(playerLevel * manaScaleFactor.Value + 100);


                    if (currentWeapon != null && (Object)(object)currentWeapon.m_dropPrefab != (Object)null)
                    {
                        if (scaleWithLevel.Value)
                        {
                            ____manaRegen = regenScaled;
                            ____maxMana = manaScaled;
                        }

                        else if (!scaleWithLevel.Value)
                        {
                            ____manaRegen = manaRegen.Value;
                            ____maxMana = maxMana.Value;
                        }

                        else
                        {
                            ____manaRegen = 3f;
                            ____maxMana = 100f;
                        }

                        if (isDebug.Value)
                        {
                            Debug.Log($"Scale With Level: {scaleWithLevel.Value}");
                            Debug.Log($"Current Weapon: {currentWeapon.m_dropPrefab.name}");
                            Debug.Log($"Mana Level: {____maxMana}");
                            Debug.Log($"Regen Factor: {____manaRegen}");
                            Debug.Log($"Skill Level: {playerLevel}");
                        }
                    }
                }
            }
        }
    }
}