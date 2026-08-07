using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Audio;

namespace SilkMetalOST.Helpers
{
    [HarmonyPatch(typeof(AudioManager), "BeginApplyMusicSnapshot")]
    public static class MusicTweaks
    {
        [HarmonyPrefix]
        private static void OnBeginApplyMusicSnapshot(AudioManager __instance, AudioMixerSnapshot snapshot, ref float delayTime, ref float transitionTime, bool blockMusicMarker)
        {
            //SilkMetalOST.Log($"Started BeginApplyMusicSnapshot: |{snapshot.name}|");
            //Log($"Transition time = |{transitionTime}|");
            //Log($"Delay time = |{delayTime}|");

            if (SilkMetalOST.CurrentScene == "Organ_01")
            {
                if (snapshot.name == "Normal - SlightlyMuffledOrgan")
                {
                    SilkMetalOST.Log("Changing transitiontime TO slightlymufled organ to 3 seconds.");
                    transitionTime = 3f;
                }
                else if (snapshot.name == "Normal")
                {
                    SilkMetalOST.Log("Changing transitiontime FROM slightlymufled organ to normal to 20 seconds");
                    transitionTime = 20f;
                }
                //HK Decline 4
                else if (snapshot.name == "HK Decline 4")
                {
                    SilkMetalOST.Log("Changing delaytime to crazy numbers so it just doesn't happen.");
                    delayTime = 1000f;
                }
            }
            else if (SilkMetalOST.CurrentScene == "Bone_11b")
            {
                if (snapshot.name == "Silent")
                {
                    SilkMetalOST.Log("Changing transitiontime for silent to 1 second, and removing the delay.");
                    transitionTime = 1f;
                    delayTime = 0f;
                }
            }

        }
    }
}
