using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using SvLib;
using Environment = SvLib.Environment;
using EventArgs = SvLib.EventArgs;
using Config = SvLib.Config;

namespace SmartMod.Menus.Controls
{
    public class InputListener : OptionsElement
    {
        public static Rectangle setButtonSource = new Rectangle(0x126, 0x1ac, 0x15, 11);
        public List<string> buttonNames;
        private string listenerMessage;
        private bool listening;
        private Rectangle setbuttonBounds;

        public InputListener(string label, int whichOption, int slotWidth, [Optional, DefaultParameterValue(-1)] int x,
            [Optional, DefaultParameterValue(-1)] int y)
            : base(label, x, y, slotWidth - x, 11*Game1.pixelZoom, whichOption)
        {
            buttonNames = new List<string>();
            setbuttonBounds = new Rectangle(slotWidth - (0x1c*Game1.pixelZoom), y + (Game1.pixelZoom*3),
                0x15*Game1.pixelZoom, 11*Game1.pixelZoom);
            var field = Environment.Fields.First(f => f.Id == whichOption);
            if (field == null) return;
            buttonNames.Add(field.Value.ToString());
            Events.KeyDown += Events_KeyDown;
            ControlEvents.KeyReleased += ControlEvents_KeyReleased;
        }

        private void ControlEvents_KeyReleased(object sender, EventArgsKeyPressed e)
        {
            if (!greyedOut && listening && listenerMessage.Trim()!="")
            {
                var field = Environment.Fields.First(f => f.Id == whichOption);
                field.Value = listenerMessage;
                buttonNames[0] = listenerMessage;
                Game1.soundBank.PlayCue("coin");
                listening = false;
                GameMenu.forcePreventClose = false;
                Config.Save();
            }
        }

        private void Events_KeyDown(object sender, EventArgs.KeyDown e)
        {
            if (!greyedOut && listening)
            {
                if (e.Key == Keys.Escape)
                {
                    Game1.soundBank.PlayCue("bigDeSelect");
                    listening = false;
                    GameMenu.forcePreventClose = false;
                }
                else
                {
                    var keys = InputHelper.Helper.CurrentKeyboardState.GetPressedKeys().Aggregate("", (curr, k) => curr + k.ToString() + " + ");
                    keys = keys.Trim();
                    if (keys.EndsWith("+")) keys = keys.Substring(0, keys.LastIndexOf("+", StringComparison.Ordinal));
                    listenerMessage = keys;
                }
            }
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            if (buttonNames.Count() == 0)
            {
                Utility.drawTextWithShadow(b, label, Game1.dialogueFont, new Vector2(bounds.X + slotX, bounds.Y + slotY),
                    Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
            }
            else
            {
                Utility.drawTextWithShadow(b,
                    label + ": " + buttonNames.Last() + ((buttonNames.Count() > 1) ? (", " + buttonNames.First()) : ""),
                    Game1.dialogueFont, new Vector2(bounds.X + slotX, bounds.Y + slotY), Game1.textColor, 1f, 0.15f, -1,
                    -1, 1f, 3);
            }
            Utility.drawWithShadow(b, Game1.mouseCursors,
                new Vector2(setbuttonBounds.X + slotX, setbuttonBounds.Y + slotY), setButtonSource, Color.White, 0f,
                Vector2.Zero, Game1.pixelZoom, false, 0.15f, -1, -1, 0.35f);
            if (listening)
            {
                b.Draw(Game1.staminaRect,
                    new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width,
                        Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, 1, 1), Color.Black*0.75f, 0f,
                    Vector2.Zero, SpriteEffects.None, 0.999f);
                var p = Game1.dialogueFont.MeasureString(listenerMessage);
                var p2 = Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0);
                
                b.DrawString(Game1.dialogueFont, listenerMessage,
                    new Vector2(p2.X + (float)Game1.tileSize / 2 - p.X / 2, p2.Y - p.Y / 2), Color.White,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            var greyedOut = this.greyedOut;
        }

        public override void receiveKeyPress(Keys key)
        {

        }

        public override void receiveLeftClick(int x, int y)
        {
            if ((!greyedOut && !listening) && setbuttonBounds.Contains(x, y))
            {
                if (buttonNames.Count() == 0)
                {
                    switch (whichOption)
                    {
                    }
                }
                else
                {
                    listening = true;
                    Game1.soundBank.PlayCue("breathin");
                    GameMenu.forcePreventClose = true;
                    listenerMessage = "Press new key...";
                }
            }
        }
    }
}