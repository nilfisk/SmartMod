using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SvLib.Structure;

namespace SvLib
{
    public static class Data
    {
        #region Mod Menu

        public static ConfigField ModMenuKey = new ConfigField(0xA0, "ModMenuKey", "Mod Menu", new List<Keys> { Keys.F1 }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField ModMenuKeyHotkey = new ConfigField(1, "ModMenuKeyHotkey", "Mod Menu", "true");

        #endregion

        #region Button Menu

        public static ConfigField ButtonMenu = new ConfigField(2, "ButtonMenu", "Button Menu", "true");

        #endregion

        #region Smart Storage

        public static ConfigField SmartStorage = new ConfigField(3, "SmartStorage", "Smart Storage", "true");
        public static ConfigField SmartStorageHotkey = new ConfigField(13, "SmartStorageHotkey", "Smart Storage", "true");
        public static ConfigField SmartStorageKeyA = new ConfigField(0xA1, "SmartStorageKeyA", "Smart Storage A", new List<Keys> { Keys.Tab }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField SmartStorageKeyB = new ConfigField(0xA2, "SmartStorageKeyB", "Smart Storage B", new List<Keys> { Keys.Tab, Keys.LeftShift }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField SmartStorageKeyC = new ConfigField(0xA3, "SmartStorageKeyC", "Smart Storage C", new List<Keys> { Keys.Tab, Keys.LeftControl }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField SmartStorageKeyD = new ConfigField(0xA4, "SmartStorageKeyD", "Smart Storage D", new List<Keys> { Keys.Tab, Keys.LeftShift, Keys.LeftControl }.Aggregate("", (c, k) => c + "+" + k).Substring(1));

        #endregion

        #region Slot Switch

        public static ConfigField SlotSwitch = new ConfigField(4, "SlotSwitch", "Slot Switch", "true");
        public static ConfigField SlotSwitchHotkey = new ConfigField(14, "SlotSwitchHotkey", "Slot Switch", "true");
        public static ConfigField SlotSwitchKeyA = new ConfigField(0xA5, "SlotSwitchKeyA", "Slot Switch A", new List<Keys> { Keys.LeftAlt, Keys.OemTilde }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField SlotSwitchKeyB = new ConfigField(0xA6, "SlotSwitchKeyB", "Slot Switch B", new List<Keys> { Keys.LeftAlt, Keys.D1 }.Aggregate("", (c, k) => c + "+" + k).Substring(1));
        public static ConfigField SlotSwitchKeyC = new ConfigField(0xA7, "SlotSwitchKeyC", "Slot Switch C", new List<Keys> { Keys.LeftAlt, Keys.D2 }.Aggregate("", (c, k) => c + "+" + k).Substring(1));

        #endregion

        #region Quick Stack

        public static ConfigField QuickStack = new ConfigField(5, "QuickStack", "Quick Stack", "true");
        public static ConfigField QuickStackHotkey = new ConfigField(15, "QuickStackHotkey", "Quick Stack", "true");
        public static ConfigField QuickStackKeyA = new ConfigField(0xA8, "QuickStackKeyA", "Quick Stack A", new List<Keys> { Keys.Space }.Aggregate("", (c, k) => c + "+" + k).Substring(1));

        #endregion

        #region Slot Lock

        public static ConfigField SlotLock = new ConfigField(6, "SlotLock", "Slot Lock", "true");
        public static ConfigField SlotLockHotkey = new ConfigField(16, "SlotLockHotkey", "Slot Lock", "true");
        public static ConfigField SlotLockKeyA = new ConfigField(0xA9, "SlotLockKeyA", "Slot Lock A", new List<Keys> { Keys.OemTilde }.Aggregate("", (c, k) => c + "+" + k).Substring(1));

        #endregion

        #region Display Detial

        public static ConfigField DisplayDetial = new ConfigField(7, "DisplayDetial", "Display Detial", "true");
        public static ConfigField DisplayDetialHp = new ConfigField(20, "DisplayDetialHp", "Display HP", "false");
        public static ConfigField DisplayDetialSp = new ConfigField(21, "DisplayDetialSp", "Display SP", "false");
        public static ConfigField DisplayDetialLuck = new ConfigField(22, "DisplayDetialLuck", "Display Lucky", "false");
        public static ConfigField DisplayDetialSpeed = new ConfigField(23, "DisplayDetialSpeed", "Display Move Spd", "false");
        public static ConfigField DisplayDetialLevel = new ConfigField(24, "DisplayDetialLevel", "Display Level", "false");
        public static ConfigField DisplayDetialPrice = new ConfigField(25, "DisplayDetialPrice", "Display TotalPrice", "true");
        public static ConfigField DisplayDetialSkill = new ConfigField(26, "DisplayDetialSkill", "Display Skill", "true");

        #endregion

        public static class Textures
        {
            public static Texture2D Texture;
        }
    }
}