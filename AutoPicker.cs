using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AutoPicker
{
    internal class AutoPicker : MonoBehaviour
    {
        private static readonly AccessTools.FieldRef<Pickable, bool> m_picked =
            AccessTools.FieldRefAccess<Pickable, bool>("m_picked");

        internal static PluginInfo PluginInfo;
        private static readonly System.Random Rng = new();

        private void Awake()
        {
            InvokeRepeating(nameof(CheckAndPick), (float)Rng.NextDouble() * 2, 0.2f);
        }

        private void CheckAndPick()
        {
            if (Player.m_localPlayer is not null
                && (transform.position - Player.m_localPlayer.transform.position).sqrMagnitude < 4)
            {
                var pickable = GetComponentInChildren<Pickable>();
                if (m_picked(pickable))
                {
                    return;
                }

                if (PluginInfo?.Instance is not null
                    && PluginInfo.Instance.Config.TryGetEntry(new ConfigDefinition("General", "AutoPickupBlockList"), out ConfigEntry<string> config)
                    && !config.Value.Contains(pickable.m_itemPrefab.name))
                {
                    pickable.Interact(Player.m_localPlayer, false);
                }
                else if (PluginInfo?.Instance is null)
                {
                    pickable.Interact(Player.m_localPlayer, false);
                }
            }
        }
    }
}
