using Microsoft.Xna.Framework.Input;
using SmartMod.Menus;
using StardewValley;
using SvLib;

namespace SmartMod
{
    public static class ModMenu
    {
        public static void Entry(Keys key)
        {
            if (!Game1.hasLoadedGame || !Menu.Get.IsNull()) return;
            if (!InputParse.IsSame(InputHelper.PressedKeys(key), Data.ModMenuKey.ToKeys)) return;
            Show();
        }

        public static void Show()
        {
            if (!Game1.hasLoadedGame) return;
            Menu.Set(new SmartModMenu());
        }
    }
}
