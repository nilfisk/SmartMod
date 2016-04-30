using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SmartMod.Menus.Controls;
using StardewValley;
using StardewValley.Menus;
using SvLib;
using Environment = SvLib.Environment;

namespace SmartMod.Menus
{
    public class SettingPage : IClickableMenu
    {
        public const int indexOfGraphicsPage = 6;
        public const int itemsPerPage = 7;
        private readonly ClickableTextureComponent downArrow;
        private readonly List<OptionsElement> options;
        private readonly List<ClickableComponent> optionSlots;
        private readonly ClickableTextureComponent scrollBar;
        private readonly ClickableTextureComponent upArrow;
        public int currentItemIndex;
        private string descriptionText;
        private string hoverText;
        private int optionsSlotHeld;
        private Rectangle scrollBarRunner;
        private bool scrolling;

        public SettingPage(int x, int y, int width, int height) : base(x, y, width, height, false)
        {
            descriptionText = "";
            hoverText = "";
            optionSlots = new List<ClickableComponent>();
            options = new List<OptionsElement>();
            optionsSlotHeld = -1;
            upArrow =
                new ClickableTextureComponent(
                    new Rectangle((xPositionOnScreen + width) + (Game1.tileSize/4), yPositionOnScreen + Game1.tileSize,
                        11*Game1.pixelZoom, 12*Game1.pixelZoom), "", "", Game1.mouseCursors,
                    new Rectangle(0x1a5, 0x1cb, 11, 12), Game1.pixelZoom);
            downArrow =
                new ClickableTextureComponent(
                    new Rectangle((xPositionOnScreen + width) + (Game1.tileSize/4),
                        (yPositionOnScreen + height) - Game1.tileSize, 11*Game1.pixelZoom, 12*Game1.pixelZoom), "", "",
                    Game1.mouseCursors, new Rectangle(0x1a5, 0x1d8, 11, 12), Game1.pixelZoom);
            scrollBar =
                new ClickableTextureComponent(
                    new Rectangle(upArrow.bounds.X + (Game1.pixelZoom*3),
                        (upArrow.bounds.Y + upArrow.bounds.Height) + Game1.pixelZoom, 6*Game1.pixelZoom,
                        10*Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(0x1b3, 0x1cf, 6, 10),
                    Game1.pixelZoom);
            scrollBarRunner = new Rectangle(scrollBar.bounds.X,
                (upArrow.bounds.Y + upArrow.bounds.Height) + Game1.pixelZoom, scrollBar.bounds.Width,
                ((height - (Game1.tileSize*2)) - upArrow.bounds.Height) - (Game1.pixelZoom*2));
            for (var i = 0; i < 7; i++)
            {
                optionSlots.Add(
                    new ClickableComponent(
                        new Rectangle(xPositionOnScreen + (Game1.tileSize/4),
                            ((yPositionOnScreen + ((Game1.tileSize*5)/4)) + Game1.pixelZoom) +
                            (i*((height - (Game1.tileSize*2))/7)), width - (Game1.tileSize/2),
                            ((height - (Game1.tileSize*2))/7) + Game1.pixelZoom), ""));
            }
            options.Add(new OptionsElement(Environment.Assembly + " Menu v" + Environment.Version));
            options.Add(new Checkbox(Data.ButtonMenu.Display, Data.ButtonMenu.Id, -1, -1));
            options.Add(new Checkbox(Data.SmartStorage.Display, Data.SmartStorage.Id, -1, -1));
            options.Add(new Checkbox(Data.SlotSwitch.Display, Data.SlotSwitch.Id, -1, -1));
            options.Add(new Checkbox(Data.QuickStack.Display, Data.QuickStack.Id, -1, -1));
            options.Add(new Checkbox(Data.SlotLock.Display, Data.SlotLock.Id, -1, -1));
            options.Add(new Checkbox(Data.DisplayDetial.Display, Data.DisplayDetial.Id, -1, -1));

            options.Add(new OptionsElement("Stats Display Option"));
            options.Add(new Checkbox("Hp", Data.DisplayDetialHp.Id, -1, -1));
            options.Add(new Checkbox("Sp", Data.DisplayDetialSp.Id, -1, -1));
            options.Add(new Checkbox("Luck", Data.DisplayDetialLuck.Id, -1, -1));
            options.Add(new Checkbox("Speed", Data.DisplayDetialSpeed.Id, -1, -1));
            options.Add(new Checkbox("Level", Data.DisplayDetialLevel.Id, -1, -1));
            options.Add(new Checkbox("Price", Data.DisplayDetialPrice.Id, -1, -1));
            options.Add(new Checkbox("Skill", Data.DisplayDetialSkill.Id, -1, -1));

            options.Add(new OptionsElement("Enable Hotkey:"));
            options.Add(new Checkbox(Data.ModMenuKeyHotkey.Display, Data.ModMenuKeyHotkey.Id, -1, -1));
            options.Add(new Checkbox(Data.SmartStorageHotkey.Display, Data.SmartStorageHotkey.Id, -1, -1));
            options.Add(new Checkbox(Data.SlotSwitchHotkey.Display, Data.SlotSwitchHotkey.Id, -1, -1));
            options.Add(new Checkbox(Data.QuickStackHotkey.Display, Data.QuickStackHotkey.Id, -1, -1));
            options.Add(new Checkbox(Data.SlotLockHotkey.Display, Data.SlotLockHotkey.Id, -1, -1));

            options.Add(new OptionsElement("Hotkey Binding:"));
            options.Add(new InputListener(Data.ModMenuKey.Display, Data.ModMenuKey.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SmartStorageKeyA.Display, Data.SmartStorageKeyA.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SmartStorageKeyB.Display, Data.SmartStorageKeyB.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SmartStorageKeyC.Display, Data.SmartStorageKeyC.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SmartStorageKeyD.Display, Data.SmartStorageKeyD.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SlotSwitchKeyA.Display, Data.SlotSwitchKeyA.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SlotSwitchKeyB.Display, Data.SlotSwitchKeyB.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SlotSwitchKeyC.Display, Data.SlotSwitchKeyC.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.QuickStackKeyA.Display, Data.QuickStackKeyA.Id, optionSlots[0].bounds.Width, -1, -1));
            options.Add(new InputListener(Data.SlotLockKeyA.Display, Data.SlotLockKeyA.Id, optionSlots[0].bounds.Width, -1, -1));

            options.Add(new OptionsElement("Link View:"));
            options.Add(new LinkButton("Forum Thread", "http://community.playstarbound.com/threads/smapi-smartmod.108104/", optionSlots[0].bounds.Width, -1, -1));
            options.Add(new LinkButton("My Website", "http://kurobear.cn/", optionSlots[0].bounds.Width, -1, -1));

        }

        private void downArrowPressed()
        {
            downArrow.scale = downArrow.baseScale;
            currentItemIndex++;
            setScrollBarToCurrentIndex();
        }

        public override void draw(SpriteBatch b)
        {
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            for (var i = 0; i < optionSlots.Count(); i++)
            {
                if ((currentItemIndex >= 0) && ((currentItemIndex + i) < options.Count()))
                {
                    options[currentItemIndex + i].draw(b, optionSlots[i].bounds.X, optionSlots[i].bounds.Y);
                }
            }
            b.End();
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (!GameMenu.forcePreventClose)
            {
                upArrow.draw(b);
                downArrow.draw(b);
                if (options.Count() > 7)
                {
                    drawTextureBox(b, Game1.mouseCursors, new Rectangle(0x193, 0x17f, 6, 6), scrollBarRunner.X,
                        scrollBarRunner.Y, scrollBarRunner.Width, scrollBarRunner.Height, Color.White, Game1.pixelZoom,
                        false);
                    scrollBar.draw(b);
                }
            }
            if (!hoverText.Equals(""))
            {
                drawHoverText(b, hoverText, Game1.smallFont, 0, 0, -1, null, -1, null, null, 0, -1, -1, -1, -1, 1f, null);
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (!GameMenu.forcePreventClose)
            {
                base.leftClickHeld(x, y);
                if (scrolling)
                {
                    var num = scrollBar.bounds.Y;
                    scrollBar.bounds.Y =
                        Math.Min(
                            (((yPositionOnScreen + height) - Game1.tileSize) - (Game1.pixelZoom*3)) -
                            scrollBar.bounds.Height,
                            Math.Max(y, (yPositionOnScreen + upArrow.bounds.Height) + (Game1.pixelZoom*5)));
                    var num2 = (y - scrollBarRunner.Y)/((float) scrollBarRunner.Height);
                    currentItemIndex = Math.Min(options.Count - 7, Math.Max(0, (int) (options.Count*num2)));
                    setScrollBarToCurrentIndex();
                    if (num != scrollBar.bounds.Y)
                    {
                        Game1.soundBank.PlayCue("shiny4");
                    }
                }
                else if ((optionsSlotHeld != -1) && ((optionsSlotHeld + currentItemIndex) < options.Count))
                {
                    options[currentItemIndex + optionsSlotHeld].leftClickHeld(
                        x - optionSlots[optionsSlotHeld].bounds.X, y - optionSlots[optionsSlotHeld].bounds.Y);
                }
            }
        }

        public override void performHoverAction(int x, int y)
        {
            if (!GameMenu.forcePreventClose)
            {
                descriptionText = "";
                hoverText = "";
                upArrow.tryHover(x, y, 0.1f);
                downArrow.tryHover(x, y, 0.1f);
                scrollBar.tryHover(x, y, 0.1f);
                var scrolling = this.scrolling;
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((optionsSlotHeld != -1) && ((optionsSlotHeld + currentItemIndex) < options.Count))
            {
                options[currentItemIndex + optionsSlotHeld].receiveKeyPress(key);
            }
        }

        public override void receiveLeftClick(int x, int y, [Optional, DefaultParameterValue(true)] bool playSound)
        {
            if (!GameMenu.forcePreventClose)
            {
                if (downArrow.containsPoint(x, y) && (currentItemIndex < Math.Max(0, options.Count() - 7)))
                {
                    downArrowPressed();
                    Game1.soundBank.PlayCue("shwip");
                }
                else if (upArrow.containsPoint(x, y) && (currentItemIndex > 0))
                {
                    upArrowPressed();
                    Game1.soundBank.PlayCue("shwip");
                }
                else if (scrollBar.containsPoint(x, y))
                {
                    scrolling = true;
                }
                else if (((!downArrow.containsPoint(x, y) && (x > (xPositionOnScreen + width))) &&
                          ((x < ((xPositionOnScreen + width) + (Game1.tileSize*2))) && (y > yPositionOnScreen))) &&
                         (y < (yPositionOnScreen + height)))
                {
                    scrolling = true;
                    leftClickHeld(x, y);
                    releaseLeftClick(x, y);
                }
                currentItemIndex = Math.Max(0, Math.Min(options.Count() - 7, currentItemIndex));
                for (var i = 0; i < optionSlots.Count(); i++)
                {
                    if ((optionSlots[i].bounds.Contains(x, y) && ((currentItemIndex + i) < options.Count())) &&
                        options[currentItemIndex + i].bounds.Contains(x - optionSlots[i].bounds.X,
                            y - optionSlots[i].bounds.Y))
                    {
                        options[currentItemIndex + i].receiveLeftClick(x - optionSlots[i].bounds.X,
                            y - optionSlots[i].bounds.Y);
                        optionsSlotHeld = i;
                        return;
                    }
                }
            }
        }

        public override void receiveRightClick(int x, int y, [Optional, DefaultParameterValue(true)] bool playSound)
        {
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (!GameMenu.forcePreventClose)
            {
                base.receiveScrollWheelAction(direction);
                if ((direction > 0) && (currentItemIndex > 0))
                {
                    upArrowPressed();
                    Game1.soundBank.PlayCue("shiny4");
                }
                else if ((direction < 0) && (currentItemIndex < Math.Max(0, options.Count() - 7)))
                {
                    downArrowPressed();
                    Game1.soundBank.PlayCue("shiny4");
                }
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (!GameMenu.forcePreventClose)
            {
                base.releaseLeftClick(x, y);
                if ((optionsSlotHeld != -1) && ((optionsSlotHeld + currentItemIndex) < options.Count))
                {
                    options[currentItemIndex + optionsSlotHeld].leftClickReleased(
                        x - optionSlots[optionsSlotHeld].bounds.X, y - optionSlots[optionsSlotHeld].bounds.Y);
                }
                optionsSlotHeld = -1;
                scrolling = false;
            }
        }

        private void setScrollBarToCurrentIndex()
        {
            if (options.Count() > 0)
            {
                scrollBar.bounds.Y = (((scrollBarRunner.Height/Math.Max(1, (options.Count - 7) + 1))*currentItemIndex) +
                                      upArrow.bounds.Bottom) + Game1.pixelZoom;
                if (currentItemIndex == (options.Count() - 7))
                {
                    scrollBar.bounds.Y = (downArrow.bounds.Y - scrollBar.bounds.Height) - Game1.pixelZoom;
                }
            }
        }

        private void upArrowPressed()
        {
            upArrow.scale = upArrow.baseScale;
            currentItemIndex--;
            setScrollBarToCurrentIndex();
        }
    }
}