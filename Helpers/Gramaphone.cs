using HarmonyLib;
using UnityEngine;

namespace SilkMetalOST.Helpers
{
    [HarmonyPatch(typeof(CollectionGramaphone), "Play")]
    public static class Gramaphone
    {
        [HarmonyPostfix]
        private static void OnPersistentAudioInstanceStart(CollectionGramaphone __instance, CollectableRelic playingRelicAudio, bool alreadyPlaying, RelicBoardOwner owner)
        {
            if (__instance.source != null && __instance.source.clip != null)
            {
                AudioClip clip = __instance.source.clip;
                //SilkMetalOST.Log($"Gramaphone clip name = |{clip.name}|");
                if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                {
                    SilkMetalOST.Log($"Replaced gramaphone clip |{clip.name}|");
                    __instance.source.clip = SilkMetalOST.GetAudioOrNull(clip.name);
                    if (!__instance.source.isPlaying)
                    {
                        __instance.source.Play();
                    }
                }
            }
        }
    }
}
