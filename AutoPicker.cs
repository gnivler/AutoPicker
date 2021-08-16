using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AutoPicker
{
    internal class AutoPicker : MonoBehaviour
    {
        private static readonly AccessTools.FieldRef<Pickable, bool> m_picked =
            AccessTools.FieldRefAccess<Pickable, bool>("m_picked");

        private static readonly System.Random Rng = new();

        private void Awake()
        {
            InvokeRepeating(nameof(CheckAndPick), (float)Rng.NextDouble() * 2, 0.2f);
        }

        private void CheckAndPick()
        {
            if (Player.m_localPlayer is not null
                && (transform.position - Player.m_localPlayer.transform.position).sqrMagnitude < Mod.Radius.Value * Mod.Radius.Value)
            {
                var pickable = GetComponentInChildren<Pickable>();
                if (m_picked(pickable))
                {
                    return;
                }

                if (Mod.ConfigFile.TryGetEntry("Items", Localization.instance.Localize(pickable.GetHoverName()), out ConfigEntry<bool> configEntry))
                {
                    if (configEntry.Value)
                    {
                        pickable.Interact(Player.m_localPlayer, false);
                    }
                }
            }
        }
    }
}
