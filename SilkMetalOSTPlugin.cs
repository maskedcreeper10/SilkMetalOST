using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using SilkMetalOST.Helpers;

namespace SilkMetalOST;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public partial class SilkMetalOST : BaseUnityPlugin
{
    //Global vars
    internal static ManualLogSource log;
    public readonly Assembly assembly = Assembly.GetExecutingAssembly();
    public static ConfigSettings settings = new ConfigSettings(); 

    private static Dictionary<string, AudioClip> AudioCache = new Dictionary<string, AudioClip>();



    private static readonly Dictionary<string, string> Exceptions = new Dictionary<string, string>
    {
        {"flea_games_start_sting", "H204 GEB A Start Sting 3"}, // the same (for some reason) ui_music_sting_to_use_after_locking_in_silk_skill_to_crest
        {"Bellhart_Intro_no_audio_walk_entrance", "Bellheart_Intro" },
        {"needolin_bell_beast_v2_amplified", "needolin_bell_beast_v2" },
        {"H179_new_wish_promised redo", "H179_new_wish_granted redo" }, //swap these 2 because TC can't name files correctly, and they are actually swapped ingame.
        {"H179_new_wish_granted redo", "H179_new_wish_promised redo" }, //swap these 2 because TC can't name files correctly, and they are actually swapped ingame.
    };

    //settings categories:
    
