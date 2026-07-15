using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace EndlessServiceShaft.Hooks;

public static class DatabaseModifier
{   
    private static T GetObjectFromResources<T>(string name) where T : UnityEngine.Object
{

    foreach (T obj in Resources.FindObjectsOfTypeAll<T>())
    {
        if (obj != null && obj.name == name)
        {
            return obj;
        }
    }
    return null;
}
    
    private static M_Level.LevelAssetHolder GetLevelAsset(string name, WKAssetDatabase database)
    {
        foreach (M_Level.LevelAssetHolder levelAssetHolder in database.levelAssets)
        {
            if (levelAssetHolder.id.ToLower() == name.ToLower())
            {
                return levelAssetHolder;
            }
        }
        
        Plugin.Logger.LogError($"{name.ToLower()} not found!");
        return null;
    }

    private static bool added = false;
    
    [HarmonyPatch(typeof(CL_Initializer), "Awake")]
    [HarmonyPrefix]
    public static void InitializerAwake(ref CL_Initializer __instance)
    {
        if (added) return;
        
        M_Gamemode endlessBase = __instance.baseDatabase.gamemodeAssets.First((x) => x.name == "GM_Cheatrooms");

        M_Gamemode gm = endlessBase;
        gm.allowAchievements = false;
        gm.allowCheatedScores = false;
        gm.allowCheats = false;
        gm.allowLeaderboardScoring = false;
        gm.steamLeaderboardName = "";
        gm.allowHeightAchievements = false;
        gm.baseGamemode = true;
        gm.useGamemodeSettings = true;
        gm.modeType = M_Gamemode.GameType.standard;
        gm.capsuleName = "";
        gm.capsuleArt = endlessBase.capsuleArt;
        gm.gamemodeName = "Region_Endless_Cheatrooms";
        gm.introText = "";
        gm.isEndless = true;
        gm.hasPerks = false;
        gm.hasRevives = false;
        gm.name = "Region_Endless_Cheatrooms";
        gm.newGameText = "Start Run";
        
        gm.startItems = [
        ];
        
        gm.regions = [GetObjectFromResources<M_Region>("Region_Cheatrooms_Endless")]

        gm.gamemodeScene = endlessBase.gamemodeScene;
        gm.roachBankID = endlessBase.roachBankID;
        gm.gamemodePanel = endlessBase.gamemodePanel;
        gm.loseScreen = endlessBase.loseScreen;
        gm.winScreen = endlessBase.winScreen;
        gm.modeTags = endlessBase.modeTags;
        gm.unlockAchievement = endlessBase.unlockAchievement;
        gm.gamemodeModule = endlessBase.gamemodeModule;
        gm.levelsToGenerate = endlessBase.levelsToGenerate;
        gm.availableTrinkets = endlessBase.availableTrinkets;
        gm.gamemodeColor = endlessBase.gamemodeColor;
        gm.gamemodeSettings = endlessBase.gamemodeSettings;
        gm.playlistLevelAssets = endlessBase.playlistLevelAssets;
        gm.playlistLevels = endlessBase.playlistLevels;
        gm.roachEndSprite = endlessBase.roachEndSprite;
        gm.screenArt = endlessBase.screenArt;
        gm.gamemodeObjects = endlessBase.gamemodeObjects;
        gm.unlockHint = endlessBase.unlockHint;

        __instance.baseDatabase.gamemodeAssets.Add(gm);
        Plugin.endlessServiceShaftGamemode = gm;

        added = true;
    }
}
