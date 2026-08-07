using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System;
using UnityEngine;

namespace SilkMetalOST.Helpers
{
    public static class Needolin
    {
        //
        //  Regular needolin
        //

        //Made use of https://github.com/danielstegink/Silksong.DanielSteginkUtils/blob/915a4aece66c829458cac1ec73d494f3f2b7a37f/DanielSteginkUtils/Library/Helpers/Needolin.cs needolin helper class and CustomNeedolin both from DanielStegink
        [HarmonyPatch(typeof(StartNeedolinAudioLoop), "OnEnter")]
        public static class OnStartNeedolinAudioLoop
        {
            [HarmonyPrefix]
            private static void pre_start_needolin(StartNeedolinAudioLoop __instance)
            {
                SilkMetalOST.Log("[PRE-NEEDOLIN] Started playing needolin");


                if (__instance.State.Name == "Start Needolin Proper")
                {
                    FsmState state = __instance.State;
                    StartNeedolinAudioLoop action = (StartNeedolinAudioLoop)state.Actions[6];
                    if (SilkMetalOST.GetAudioOrNull(action.DefaultClip.value.name) == null)
                    {
                        return;
                    }
                    action.DefaultClip.value = SilkMetalOST.GetAudioOrNull(action.DefaultClip.value.name);
                    SilkMetalOST.Log("[PRE-NEEDOLIN] SUCCESFULLY set new regular audio");
                }
            }
        }


        //
        // Melody of the deep and bell beast melody
        //

        [HarmonyPatch(typeof(SetAudioClip), "OnEnter")]
        public static class onFSMSetAudioClip
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
                        //needolin
                        if ((clip.name == "needolin_alt_melodies_deep" || clip.name == "needolin_bell_beast_v2"))
                        {
                            //SilkMetalOST.Log("Found needolin.");
                            if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                            {
                                //SilkMetalOST.Log("Found needolin in cache and replaced it.");
                                __instance.audioClip.value = SilkMetalOST.GetAudioOrNull(clip.name);
                            }
                            else SilkMetalOST.Log("But could not find needolin in audiocache.");
                        }
                    }
                }
            }
        }


        //
        //  Special needolin (EG act 2 melodies)
        //

        [HarmonyPatch(typeof(OverrideNeedolinLoop), "StartSyncedAudio")]
        public static class OnStartSyncedAudio
        {
            [HarmonyPostfix]
            private static void PostStartSyncedAudio(OverrideNeedolinLoop __instance, AudioSource targetSource, AudioClip defaultClip)
            {
                //SilkMetalOST.Log("[OVERRIDENEEDOLINLOOP-POST] Started overrideneedolinloop.");

                if (targetSource.clip != null)
                {
                    //SilkMetalOST.Log($"[OVERRIDENEEDOLINLOOP-POST] Audio source clip name = |{targetSource.clip.name}|");
                    if (SilkMetalOST.GetAudioOrNull(targetSource.clip.name) != null)
                    {
                        SilkMetalOST.Log($"Found {targetSource.clip.name} in cache and replaced it.");
                        float time = targetSource.time;
                        targetSource.clip = SilkMetalOST.GetAudioOrNull(targetSource.clip.name);
                        time = Mathf.Clamp(time, 0f, targetSource.clip.length);
                        targetSource.time = time;
                        if (!targetSource.isPlaying)
                        {
                            targetSource.Play();
                            //SilkMetalOST.Log("Made targetsource play because wasn't playing");
                        }
                        
                    }
                }
            }


            //
            // Shakra rite ending
            //
            [HarmonyPatch(typeof(AudioPlayRandomVoice), "OnEnter")]
            public class OnAudioPlayRandomVoice
            {
                [HarmonyPrefix]
                private static void PreAudioPlayRandomVoice(AudioPlayRandomVoice __instance)
                {
                    if (__instance.audioClips != null && __instance.audioClips.Length > 0)
                    {
                        AudioClip clip = __instance.audioClips[0];
                        //SilkMetalOST.Log($"AudioPlayRandomVoice audio clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name))
                        {
                            SilkMetalOST.Log("Found shakra needolin/voice in cache and replaced it.");
                            __instance.audioClips[0] = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                }
            }


            //Used for the snapping of needolin strings
            [HarmonyPatch(typeof(PlayAudioEventRandom), "UpdateClipsArray")]
            public static class OnAudioEventRandomAwake
            {
                [HarmonyPrefix]
                private static void PostAudioEventRandomAwake(PlayAudioEventRandom __instance)

                {
                    if (__instance.State.name == "Silk Drained Effect")
                    {
                        if (__instance.audioClipsArray != null && __instance.audioClipsArray.Length > 0)
                        {
                            foreach (AudioClip clip in __instance.audioClipsArray)
                            {
                                //SilkMetalOST.Log($"AudioEventRandom audio clip name = |{clip.name}|");
                                if (SilkMetalOST.GetAudioOrNull(clip.name))
                                {
                                    //SilkMetalOST.Log("Found needolin break (array) and replaced it.");
                                    __instance.audioClipsArray[0] = SilkMetalOST.GetAudioOrNull(clip.name);
                                }
                            }
                        }
                        if (__instance.audioClips != null)
                        {
                            foreach (AudioClip clip in (__instance.audioClips.Values))
                            {
                                //SilkMetalOST.Log($"AudioEventRandom audio clip name = |{clip.name}|");
                                if (SilkMetalOST.GetAudioOrNull(clip.name))
                                {
                                    //SilkMetalOST.Log("Found needolin break (fsm) and replaced it.");
                                    int index = Array.FindIndex(__instance.audioClips.Values, c => c == clip);
                                    __instance.audioClips.Values[index] = SilkMetalOST.GetAudioOrNull(clip.name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