    private static readonly List<string> BossList = new List<string>
    {
        "H196-25 Strive",
        "H204 Generic Enemy Battle A Track",
        "RIP AND SHRED",
        "H92 Chorus",
        "Creepy Ambient",
        "Creepy Main",
        "Shreddy Game",
        "Shreddy Re-arrangement 2nd phase",
        "H177 Bell Beast with Live-17",
        "Grind Main Layer",
        "Grind Big Layer",
        "Grind Perc Option v2",
        "H170 v2-04 Choir Battle Track",
        "Abyss Battle Loop",
        "Tension Two",
        "H49-102 ACTION",
        "H54-220 Lace Battle",
        "H118 Final Judge v2-105",
        "H78 Phantom of the Organ RESTRUCTURE-33",
        "Cloak Rescue",
        "Cogwork Dancers Phase 1",
        "Cogwork Dancers Phase 2",
        "Cogwork Dancers Phase 3",
        "Cogwork Dancers Phase 4",
        "Cogwork Dancers Sting",
        "H144-71 WIP Trobbio",
        "H191-09 SILK A",
        "H191-14 SILK B",
        "H87 Flower Live Vox-117 Game ORCH STEM w intro vox",
        "H87 Flower paired back orch stem",
        "H87 Flower Live Vox-117 Game VOX STEM w intro vox",
        "Tormented Trobbio v2-06",
        "Coral Tower Battle-12",
        "H85 Coral Boss v2-170",
        "Seth",
        "Petals v2",
        "Clover Dancers Revision-12",
        "Final Fight v2-10 Phase 1 v2",
        "Final Fight v2-10 Phase 2",

    };
    private static readonly List<string> AreaList = new List<string>
    {
        "Title",
        "H6-08 Safe",
        "Fleatopia Paired Back A",
        "Fleatopia Paired Back B",
        "Flea Festival",
        "H32 Moss Cave-60 MAIN",
        "H32 Moss Cave-60 SUB",
        "Bonetown_main",
        "H49-102 MAIN BONE",
        "H49-102 MAIN LAVA",
        "H49-102 SUB",
        "H49-102 MAIN MOSSTOWN",
        "H49-102 MOSSTOWN SUB",
        "Ambient Silk",
        "Ambient Silk Escalated",
        "Ambient Silk Tension Sub",
        "H56-13 Deep",
        "H117 Wilds-30 MAIN",
        "H117 Wilds-30 SUB",
        "Memories",
        "H108-71 Greymoor v4 MAIN",
        "H108-71 Greymoor v4 SUB",
        "H82 Hunters Trail v4-47 MAIN",
        "H82 Hunters Trail v4-47 ACTION",
        "H74 Bellheart Cursed-08",
        "Bellhart",
        "Shellwood_main",
        "Shellwood_sub",
        "Ways",
        "H116 Coral Steps-42 MAIN ALT",
        "H116 Coral Steps-42 SUB",
        "Dust-fromSwampMain",
        "Dust-fromSwampSub",
        "Sad Wanderer - MAIN",
        "Sad Wanderer - SUB",
        "H159_MistMazeMain",
        "H159_MistMazeAmbient",
        "H159_MistMazeSub",
        "MistmazeSingleNote",
        "Coral Ruins Main",
        "Coral Ruins Sub",
        "H207 OpenUp only additional layer",
        "H207 OpenUp",
        "H207 OpenUp with additional layer 2",
        "H80-12 PEAK MAIN",
        "H80-12 PEAK CRYSTAL",
        "H80-12 PEAK SUB",
        "H211 Bit Dreamy",
        "Tomb",
        "H182 ambient citadel surrounds",
        "H208 Oppressed Main",
        "H208 Oppressed Sub",
        "H149-88 Citadel Halls MAIN",
        "H149-88 Citadel Halls SUB",
        "Cogwork Core v2 MAIN",
        "Cogwork Core v2 SUB",
        "H129-25 ward",
        "H180-02 HANG",
        "New Vaults Temp",
        "H190 Memorium",
        "H121 Enclave Split Main",
        "H121 Enclave Split Sub",
        "DeepDeepDocks",
        "H221 tense",
        "Last Dive Prologue v2 Layer 1",
        "Last Dive Prologue v2 Layer 2",
        "Last Dive Prologue v2 Layer 3",
        "H15-111 MAIN",
        "H15-111 MAIN ACTION",
        "H15-111 SUB",
        "H101-62 ASPID REVISED",
        "H101-62 ASPID REVISED SUB",
        "Bell Ambient Music",
        "H129-25 Weaver experiment 2",
        "Coral Tower Ambient-03",
        "RedMemory 1 Drone",
        "RedMemory 2 Theme Layer A",
        "RedMemory 3 Theme Layer B",
        "RedMemory 4 Theme Alt",
        "RedMemory 6 Theme Big Stand",
        "H182 Chapel Ambient",
        "H182 Chapel Ambient Battle Layer",
        "Lace After Fight",
        "Shrine Act 3",
        "Pinstress Battle",
        "Mini Games-07",
        "Enclave_Credits",
        "Final_Credits",
    };
    private static readonly List<string> NeedolinList = new List<string>
    {
        "needolin_bell_beast_v2_amplified",
        "needolin_alt_melodies_deep",
        "hornet_needolin_general_02_loud_and_echo_2d",
        "hornet_needolin_general_02_loud",
        "hornet_needolin_general_02",
        "melody_a_b_c_needolin_loop",
        "melody_a_finish",
        "melody_a_needolin_loop",
        "melody_b_loop_with_end_baked_inv2",
        "melody_b_needolin_loop",
        "melody_c_needolin_loop",
        "melody_trio_song_delivery_all_join_in_2d",
        "needolin_break_1",
        "needolin_break_2",
        "needolin_break_3",
        "architect_mel_needolin_initial_solo",
        "architect_mel_sequence_all_join_in_2d",
        "silk_fight_hornet_needolin_loop",
        "shakra_sing_rite_accompaniment_loop",
        "shakra_sing_rite_accompaniment_ending",
        "Shakra_sing_rite_first_loop",
        "Shakra_sing_rite_ending",
        "Memory_Needolin_sound",
        "widow harps wip",
    };
    private static readonly List<string> StingList = new List<string>
    {
        "bellway_toll_machine_melody_new_2",
        "H110 Boss Defeat-09", //unknown if it actually plays anywhere
        "H179 Boss Defeat Revision 2",
        "H179_new_wish_promised redo",
        "H179_new_wish_granted redo",
        "H109 Hornet Sting v3",
        "Abyss Battle Start Sting",
        "Abyss Battle End Sting",
        "H204 GEB A Start Sting 3",
        "H204 GEB A Start Sting 2",
        "secret_discovered_temp",
        "flea_games_start_sting",
        "bench_rest",
        "ui_music_sting_to_use_after_locking_in_silk_skill_to_crest",
        "ui_titlecard_pt_1",
        "ui_titlecard_pt_2",
    };
    private static readonly List<string> PsalmCylindersList = new List<string>
    {
        "cylinder_hearing",
        "cylinder_music_box_citadel_halls",
        "cylinder_music_box_hang",
        "cylinder_singing",
        "cylinder_surgery",
        "melody_b_musicbox_gramaphone",
    };


