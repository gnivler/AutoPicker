using System;
using System.Runtime.InteropServices;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;

namespace AutoPicker
{
    public static class Patches
    {
        // add to every Pickable
        [HarmonyPatch(typeof(Pickable), "Awake")]
        public static class PickableAwakePatch
        {
            public static void Postfix(Pickable __instance)
            {
                __instance.gameObject.AddComponent<AutoPicker>();
            }
        }

        // try once to retrieve the config when it will be present
        [HarmonyPatch(typeof(Hud), "Awake")]
        public class HudAwakePatch
        {
            public static void Postfix()
            {
                Chainloader.PluginInfos.TryGetValue("RagnarsRokare.AutoPickupSelector", out AutoPicker.PluginInfo);
            }
        }

        // remove 0.5s hardcoded delay on item spawning before pickup is allowed
        [HarmonyPatch(typeof(ItemDrop), "CanPickup")]
        public static class ItemDropCanPickupPatch
        {
            public static bool Prefix(ZNetView ___m_nview, ref bool __result)
            {
                if (___m_nview == null || !___m_nview.IsValid())
                {
                    __result = true;
                    return false;
                }

                __result = ___m_nview.IsOwner();
                return false;
            }
        }
    }
}
