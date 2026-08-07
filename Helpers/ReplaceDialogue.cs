using HarmonyLib;
using TeamCherry.Localization;

namespace SilkMetalOST.Helpers
{
    //Used for replacing the dialogue for Metal Soul.
    [HarmonyPatch(typeof(Language), nameof(Language.Get), [typeof(string), typeof(string)])]
    public static class ReplaceDialogue
    {
        [HarmonyPostfix]
        private static void OnGetLocalizedText(string? key, string? sheetTitle, ref string __result)
        {
            if (SilkMetalOST.settings.ReplaceDialogue.Value == false)
            {
                return;
            }
            //Log($"Found text, key = |{key}| and sheettitle = |{sheetTitle}| ");
            //SilkMetalOST.Log($"original text = |{__result}|");
            if (PlayerData.instance.GetInt("permadeathMode") == 1 && SilkMetalOST.SteelSoulTextReplacement.ContainsKey(__result))
            {
                __result = SilkMetalOST.SteelSoulTextReplacement[__result];
                //Log("Steel soul active");
            }
            if (SilkMetalOST.TextReplacements.ContainsKey(__result))
            {
                __result = SilkMetalOST.TextReplacements[__result];
                SilkMetalOST.Log($"Replaced text with |{__result}|");
            }
            return;
        }
    }
}
