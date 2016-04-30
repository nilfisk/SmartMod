using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using SvLib;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace SmartMod
{
    public static class SlotLock
    {
        public static void Entry(Keys key)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull()) return;
            if (!(Menu.Get.IsGameMenu() && Menu.Get.GetPages() != null &&
                  Menu.Get.GetPages()[((GameMenu)Menu.Get).currentTab].IsInventoryPage()) &&
                !(Menu.Get.IsItemGrabMenu() && Menu.Get.IsChest()) &&
                !(SmartStorage.AccessChestAnywhere.IsCorrectMenu(Menu.Get))) return;
            if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SlotLockKeyA.ToKeys) || key == Keys.None) Data.SlotLock.Value = !Data.SlotLock.ToBool;
        }

        public static void Draw(SpriteBatch sB)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull() || !Data.SlotLock.ToBool) return;

            Point point;
            var size = new Size(Game1.tileSize / 4, Game1.tileSize / 4);
            if (Menu.Get.IsItemGrabMenu() && Menu.Get.IsChest())
            {
                point = new Point(
                    Menu.Get.xPositionOnScreen + Game1.tileSize / 2 - (Game1.tileSize / 8) - (Game1.tileSize / 16) -
                    (Game1.tileSize / 32),
                    Menu.Get.yPositionOnScreen + Game1.tileSize * 5 + Game1.tileSize / 8
                    );
            }
            else if (SmartStorage.AccessChestAnywhere.IsCorrectMenu(Menu.Get))
            {
                point = new Point(
                    Menu.Get.xPositionOnScreen + Game1.tileSize / 2 - (Game1.tileSize / 8) - (Game1.tileSize / 16) -
                    (Game1.tileSize / 32),
                    Menu.Get.yPositionOnScreen + (int)(Game1.tileSize * 3.7) + Game1.tileSize / 8
                    );
                if (new Rectangle(point.X, point.Y, size.Width, size.Height).Contains(Game1.oldMouseState.X, Game1.oldMouseState.Y))
                    IClickableMenu.drawHoverText(sB, "Has been Injected by SmartMod" + SvLib.Environment.NewLine + "Only 'Quick Stack' available", Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
            }
            else if (Menu.Get.IsGameMenu() && Menu.Get.GetPages() != null &&
                     Menu.Get.GetPages()[((GameMenu)Menu.Get).currentTab].IsInventoryPage())
            {
                var pageInventory = Menu.Get.GetPages()[((GameMenu)Menu.Get).currentTab];
                point = new Point(
                    pageInventory.xPositionOnScreen + Game1.tileSize - Game1.tileSize / 4 - (Game1.tileSize / 8) -
                    (Game1.tileSize / 32),
                    pageInventory.yPositionOnScreen + Game1.tileSize * 2 + Game1.tileSize / 4
                    );
            }
            else { return; }
            var rect = new Rectangle(point.X, point.Y, size.Width, size.Height);
            sB.Draw(Data.Textures.Texture, rect, new Rectangle(128, 0, 16, 16), Color.White);
            Graphics.DrawCursor(sB);
        }
    }
}
