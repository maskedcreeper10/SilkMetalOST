using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace SilkMetalOST.Helpers
{
    public static class MusicSceneEnter
    {
        internal static void enterscene(Scene scene, LoadSceneMode mode)
        {
            //SilkMetalOST.Log("Entered room " + scene.name);
            SilkMetalOST.CurrentScene = scene.name;
            //Next bit is pretty much entirely from Cogwork waltz mod
            if (scene.name == "Cog_Dancers_boss")
            {
                SilkMetalOST.Log("Entered cogwork dancers room.");
                GameObject controllobject = GameObject.Find("Dancer Control");
                if (controllobject != null)
                {
                    //Log("Found controlobject");
                    PlayMakerFSM controllFSM = null;
                    foreach (PlayMakerFSM fsm in controllobject.GetComponents<PlayMakerFSM>())
                    {
                        //Log($"Found fsm named {fsm.FsmName}");
                        if (fsm.FsmName == "Music Control")
                        {
                            //Log("Found Music Control");
                            controllFSM = fsm;
                            break;
                        }
                    }
                    if (controllFSM != null)
                    {
                        //Log("Found controlFSM");
                        foreach (FsmState state in controllFSM.FsmStates)
                        {
                            //SilkMetalOST.Log("COGWORK DANCER STATE NAME: |" + state.name + "|");
                            if (SilkMetalOST.CogworkDancersNames.ContainsKey(state.name) && SilkMetalOST.GetAudioOrNull(SilkMetalOST.CogworkDancersNames[state.name]) != null)
                            {
                                if (state.name == "Stop")
                                {
                                    state.Actions.OfType<AudioPlaySimple>().First().oneShotClip = SilkMetalOST.GetAudioOrNull(SilkMetalOST.CogworkDancersNames["Stop"]);
                                }
                                else
                                {
                                    state.Actions.OfType<SetAudioClip>().First().audioClip = SilkMetalOST.GetAudioOrNull(SilkMetalOST.CogworkDancersNames[state.name]);
                                }
                            }
                        }
                    }
                }
            }
            else if (SilkMetalOST.SceneGOPaths.ContainsKey(scene.name))
            {
                int amount = SilkMetalOST.SceneGOPaths[scene.name].Length;
                foreach (string GOpath in SilkMetalOST.SceneGOPaths[scene.name])
                {
                    GameObject GO = GameObject.Find(GOpath);
                    if (GO != null)
                    {
                        //SilkMetalOST.Log("Found GO");
                        AudioSource audioSource = GO.GetComponent<AudioSource>();
                        if (audioSource != null)
                        {
                            //SilkMetalOST.Log("Found audio source");
                            if (audioSource.clip != null)
                            {
                                if (SilkMetalOST.GetAudioOrNull(audioSource.clip.name) != null)
                                {
                                    audioSource.clip = SilkMetalOST.GetAudioOrNull(audioSource.clip.name);
                                    SilkMetalOST.Log($"Replaced scene audio source clip with: {audioSource.clip.name}");
                                    amount -= 1;
                                }
                                else SilkMetalOST.Log($"Could not find: {audioSource.clip.name}| in audiocache");
                            }
                            
                        }
                        else SilkMetalOST.Log("Did not find scene audio source");
                    }
                    else SilkMetalOST.Log($"Could not find GO: {GOpath}");
                }
                if (amount <= 0) return;


                //Could not find them by path, meaning they are probably inactive, so instead using Resources.FindObjectsOfTypeAll.
                AudioSource[] sources = Resources.FindObjectsOfTypeAll<AudioSource>();
                foreach (string GOpath in SilkMetalOST.SceneGOPaths[scene.name])
                {
                    string GOname = GOpath.Split('/').Last();
                    foreach (AudioSource source in sources)
                    {
                        if (source.gameObject.name == GOname)
                        {
                            //SilkMetalOST.Log($"Found audio source by name: {GOname}");
                            if (SilkMetalOST.GetAudioOrNull(source.clip.name) != null)
                            {
                                source.clip = SilkMetalOST.GetAudioOrNull(source.clip.name);
                                SilkMetalOST.Log($"Replaced scene audio source clip with: {source.clip.name}");
                                amount -= 1;
                            }
                            else SilkMetalOST.Log($"Could not find: {source.clip.name}| in audiocache");
                        }
                    }
                }
            }
        }
    }
}
