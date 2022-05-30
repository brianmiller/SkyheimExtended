using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace SkyheimExtended
{
    [BepInPlugin("posixone.SkyheimExtended", "SkyheimExtended", "1.0.3")]
    [BepInDependency("skyheim")]
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
            frostManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "frostManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Frost type runes--formula: playerLevel * frostManaScaleFactor + 100. scaleWithLevel must be set to true."));
            frostRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "frostRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Frost type runes--formula: playerLevel * frostRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            fireManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "fireManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Fire type runes--formula: playerLevel * fireManaScaleFactor + 100. scaleWithLevel must be set to true."));
            fireRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "fireRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Fire type runes--formula: playerLevel * fireRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            holyManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "holyManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Holy type runes--formula: playerLevel * holyManaScaleFactor + 100. scaleWithLevel must be set to true."));
            holyRegenScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "holyRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Holy type runes--formula: playerLevel * holyRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            natureManaScaleFactor = Config.Bind(new ConfigDefinition("Scaling", "natureManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Nature type runes--formula: playerLevel * natureManaScaleFactor + 100. scaleWithLevel must be set to true."));
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
    }
}