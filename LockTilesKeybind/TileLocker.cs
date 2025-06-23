using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LockTilesKeybind
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class TileLocker : BaseUnityPlugin
    {
        private const string modGUID = "Bnuey.TileLock";
        private const string modName = " TileLocker";
        private const string modVersion = "1.0.0.4";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static TileLocker Instance;

        internal ManualLogSource mls;

        private static ConfigEntry<KeyboardShortcut> ToggleLock;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("TileLocker Enabled: Press R in game to lock/unlock tiles");

            harmony.PatchAll();

            ToggleLock = Config.Bind("Hotkeys", "Toggle Lock", new KeyboardShortcut(KeyCode.R));
        }

        [HarmonyPatch(typeof(CameraController))]

        internal class CameraControllerBPatch
        {
            static PlayerData _data;
            static bool _on;

            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            static void LockKeybindPatch()
            {
                _data = FindObjectOfType<PlayerData>();
            }

            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            static void UpdatePatch()
            {
                if (ToggleLock.Value.IsDown())
                {
                    _on = !_on;
                    _data.ChangeGroundLocked(_on);
                }
            }
        }

    }

    
}
