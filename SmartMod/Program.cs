using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartMod.Menus;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Monsters;
using SvLib;
using Config = SvLib.Config;
using EventArgs = System.EventArgs;
using Texture = SvLib.Texture;

namespace SmartMod
{
    public class SmartMod : Mod
    {
        public override void Entry(params object[] objects)
        {
            GameEvents.GameLoaded += GameEvents_GameLoaded;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
            GraphicsEvents.OnPostRenderEvent += GraphicsEvents_OnPostRenderEvent;
            Config.Load();
        }

        private void GameEvents_GameLoaded(object sender, EventArgs e) => Event_GameLoaded();
        private void Event_GameLoaded()
        {
            Data.Textures.Texture = Texture.Create(Graphics.Device, Stream.FromAssembly("Content", "Texture", "icon.png"));
            ButtonMenu.Register();
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e) => Event_KeyPressed(e.KeyPressed);
        private void Event_KeyPressed(Keys key)
        {
            if (Data.SlotLockHotkey.ToBool) SlotLock.Entry(key);
            if (Data.SlotSwitchHotkey.ToBool) SlotSwitch.Entry(key);
            if (Data.SmartStorageHotkey.ToBool) SmartStorage.Entry(key);
            if (Data.QuickStackHotkey.ToBool) QuickStack.Entry(key);
            if (Data.ModMenuKeyHotkey.ToBool) ModMenu.Entry(key);
        }

        private void GraphicsEvents_OnPostRenderEvent(object sender, EventArgs e) => Events_PostRender();
        private void Events_PostRender()
        {
            InputHelper.Helper.Update();
            if (!Game1.hasLoadedGame) return;
            var sB = Graphics.SpriteBatch;
            SlotLock.Draw(sB);
            DisplayDetial.Draw(sB);
            ButtonMenu.Draw(sB);
            TileGrid.Draw(sB);
        }
    }
}