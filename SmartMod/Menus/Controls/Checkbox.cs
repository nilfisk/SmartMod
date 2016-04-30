using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using SvLib;

namespace SmartMod.Menus.Controls
{
    public class Checkbox : OptionsElement
    {
        public const int pixelsWide = 9;
        public static Rectangle sourceRectChecked = new Rectangle(0xec, 0x1a9, 9, 9);
        public static Rectangle sourceRectUnchecked = new Rectangle(0xe3, 0x1a9, 9, 9);
        public bool isChecked;

        public Checkbox(string label, int whichOption, [Optional, DefaultParameterValue(-1)] int x,
            [Optional, DefaultParameterValue(-1)] int y)
            : base(label, x, y, 9*Game1.pixelZoom, 9*Game1.pixelZoom, whichOption)
        {
            var field = Environment.Fields.First(f => f.Id == whichOption);
            isChecked = field?.ToBool ?? false;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Game1.mouseCursors, new Vector2(slotX + bounds.X, slotY + bounds.Y),
                isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked,
                Color.White*(greyedOut ? 0.33f : 1f), 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(b, slotX, slotY);
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (!greyedOut)
            {
                Game1.soundBank.PlayCue("drumkit6");
                base.receiveLeftClick(x, y);
                isChecked = !isChecked;

                var field = Environment.Fields.First(f => f.Id == whichOption);
                field.Value = !field.ToBool;
                Config.Save();
            }
        }
    }
}