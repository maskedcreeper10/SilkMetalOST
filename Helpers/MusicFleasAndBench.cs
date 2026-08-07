using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilkMetalOST.Helpers
{
    public static class MusicFleasAndBench
    {
        //
        //  Bench music (although also works for some others)
        //

        [HarmonyPatch(typeof(SetAudioClip), "OnEnter")]
        public static class OnSetAudioClip
        {
            [HarmonyPrefix]

            private static void OnFSMSetAudioClip(SetAudioClip __instance)
            {
                if (__instance.audioClip != null)
                {
                    if (__instance.audioClip.value != null)
                    {
                        AudioClip clip = (AudioClip)__instance.audioClip.value;
                        //SilkMetalOST.Log($"FSM audio clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name) != null && !(clip.name == "needolin_alt_melodies_deep" || clip.name == "needolin_bell_beast_v2"))
                        {
                            SilkMetalOST.Log($"Found FSM audio clip |{clip.name}| and replaced it");
                            __instance.audioClip.value = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                    //else SilkMetalOST.Log("OnFSMSetAudioClip ERROR __instance.audioclip isn't null, but __instance.audioclip.value IS null.");
                }
                //else SilkMetalOST.Log("OnFSMSetAudioClip ERROR __instance.audioclip is null");
            }
        }


        //
        // Flea caravan music
        //

        [HarmonyPatch(typeof(FadeAudio), "OnEnter")]
        public static class OnFadeAudio
        {
            [HarmonyPrefix]

            private static void OnFSMFadeAudio(FadeAudio __instance)
            {
                //Log($"Started FSM fade on fsm name: {__instance.fsm.name}");
                //Log($"Started FSM fade on state name: {__instance.fsmState.name}");
                //Log($"Started FSM fade on state: {__instance.State.name}");
                //Log($"Started FSM fade on volume: {__instance.startVolume.value}");
                //Log($"Ended FSM fade on volue: {__instance.endVolume.value}");
                //Log("");

                if (__instance.fsm.name == "RestArea Music Control")
                {
                    //Log("Playing from flea FSM.");
                    GameObject GO = GameObject.Find("_GameManager/AudioManager/Music/FleaCaravan");
                    if (GO != null)
                    {
                        AudioSource audioSource = GO.GetComponent<AudioSource>();
                        if (audioSource != null)
                        {
                            //Log("Found flea caravan audio source");

                            if (SilkMetalOST.GetAudioOrNull(audioSource.clip.name) != null)
                            {
                                audioSource.clip = SilkMetalOST.GetAudioOrNull(audioSource.clip.name);
                                if (!audioSource.isPlaying)
                                {
                                    audioSource.Play();
                                }
                                SilkMetalOST.Log($"Replaced flea caravan audio source clip with: {audioSource.clip.name}");
                            }
                            else SilkMetalOST.Log($"Could not find: {audioSource.clip.name}| in audiocache");
                        }
                        else SilkMetalOST.Log("Did not find flea caravan audio source");
                    }
                }
            }
        }
    }
}
