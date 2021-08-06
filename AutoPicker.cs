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

        private void Awake()
        {
            InvokeRepeating(nameof(CheckAndPick), 1, 0.1f);
        }

        private void CheckAndPick()
        {
            var pickable = GetComponentInChildren<Pickable>();
            if (Player.m_localPlayer is not null
                && !m_picked(pickable)
                && Vector3.Distance(transform.position, Player.m_localPlayer.transform.position) < 1.5f)
            {
                if (PluginInfo.Instance.Config.TryGetEntry(new ConfigDefinition("General", "AutoPickupBlockList"), out ConfigEntry<string> config)
                    && !config.Value.Contains(pickable.m_itemPrefab.name))
                {
                    pickable.Interact(Player.m_localPlayer, false);
                }

                if (string.IsNullOrEmpty(config.Value))
                {
                    pickable.Interact(Player.m_localPlayer, false);
                }
            }
        }
    }
}
