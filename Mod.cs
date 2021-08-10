using BepInEx;
using HarmonyLib;

namespace AutoPicker
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "C2AB543C-BF77-4853-AB76-662D422D3B6B";
        public const string PluginName = "AutoPicker";
        public const string PluginVersion = "1.0.2";

        internal static void L(object input)
        {
            //FileLog.Log(input?.ToString() ?? "null");
        }

        private void Awake()
        {
            // Do all your init stuff here
            // Acceptable value ranges can be defined to allow configuration via a slider in the BepInEx ConfigurationManager: https://github.com/BepInEx/BepInEx.ConfigurationManager
            //Config.Bind<int>("Main Section", "Example configuration integer", 1, new ConfigDescription("This is an example config, using a range limitation for ConfigurationManager", new AcceptableValueRange<int>(0, 100)));
            var harmony = new Harmony("ca.gnivler.Valheim.AutoPicker");
            harmony.PatchAll();
        }
    }
}
