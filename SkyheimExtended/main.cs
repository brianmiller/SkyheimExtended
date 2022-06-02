using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace SkyheimExtended
{
    [BepInPlugin("posixone.SkyheimExtended", "SkyheimExtended", "1.0.4")]
    [BepInDependency("skyheim")]
    [BepInProcess("valheim.exe")]

    public class SkyheimExtended : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("posixone.SkyheimExtended");

        public static ConfigEntry<bool> skyheimExtendedEnabled;
        public static ConfigEntry<bool> isDebug;
        public static ConfigEntry<bool> scaleWithLevel;

        public static ConfigEntry<double> scalingFrostManaFactor;
        public static ConfigEntry<double> scalingFrostRegenFactor;
        public static ConfigEntry<double> scalingFireManaFactor;
        public static ConfigEntry<double> scalingFireRegenFactor;
        public static ConfigEntry<double> scalingHolyManaFactor;
        public static ConfigEntry<double> scalingHolyRegenFactor;
        public static ConfigEntry<double> scalingNatureManaFactor;
        public static ConfigEntry<double> scalingNatureRegenFactor;

        public static ConfigEntry<double> scalingFrostManaDrainFactor;
        public static ConfigEntry<double> scalingFireManaDrainFactor;
        public static ConfigEntry<double> scalingHolyManaDrainFactor;
        public static ConfigEntry<double> scalingNatureManaDrainFactor;

        public static ConfigEntry<float> staticManaRegen;
        public static ConfigEntry<float> staticMaxMana;
        public static ConfigEntry<int> staticManaDrain;
        


        void Awake()
        {
            //global settings
            skyheimExtendedEnabled = Config.Bind(new ConfigDefinition("Global", "skyheimExtendedEnabled"), true, new ConfigDescription("Set this to true to enable and false to disable this mod."));
            isDebug = Config.Bind(new ConfigDefinition("Global", "isDebug"), false, new ConfigDescription("Set this to true to enable and false to disable debug mode. This spams the console and will impact performance."));
            scaleWithLevel = Config.Bind(new ConfigDefinition("Scaling", "scaleWithLevel"), true, new ConfigDescription("Set this to true to automatically scale maximum mana and mana regen rates with player skill level. See scale factors below for formulas."));

            //scaling settings (when scaleWithLevel = true)
            scalingFrostManaFactor = Config.Bind(new ConfigDefinition("Scaling", "frostManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Frost type runes--formula: skillLevel * frostManaScaleFactor + 100. scaleWithLevel must be set to true."));
            scalingFrostRegenFactor = Config.Bind(new ConfigDefinition("Scaling", "frostRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Frost type runes--formula: skillLevel * frostRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            scalingFireManaFactor = Config.Bind(new ConfigDefinition("Scaling", "fireManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Fire type runes--formula: skillLevel * fireManaScaleFactor + 100. scaleWithLevel must be set to true."));
            scalingFireRegenFactor = Config.Bind(new ConfigDefinition("Scaling", "fireRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Fire type runes--formula: skillLevel * fireRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            scalingHolyManaFactor = Config.Bind(new ConfigDefinition("Scaling", "holyManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Holy type runes--formula: skillLevel * holyManaScaleFactor + 100. scaleWithLevel must be set to true."));
            scalingHolyRegenFactor = Config.Bind(new ConfigDefinition("Scaling", "holyRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Holy type runes--formula: skillLevel * holyRegenScaleFactor + 3. scaleWithLevel must be set to true."));
            scalingNatureManaFactor = Config.Bind(new ConfigDefinition("Scaling", "natureManaScaleFactor"), 1.35, new ConfigDescription("Max Mana scale factor for Nature type runes--formula: skillLevel * natureManaScaleFactor + 100. scaleWithLevel must be set to true."));
            scalingNatureRegenFactor = Config.Bind(new ConfigDefinition("Scaling", "natureRegenScaleFactor"), 0.05, new ConfigDescription("Mana Regen scale factor for Nature type runes--formula: skillLevel * natureRegenScaleFactor + 3. scaleWithLevel must be set to true."));

            scalingFrostManaDrainFactor = Config.Bind(new ConfigDefinition("Scaling", "scalingFrostManaDrainScaleFactor"), 15.0, new ConfigDescription("Mana drain (consumption) scale factor for Frost type runes."));


            //static settings (when scaleWithLevel = false)
            staticManaRegen = Config.Bind(new ConfigDefinition("Static", "staticManaRegen"), 7f, new ConfigDescription("Statically set Mana Regen rate. Skyheim's default is 3. scaleWithLevel must be set to false."));
            staticMaxMana = Config.Bind(new ConfigDefinition("Static", "staticMaxMana"), 125f, new ConfigDescription("Statically set Max Mana. Skyheim's default is 100. scaleWithLevel must be set to false."));
            staticManaDrain = Config.Bind(new ConfigDefinition("Static", "staticManaDrain"), 15, new ConfigDescription("Statically set Mana drain for all runes. Skyheim's default is 15. scaleWithLevel must be set to false."));



            //if this mod isn't enabled, don't run
            if (!skyheimExtendedEnabled.Value)
            {
                return;
            }

            harmony.PatchAll();
        }
    }
}