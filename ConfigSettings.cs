using BepInEx.Configuration;

namespace SilkMetalOST
{
    public class ConfigSettings
    {
        public ConfigEntry<PossibleTrackOptions> TracksToPlay;
        public ConfigEntry<bool> ReplaceNeedolin;
        public ConfigEntry<bool> ReplacePsalmCylinders;
        public ConfigEntry<bool> ReplaceStings;
        public ConfigEntry<bool> ReplaceCutscenes;
        public ConfigEntry<bool> ReplaceDialogue;

        public enum PossibleTrackOptions
        {
            All,
            OnlyBosses,
            OnlyAreas,
            None
        }
        internal void InitMenu(ConfigFile config)
        {
            TracksToPlay = config.Bind(
                "general",
                "Metalify tracks",
                PossibleTrackOptions.All,
                "Changes which main tracks the mod affects.");
            ReplaceNeedolin = config.Bind(
                "general",
                "Replace needolin",
                true,
                "Toggles whether the mod replaces Needolin's music with metal versions."
                );
            ReplacePsalmCylinders = config.Bind(
                "general",
                "Replace Psalm Cylinders",
                true,
                "Toggles whether the mod replaces Psalm Cylinders with metal versions."
                );
            ReplaceStings = config.Bind(
                "general",
                "Replace miscellaneous audio",
                true,
                "Toggles things like wish granted, boss defeated, and secret discovered."
                );

            ReplaceCutscenes = config.Bind(
                "general",
                "Replace cutscenes",
                true,
                "Toggles whether the mod replaces cutscene music with metal versions."
                );
            ReplaceDialogue = config.Bind(
                "general",
                "Replace dialogue",
                true,
                "Toggles whether the mod replaces steel soul dialogue to metal soul dialogue."
                );
        }
    }
}
