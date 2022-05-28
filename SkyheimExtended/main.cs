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

        public static ConfigEntry<double> frostManaScaleFactor;
        public static ConfigEntry<double> frostRegenScaleFactor;
        public static ConfigEntry<double> fireManaScaleFactor;
        public static ConfigEntry<double> fireRegenScaleFactor;
        public static ConfigEntry<double> holyManaScaleFactor;
        public static ConfigEntry<double> holyRegenScaleFactor;
        public static ConfigEntry<double> natureManaScaleFactor;
        public static ConfigEntry<double> natureRegenScaleFactor;

        public static ConfigEntry<float> staticManaRegen;
        public static ConfigEntry<float> staticMaxMana;


        void Awake()
        { 
            //global settings
            skyheimExtendedEnabled = Config.Bind(new ConfigDefinition("Global", "skyheimExtendedEnabled"), true, new ConfigDescription("Set this to true to enable and false to disable this mod."));
            isDebug = Config.Bind(new ConfigDefinition("Global", "isDebug"), false, new ConfigDescription("Set this to true to enable and false to disable debug mode. This spams the console and will impact performance."));
            scaleWithLevel = Config.Bind(new ConfigDefinition("Scaling", "scaleWithLevel"), true, new ConfigDescription("Set this to true to automatically scale maximum mana and mana regen rates with player skill level. See scale factors below for formulas."));
 
            //scaling settings (when scaleWithLevel = true)
            frostManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "frostManaScaleFactor"), 1.25, new ConfigDescription("Max Mana scale factor for Frost type runes--formula: playerLevel * frostManaScaleFactor + 100. scaleWithLevel must be set to true."));
            frostRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "frostRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Frost type runes--formula: playerLevel * frostRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            fireManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "fireManaScaleFactor"), 1.25, new ConfigDescription("Max Mana scale factor for Fire type runes--formula: playerLevel * fireManaScaleFactor + 100. scaleWithLevel must be set to true."));
            fireRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "fireRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Fire type runes--formula: playerLevel * fireRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            holyManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "holyManaScaleFactor"), 1.25, new ConfigDescription("Max Mana scale factor for Holy type runes--formula: playerLevel * holyManaScaleFactor + 100. scaleWithLevel must be set to true."));
            holyRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "holyRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Holy type runes--formula: playerLevel * holyRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            natureManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "natureManaScaleFactor"), 1.25, new ConfigDescription("Max Mana scale factor for Nature type runes--formula: playerLevel * natureManaScaleFactor + 100. scaleWithLevel must be set to true."));
            natureRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "natureRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Nature type runes--formula: playerLevel * natureRegenScaleFactor + 3. scaleWithLevel must be set to true."));

            //static settings (when scaleWithLevel = false)
            staticManaRegen = Config.Bind(new ConfigDefinition("Static", "staticManaRegen"), 7f, new ConfigDescription("Statically set Mana Regen rate. Skyheim's default is 3. scaleWithLevel must be set to false."));
            staticMaxMana = Config.Bind(new ConfigDefinition("Static", "staticMaxMana"), 125f, new ConfigDescription("Statically set Max Mana. Skyheim's default is 100. scaleWithLevel must be set to false."));

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
                                       
                    //scaling floats
                    float skillLevel = 1;
                    float regenScaled = 1;
                    float manaScaled = 1;
                    double regenScaleFactor = 1;
                    double manaScaleFactor = 1;

                    //frost magic
                    if (currentWeapon.m_dropPrefab.name == "rune_frostbolt" || currentWeapon.m_dropPrefab.name == "rune_glacial_spike")
                    {                       
                        skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FrostMagic) * 100f + 0.000001f;
                        regenScaleFactor = frostRegenScaleFactor.Value;
                        manaScaleFactor = frostManaScaleFactor.Value;
                    } 
                    //fire magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_firebolt" || currentWeapon.m_dropPrefab.name == "rune_immolate" || currentWeapon.m_dropPrefab.name == "rune_warmth")
                    {
                        skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.FireMagic) * 100f + 0.000001f;
                        regenScaleFactor = fireRegenScaleFactor.Value;
                        manaScaleFactor = fireManaScaleFactor.Value;
                    }
                    //holy magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_light")
                    {
                        skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.HolyMagic) * 100f + 0.000001f;
                        regenScaleFactor = holyRegenScaleFactor.Value;
                        manaScaleFactor = holyManaScaleFactor.Value;
                    }
                    //nature magic
                    else if (currentWeapon.m_dropPrefab.name == "rune_heal" || currentWeapon.m_dropPrefab.name == "rune_chain_lighting" )
                    {
                        skillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;
                        regenScaleFactor = natureRegenScaleFactor.Value;
                        manaScaleFactor = natureManaScaleFactor.Value;
                    }

                    //scale it
                    regenScaled = (float)(skillLevel * regenScaleFactor + 3);
                    manaScaled = (float)(skillLevel * manaScaleFactor + 100);

                    //if a weapon is equipped, do it
                    if (currentWeapon != null && (Object)(object)currentWeapon.m_dropPrefab != (Object)null)
                    {
                        if (scaleWithLevel.Value)
                        {
                            ____manaRegen = regenScaled;
                            ____maxMana = manaScaled;
                        }

                        else if (!scaleWithLevel.Value)
                        {
                            ____manaRegen = staticManaRegen.Value;
                            ____maxMana = staticMaxMana.Value;
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
                            Debug.Log($"Skill Level: {skillLevel}");
                        }
                    }
                }
            }
        }
    }
}