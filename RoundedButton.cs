using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311A2024_DATABASE
{
    public class RoundedButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 20; // Set the radius for rounding
                path.AddArc(0, 0, radius, radius, 180, 90); // Top left
                path.AddArc(Width - radius - 1, 0, radius, radius, 270, 90); // Top right
                path.AddArc(Width - radius - 1, Height - radius - 1, radius, radius, 0, 90); // Bottom right
                path.AddArc(0, Height - radius - 1, radius, radius, 90, 90); // Bottom left
                path.CloseFigure();
                this.Region = new Region(path);
            }

            base.OnPaint(pevent);
        }
    }
}
