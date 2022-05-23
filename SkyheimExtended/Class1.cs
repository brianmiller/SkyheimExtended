using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using skyheim;


namespace SkyheimExtended
{
    [BepInPlugin("posixone.SkyheimExtended", "SkyheimExtended", "1.0.3")]
    [BepInProcess("valheim.exe")]
    public class SkyheimExtended : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("posixone.SkyheimExtended");

        public static ConfigEntry<bool> scaleWithLevel;
        public static ConfigEntry<float> scale;
        public static ConfigEntry<float> manaRegen;
        public static ConfigEntry<float> maxMana;

        void Awake()
        {
            scaleWithLevel = Config.Bind("Global", "scaleWithLevel", true);
            scale = Config.Bind("Global", "scale", 2f);
            manaRegen = Config.Bind("Global", "manaRegen", 3f);
            maxMana = Config.Bind("Global", "maxMana", 100f);

            harmony.PatchAll();
        }


        [HarmonyPatch(typeof(skyheim.SkyheimMana), nameof(skyheim.SkyheimMana.UpdateMana))]


        class SkyheimMana_Patch
        {

            //public static readonly ItemDrop.ItemData itemData = Player.m_localPlayer.GetCurrentWeapon();

            static void Postfix(ref float ____manaRegen, ref float ____maxMana, bool scaleWithLevel)
            {
                //ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();

                //if (currentWeapon.m_dropPrefab.name == "rune_frostbolt")
                //{
                //____manaRegen = manaRegen.Value;
                //____maxMana = maxMana.Value;
                //}              


                //Only run code when weapon is equipped
                if ((Object)(object)Player.m_localPlayer != (Object)null)
                {
                    ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();

                    //float frostbolt_skillLevel = ((Player.m_localPlayer.GetSkillFactor((Skills.SkillType.Swim)) * 100f) + 0.000001f);
                    float playerLevel = Player.m_localPlayer.GetLevel();
                    float scaleFactor = 2;

                    //scaleFactor = scaleFactor

                    float regenScaled = playerLevel * scaleFactor;
                    float manaScaled = playerLevel * scaleFactor;


                    if (currentWeapon != null && (Object)(object)currentWeapon.m_dropPrefab != (Object)null)
                    {
                        string equipped = currentWeapon.m_dropPrefab.name;
                        

                        //Debug.Log($"Current Weapon: {equipped}");

                        //if the Skyheim Frostbolt Rune is equipped
                        if (equipped == "rune_frostbolt")
                        {
                            if (scaleWithLevel == true)
                            {
                                ____manaRegen = regenScaled;
                                ____maxMana = manaScaled;
                            }
                            //____manaRegen = manaRegen.Value;
                            //____maxMana = maxMana.Value;
                            
                            Debug.Log($"Skill Level: {playerLevel}");
                        }
                    }
                        //bool skyheim_engaged = true;
                        

                }   

                //if (skyheim_engaged = true)
                //{
                //    ItemDrop.ItemData currentWeapon = Player.m_localPlayer.GetCurrentWeapon();
                //    ____manaRegen = manaRegen.Value;
                //    ____maxMana = maxMana.Value;
                //    Debug.Log($"Weapon: {currentWeapon.m_dropPrefab.name}");
                //    Debug.Log($"Mana: {____currentMana}");
                //}

                //else if (currentWeapon.m_dropPrefab.name == "rune_firebolt")
                //{
                //    ____manaRegen = manaRegen.Value;
                //____maxMana = maxMana.Value;
                // }


                //else
                //{
                //    ____manaRegen = 3f;
                //____maxMana = 100f;
                //}

                //Debug.Log($"Skyheim Extended: {se_GetCurrentWeapon()}");         
            }
        }
    }
}