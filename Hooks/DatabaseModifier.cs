using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace EndlessServiceShaft.Hooks;

public static class DatabaseModifier
{

    /*

        M3_Habitation_Shaft_Intro
        M3_Habitation_Endless_Shaft_Start
        M3_Habitation_Shaft_01
        M3_Habitation_Shaft_02
        M3_Habitation_Shaft_03
        M3_Habitation_Shaft_04
        M3_Habitation_Shaft_05
        M3_Habitation_Shaft_06
        M3_Habitation_Shaft_07
        M3_Habitation_Endless_Breakroom
   
    */

    // i'm so scared
    
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
        
        M_Gamemode endlessBase = __instance.baseDatabase.gamemodeAssets.First((x) => x.name == "GM_Endless_Pipeworks");
        M_Gamemode endlessBase2 = __instance.baseDatabase.gamemodeAssets.First((x) => x.name == "GM_Endless_Habitation");

        M_Gamemode gm = ScriptableObject.CreateInstance<M_Gamemode>();
        gm.allowAchievements = false;
        gm.allowCheatedScores = false;
        gm.allowCheats = true;
        gm.allowLeaderboardScoring = false;
        gm.steamLeaderboardName = "";
        gm.allowHeightAchievements = true;
        gm.baseGamemode = true;
        gm.useGamemodeSettings = true;
        gm.modeType = M_Gamemode.GameType.standard;
        gm.capsuleName = "";
        gm.capsuleArt = endlessBase2.capsuleArt;
        gm.gamemodeName = "Endless Service Shaft";
        gm.introText = "ONLY SERVICE SHAFT";
        gm.isEndless = true;
        gm.hasPerks = true;
        gm.hasRevives = false;
        gm.name = "GM_Endless_ServiceShaft";
        gm.newGameText = "Start Run";
        
        gm.startItems = [
            new M_Gamemode.SpawnItem { itemid = "Item_Hammer", position = new Vector2(-0.4f, 0.0f) },
            new M_Gamemode.SpawnItem { itemid = "Item_Flashlight", position = new Vector2(0.4f, 0.0f) }
        ];
        
        gm.regions = [
            new M_Region() {
                regionName = "Service Shaft",
                startLevelReferences = [
                    GetLevelAsset("M3_Habitation_Shaft_Intro", __instance.baseDatabase)
                ],
                regionOrder = M_Region.RegionOrder.playlist,
                useRegionHeight = false,
                subregionGroups = [
                    new M_Region.SubregionGroup() {
                        subregions = [
                            new M_Subregion() {
                                subregionName = "ServiceShaft",
                                name = "ServiceShaft",
                                subregionHeight = 150,
                                useLevelCount = true,
                                announcementGroups = [],
                                flagBlacklist = [],
                                flagWhitelist = [],
                                subregionMaxLength = 5,
                                subregionMinLength = 3,
                                subregionOrder = M_Subregion.SubregionOrder.standard,
                                levelReferences = [
                                    GetLevelAsset("M3_Habitation_Shaft_01", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_02", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_03", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_04", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_05", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_06", __instance.baseDatabase),
                                    GetLevelAsset("M3_Habitation_Shaft_07", __instance.baseDatabase),
                                ],
                                sessionEventLists = [],
                            }
                        ]
                    }
                ],
                transitionLevels = [
                    new() {
                        fromRegion = "Breakroom",
                        levelReferences = [
                            GetLevelAsset("M3_Habitation_Endless_Shaft_Start", __instance.baseDatabase)
                        ],
                        levels = [
                            GetLevelAsset("M3_Habitation_Endless_Shaft_Start", __instance.baseDatabase).level
                        ],
                    },
                ],
                sessionEventLists = [],
            },
            new M_Region() {
                regionName = "Breakroom",
                startLevelReferences = [],
                regionOrder = M_Region.RegionOrder.playlist,
                regionHeight = 1f,
                subregionGroups = [
                    new M_Region.SubregionGroup() {
                        subregions = [
                            new M_Subregion() {
                                subregionName = "Breakroom",
                                name = "Breakroom",
                                subregionHeight = 150,
                                announcementGroups = [],
                                flagBlacklist = [],
                                flagWhitelist = [],
                                subregionMaxLength = 5,
                                subregionMinLength = 3,
                                subregionOrder = M_Subregion.SubregionOrder.single,
                                levelReferences = [
                                    GetLevelAsset("M3_Habitation_Endless_Breakroom_01", __instance.baseDatabase),
                                ],
                                sessionEventLists = [],
                            }
                        ]
                    }
                ],
                transitionLevels = [
                    new() {
                        fromRegion = "Service Shaft",
                        levelReferences = [
                            GetLevelAsset("M3_Habitation_Endless_Shaft_End", __instance.baseDatabase)
                        ],
                        levels = [
                            GetLevelAsset("M3_Habitation_Endless_Shaft_End", __instance.baseDatabase).level
                        ],
                    },
                ],
                sessionEventLists = [],
            }
        ];

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
        gm.screenArt = endlessBase2.screenArt;
        gm.gamemodeObjects = endlessBase.gamemodeObjects;
        gm.unlockHint = endlessBase.unlockHint;

        __instance.baseDatabase.gamemodeAssets.Add(gm);
        Plugin.endlessServiceShaftGamemode = gm;

        added = true;
    }
}