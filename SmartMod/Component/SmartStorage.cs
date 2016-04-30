using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using SvLib;
using Item = StardewValley.Item;

namespace SmartMod
{
    public static class SmartStorage
    {
        public static void Entry(Keys key)
        {
            if (!Game1.hasLoadedGame || Menu.Get.IsNull() || !Data.SmartStorage.ToBool) return;
            var isIsItemGrabMenu = Menu.Get.IsItemGrabMenu();
            var isAccessChestAnywhere = AccessChestAnywhere.IsCorrectMenu(Menu.Get);
            if (!(isIsItemGrabMenu || isAccessChestAnywhere)) return;
            if (isAccessChestAnywhere)
            {
                var menu = Menu.Get;
                var invChest = Menu.Get.GetPublicField<List<Item>>("chestItems");
                var invPlayer = Menu.Get.GetPrivateField<List<Item>>("playerItems") ?? Game1.player.items;
                if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyD.ToKeys))
                    AccessChestAnywhere.TakeToPlayer(menu, invPlayer, invChest);
                else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyC.ToKeys))
                    AccessChestAnywhere.TakeToChest(menu, invPlayer, invChest);
                else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyB.ToKeys))
                    AccessChestAnywhere.StockToPlayer(menu, invPlayer, invChest);
                else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyA.ToKeys))
                    AccessChestAnywhere.StockToChest(menu, invPlayer, invChest);
                else return;
            }
            else
            {
                var menu = (ItemGrabMenu)Menu.Get;
                var invChest = ((ItemGrabMenu)Menu.Get).GetPrivateField<InventoryMenu>("ItemsToGrabMenu");
                var invPlayer = ((ItemGrabMenu)Menu.Get).inventory;
                if (menu.source == ItemGrabMenu.source_chest)
                {
                    if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyD.ToKeys))
                        TakeToPlayer(invPlayer, invChest);
                    else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyC.ToKeys))
                        TakeToChest(invPlayer, invChest);
                    else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyB.ToKeys))
                        StockToPlayer(invPlayer, invChest);
                    else if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyA.ToKeys))
                        StockToChest(invPlayer, invChest);
                    else return;
                }
                else if (menu.source == ItemGrabMenu.source_fishingChest)
                {
                    if (InputParse.IsSame(InputHelper.PressedKeys(key), Data.SmartStorageKeyA.ToKeys))
                        TakeToPlayer(invPlayer, invChest);
                    else return;
                }
                else return;
                menu.hoveredItem = null;
            }
            Game1.soundBank.PlayCue("stoneStep");
        }

        public static void StockToChest(InventoryMenu invPlayer, InventoryMenu invChest)
        {
            var invPlayerRemove = new List<Item>();
            foreach (var itemChest in invChest.actualInventory)
            {
                if (itemChest.IsNull() || !itemChest.CanStack() || itemChest.IsFull()) continue;
                for (var i = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                    i < invPlayer.actualInventory.Count;
                    i++)
                {
                    var itemPlayer = invPlayer.actualInventory[i];
                    if (itemPlayer.IsNull() || !itemPlayer.SameKind(itemChest) || invPlayerRemove.Contains(itemPlayer))
                        continue;
                    var intStackMix = itemChest.Stack + itemPlayer.Stack;
                    var intStackMax = itemChest.GetMaxStack();
                    if (intStackMix > intStackMax)
                    {
                        itemPlayer.RemoveStack(intStackMax - itemChest.Stack);
                        itemChest.MaxStack();
                    }
                    else
                    {
                        itemChest.AddStack(itemPlayer.Stack);
                        invPlayerRemove.Add(itemPlayer);
                    }
                }
            }
            foreach (var itemRemove in invPlayerRemove) Inventory.RemoveItem(invPlayer, itemRemove, Inventory.GetSpots(true));
        }

        public static void StockToPlayer(InventoryMenu invPlayer, InventoryMenu invChest)
        {
            var invChestRemove = new List<Item>();
            foreach (var itemPlayer in invPlayer.actualInventory)
            {
                if (itemPlayer.IsNull() || itemPlayer.GetMaxStack() < 0 || itemPlayer.IsFull()) continue;
                foreach (var itemChest in invChest.actualInventory)
                {
                    if (itemChest.IsNull() || !itemPlayer.SameKind(itemChest) || invChestRemove.Contains(itemChest))
                        continue;
                    var intStackMix = itemChest.Stack + itemPlayer.Stack;
                    var intStackMax = itemPlayer.GetMaxStack();
                    if (intStackMix > intStackMax)
                    {
                        itemChest.Stack -= intStackMax - itemPlayer.Stack;
                        itemPlayer.MaxStack();
                    }
                    else
                    {
                        itemPlayer.Stack += itemChest.Stack;
                        invChestRemove.Add(itemChest);
                    }
                }
            }
            foreach (var itemRemove in invChestRemove) Inventory.RemoveItem(invChest, itemRemove, Inventory.GetSpots());
        }

        public static void TakeToChest(InventoryMenu invPlayer, InventoryMenu invChest)
        {
            var invChestAdd = new List<Item>();
            var invPlayerRemove = new List<Item>();
            StockToChest(invPlayer, invChest);
            for (var i = 0; i < Inventory.GetSpotsFree(invChest); i++)
            {
                for (var iPlayer = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                    iPlayer < Inventory.GetSpots(true);
                    iPlayer++)
                {
                    var itemPlayer = invPlayer.actualInventory[iPlayer];
                    if (itemPlayer.IsNull() || invPlayerRemove.Contains(itemPlayer)) continue;
                    invChestAdd.Add(itemPlayer);
                    invPlayerRemove.Add(itemPlayer);
                    break;
                }
            }
            foreach (var itemAdd in invChestAdd) Inventory.AddItem(invChest, itemAdd, Inventory.GetSpots(), true);
            foreach (var itemRemove in invPlayerRemove) Inventory.RemoveItem(invPlayer, itemRemove, Inventory.GetSpots(true));
        }

        public static void TakeToPlayer(InventoryMenu invPlayer, InventoryMenu invChest)
        {
            var invPlayerAdd = new List<Item>();
            var invChestRemove = new List<Item>();
            StockToPlayer(invPlayer, invChest);
            for (var i = 0; i < Inventory.GetSpots(true); i++)
            {
                var itemPlayer = invPlayer.actualInventory[i];
                if (itemPlayer != null) continue;
                foreach (var itemChest in invChest.actualInventory)
                {
                    if (itemChest == null || invChestRemove.Contains(itemChest)) continue;
                    invPlayerAdd.Add(itemChest);
                    invChestRemove.Add(itemChest);
                    break;
                }
            }
            foreach (var itemAdd in invPlayerAdd) Inventory.AddItem(invPlayer, itemAdd, Inventory.GetSpots(true));
            foreach (var itemRemove in invChestRemove) Inventory.RemoveItem(invChest, itemRemove, Inventory.GetSpots());
        }

        #region Compatible

        public static class AccessChestAnywhere
        {
            public const string MenuName = "AccessChestAnywhere.ACAMenu";
            public static bool IsCorrectMenu(IClickableMenu menu) => menu.IsCustomerMenu(MenuName);
            public const string PlayerItems = "playerItems";
            public const string ChestItems = "chestItems";

            public static void StockToChest(object menu, List<Item> invPlayer, List<Item> invChest)
            {
                RemoveSlots(menu, invChest);
                var invPlayerRemove = new List<Item>();
                foreach (var itemChest in invChest)
                {
                    if (itemChest.IsNull() || !itemChest.CanStack() || itemChest.IsFull()) continue;
                    for (var i = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                        i < invPlayer.Count;
                        i++)
                    {
                        var itemPlayer = invPlayer[i];
                        if (itemPlayer.IsNull() || !itemPlayer.SameKind(itemChest) || invPlayerRemove.Contains(itemPlayer))
                            continue;
                        var intStackMix = itemChest.Stack + itemPlayer.Stack;
                        var intStackMax = itemChest.GetMaxStack();
                        if (intStackMix > intStackMax)
                        {
                            itemPlayer.RemoveStack(intStackMax - itemChest.Stack);
                            itemChest.MaxStack();
                        }
                        else
                        {
                            itemChest.AddStack(itemPlayer.Stack);
                            invPlayerRemove.Add(itemPlayer);
                        }
                    }
                }
                foreach (var itemRemove in invPlayerRemove) invPlayer = Inventory.RemoveItemToList(invPlayer, itemRemove, Inventory.GetSpots(true));
                menu.SetPrivateFieldInBase(PlayerItems, invPlayer);
            }

            public static void StockToPlayer(object menu, List<Item> invPlayer, List<Item> invChest)
            {
                RemoveSlots(menu, invChest);
                var invChestRemove = new List<Item>();
                foreach (var itemPlayer in invPlayer)
                {
                    if (itemPlayer.IsNull() || itemPlayer.GetMaxStack() < 0 || itemPlayer.IsFull()) continue;
                    foreach (var itemChest in invChest)
                    {
                        if (itemChest.IsNull() || !itemPlayer.SameKind(itemChest) || invChestRemove.Contains(itemChest))
                            continue;
                        var intStackMix = itemChest.Stack + itemPlayer.Stack;
                        var intStackMax = itemPlayer.GetMaxStack();
                        if (intStackMix > intStackMax)
                        {
                            itemChest.Stack -= intStackMax - itemPlayer.Stack;
                            itemPlayer.MaxStack();
                        }
                        else
                        {
                            itemPlayer.Stack += itemChest.Stack;
                            invChestRemove.Add(itemChest);
                        }
                    }
                }
                foreach (var itemRemove in invChestRemove) invChest = Inventory.RemoveItemToList(invChest, itemRemove, Inventory.GetSpots());
                menu.SetPrivateFieldInBase(ChestItems, invChest);
            }

            public static void TakeToChest(object menu, List<Item> invPlayer, List<Item> invChest)
            {
                var invChestAdd = new List<Item>();
                var invPlayerRemove = new List<Item>();
                StockToChest(menu, invPlayer, invChest);
                for (var i = 0; i < Inventory.GetSpotsFree(invChest); i++)
                {
                    for (var iPlayer = !Data.SlotLock.ToBool ? 0 : Inventory.GetSpotsLine();
                        iPlayer < Inventory.GetSpots(true);
                        iPlayer++)
                    {
                        var itemPlayer = invPlayer[iPlayer];
                        if (itemPlayer.IsNull() || invPlayerRemove.Contains(itemPlayer)) continue;
                        invChestAdd.Add(itemPlayer);
                        invPlayerRemove.Add(itemPlayer);
                        break;
                    }
                }
                foreach (var itemAdd in invChestAdd) invChest = Inventory.AddItemToList(invChest, itemAdd, Inventory.GetSpots(), true);
                foreach (var itemRemove in invPlayerRemove) invPlayer = Inventory.RemoveItemToList(invPlayer, itemRemove, Inventory.GetSpots(true));
                menu.SetPrivateFieldInBase(ChestItems, invChest);
                menu.SetPrivateFieldInBase(PlayerItems, invPlayer);
            }

            public static void TakeToPlayer(object menu, List<Item> invPlayer, List<Item> invChest)
            {
                var invPlayerAdd = new List<Item>();
                var invChestRemove = new List<Item>();
                StockToPlayer(menu, invPlayer, invChest);
                for (var i = 0; i < Inventory.GetSpots(true); i++)
                {
                    var itemPlayer = invPlayer[i];
                    if (itemPlayer != null) continue;
                    foreach (var itemChest in invChest)
                    {
                        if (itemChest == null || invChestRemove.Contains(itemChest)) continue;
                        invPlayerAdd.Add(itemChest);
                        invChestRemove.Add(itemChest);
                        break;
                    }
                }
                foreach (var itemAdd in invPlayerAdd) invPlayer = Inventory.AddItemToList(invPlayer, itemAdd, Inventory.GetSpots(true));
                foreach (var itemRemove in invChestRemove) invChest = Inventory.RemoveItemToList(invChest, itemRemove, Inventory.GetSpots());
                menu.SetPrivateFieldInBase(PlayerItems, invPlayer);
                menu.SetPrivateFieldInBase(ChestItems, invChest);
            }

            public static void RemoveSlots(object menu, List<Item> inv)
            {
                var remove = inv.Where(item => item.IsNull()).ToList();
                remove.ForEach(item => inv.Remove(item));
                menu.SetPrivateFieldInBase(ChestItems, inv);
            }
        }

        #endregion
    }
}
