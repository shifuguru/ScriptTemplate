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
        public static string log = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScriptTemplate", "ScriptTemplate.log");
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
        private static readonly NativeCheckboxItem ModEnabledToggleMenuItem = new NativeCheckboxItem("Mod Enabled: ", "Enables/Disables the Mod.", modEnabled);
        private static readonly NativeCheckboxItem DebugEnabledToggleMenuItem = new NativeCheckboxItem("Debug Enabled: ", "Enables Debug Notifications. Recommended: False", debugEnabled);

        public Main()
        {
            try
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
            catch (Exception ex)
            {
                LogException("Main", ex);
            }
        }

        public static void LoadSettings()
        {
            try
            {
                settings = ScriptSettings.Load("scripts\\ScriptTemplate.ini");

                modToggleKey = settings.GetValue<Keys>("Controls", "Menu Toggle Key", modToggleKey);
                menuControl = settings.GetValue<Control>("Controls", "Menu Control", menuControl);
                modEnabled = settings.GetValue<bool>("Options", "Mod Enabled", modEnabled);
                debugEnabled = settings.GetValue<bool>("Options", "Debug Enabled", debugEnabled);

                SaveSettings();
            }
            catch (Exception ex)
            {
                LogException("LoadSettings", ex);
            }
        }
        public static void SaveSettings()
        {
            try
            {
                settings = ScriptSettings.Load("scripts\\ScriptTemplate.ini");

                settings.SetValue<Keys>("Controls", "Menu Toggle Key", modToggleKey);
                settings.SetValue<Control>("Controls", "Menu Control", menuControl);
                settings.SetValue<bool>("Options", "Mod Enabled", modEnabled);
                settings.SetValue<bool>("Options", "Debug Enabled", debugEnabled);
                settings.Save();
            }
            catch (Exception ex)
            {
                LogException("SaveSettings", ex);
            }
        }

        private void LoadMenuItems()
        {
            try
            {
                // INITIALISE ITEMS FOR LEMONUI MENU: 
                pool.Add(menu);
                menu.Add(ModEnabledToggleMenuItem);
                menu.Add(DebugEnabledToggleMenuItem);
                // Create Methods: 
                ModEnabledToggleMenuItem.Activated += ToggleModEnabled;
                DebugEnabledToggleMenuItem.Activated += ToggleDebugEnabled;
                // Set Variables: 
                ModEnabledToggleMenuItem.Checked = modEnabled;
                DebugEnabledToggleMenuItem.Checked = debugEnabled;
            }
            catch (Exception ex)
            {
                LogException("LoadMenuItems", ex);
            }
        }

        private void LoadiFruitAddon()
        {
            try
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
            catch (Exception ex)
            {
                LogException("LoadiFruitAddon", ex);
            }
        }

        private void ContactAnswered(iFruitContact contact)
        {
            try
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
            catch (Exception ex)
            {
                LogException("ContactAnswered", ex);
            }
        }

        public static void ToggleModEnabled(object sender, EventArgs e)
        {
            try
            {
                modEnabled = !modEnabled;
                ModEnabledToggleMenuItem.Checked = modEnabled;
                SaveSettings();

                if (modEnabled)
                {
                    Notification.Show("Mod ~g~Enabled~s~.");
                }
                else
                {
                    Notification.Show("Mod ~r~Disabled~s~.");
                }
            }
            catch (Exception ex)
            {
                LogException("ToggleModEnabled", ex);
            }
        }

        public static void ToggleDebugEnabled(object sender, EventArgs e)
        {
            try
            {
                debugEnabled = !debugEnabled;
                DebugEnabledToggleMenuItem.Checked = debugEnabled;
                SaveSettings();

                if (debugEnabled)
                {
                    Notification.Show("Debug ~g~Enabled~s~.");
                }
                else
                {
                    Notification.Show("Debug ~r~Disabled~s~.");
                }
            }
            catch (Exception ex)
            {
                LogException("ToggleDebugEnabled", ex);
            }            
        }

        private void ToggleMenu()
        {
            try
            {
                menu.Visible = !menu.Visible;
            }
            catch (Exception ex)
            {
                LogException("ToggleMenu", ex);
            }            
        }


        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                _iFruit.Update();
                pool.Process();

                // .. 
            }
            catch (Exception ex)
            {
                LogException("OnTick", ex);
            }            
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Use this method if you just want a Keyboard action, i.e. Enabling/Disabling the Script.
                // Be aware that this method also fires repeatedly while the Key is held down. 
                // I advise avoiding this method, or using it sparingly. 
            }
            catch (Exception ex)
            {
                LogException("OnKeyDown", ex);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        { 
            try
            {
                // Use this method for keyboard events.
                // i.e. toggling a setting from the Keyboard. 
                // Personally, I use LemonMenu to toggle my scripts.

                // Simple toggle switch: 
                if (e.KeyCode == modToggleKey)
                {
                    ToggleModEnabled(sender, e);
                }
            }
            catch (Exception ex)
            {
                LogException("OnKeyUp", ex);
            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                LogException("OnAborted", ex);
            }
        }

        public static void LogException(string methodName, Exception ex)
        {
            try
            {
                string message = $"[{DateTime.Now}] Error in {methodName} method. Exception: {ex.Message}";
                File.AppendAllText(log, $"{message}{Environment.NewLine}");
            }
            catch (Exception ex0)
            {
                Console.WriteLine($"Failed to log exception: {ex0.Message}");
            }
        }
    }
}