    // info necesary for helpers
    public static readonly Dictionary<string, string> CogworkDancersNames = new Dictionary<string, string>
    {
        {"P1 Music", "Cogwork Dancers Phase 1" },
        {"P2 Music", "Cogwork Dancers Phase 2" },
        {"P3 Music", "Cogwork Dancers Phase 3" },
        {"P4 Music", "Cogwork Dancers Phase 4" },
        {"Stop", "Cogwork Dancers Sting" },
    };
    public static readonly Dictionary<string, string[]> VideoAudioGOPaths = new Dictionary<string, string[]>
    {
        {"Intro_Cinematic", ["Sequence"] },
        {"Lace_Battle", ["Cinematic Player/Additional Audio"] },
        {"Cinematic_Ending_A", ["Cinematic Player"] },
        {"Ending_C", ["Cinematic Player/Extra Audio"] }, //unsure, but ending A does used Cinematic Player.
        {"Ending_A", ["Cinematic Player"] },
        {"Ending_E", ["Cinematic Player"] },
        {"City_Reveal", ["Cinematic Player"] },
        {"City_Reveal_Inner", ["Cinematic Player"] },
        {"Bellhart_Intro", ["Spider Entry Audio", "Door Entry Audio"] },
        {"Pinstress_Battle", ["Cinematic Player"] },
        {"Plinney_Sharpen", ["Cinematic Player"] },
        {"Plinney_Sharpen_Oil", ["Cinematic Player"] },
        {"Seamstress_Flash", ["Cinematic Player"] },

    };
    public static readonly Dictionary<string, string[]> SceneGOPaths = new Dictionary<string, string[]>
    {
        {"Memory_Ant_Queen", ["Boss Scene/Audio Loop Carmelita Singing"] },
        {"Last_Dive", ["Scene Control/Cutscene Audio"] },
        {"Sprintmaster_Cave", ["Race Group/Sprintmaster Runner/Race Music Loop"] },
        {"Peak_08b", ["DJ Get Sequence/Calling Device/Tuning Fork/Needolin Audio", "DJ Get Sequence/Audio Loop NeedolinAmplified"] },
        {"Cradle_03", ["Boss Scene/Death Sequence/Audio Loop Needolin"] },
        {"Memory_Needolin", ["Memory Control/Sound"] },
        {"Cog_09", ["puzzle cylinders/All Choir Audio", "puzzle cylinders/Hornet Needolin Audio"] },
        //{"Belltown_04", ["Audio Widow distant harp"] },
        {"Belltown_Shrine", ["Black Thread States Thread Only Variant/Normal World/Boss Scene/Audio Loop Strum"] },
        {"Aqueduct_05_festival", ["Caravan_States/Flea Festival Outro/Audio Music"] },
        {"End_Credits_Scroll", ["Abyss_to_Surface_Style/Audio Player Music"] },
        {"End_Credits", ["Hornet_Credits_Plates"] },
        {"Under_27", ["Break Effects/One Way Wall break effect/Audio Player Actor (2)"] }
    };
    public static readonly List<string> EventResponderNames = new List<string>
    {
        "Secret Tone",
        "Fanfare Boss Defeat",
        "Fanfare Enemy Battle Clear", //unsure if this one is actually used
    };

