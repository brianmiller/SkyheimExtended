using HarmonyLib;
using UnityEngine;
using SkyheimExtended.Mana;
using skyheim;

namespace SkyheimExtended.Debugger
{
    class SkyheimExtended_Debug
    {
        [HarmonyPatch(typeof(Player), "SetControls")]
        public static class SE_Debug
        {
            public static void Postfix(ref bool attack)
            {

                //only run if isDebug=true in the user config file
                if (!SkyheimExtended.isDebug.Value)
                {
                    return;
                }

                //only run code when a player is active
                if ((Object)(object)Player.m_localPlayer != (Object)null)
                {
                    //only run code when a weapon is equipped AND i. If no weapon is equipped, do not run.
                    if (!Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab)
                    {
                        return;
                    }
                }

                //if attack command is sent, send the following information to the console.
                if (attack)
                {
                    Debug.Log("");
                    Debug.Log($"[SkyheimExtended] Current weapon: {Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab.name}");
                    Debug.Log($"[SkyheimExtended] Current weapon is a rune: {Player.m_localPlayer.GetCurrentWeapon().m_dropPrefab.name.StartsWith("rune_")}");
                    Debug.Log($"[SkyheimExtended] Skill: {SkyheimMana_Patch.skillFlavor}");
                    Debug.Log($"[SkyheimExtended] Scale with skill: {SkyheimExtended.scaleWithLevel.Value}");
                    Debug.Log($"[SkyheimExtended] Skill level: {SkyheimMana_Patch.skillLevel}");
                    Debug.Log($"[SkyheimExtended] Mana level: {SkyheimMana_Patch.manaScaled}");
                    Debug.Log($"[SkyheimExtended] Regen factor: {SkyheimMana_Patch.regenScaled}");
                    Debug.Log("");
                }
            }
        }
    }
}