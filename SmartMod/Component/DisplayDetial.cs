using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using SvLib;

namespace SmartMod
{
    public static class DisplayDetial
    {
        public static void Entry(Keys key)
        {
            
        }

        public static void Draw(SpriteBatch sB)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull() || !Data.DisplayDetial.ToBool) return;
            if (!Menu.Get.IsGameMenu() || Menu.Get.GetPages() == null ||
                !Menu.Get.GetPages()[((GameMenu)Menu.Get).currentTab].IsInventoryPage()) return;

            var font = Game1.smallFont;
            var totalprice =
                Game1.player.items.Where(item => item != null && item.maximumStackSize() > 2)
                    .Sum(item => (item.salePrice() / 2) * item.Stack);
            var listMessage = new List<string>();
            var listHover = new List<string>();
            if (Data.DisplayDetialHp.ToBool)
            {
                listMessage.Add(string.Format("Hp: {0}", Game1.player.health));
                listHover.Add(string.Format("Now: {0}\nMax: {1}", Game1.player.health, Game1.player.maxHealth));
            }
            if (Data.DisplayDetialSp.ToBool)
            {
                listMessage.Add(string.Format("Sp: {0}", Game1.player.stamina));
                listHover.Add(string.Format("Now: {0}\nMax: {1}", Game1.player.stamina, Game1.player.maxStamina));
            }
            if (Data.DisplayDetialLuck.ToBool)
            {
                listMessage.Add(string.Format("Luck: {0}", Game1.player.LuckLevel));
                listHover.Add(string.Format("{0}", Game1.player.experiencePoints[5]));
            }
            if (Data.DisplayDetialSpeed.ToBool)
            {
                listMessage.Add(string.Format("Speed: {0}", Game1.player.speed));
                listHover.Add(string.Format("{0}", Game1.player.speed));
            }
            if (Data.DisplayDetialLevel.ToBool)
            {
                listMessage.Add(string.Format("Level: {0}", Game1.player.Level));
                listHover.Add(string.Format("{0}", Game1.player.Level));
            }
            if (Data.DisplayDetialPrice.ToBool)
            {
                listMessage.Add(string.Format("Price: {0}", totalprice));
                listHover.Add(string.Format("${0}", totalprice));
            }
            if (Data.DisplayDetialSkill.ToBool)
            {
                listMessage.Add(string.Format("L.Farm: {0}", Game1.player.farmingLevel));
                listHover.Add(string.Format("Exp: {0}", Game1.player.experiencePoints[0]));
                listMessage.Add(string.Format("L.Fish: {0}", Game1.player.fishingLevel));
                listHover.Add(string.Format("Exp: {0}", Game1.player.experiencePoints[1]));
                listMessage.Add(string.Format("L.Forage: {0}", Game1.player.foragingLevel));
                listHover.Add(string.Format("Exp: {0}", Game1.player.experiencePoints[2]));
                listMessage.Add(string.Format("L.Mine: {0}", Game1.player.miningLevel));
                listHover.Add(string.Format("Exp: {0}", Game1.player.experiencePoints[3]));
                listMessage.Add(string.Format("L.Combat: {0}", Game1.player.combatLevel));
                listHover.Add(string.Format("Exp: {0}", Game1.player.experiencePoints[4]));
            }
            if (listMessage.Count == 0) return;
            float width = listMessage.Select(str => (int)font.MeasureString(str).X).Concat(new[] { 0 }).Max() +
                          (Game1.tileSize / 2);
            float height = listMessage.Select(str => (int)font.MeasureString(str).Y).Concat(new[] { 0 }).Max() +
                           (Game1.tileSize / 3);
            float x = (Menu.Get.xPositionOnScreen + Game1.tileSize / 2) - width;
            float y = Menu.Get.yPositionOnScreen + Game1.tileSize + (Game1.tileSize / 4);
            if (x < 0) x = 0;
            if ((y + height) > Game1.graphics.GraphicsDevice.Viewport.Height)
                y = Game1.graphics.GraphicsDevice.Viewport.Height - height;

            for (var i = 0; i < listMessage.Count; i++)
            {
                var floatBoxX = x;
                var floatBoxY = y + height * i;
                var floatBoxWidth = width;
                var floatBoxHeight = height;
                var floatTextX = floatBoxX + (float)Game1.tileSize / 4;
                var floatTextY = floatBoxY + (float)Game1.tileSize / 4;
                IClickableMenu.drawTextureBox(sB, Game1.menuTexture, new Rectangle(0, 0x100, 60, 60), (int)floatBoxX,
                    (int)floatBoxY, (int)floatBoxWidth, (int)floatBoxHeight, Color.White, 1f, false);
                Utility.drawTextWithShadow(sB, listMessage[i], font, new Vector2((int)floatTextX, (int)floatTextY),
                    Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            }
            var mousePoint = new Point(Game1.oldMouseState.X, Game1.oldMouseState.Y);
            var hoverText = listHover.Select((l, i) => new
            {
                Text = l,
                Rect =
                    new Rectangle((int)(x * Graphics.ZoomLevel), (int)((y + height * i) * Graphics.ZoomLevel),
                        (int)(width * Graphics.ZoomLevel), (int)(height * Graphics.ZoomLevel))
            })
                .FirstOrDefault(r => r.Rect.Contains(mousePoint))?.Text;
            if (!string.IsNullOrEmpty(hoverText)) IClickableMenu.drawHoverText(sB, hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
            Graphics.DrawCursor(sB);
        }
    }
}
