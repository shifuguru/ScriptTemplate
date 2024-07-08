using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Screen = GTA.UI.Screen;
using Control = GTA.Control;
using LemonUI;
using LemonUI.Menus;
using LemonUI.Elements;
using iFruitAddon2;

namespace ScriptTemplate
{
    public class Class1 : Script
    {
        public static string modName = "Script Name";
        public static string modVer = "Version 1.0";

        public static bool modEnabled = true;
        public static bool debugEnabled = true;

        public static ScriptSettings settings;

        public static Keys menuToggleKey = Keys.J;

        CustomiFruit _iFruit;

        private static readonly ObjectPool pool = new ObjectPool();
        private static readonly NativeMenu menu = new NativeMenu($"{modName}", $"{modVer}", " ");
        private static readonly NativeCheckboxItem ModToggleMenuItem = new NativeCheckboxItem("Mod Enabled: ", "Enables/Disables the Mod.", modEnabled);
        private static readonly NativeCheckboxItem DebugToggleMenuItem = new NativeCheckboxItem("Debug Enabled: ", "Enables Debug Notifications. Recommended: False", debugEnabled);

        public Class1()
        {
            LoadSettings();
            LoadMenuItems();
            LoadiFruitAddon();
            SaveSettings();

            Tick += OnTick;
            KeyDown += OnKeyDown;
            Aborted += OnAborted;
            Interval = 10;
        }

        public static void LoadSettings()
        {

        }
        public static void LoadMenuItems()
        {

        }
        public static void LoadiFruitAddon()
        {

        }
        public static void SaveSettings()
        {

        }

        private void OnTick(object sender, EventArgs e)
        {

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnAborted(object sender, EventArgs e)
        {

        }


    }
}
