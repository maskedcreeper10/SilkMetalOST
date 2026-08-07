using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace SilkMetalOST.Helpers
{
    public static class MusicStings
    {

        [HarmonyPatch(typeof(AudioPlayerOneShotSingle), "OnEnter")]
        public static class OnAudioPlayerOneShotSingle
        {
            [HarmonyPrefix]

            private static void onAudioPlayerOneShotSingle(AudioPlayerOneShotSingle __instance)
            {
                //SilkMetalOST.Log("Started AudioPlayerOneShotSingle");
                if (__instance.audioClip != null)
                {
                    //SilkMetalOST.Log("Audioclip != null");
                    if (__instance.audioClip.Value != null)
                    {
                        //SilkMetalOST.Log("Audioclip.value != null");
                        AudioClip clip = (AudioClip)__instance.audioClip.value;
                        //SilkMetalOST.Log($"FSM audio player one shot single clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                        {
                            SilkMetalOST.Log($"Found FSM audio player one shot single action with clip {clip.name} and replaced it");
                            __instance.audioClip.value = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                    //else SilkMetalOST.Log("[ERROR] __instance.audioClip.Value is null");
                }
            }
        }
        [HarmonyPatch(typeof(AudioPlayerOneShot), "OnEnter")]
        public static class OnAudioPlayerOneShot
        {
            [HarmonyPrefix]
            private static void onAudioPlayerOneShot(AudioPlayerOneShot __instance)
            {
                //SilkMetalOST.Log("Started AudioPlayerOneShot");
                if (__instance.audioClips != null && __instance.audioClips.Length > 0)
                {
                    //SilkMetalOST.Log("Audioclip != null");
                    if (__instance.audioClips[0] != null)
                    {
                        //SilkMetalOST.Log("Audioclip.value != null");
                        AudioClip clip = __instance.audioClips[0];
                        //SilkMetalOST.Log($"FSM audio player one shot clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                        {
                            SilkMetalOST.Log($"Found FSM audio player one shot action with clip |{clip.name}| and replaced it");
                            __instance.audioClips[0] = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                    //else SilkMetalOST.Log("[ERROR] __instance.audioClip.Value is null");
                }
            }
        }



        //might not be necesary
        [HarmonyPatch(typeof(PlayAudioEvent), "SpawnAudioEvent")]
        public static class OnPlayAudioEvent
        {
            [HarmonyPrefix]

            private static void OnFSMPlayAudioEvent(PlayAudioEvent __instance)
            {
                if (__instance.audioClip != null)
                {
                    if (__instance.audioClip.value != null)
                    {
                        AudioClip clip = (AudioClip)__instance.audioClip.value;
                        //SilkMetalOST.Log($"FSM play audio event clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                        {
                            SilkMetalOST.Log($"Found FSM play audio event action with clip |{clip.name}| and replaced it");
                            __instance.audioClip.value = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AudioPlaySimple), "OnEnter")]
        public static class OnAudioPlaySimple
        {
            [HarmonyPrefix]
            private static void PreFSMAudioPlaySimple(AudioPlaySimple __instance)
            {
                //SilkMetalOST.Log("Started AudioPlaySimple");
                if (__instance.oneShotClip != null)
                {
                    //SilkMetalOST.Log("oneShotClip != null");
                    AudioClip clip = (AudioClip)__instance.oneShotClip.Value;
                    //if (__instance.oneShotClip.Value == null) SilkMetalOST.Log("[Pre-ERROR] oneshotclip.value == null");
                    if (clip != null)
                    {
                        //SilkMetalOST.Log($"FSM audio play simple clip name = |{clip.name}|");
                        if (SilkMetalOST.GetAudioOrNull(clip.name) != null)
                        {
                            SilkMetalOST.Log($"Found FSM audio play simple action with clip |{clip.name}| and replaced it");
                            __instance.oneShotClip = SilkMetalOST.GetAudioOrNull(clip.name);
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(EventResponder), "Awake")]
        public static class OnEventResponderAwake
        {
            [HarmonyPrefix]
            private static void OnFSMEventResponderAwake(EventResponder __instance)
            {
                if (__instance.Event != null)
                {
                    //SilkMetalOST.Log($"Found event by name: |{__instance.Event.name}|");
                    if (SilkMetalOST.EventResponderNames.Contains(__instance.Event.name))
                    {
                        GameObject GO = __instance.gameObject;
                        if (GO != null)
                        {
                            AudioSource source = GO.GetComponent<AudioSource>();
                            if (source != null)
                            {
                                //SilkMetalOST.Log($"{__instance.Event.name} Awake source clip name = |{source.clip.name}|");
                                if (SilkMetalOST.GetAudioOrNull(source.clip.name) != null)
                                {
                                    SilkMetalOST.Log($"Found event |{__instance.Event.name}| Awake action with clip |{source.clip.name}| and replaced it");
                                    source.clip = SilkMetalOST.GetAudioOrNull(source.clip.name);
                                }
                            }
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(BattleScene), "Awake")]
        public static class OnBattleSceneAwake
        {
            [HarmonyPrefix]
            private static void onBattleSceneAwake(BattleScene __instance)
            {
                if (__instance.battleStartClip != null)
                {
                    if (SilkMetalOST.GetAudioOrNull(__instance.battleStartClip.name) != null)
                    {
                        SilkMetalOST.Log($"Found BattleScene start sting with clip |{__instance.battleStartClip.name}| and replaced it");
                        __instance.battleStartClip = SilkMetalOST.GetAudioOrNull(__instance.battleStartClip.name);
                        if (__instance.battleStartClipPause < 1.5f) __instance.battleStartClipPause = 1.5f;
                    }
                }
                if (__instance.battleEndClip != null)
                {
                    if (SilkMetalOST.GetAudioOrNull(__instance.battleEndClip.name) != null)
                    {
                        SilkMetalOST.Log($"Found BattleScene end sting with clip |{__instance.battleEndClip.name}| and replaced it");
                        __instance.battleEndClip = SilkMetalOST.GetAudioOrNull(__instance.battleEndClip.name);
                    }
                }
            }
        }


        //Used specifically for the music that plays when you beat up the flea effigy in fleatopia in act 3.
        [HarmonyPatch(typeof(AudioEventAnimationEvents), "PlayAudioEvent")]
        public static class OnAudioEventAnimationEventsPlayAudioevent
        {
            [HarmonyPrefix]
            private static void onAudioEventAnimationEventsPlayAudioevent(AudioEventAnimationEvents __instance, int index)
            {
                if (__instance.audioEvents != null && __instance.audioEvents.Length > 0)
                {
                    if (index >= 0 && index < __instance.audioEvents.Length)
                    {
                        AudioEventRandom audioEvent = __instance.audioEvents[index];
                        if (audioEvent.Clips != null && audioEvent.Clips.Length > 0)
                        {
                            //SilkMetalOST.Log($"AudioEventAnimationEvents PlayAudioEvent clip name = |{audioEvent.Clips[0].name}|");
                            if (SilkMetalOST.GetAudioOrNull(audioEvent.Clips[0].name) != null)
                            {
                                SilkMetalOST.Log($"Found AudioEventAnimationEvents PlayAudioEvent with clip |{audioEvent.Clips[0].name}| and replaced it");
                                audioEvent.Clips[0] = SilkMetalOST.GetAudioOrNull(audioEvent.Clips[0].name);
                            }
                        }
                    }
                }
            }
        }

        //Used for the noise that plays when you get a new crest and other abilities. (ui_crest_or_art_get_big_icon_on_screen)
        [HarmonyPatch(typeof(PlayRandomAudioEvent), "Play")]
        public static class playRandomAudioEvent
        {
            [HarmonyPrefix]
            private static void OnPlayRandomAudioEvent(PlayRandomAudioEvent __instance)
            {
                if (__instance.audioEvent.Clips != null && __instance.audioEvent.Clips.Length > 0)
                {
                    //SilkMetalOST.Log($"AudioEvent play random clip name = |{__instance.audioEvent.Clips[0].name}|");
                    if (SilkMetalOST.GetAudioOrNull(__instance.audioEvent.Clips[0].name) != null)
                    {
                        SilkMetalOST.Log($"Found AudioEvent play random with clip |{__instance.audioEvent.Clips[0].name}| and replaced it");
                        __instance.audioEvent.Clips[0] = SilkMetalOST.GetAudioOrNull(__instance.audioEvent.Clips[0].name);
                    }
                }
            }
        }

        //Sometimes necesary for widow pre fight needolin playing, in the room before
        [HarmonyPatch(typeof(RandomAudioStart), "Start")]
        public static class randomAudioStart
        {
            [HarmonyPostfix]
            private static void OnRandomAudioStart(RandomAudioStart __instance)
            {
                if (__instance.audioSource != null && __instance.audioSource.clip != null)
                {
                    //SilkMetalOST.Log($"RandomAudioStart clip name = |{__instance.audioSource.clip.name}|");
                    if (SilkMetalOST.GetAudioOrNull(__instance.audioSource.clip.name) != null)
                    {
                        SilkMetalOST.Log($"Found RandomAudioStart with clip |{__instance.audioSource.clip.name}| and replaced it");
                        __instance.audioSource.clip = SilkMetalOST.GetAudioOrNull(__instance.audioSource.clip.name);
                        if (!__instance.audioSource.isPlaying) __instance.audioSource.Play();
                    }
                }
            }
        }
    }
}
