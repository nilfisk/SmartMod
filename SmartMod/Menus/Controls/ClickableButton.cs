using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace SmartMod
{
    public class ClickableButton
    {
        public ClickableButton(Rectangle bound, string name, [Optional, DefaultParameterValue("")] string description)
        {
            Bound = bound;
            Name = name;
            Description = description;
        }

        public Rectangle Bound { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Texture2D Texture { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public string Description { get; set; }
        public Rectangle DrawArea => new Rectangle(Location.X, Location.Y, Size.Width, Size.Height);
        public bool IsHover(int x, int y) => DrawArea.Contains(new Point(x, y));
        public bool IsHover(float x, float y) => IsHover((int) x, (int) y);
        public bool IsHover(Point point) => IsHover(point.X, point.Y);
        public bool IsHover(Vector2 point) => IsHover(point.X, point.Y);

        public static List<ClickableButton> GetByName(List<string> names, List<ClickableButton> buttons)
        {
            var listButton = new List<ClickableButton>();
            foreach (var name in names)
            {
                foreach (var button in buttons)
                {
                    if (button.Name != name.Trim()) continue;
                    listButton.Add(button);
                    break;
                }
            }
            return listButton;
        }
    }
}