using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace EndlessServiceShaft.Hooks;

public static class AddGamemode
{
    [HarmonyPatch(typeof(UI_PlayPane), "Start")]
    [HarmonyPrefix]
    public static void PlayPaneStart(ref UI_PlayPane __instance)
    {
        GameObject otherEndless02 = GameObject.FindObjectsOfType<GameObject>(true).First(x => x.name == "Other Endless 02" && x.transform.parent.parent.parent.name == "Play Pane - Scroll View Tab - Endless Variant");

        GameObject otherEndless03 = GameObject.Instantiate(otherEndless02);
        otherEndless03.name = "Other Endless 03";
        otherEndless03.transform.SetParent(otherEndless02.transform.parent);

        Transform endlessBase = otherEndless03.transform.GetChild(0);

        for (int i = 0; i < otherEndless03.transform.childCount; i++)
        {
            if (i == 0) continue;
            GameObject.Destroy(otherEndless03.transform.GetChild(i).gameObject);
        }

        GameObject endlessServiceShaft = endlessBase.gameObject;
        endlessServiceShaft.name = "Mode Selection Button - Service Shaft";

        endlessServiceShaft.GetComponent<Button>().onClick.RemoveAllListeners();
        endlessServiceShaft.GetComponent<UI_CapsuleButton>().unlockAchievement = Plugin.endlessServiceShaftGamemode.unlockAchievement; 
        endlessServiceShaft.GetComponent<UI_Gamemode_Button>().gamemode = Plugin.endlessServiceShaftGamemode;
        endlessServiceShaft.GetComponent<UI_Gamemode_Button>().Initialize();
        otherEndless03.transform.localScale = Vector3.one;

        var csf = otherEndless03.transform.parent.GetComponent<ContentSizeFitter>();
        UI_TabGroup tabGroup = otherEndless03.transform.parent.parent.parent.parent.parent.Find("Tabs").GetComponent<UI_TabGroup>();
        tabGroup.EventOnChangeTab.AddListener(() => {
            csf.Invoke("SetLayoutHorizontal", 0.05f);
        });
    }
}