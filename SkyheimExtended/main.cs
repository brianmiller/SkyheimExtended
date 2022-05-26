using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using skyheim;
//using System;


namespace SkyheimExtended
{
    [BepInPlugin("posixone.SkyheimExtended", "SkyheimExtended", "1.0.2")]
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
        class SkyheimMana_Patch
        {
            static void Postfix(ref float ____manaRegen, ref float ____maxMana)
            {
                //Only run code when weapon is equipped
                if ((Object)(object)Player.m_localPlayer != (Object)null)
                {
                    ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();

                    //float playerLevel = Player.m_localPlayer.GetLevel();
                    //use run skill for now
                    float playerLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType.Run)) * 100f + 0.000001f;
                    
                    //Get natureskill level
                    float natureSkillLevel = (float)Player.m_localPlayer.GetSkillFactor((Skills.SkillType)SkyheimItemData.ESkillType.NatureMagic) * 100f + 0.000001f;

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
                                Debug.Log($"Player Level: {playerLevel}");
                                Debug.Log($"Mana Level: {____maxMana}");
                                Debug.Log($"Regen Factor: {____manaRegen}");
                            }
                    }                    
                }            
            }
        }
    }
}
