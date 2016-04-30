using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SmartMod
{
    public class SplitLabel : OptionsElement
    {
        public SplitLabel(string label) : base(label)
        {
            bounds = new Rectangle(0, 0, 0, 0);
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            Utility.drawTextWithShadow(b, label, Game1.tinyFont,
                new Vector2(((slotX + bounds.X) + bounds.Width) + (Game1.pixelZoom*2), slotY + bounds.Y),
                greyedOut ? Game1.textColor*0.33f : Color.White, 1f, 0.1f, -1, -1, 1f, 0);
        }
    }
}