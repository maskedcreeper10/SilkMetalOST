using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SilkMetalOST.Helpers
{
    [HarmonyPatch(typeof(AudioManager), "ApplyMusicCue")]
    public static class MusicMain
    {
        private static string musicCueName = "";
        [HarmonyPrefix]
        public static void OnAudioManagerBeginApplyMusicCue(AudioManager __instance, MusicCue musicCue, float delayTime, float transitionTime, bool applySnapshot)
        {
            musicCueName = musicCue.name;
            SilkMetalOST.Log($"MusicCue = {musicCueName}");

            //Normal functionality
            MusicCue.MusicChannelInfo[] infos = Traverse.Create(musicCue).Field("channelInfos").GetValue<MusicCue.MusicChannelInfo[]>();
            foreach (MusicCue.MusicChannelInfo info in infos)
            {
                AudioClip origAudio = info.clip;
                if (origAudio != null)
                {
                    SilkMetalOST.Log($"Orignal audio name = {origAudio.name}");
                    AudioClip possibleReplace = null;

                    if (SilkMetalOST.GetAudioOrNull(origAudio.name) != null)
                    {
                        possibleReplace = SilkMetalOST.GetAudioOrNull(origAudio.name);
                    }
                    if (possibleReplace != null)
                    {
                        SilkMetalOST.Log($"Replaced clip with {possibleReplace.name}");

                        Traverse.Create(info).Field("clip").SetValue(possibleReplace);
                        Traverse.Create(musicCue).Field("channelInfos").SetValue(infos);
                    }
                }
            }
        }

        [HarmonyPostfix]
        public static void PostAudioManagerBeginApplyMusicCue(AudioManager __instance, MusicCue musicCue, float delayTime, float transitionTime, bool applySnapshot)
        {
            if (musicCue.name != musicCueName)
            {
                SilkMetalOST.Log($"Act 3 exception to beginapplymusiccue found, original = {musicCueName}, new = {musicCue.name}");
                MusicCue.MusicChannelInfo[] infos = Traverse.Create(musicCue).Field("channelInfos").GetValue<MusicCue.MusicChannelInfo[]>();
                foreach (MusicCue.MusicChannelInfo info in infos)
                {
                    AudioClip origAudio = info.clip;
                    if (origAudio != null)
                    {
                        SilkMetalOST.Log($"Orignal audio name = {origAudio.name}");
                        AudioClip possibleReplace = null;

                        if (SilkMetalOST.GetAudioOrNull(origAudio.name) != null)
                        {
                            //Log($"Cache hit for {origAudio.name}");
                            possibleReplace = SilkMetalOST.GetAudioOrNull(origAudio.name);
                        }
                        if (possibleReplace != null)
                        {
                            SilkMetalOST.Log($"Replaced clip with {possibleReplace.name}");

                            Traverse.Create(info).Field("clip").SetValue(possibleReplace);
                            Traverse.Create(musicCue).Field("channelInfos").SetValue(infos);
                        }
                    }
                }
            }
        }
    }
}
