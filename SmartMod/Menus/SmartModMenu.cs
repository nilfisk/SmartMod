using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace SmartMod.Menus
{
    public class SmartModMenu : IClickableMenu
    {
        public const int collectionsTab = 5;
        public const int craftingTab = 4;
        public const int exitTab = 7;
        public const int inventoryTab = 0;
        public const int mapTab = 3;
        public const int numberOfTabs = 7;
        public const int optionsTab = 6;
        public const int skillsTab = 1;
        public const int socialTab = 2;
        public static bool forcePreventClose;
        private readonly List<IClickableMenu> pages;
        private readonly List<ClickableComponent> tabs;
        public int currentTab;
        private string descriptionText;
        private string hoverText;
        public bool invisible;
        private ClickableTextureComponent junimoNoteIcon;

        public SmartModMenu()
            : base(
                (Game1.viewport.Width/2) - ((800 + (borderWidth*2))/2),
                (Game1.viewport.Height/2) - ((600 + (borderWidth*2))/2), 800 + (borderWidth*2), 600 + (borderWidth*2),
                true)
        {
            hoverText = "";
            descriptionText = "";
            tabs = new List<ClickableComponent>();
            pages = new List<IClickableMenu>();
            tabs.Add(
                new ClickableComponent(
                    new Rectangle(xPositionOnScreen + Game1.tileSize,
                        (yPositionOnScreen + tabYPositionRelativeToMenuY) + Game1.tileSize, Game1.tileSize,
                        Game1.tileSize), "Options"));
            pages.Add(new SettingPage(xPositionOnScreen, yPositionOnScreen, width, height));
            forcePreventClose = false;
            if (Game1.options.gamepadControls && Game1.isAnyGamePadButtonBeingPressed())
            {
                setUpForGamePadMode();
            }
        }

        public SmartModMenu(int startingTab, [Optional, DefaultParameterValue(-1)] int extra) : this()
        {
            changeTab(startingTab);
            if ((startingTab == 6) && (extra != -1))
            {
                (pages[6] as OptionsPage).currentItemIndex = extra;
            }
        }

        public override bool areGamePadControlsImplemented()
        {
            return pages[currentTab].gamePadControlsImplemented;
        }

        public void changeTab(int whichTab)
        {
            width = 800 + (borderWidth*2);
            initializeUpperRightCloseButton();
            invisible = false;
            Game1.playSound("smallSelect");
        }

        public override void draw(SpriteBatch b)
        {
            if (!invisible)
            {
                if (!Game1.options.showMenuBackground)
                {
                    b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds,
                        Color.Black*0.4f);
                }
                Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, pages[currentTab].width,
                    pages[currentTab].height, false, true, null, false);
                pages[currentTab].draw(b);
                b.End();
                b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                if (!forcePreventClose)
                {
                    foreach (var component in tabs)
                    {
                        var num = 0;
                        switch (component.name)
                        {
                            case "Options":
                                num = 6;
                                break;
                        }
                        b.Draw(Game1.mouseCursors,
                            new Vector2(component.bounds.X,
                                component.bounds.Y + ((currentTab == getTabNumberFromName(component.name)) ? 8 : 0)),
                            new Rectangle(num*0x10, 0x170, 0x10, 0x10), Color.White, 0f, Vector2.Zero, Game1.pixelZoom,
                            SpriteEffects.None, 0.0001f);
                    }
                    b.End();
                    b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                    if (junimoNoteIcon != null)
                    {
                        junimoNoteIcon.draw(b);
                    }
                    if (!hoverText.Equals(""))
                    {
                        drawHoverText(b, hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1,
                            1f, null);
                    }
                }
            }
            else
            {
                pages[currentTab].draw(b);
            }
            if (!forcePreventClose)
            {
                base.draw(b);
            }
            b.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()),
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 0x2c : 0,
                    0x10, 0x10), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + (Game1.dialogueButtonScale/150f),
                SpriteEffects.None, 1f);
        }

        public static string getNameOfTabFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return "Options";
            }
            return "";
        }

        public int getTabNumberFromName(string name)
        {
            switch (name)
            {
                case "Options":
                    return 0;
            }
            return -1;
        }

        public override void leftClickHeld(int x, int y)
        {
            base.leftClickHeld(x, y);
            pages[currentTab].leftClickHeld(x, y);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            hoverText = "";
            pages[currentTab].performHoverAction(x, y);
            foreach (var component in tabs)
            {
                if (component.containsPoint(x, y))
                {
                    hoverText = component.name;
                    return;
                }
            }
            if (junimoNoteIcon != null)
            {
                junimoNoteIcon.tryHover(x, y, 0.1f);
                if (junimoNoteIcon.containsPoint(x, y))
                {
                    hoverText = junimoNoteIcon.hoverText;
                }
            }
        }

        public override bool readyToClose()
        {
            return (!forcePreventClose && pages[currentTab].readyToClose());
        }

        public override void receiveGamePadButton(Buttons b)
        {
            base.receiveGamePadButton(b);
            if (b == Buttons.RightTrigger)
            {
                if (currentTab == 3)
                {
                    Game1.activeClickableMenu = new GameMenu(4, -1);
                }
                else if ((currentTab < 6) && pages[currentTab].readyToClose())
                {
                    changeTab(currentTab + 1);
                }
            }
            else if (b == Buttons.LeftTrigger)
            {
                if (currentTab == 3)
                {
                    Game1.activeClickableMenu = new GameMenu(2, -1);
                }
                else if ((currentTab > 0) && pages[currentTab].readyToClose())
                {
                    changeTab(currentTab - 1);
                }
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (Game1.options.menuButton.Contains(new InputButton(key)) && readyToClose())
            {
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }
            pages[currentTab].receiveKeyPress(key);
        }

        public override void receiveLeftClick(int x, int y, [Optional, DefaultParameterValue(true)] bool playSound)
        {
            base.receiveLeftClick(x, y, playSound);
            if (!invisible && !forcePreventClose)
            {
                for (var i = 0; i < tabs.Count; i++)
                {
                    if ((tabs[i].containsPoint(x, y) && (currentTab != i)) &&
                        pages[currentTab].readyToClose())
                    {
                        changeTab(getTabNumberFromName(tabs[i].name));
                        return;
                    }
                }
                if ((junimoNoteIcon != null) && junimoNoteIcon.containsPoint(x, y))
                {
                    Game1.activeClickableMenu = new JunimoNoteMenu(true, 1, false);
                }
            }
            pages[currentTab].receiveLeftClick(x, y, true);
        }

        public override void receiveRightClick(int x, int y, [Optional, DefaultParameterValue(true)] bool playSound)
        {
            pages[currentTab].receiveRightClick(x, y, true);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            base.receiveScrollWheelAction(direction);
            pages[currentTab].receiveScrollWheelAction(direction);
        }

        public override void releaseLeftClick(int x, int y)
        {
            base.releaseLeftClick(x, y);
            pages[currentTab].releaseLeftClick(x, y);
        }

        public override void setUpForGamePadMode()
        {
            base.setUpForGamePadMode();
            if (pages.Count() > currentTab)
            {
                pages[currentTab].setUpForGamePadMode();
            }
        }
    }
}