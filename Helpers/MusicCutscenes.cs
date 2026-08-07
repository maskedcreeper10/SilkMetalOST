using HarmonyLib;
using UnityEngine;
using UnityEngine.Video;

namespace SilkMetalOST.Helpers
{
    [HarmonyPatch(typeof(VideoPlayer), "Play")]
    public static class MusicCutscenes
    {
        [HarmonyPostfix]
        private static void StartVideo(VideoPlayer __instance)
        {

            if (__instance.clip == null)
            {
                //SilkMetalOST.Log("Error, cinematic clip is null");
                return;
            }
            //SilkMetalOST.Log($"Starting cinematic: {__instance.clip.name}");

            if (!SilkMetalOST.settings.ReplaceCutscenes.Value)
            {
                //SilkMetalOST.Log("But setting ReplaceCutscenes is false, so not replacing.");
                return;
            }


            if (!SilkMetalOST.VideoAudioGOPaths.ContainsKey(__instance.clip.name))
            {
                //SilkMetalOST.Log("No GO path found for set cinematic");
                return;
            }
            foreach (string path in SilkMetalOST.VideoAudioGOPaths[__instance.clip.name])
            {
                GameObject GO = GameObject.Find(path);
                if (GO != null)
                {
                    AudioSource audioSource = GO.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        //SilkMetalOST.Log("Found audiosource for cinematic.");
                        if (audioSource.clip != null)
                        {
                            //SilkMetalOST.Log($"Cinematic audiosource clip: {audioSource.clip.name}");
                            if (SilkMetalOST.GetAudioOrNull(audioSource.clip.name) != null)
                            {
                                audioSource.clip = SilkMetalOST.GetAudioOrNull(audioSource.clip.name);
                                SilkMetalOST.Log($"Replaced cinematic: {audioSource.clip.name}");
                            }
                        }
                        else SilkMetalOST.Log("Couldn't find clip for cinematic audiosource.");
                    }
                    else SilkMetalOST.Log("Couldn't find audiosource for cinematic.");
                }
                else SilkMetalOST.Log("Couldn't find GO for cinematic.");
            }
        }
    }
}