    // text replacements for Metal Soul mode
    public static readonly Dictionary<string, string> TextReplacements = new Dictionary<string, string>
    {
        {
            "Steel Soul", 
            "Metal Soul" 
        },
        {
            "No reviving. Death is permanent.", 
            "No reviving. Death is permanent. <br>All is metal." 
        },
        {
            "Finish the game in Steel Soul mode", 
            "Finish the game in Metal Soul mode" 
        },
        {
            "Steel Heart", 
            "Metal Heart" 
        },
        {
            "Achieve 100% game completion and finish the game in Steel Soul mode", 
            "Achieve 100% game completion and finish the game in Metal Soul mode" 
        },
        {
            "Steel", 
            "Metal" 
        },
        {
            "STEEL SOUL MODE", 
            "METAL SOUL MODE " 
        },
        {
            "You played masterfully and proved you have a Steel Soul.<br>Thank you for taking the time to explore and conquer the world of<br>Hollow Knight: Silksong.<br>We’ll meet again with a new challenge for you...", 
            "You played masterfully and proved you have a Metal Soul.<br>Thank you for taking the time to explore and conquer the world of<br>Hollow Knight: Silksong.<br>We’ll meet again with a new challenge for you..." 
        },
        {
            "...Bug... Higher... It has come seeking... Zi is found... She is awoken.<hpage>Greetings, child of steel. I have known your type, in past. From your presence, it is clear this land too has caught the long gaze of your Masters.<page>Masters... Yes... For them, Zi observes... obedient... always...<page>And you... a creature in half... Pale It... Bug It... old... stubborn... alone... You too are observed.",
            "...Bug... Higher... It has come seeking... Zi is found... She is awoken.<hpage>Greetings, child of metal. I have known your type, in past. From your presence, it is clear this land too has caught the long gaze of your Masters.<page>Masters... Yes... For them, Zi observes... obedient... always...<page>And you... a creature in half... Pale It... Bug It... old... stubborn... alone... You too are observed."
        },
        {
            "Steel Seer",
            "Metal Seer"
        },
        {
            "<hpage>This location was marked by the steel child. This camp is old, but still shows clear evidence of an attempt at a rite.<hpage>Whatever the vassal hoped to achieve here seems to have failed. The other marked locations may provide more clues to its intent.",
            "<hpage>This location was marked by the metal child. This camp is old, but still shows clear evidence of an attempt at a rite.<hpage>Whatever the vassal hoped to achieve here seems to have failed. The other marked locations may provide more clues to its intent."
        },
        {
            "Investigate the resting sites marked by Steel Seer Zi to uncover the location of her vassal.",
            "Investigate the resting sites marked by Metal Seer Zi to uncover the location of her vassal."
        },
        {
            "<hpage>The second spot marked by the child of steel. This camp too was used by their vassal, and another rite was conducted here, this one incomplete. <hpage>The action was clearly taken in haste. Did they believe themself pursued? The etchings here are crude, but the words are disturbing... and desperate.",
            "<hpage>The second spot marked by the child of metal. This camp too was used by their vassal, and another rite was conducted here, this one incomplete. <hpage>The action was clearly taken in haste. Did they believe themself pursued? The etchings here are crude, but the words are disturbing... and desperate."
        },
        {
            "<hpage>I understand now... The steel child’s servant grew bold...<hpage>I know what the fleeing bug seeks to call, and their etchings here suggest clear clues to where. <hpage>What they attempt is dangerous, stupid. It can only end badly. I must hurry if I’m to stop this.",
            "<hpage>I understand now... The metal child’s servant grew bold...<hpage>I know what the fleeing bug seeks to call, and their etchings here suggest clear clues to where. <hpage>What they attempt is dangerous, stupid. It can only end badly. I must hurry if I’m to stop this."
        },
        {
            "No. No. No... My champion slain!<page>Why do you do this, cruel slasher? Why must I stay, and serve, so long, long, long?<hpage>That void must not be called, bug. Not ever. None can control it, though many have tried. <hpage>Any strength you hoped it offered would fast be proven false.<hpage>Consider yourself fortunate I arrived in time. Your shell is saved, all by wish of your steel companion, and fear of her Masters’ punishment.",
            "No. No. No... My champion slain!<page>Why do you do this, cruel slasher? Why must I stay, and serve, so long, long, long?<hpage>That void must not be called, bug. Not ever. None can control it, though many have tried. <hpage>Any strength you hoped it offered would fast be proven false.<hpage>Consider yourself fortunate I arrived in time. Your shell is saved, all by wish of your metal companion, and fear of her Masters’ punishment."
        },
        {
            "<page>But the seer knows nothing! Nothing of the pain. All small Sula’s to bear... To watch, watch these lands dying, slow, the bugs suffering, falling, while our shell sustains...<page>The steelhearts feel so little... but we forced to serve, we endure it all. We alone. Why?<hpage>...Others may know that pain, small bug... Or at least something similar.<page>So will you chain me again? Herd me by point of your blade, and knowingly return me to that pain?<hpage>...I will not. I will inform the child of steel of what has transpired here, but I cannot force any bug to return to a task so cruel.<page>Then, then... Sula is free? At least to run? By your mercy, bug, these legs must move, quick, quick and far, far!",
            "<page>But the seer knows nothing! Nothing of the pain. All small Sula’s to bear... To watch, watch these lands dying, slow, the bugs suffering, falling, while our shell sustains...<page>The metalhearts feel so little... but we forced to serve, we endure it all. We alone. Why?<hpage>...Others may know that pain, small bug... Or at least something similar.<page>So will you chain me again? Herd me by point of your blade, and knowingly return me to that pain?<hpage>...I will not. I will inform the child of metal of what has transpired here, but I cannot force any bug to return to a task so cruel.<page>Then, then... Sula is free? At least to run? By your mercy, bug, these legs must move, quick, quick and far, far!"
        },
        {
            "Return to Steel Seer Zi and inform her of Sula’s escape.",
            "Return to Metal Seer Zi and inform her of Sula’s escape."
        },
        {
            "Sermon of service, delivered by an ordained Vaultkeeper to acolytes in training.",
            "Guide track for the choir, with a theme of blood."
        },
        {
            "Last surgery of Conductor Mizello, performed in Whiteward’s operating theatre.",
            "Last rehearsal of Conductor Mizello, performed in Whiteward’s operating theatre."
        }

    };
    public static readonly Dictionary<string, string> SteelSoulTextReplacement = new Dictionary<string, string>
    {
    };




