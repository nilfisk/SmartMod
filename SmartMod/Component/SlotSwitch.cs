using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using SvLib;
using Item = StardewValley.Item;

namespace SmartMod
{
    public static class SlotSwitch
    {
        public static void Entry(Keys key)
        {
            if (!Game1.hasLoadedGame || !Data.SlotSwitch.ToBool) return;
            if (!Menu.Get.IsNull() && !Menu.Get.IsItemGrabMenu() && !Menu.Get.IsInventoryMenu() &&
                !Menu.Get.IsGameMenu() && !Menu.Get.IsShopMenu()) return;
            if (Inventory.GetPlayerLevel() == 1) return;

            if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SlotSwitchKeyA.ToKeys))
            {
                MoveForward();
                Game1.soundBank.PlayCue("stoneStep");
            }
            else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SlotSwitchKeyB.ToKeys))
            {

                MoveSecond();
                Game1.soundBank.PlayCue("stoneStep");
            }
            else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SlotSwitchKeyC.ToKeys))
            {

                MoveThird();
                Game1.soundBank.PlayCue("stoneStep");
            }
        }

        public static void MoveForward()
        {
            var itemPlayer = Game1.player.items;
            itemPlayer.AddRange(itemPlayer.GetRange(0, Inventory.GetSpotsLine()));
            itemPlayer.RemoveRange(0, Inventory.GetSpotsLine());
        }

        public static void MoveSecond()
        {
            var itemPlayer = Game1.player.items;
            var intLine = 0;
            List<Item> listSlot;
            List<Item> listReplace;
            intLine = 1;
            listSlot = itemPlayer.GetRange(0, Inventory.GetSpotsLine());
            listReplace = itemPlayer.GetRange(Inventory.GetSpotsLine() * intLine, Inventory.GetSpotsLine());
            itemPlayer.RemoveRange(Inventory.GetSpotsLine() * intLine, Inventory.GetSpotsLine());
            itemPlayer.RemoveRange(0, Inventory.GetSpotsLine());
            itemPlayer.InsertRange(0, listReplace);
            itemPlayer.InsertRange(Inventory.GetSpotsLine() * intLine, listSlot);
        }

        public static void MoveThird()
        {
            var itemPlayer = Game1.player.items;
            var intLine = 0;
            List<Item> listSlot;
            List<Item> listReplace;
            intLine = 2;
            listSlot = itemPlayer.GetRange(0, Inventory.GetSpotsLine());
            listReplace = itemPlayer.GetRange(Inventory.GetSpotsLine() * intLine, Inventory.GetSpotsLine());
            itemPlayer.RemoveRange(Inventory.GetSpotsLine() * intLine, Inventory.GetSpotsLine());
            itemPlayer.RemoveRange(0, Inventory.GetSpotsLine());
            itemPlayer.InsertRange(0, listReplace);
            itemPlayer.InsertRange(Inventory.GetSpotsLine() * intLine, listSlot);
        }
    }
}
