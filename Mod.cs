using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace AutoPicker
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "C2AB543C-BF77-4853-AB76-662D422D3B6B";
        public const string PluginName = "AutoPicker";
        public const string PluginVersion = "1.1";
        internal static ConfigEntry<float> Radius;
        internal static ConfigFile ConfigFile;

        internal static void L(object input)
        {
            FileLog.Log(input?.ToString() ?? "null");
        }

        private void Awake()
        {
            // Do all your init stuff here
            // Acceptable value ranges can be defined to allow configuration via a slider in the BepInEx ConfigurationManager: https://github.com/BepInEx/BepInEx.ConfigurationManager
            //Config.Bind<int>("Main Section", "Example configuration integer", 1, new ConfigDescription("This is an example config, using a range limitation for ConfigurationManager", new AcceptableValueRange<int>(0, 100)));
            var harmony = new Harmony("ca.gnivler.Valheim.AutoPicker");
            harmony.PatchAll();
            ConfigFile = Config;
            Radius = Config.Bind("Settings", "AutoPick Radius - Game autoloot radius is 2.  Requires logout or restart.", 2f, new ConfigDescription("Radius at which pickables will be auto-picked.", new AcceptableValueRange<float>(0.75f, 20f)));
        }
    }
}
