using BepInEx;
using BepInEx.Logging;
using EndlessServiceShaft.Hooks;
using HarmonyLib;

namespace EndlessServiceShaft;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static M_Gamemode endlessServiceShaftGamemode;
    
    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"{PluginInfo.Name} has loaded!");

        Harmony harmony = new Harmony(PluginInfo.GUID);
        harmony.PatchAll(typeof(DatabaseModifier));
        harmony.PatchAll(typeof(AddGamemode));
    }
}