    //private static Fsm? NeedolinFSM = null;
    public static string CurrentScene = "";

    //loadup

    internal static void Log(string message)
    {
        log.LogInfo(message);
    }
    private void Awake()
    {
        log = Logger;
        Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        settings.InitMenu(Config);
        
        // load tracks
        using (Stream s = assembly.GetManifestResourceStream("SilkMetalOST.Resources.Assetbundles.audioassetbundle"))
        {
            if (s != null)
            {
                var AudioAssetBundle = AssetBundle.LoadFromStream(s);
                if (AudioAssetBundle != null)
                {
                    Log("audioassetbundle found correctly, may take a while to load.");
                    AudioClip[] cliplist = AudioAssetBundle.LoadAllAssets<AudioClip>();
                    foreach (AudioClip clip in cliplist)
                    {
                        AudioCache.Add(clip.name, clip);
                        Log("Loaded track: " + clip.name);
                    }
                    Log("Done making audiocache");
                }
                else Log("Loaded audioassetbundle is null");
            }
            else Log("ERROR: Could not find audioassetbundle");
        }
        SceneManager.sceneLoaded += MusicSceneEnter.enterscene;
      
        log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} has loaded!");
    }





    public static AudioClip? GetAudioOrNull(string name)
    {
        if (Exceptions.ContainsKey(name))
        {
            Log($"Found exception for {name} Instead using {Exceptions[name]}");
            name = Exceptions[name];
        }


        
        if (NeedolinList.Contains(name))
        {
            if (!settings.ReplaceNeedolin.Value) return null;
        }
        else if (StingList.Contains(name))
        {
            if (!settings.ReplaceStings.Value) return null;
        }
        else if (PsalmCylindersList.Contains(name))
        {
            if (!settings.ReplacePsalmCylinders.Value) return null;
        }



        else if (BossList.Contains(name))
        {
            if (settings.TracksToPlay.Value != ConfigSettings.PossibleTrackOptions.OnlyBosses && settings.TracksToPlay.Value != ConfigSettings.PossibleTrackOptions.All)
            {
                return null;
            }
        }
        else if (AreaList.Contains(name))
        {
            if (settings.TracksToPlay.Value != ConfigSettings.PossibleTrackOptions.OnlyAreas && settings.TracksToPlay.Value != ConfigSettings.PossibleTrackOptions.All)
            {
                return null;
            }
        }

        //At this point the track is either not in any of the lists (cutscenes), or it is in a list but it's allowed.
        if (AudioCache.ContainsKey(name))
        {
            return AudioCache[name];
        }
        else
        {
            return null;
        }
    }
}