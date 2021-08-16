using System;
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


        [HarmonyPatch(typeof(Localization), "Initialize")]
        public static class LocalizationInitialize
        {
            public static void Postfix()
            {
                foreach (var item in Resources.FindObjectsOfTypeAll<Pickable>())
                {
                    try
                    {
                        Mod.ConfigFile.Bind("Items", Localization.instance.Localize(item.GetHoverName()), true);
                    }
                    catch (Exception ex)
                    {
                        Mod.L(ex);
                    }
                }
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
