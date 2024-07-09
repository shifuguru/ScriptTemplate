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
    public class Main : Script
    {
        // Script Template created by Shifuguru. 

        // If you'd like to add Build Event to copy your script to your game directory after each build, 
        // Add the script below to the Build Events - Post-Build Event commandline. 
        // xcopy /Y "$(TargetDir)$(TargetFileName)" "C:\Program Files\Rockstar Games\Grand Theft Auto V\scripts"
        // IMPORTANT: Replace the Directory as required based on where your game is installed, remember to include the 'scripts' folder: 

        // Enter your Script details as desired: 
        public static string modName = "Script Name";
        public static string modVer = "Version 1.0";

        // Basic Toggles to get you started: 
        public static bool modEnabled = true;
        public static bool debugEnabled = true;

        // Reference to Script Settings file: 
        public static ScriptSettings settings;

        // Mod Key, use this to open Menu or Enable/Disable script: 
        public static Control menuControl = Control.Whistle;
        public static Keys modToggleKey = Keys.J;

        // Create iFruit reference: 
        CustomiFruit _iFruit;

        // Create basic Menu items: 
        private static readonly ObjectPool pool = new ObjectPool();
        private static readonly NativeMenu menu = new NativeMenu($"{modName}", $"{modVer}", " ");
        private static readonly NativeCheckboxItem ModToggleMenuItem = new NativeCheckboxItem("Mod Enabled: ", "Enables/Disables the Mod.", modEnabled);
        private static readonly NativeCheckboxItem DebugToggleMenuItem = new NativeCheckboxItem("Debug Enabled: ", "Enables Debug Notifications. Recommended: False", debugEnabled);

        public Main()
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
        public static void SaveSettings()
        {

        }

        private void LoadMenuItems()
        {
            // INITIALISE ITEMS FOR MOD MENU 
            pool.Add(menu);
        }

        private void LoadiFruitAddon()
        {
            // Custom phone creation
            _iFruit = new CustomiFruit();
            /*
            _iFruit.CenterButtonColor = System.Drawing.Color.Orange;
            _iFruit.LeftButtonColor = System.Drawing.Color.LimeGreen;
            _iFruit.RightButtonColor = System.Drawing.Color.Purple;
            _iFruit.CenterButtonIcon = SoftKeyIcon.Fire;
            _iFruit.LeftButtonIcon = SoftKeyIcon.Police;
            _iFruit.RightButtonIcon = SoftKeyIcon.Website;
            */
            iFruitContact contactVHUD = new iFruitContact("Vanishing HUD");
            contactVHUD.Answered += ContactAnswered;   // Linking the Answered event with our function
            contactVHUD.DialTimeout = 3000;            // Delay before answering
            contactVHUD.Active = true;                 // true = the contact is available and will answer the phone
            contactVHUD.Icon = ContactIcon.Blank;      // Contact's icon
            _iFruit.Contacts.Add(contactVHUD);         // Add the contact to the phone
        }

        private void ContactAnswered(iFruitContact contact)
        {
            // The contact has answered: 
            if (debugEnabled)
            {
                Notification.Show("Vanishing HUD Menu Opened");
            }

            if (!menu.Visible)
            {
                ToggleMenu();
            }

            // We need to close the phone as the contact picks up by calling _iFruit.Close()
            // Here, we will close the phone in 0 seconds (0ms). 
            _iFruit.Close();
        }


        private void ToggleMod()
        {
            modEnabled = !modEnabled;

            if (debugEnabled)
            {
                if (modEnabled)
                {
                    Notification.Show("Mod ~g~Enabled~s~.");
                }
                else
                {
                    Notification.Show("Mod ~r~Disabled~s~.");
                }
            }
        }

        private void ToggleMenu()
        {
            menu.Visible = !menu.Visible;
        }


        private void OnTick(object sender, EventArgs e)
        {
            _iFruit.Update();
            pool.Process();

            // .. 
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Use this method if you just want a Keyboard action, i.e. Enabling/Disabling the Script.
            // Be aware that this method also fires repeatedly while the Key is held down. 
            // I advise avoiding this method, or using it sparingly. 
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Use this method for keyboard events.
            // i.e. toggling a setting from the Keyboard. 
            // Personally, I use LemonMenu to toggle my scripts. 

            // Simple toggle switch: 
            if (e.KeyCode == modToggleKey)
            {
                ToggleMod();
            }
            
        }

        private void OnAborted(object sender, EventArgs e)
        {

        }


    }
}
