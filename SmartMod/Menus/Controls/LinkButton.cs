using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using SvLib;

namespace SmartMod.Menus.Controls
{
    public class LinkButton : OptionsElement
    {
        public const int pixelsWide = 9;
        public static Rectangle sourceRectChecked = new Rectangle(0xec, 0x1a9, 9, 9);
        public static Rectangle sourceRectUnchecked = new Rectangle(0xe3, 0x1a9, 9, 9);
        public string Link;

        public LinkButton(string label,string link, int whichOption, [Optional, DefaultParameterValue(-1)] int x,
            [Optional, DefaultParameterValue(-1)] int y)
            : base(label, x, y, 9*Game1.pixelZoom, 9*Game1.pixelZoom, whichOption)
        {
            Link = link;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Data.Textures.Texture, new Rectangle(slotX + bounds.X, slotY + bounds.Y, 32, 32),
                new Rectangle(112, 0, 16, 16), Color.White);
            base.draw(b, slotX, slotY);
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (!greyedOut)
            {
                Game1.soundBank.PlayCue("drumkit6");
                base.receiveLeftClick(x, y);
                Process.Start(Link);
            }
        }
    }
}