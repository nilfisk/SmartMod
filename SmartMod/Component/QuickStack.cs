using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using SvLib;
using Item = StardewValley.Item;

namespace SmartMod
{
    public static class QuickStack
    {
        public static void Entry(Keys key)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull() || !Data.QuickStack.ToBool) return;
            if (!Menu.Get.IsItemGrabMenu() && !Menu.Get.IsInventoryMenu() && !Menu.Get.IsGameMenu() && !Menu.Get.IsShopMenu()) return;
            if (!InputParse.IsSame(InputHelper.PressedKeys(key), Data.QuickStackKeyA.ToKeys) && key != Keys.None) return;
            var invPlayerRemove = new List<Item>();
            for (var iPlayer = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                iPlayer < Inventory.GetSpots(true);
                iPlayer++)
            {
                var itemPlayer = Game1.player.items[iPlayer];
                if (itemPlayer.IsNull() || !itemPlayer.CanStack() || itemPlayer.IsFull() ||
                    invPlayerRemove.Contains(itemPlayer)) continue;
                for (var iCombine = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                    iCombine < Inventory.GetSpots(true);
                    iCombine++)
                {
                    var itemCombine = Game1.player.items[iCombine];
                    if (itemCombine.IsNull() || itemPlayer.Same(itemCombine) || !itemPlayer.SameKind(itemCombine) ||
                        !itemCombine.CanStack() || itemCombine.IsFull()) continue;
                    var intStackMix = itemCombine.Stack + itemPlayer.Stack;
                    var intStackMax = itemCombine.GetMaxStack();
                    if (intStackMix > intStackMax)
                    {
                        itemCombine.RemoveStack(intStackMax - itemPlayer.Stack);
                        itemPlayer.Stack = intStackMax;
                    }
                    else
                    {
                        itemPlayer.AddStack(itemCombine.Stack);
                        invPlayerRemove.Add(itemCombine);
                    }
                }
            }
            foreach (var itemRemove in invPlayerRemove)
            {
                for (var i = 0; i < Game1.player.items.Count; i++)
                {
                    var itemNull = Game1.player.items[i];
                    if (itemNull.IsNull() || !itemRemove.Same(itemNull)) continue;
                    Game1.player.items[i] = null;
                    break;
                }
            }
            var listItem = new List<Item>();
            for (var i = 0; i < Inventory.GetSpots(true); i++) listItem.Add(null);
            for (var i = 0; i < Inventory.GetSpots(true); i++)
            {
                var itemPlayer = Game1.player.items[i];
                if (itemPlayer.IsNull()) continue;

                if (Data.SlotLock.ToBool && i < Inventory.GetSpotsLine())
                {
                    for (var j = 0; j < listItem.Count; j++)
                    {
                        var itemObject = listItem[j];
                        if (itemObject != null) continue;
                        listItem[j] = itemPlayer;
                        break;
                    }
                    continue;
                }

                if (!itemPlayer.CanStack())
                {
                    for (var j = 0; j < listItem.Count; j++)
                    {
                        var itemObject = listItem[j];
                        if (itemObject != null) continue;
                        listItem[j] = itemPlayer;
                        break;
                    }
                }
                else
                {
                    listItem.Reverse();
                    for (var j = 0; j < listItem.Count; j++)
                    {
                        var itemObject = listItem[j];
                        if (!itemObject.IsNull()) continue;
                        listItem[j] = itemPlayer;
                        break;
                    }
                    listItem.Reverse();
                }
            }
            Menu.Get.GetInventory().actualInventory = listItem;
            Game1.player.items = listItem;
        }
    }
}
