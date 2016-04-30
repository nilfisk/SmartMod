using System.Collections.Generic;
using System.Linq;
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
    public static class ButtonMenu
    {
        public static List<ClickableButton> ButtonList;

        public static void Entry(Keys key)
        {

        }

        public static void Register()
        {
            if (ButtonList != null) return;
            var listStr = new List<string>
            {
                "Move Up",
                "Move Down",
                "Stack Up",
                "Stack Down",
                "Quick Stack",
                "Display Menu",
                "Key",
                "Warning",
                "Lock",
                "Tab Up",
                "Tab Down",
                "Tab 1",
                "Tab 2"
            };

            var intTextureHeight = 32;
            var intTextureWidth = 192;
            var intTexturePixel = 16;
            var intTextureCount = 0;

            var floatWidth = 32f;
            var floatHeight = 32f;

            var intRow = intTextureHeight / intTexturePixel;
            var intColumn = intTextureWidth / intTexturePixel;

            ButtonList = new List<ClickableButton>();
            for (var iH = 0; iH < intRow; iH++)
            {
                for (var iW = 0; iW < intColumn; iW++)
                {
                    var floatX = intTexturePixel * iW;
                    var floatY = intTexturePixel * iH;
                    var rectBound = new Rectangle(floatX, floatY, intTexturePixel, intTexturePixel);
                    ButtonList.Add(new ClickableButton(rectBound, listStr[intTextureCount], "Null"));
                    intTextureCount++;
                    if (intTextureCount >= listStr.Count) break;
                }
                if (intTextureCount >= listStr.Count) break;
            }
        }

        public static void Draw(SpriteBatch sB)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull()) return;
            if (!Data.ButtonMenu.ToBool) return;

            float floatWidth = 32;
            float floatHeight = 32;
            float floatBoxX = 0;
            float floatBoxY = 0;
            var boolDraw = false;
            var listButton = new List<string>();

            if (Menu.Get.IsGameMenu())
            {
                boolDraw = true;
                listButton.Add("Display Menu");
            }

            if (Menu.Get.IsGameMenu() && Menu.Get.GetPages() != null &&
                Menu.Get.GetPages()[((GameMenu)Menu.Get).currentTab].IsInventoryPage())
            {
                boolDraw = true;
                listButton.Add("Quick Stack");
                listButton.Add("Lock");
                listButton.Add("Tab Up");
                listButton.Add("Tab 1");
                listButton.Add("Tab 2");
            }
            if (Menu.Get.IsItemGrabMenu() && Menu.Get.IsChest())
            {
                boolDraw = true;
                listButton.Add("Move Up");
                listButton.Add("Move Down");
                listButton.Add("Stack Up");
                listButton.Add("Stack Down");
                listButton.Add("Quick Stack");
                listButton.Add("Lock");
                listButton.Add("Tab Up");
                listButton.Add("Tab 1");
                listButton.Add("Tab 2");
            }

            if (!boolDraw) return;
            var listGetButton = ClickableButton.GetByName(listButton, ButtonList);

            //Draw Button
            foreach (var button in listGetButton)
            {
                var floatBoxWidth = floatWidth;
                var floatBoxHeight = floatHeight;
                sB.Draw(Data.Textures.Texture,
                    new Rectangle((int)floatBoxX, (int)floatBoxY, (int)floatBoxWidth, (int)floatBoxHeight),
                    button.Bound, Color.White);
                button.Location = new Point((int)floatBoxX, (int)floatBoxY);
                button.Size = new Size((int)floatBoxWidth, (int)floatBoxHeight);
                floatBoxY += floatHeight;
            }

            //Hover
            var hoverText = listGetButton.Select((l, i) => new
            {
                Text = l.Name,
                Rect =
                    new Rectangle((int)(l.Location.X * Graphics.ZoomLevel), (int)((l.Location.Y) * Graphics.ZoomLevel),
                        (int)(l.Size.Width * Graphics.ZoomLevel), (int)(l.Size.Height * Graphics.ZoomLevel))
            })
                .FirstOrDefault(r => r.Rect.Contains(Game1.oldMouseState.X, Game1.oldMouseState.Y))?.Text;
            if (!string.IsNullOrEmpty(hoverText)) IClickableMenu.drawHoverText(sB, hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);

            //Click
            foreach (var button in listGetButton)
            {
                if (!button.IsHover(InputHelper.Helper.MousePosition) ||
                    (InputHelper.Helper.CurrentMouseState.LeftButton == ButtonState.Pressed ||
                     InputHelper.Helper.LastMouseState.LeftButton != ButtonState.Pressed)) continue;
                Clicked(button);
                break;
            }
            Graphics.DrawCursor(sB);
        }

        public static void Clicked(ClickableButton button)
        {
            switch (button.Name)
            {
                case "Move Up":
                    {
                        var menu = (ItemGrabMenu)Menu.Get;
                        if (menu.source != ItemGrabMenu.source_chest) return;
                        var invChest = menu.GetPrivateField<InventoryMenu>("ItemsToGrabMenu");
                        var invPlayer = menu.inventory;
                        SmartStorage.TakeToChest(invPlayer, invChest);
                        break;
                    }
                case "Move Down":
                    {
                        var menu = (ItemGrabMenu)Menu.Get;
                        if (menu.source != ItemGrabMenu.source_chest) return;
                        var invChest = menu.GetPrivateField<InventoryMenu>("ItemsToGrabMenu");
                        var invPlayer = menu.inventory;
                        SmartStorage.TakeToPlayer(invPlayer, invChest);
                        break;
                    }
                case "Stack Up":
                    {
                        var menu = (ItemGrabMenu)Menu.Get;
                        if (menu.source != ItemGrabMenu.source_chest) return;
                        var invChest = menu.GetPrivateField<InventoryMenu>("ItemsToGrabMenu");
                        var invPlayer = menu.inventory;
                        SmartStorage.StockToChest(invPlayer, invChest);
                        break;
                    }
                case "Stack Down":
                    {
                        var menu = (ItemGrabMenu)Menu.Get;
                        if (menu.source != ItemGrabMenu.source_chest) return;
                        var invChest = menu.GetPrivateField<InventoryMenu>("ItemsToGrabMenu");
                        var invPlayer = menu.inventory;
                        SmartStorage.StockToPlayer(invPlayer, invChest);
                        break;
                    }
                case "Quick Stack":
                    {
                        QuickStack.Entry(Keys.None);
                        break;
                    }
                case "Display Menu":
                    {
                        ModMenu.Show();
                        break;
                    }
                case "Key":
                    {
                        break;
                    }
                case "Warning":
                    {
                        break;
                    }
                case "Lock":
                    {
                        SlotLock.Entry(Keys.None);
                        break;
                    }
                case "Tab Up":
                    {
                        SlotSwitch.MoveForward();
                        break;
                    }
                case "Tab Down":
                    {
                        break;
                    }
                case "Tab 1":
                    {
                        SlotSwitch.MoveSecond();
                        break;
                    }
                case "Tab 2":
                    {
                        SlotSwitch.MoveThird();
                        break;
                    }
            }
        }
    }
}