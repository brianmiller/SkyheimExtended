using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;


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
        public static ConfigEntry<bool> scaleWithSkill;

        public static ConfigEntry<double> scalingFireBoltManaFactor;
        public static ConfigEntry<double> scalingFireBoltRegenFactor;
        public static ConfigEntry<int> staticFireBoltManaDrain;

        public static ConfigEntry<double> scalingImmolateManaFactor;
        public static ConfigEntry<double> scalingImmolateRegenFactor;
        public static ConfigEntry<int> staticImmolateManaDrain;

        public static ConfigEntry<double> scalingTravelManaFactor;
        public static ConfigEntry<double> scalingTravelRegenFactor;
        public static ConfigEntry<int> staticTravelManaDrain;

        public static ConfigEntry<double> scalingWarmthManaFactor;
        public static ConfigEntry<double> scalingWarmthRegenFactor;
        public static ConfigEntry<int> staticWarmthManaDrain;

        public static ConfigEntry<double> scalingFrostBoltManaFactor;
        public static ConfigEntry<double> scalingFrostBoltRegenFactor;
        public static ConfigEntry<int> staticFrostBoltManaDrain;

        public static ConfigEntry<double> scalingGlacialSpikeManaFactor;
        public static ConfigEntry<double> scalingGlacialSpikeRegenFactor;
        public static ConfigEntry<int> staticGlacialSpikeManaDrain;

        public static ConfigEntry<double> scalingInvigorateManaFactor;
        public static ConfigEntry<double> scalingInvigorateRegenFactor;
        public static ConfigEntry<int> staticInvigorateManaDrain;

        public static ConfigEntry<double> scalingRecallManaFactor;
        public static ConfigEntry<double> scalingRecallRegenFactor;
        public static ConfigEntry<int> staticRecallManaDrain;

        public static ConfigEntry<double> scalingLightManaFactor;
        public static ConfigEntry<double> scalingLightRegenFactor;
        public static ConfigEntry<int> staticLightManaDrain;

        public static ConfigEntry<double> scalingBlinkManaFactor;
        public static ConfigEntry<double> scalingBlinkRegenFactor;
        public static ConfigEntry<int> staticBlinkManaDrain;

        public static ConfigEntry<double> scalingChainLightningManaFactor;
        public static ConfigEntry<double> scalingChainLightningRegenFactor;
        public static ConfigEntry<int> staticChainLightningManaDrain;

        public static ConfigEntry<double> scalingForceManaFactor;
        public static ConfigEntry<double> scalingForceRegenFactor;
        public static ConfigEntry<int> staticForceManaDrain;

        public static ConfigEntry<double> scalingFrostNovaManaFactor;
        public static ConfigEntry<double> scalingFrostNovaRegenFactor;
        public static ConfigEntry<int> staticFrostNovaManaDrain;

        public static ConfigEntry<double> scalingHealManaFactor;
        public static ConfigEntry<double> scalingHealRegenFactor;
        public static ConfigEntry<int> staticHealManaDrain;


        public static ConfigEntry<float> staticManaRegen;
        public static ConfigEntry<float> staticMaxMana;


        void Awake()
        {
            //global settings
            skyheimExtendedEnabled = Config.Bind(new ConfigDefinition("1-Global", "skyheimExtendedEnabled"), true, new ConfigDescription("Set this to true to enable and false to disable this mod."));
            isDebug = Config.Bind(new ConfigDefinition("1-Global", "isDebug"), false, new ConfigDescription("Set this to true to enable and false to disable debug mode. This spams the console and will impact performance."));          
            scaleWithSkill = Config.Bind(new ConfigDefinition("1-Global", "scaleWithSkill"), true, new ConfigDescription("Set this to true to automatically scale maximum mana and mana regen rates with player skill level. See scale factors below for formulas."));

            //static settings (when scaleWithLevel = false)
            staticManaRegen = Config.Bind(new ConfigDefinition("2-Static", "staticManaRegen"), 7f, new ConfigDescription("Statically set Mana Regen rate. Skyheim's default is 3. scaleWithLevel must be set to false."));
            staticMaxMana = Config.Bind(new ConfigDefinition("2-Static", "staticMaxMana"), 125f, new ConfigDescription("Statically set Max Mana. Skyheim's default is 100. scaleWithLevel must be set to false."));

            //rune specific settings (scaling parameters require scaleWithSkill=true)
            scalingFireBoltManaFactor = Config.Bind(new ConfigDefinition("FireBolt", "scalingFireBoltManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Fire Bolt rune--formula: skillLevel * scalingFireBoltManaFactor + 100. scaleWithSkill must be set to true."));
            scalingFireBoltRegenFactor = Config.Bind(new ConfigDefinition("FireBolt", "scalingFireBoltRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Fire Bolt rune--formula: skillLevel * scalingFireBoltRegenFactor + 3. scaleWithSkill must be set to true."));
            staticFireBoltManaDrain = Config.Bind(new ConfigDefinition("FireBolt", "staticFireManaDrain"), 15, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 15."));

            scalingImmolateManaFactor = Config.Bind(new ConfigDefinition("Immolate", "scalingImmolateManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Immolate rune--formula: skillLevel * scalingImmolateManaFactor + 100. scaleWithSkill must be set to true."));
            scalingImmolateRegenFactor = Config.Bind(new ConfigDefinition("Immolate", "scalingImmolateRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Immolate rune--formula: skillLevel * scalingImmolateRegenFactor + 3. scaleWithSkill must be set to true."));
            staticImmolateManaDrain = Config.Bind(new ConfigDefinition("Immolate", "staticImmolateManaDrain"), 40, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 40."));

            scalingTravelManaFactor = Config.Bind(new ConfigDefinition("Travel", "scalingTravelManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Travel rune--formula: skillLevel * scalingTravelManaFactor + 100. scaleWithSkill must be set to true."));
            scalingTravelRegenFactor = Config.Bind(new ConfigDefinition("Travel", "scalingTravelRegenFactor"), 0.06, new ConfigDescription("Max Mana scale factor for the Travel rune--formula: skillLevel * scalingTravelRegenFactor + 3. scaleWithSkill must be set to true."));
            staticTravelManaDrain = Config.Bind(new ConfigDefinition("Travel", "staticTravelManaDrain"), 3, new ConfigDescription("The rate of mana consumption while the rune is active (travel rune is time-based depletion) The lower the number the slower the consumption. Skyheim's default is 8."));

            scalingWarmthManaFactor = Config.Bind(new ConfigDefinition("Warmth", "scalingWarmthManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Warmth rune--formula: skillLevel * scalingWarmthManaFactor + 100. scaleWithSkill must be set to true."));
            scalingWarmthRegenFactor = Config.Bind(new ConfigDefinition("Warmth", "scalingWarmthRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Warmth rune--formula: skillLevel * scalingWarmthRegenFactor + 3. scaleWithSkill must be set to true."));
            staticWarmthManaDrain = Config.Bind(new ConfigDefinition("Warmth", "staticWarmthManaDrain"), 40, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 40."));

            scalingFrostBoltManaFactor = Config.Bind(new ConfigDefinition("FrostBolt", "scalingFrostBoltManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Frost Bolt rune--formula: skillLevel * scalingFrostBoltManaFactor + 100. scaleWithSkill must be set to true."));
            scalingFrostBoltRegenFactor = Config.Bind(new ConfigDefinition("FrostBolt", "scalingFrostBoltRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Frost Bolt rune--formula: skillLevel * scalingFrostBoltRegenFactor + 3. scaleWithSkill must be set to true."));
            staticFrostBoltManaDrain = Config.Bind(new ConfigDefinition("FrostBolt", "staticFrostManaDrain"), 15, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 15."));

            scalingGlacialSpikeManaFactor = Config.Bind(new ConfigDefinition("GlacialSpike", "scalingGlacialSpikeManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Fire Bolt rune--formula: skillLevel * scalingGlacialSpikeManaFactor + 100. scaleWithSkill must be set to true."));
            scalingGlacialSpikeRegenFactor = Config.Bind(new ConfigDefinition("GlacialSpike", "scalingGlacialSpikeRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Fire Bolt rune--formula: skillLevel * scalingGlacialSpikeRegenFactor + 3. scaleWithSkill must be set to true."));
            staticGlacialSpikeManaDrain = Config.Bind(new ConfigDefinition("GlacialSpike", "staticGlacialSpikeManaDrain"), 20, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 20."));

            scalingInvigorateManaFactor = Config.Bind(new ConfigDefinition("Invigorate", "scalingInvigorateManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Invigorate rune--formula: skillLevel * scalingInvigorateManaFactor + 100. scaleWithSkill must be set to true."));
            scalingInvigorateRegenFactor = Config.Bind(new ConfigDefinition("Invigorate", "scalingInvigorateRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Invigorate rune--formula: skillLevel * scalingInvigorateRegenFactor + 3. scaleWithSkill must be set to true."));
            staticInvigorateManaDrain = Config.Bind(new ConfigDefinition("Invigorate", "staticInvigorateDrain"), 40, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 40."));

            scalingRecallManaFactor = Config.Bind(new ConfigDefinition("Recall", "scalingRecallManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Recall rune--formula: skillLevel * scalingRecallManaFactor + 100. scaleWithSkill must be set to true."));
            scalingRecallRegenFactor = Config.Bind(new ConfigDefinition("Recall", "scalingRecallRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Recall rune--formula: skillLevel * scalingRecallRegenFactor + 3. scaleWithSkill must be set to true."));
            staticRecallManaDrain = Config.Bind(new ConfigDefinition("Recall", "staticRecallManaDrain"), 80, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 80."));

            scalingLightManaFactor = Config.Bind(new ConfigDefinition("Light", "scalingLightManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Light rune--formula: skillLevel * scalingLightManaFactor + 100. scaleWithSkill must be set to true."));
            scalingLightRegenFactor = Config.Bind(new ConfigDefinition("Light", "scalingLightRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Light rune--formula: skillLevel * scalingLightRegenFactor + 3. scaleWithSkill must be set to true."));
            staticLightManaDrain = Config.Bind(new ConfigDefinition("Light", "staticLightManaDrain"), 10, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 10."));

            scalingBlinkManaFactor = Config.Bind(new ConfigDefinition("Blink", "scalingBlinkManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Blink rune--formula: skillLevel * scalingBlinkManaFactor + 100. scaleWithSkill must be set to true."));
            scalingBlinkRegenFactor = Config.Bind(new ConfigDefinition("Blink", "scalingBlinkRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Blink rune--formula: skillLevel * scalingBlinkRegenFactor + 3. scaleWithSkill must be set to true."));
            staticBlinkManaDrain = Config.Bind(new ConfigDefinition("Blink", "staticBlinkManaDrain"), 20, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 20."));

            scalingChainLightningManaFactor = Config.Bind(new ConfigDefinition("ChainLightning", "scalingChainLightningManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Chain of Lightning rune--formula: skillLevel * scalingChainLightningManaFactor + 100. scaleWithSkill must be set to true."));
            scalingChainLightningRegenFactor = Config.Bind(new ConfigDefinition("ChainLightning", "scalingChainLightningRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Chain of Lightning rune--formula: skillLevel * scalingChainLightningRegenFactor + 3. scaleWithSkill must be set to true."));
            staticChainLightningManaDrain = Config.Bind(new ConfigDefinition("ChainLightning", "staticFireManaDrain"), 25, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 25."));

            scalingForceManaFactor = Config.Bind(new ConfigDefinition("Force", "scalingForceManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Force rune--formula: skillLevel * scalingForceManaFactor + 100. scaleWithSkill must be set to true."));
            scalingForceRegenFactor = Config.Bind(new ConfigDefinition("Force", "scalingForceRegenFactor"), 0.05, new ConfigDescription("Max Mana scale factor for the Force rune--formula: skillLevel * scalingForceRegenFactor + 3. scaleWithSkill must be set to true."));
            staticForceManaDrain = Config.Bind(new ConfigDefinition("Force", "staticForceManaDrain"), 20, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 20."));

            scalingFrostNovaManaFactor = Config.Bind(new ConfigDefinition("FrostNova", "scalingFrostNovaManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Frost Nova rune--formula: skillLevel * scalingFrostNovaManaFactor + 100. scaleWithSkill must be set to true."));
            scalingFrostNovaRegenFactor = Config.Bind(new ConfigDefinition("FrostNova", "scalingFrostNovaRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Frost Nova rune--formula: skillLevel * scalingFrostNovaRegenFactor + 3. scaleWithSkill must be set to true."));
            staticFrostNovaManaDrain = Config.Bind(new ConfigDefinition("FrostNova", "staticFrostNovaManaDrain"), 50, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 50."));

            scalingHealManaFactor = Config.Bind(new ConfigDefinition("Heal", "scalingHealManaFactor"), 1.41, new ConfigDescription("Max Mana scale factor for the Heal rune--formula: skillLevel * scalingHealManaFactor + 100. scaleWithSkill must be set to true."));
            scalingHealRegenFactor = Config.Bind(new ConfigDefinition("Heal", "scalingHealRegenFactor"), 0.04, new ConfigDescription("Max Mana scale factor for the Heal rune--formula: skillLevel * scalingHealRegenFactor + 3. scaleWithSkill must be set to true."));
            staticHealManaDrain = Config.Bind(new ConfigDefinition("Heal", "staticHealManaDrain"), 40, new ConfigDescription("The amount of mana used during each use. Skyheim's default is 40."));


            //if this mod isn't enabled, don't run
            if (!skyheimExtendedEnabled.Value)
            {
                return;
            }

            harmony.PatchAll();
        }
    }
}