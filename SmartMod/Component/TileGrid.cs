using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using SvLib;

namespace SmartMod
{
    public static class TileGrid
    {
        public static bool IsTileGrid;

        public static void Entry(Keys key)
        {
            return;
            if (!Game1.hasLoadedGame || !Menu.Get.IsNull()) return;
            IsTileGrid = !IsTileGrid;
        }

        public static void Draw(SpriteBatch sB)
        {
            return;
            if (!Game1.hasLoadedGame || !Menu.Get.IsNull()) return;
            if (!IsTileGrid) return;
            var x = -Game1.viewport.X % Game1.tileSize;
            float num5 = -Game1.viewport.Y % Game1.tileSize;
            for (var m = x; m < Game1.graphics.GraphicsDevice.Viewport.Width; m += Game1.tileSize)
            {
                sB.Draw(Game1.staminaRect,
                    new Rectangle(m, (int)num5, 1, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Green);
            }
            for (var n = num5; n < Game1.graphics.GraphicsDevice.Viewport.Height; n += Game1.tileSize)
            {
                sB.Draw(Game1.staminaRect, new Rectangle(x, (int)n, Game1.graphics.GraphicsDevice.Viewport.Width, 1),
                    Color.Green);
            }
        }
    }
}
