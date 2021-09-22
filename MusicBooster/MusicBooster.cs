using System;
using BepInEx;
using HarmonyLib;

namespace MusicBooster
{
    [BepInPlugin("MusicBooster", "Ultrakill Music Booster", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            (new Harmony("MusicBooster")).PatchAll();
        }
    }
    [HarmonyPatch(typeof(AudioMixerController))]
    public static class AudioMixerInjector
    {
        public static float volume;

        [HarmonyPatch("CalculateMusicVolume")]
        [HarmonyPrefix]
        public static void CalculateMusicVolumePrefix(ref float volume)
        {
            AudioMixerInjector.volume = volume;
        }

        [HarmonyPatch("CalculateMusicVolume")]
        [HarmonyPostfix]
        public static void CalculateMusicVolumePostfix(ref float __result)
        {
            if (volume == 0)
            {
                __result = -80f;
            }
            else if (volume < 0.8)
            {
                __result = (float)Math.Log10(volume / 0.8) * 20f;
            }
            else
            {
                __result = (float)Math.Log10(volume / 0.8) * 200f;
            }
        }
    }
}