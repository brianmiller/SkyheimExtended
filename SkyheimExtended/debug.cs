using HarmonyLib;
using UnityEngine;
using SkyheimExtended.Mana;

namespace SkyheimExtended.Debugger
{
    class SkyheimExtended_Debug
    {
        [HarmonyPatch(typeof(Player), "SetControls")]
        public static class SE_Debug
        {
            private static void Prefix(ref bool attack)
            {
                if (attack)
                {
                    Debug.Log($"Scale With Level: {SkyheimExtended.scaleWithLevel.Value}");
                    Debug.Log($"Current weapon is a rune: {Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab.name.StartsWith("rune_")}");
                    Debug.Log($"Current Weapon: {Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab.name}");
                    Debug.Log($"Mana Level: {SkyheimMana_Patch.manaScaled}");
                    Debug.Log($"Regen Factor: {SkyheimMana_Patch.regenScaled}");
                    Debug.Log($"Skill Level: {SkyheimMana_Patch.skillLevel}");                 
                }
            }
        }
    }
}