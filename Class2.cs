using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedButton : Button
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        // Draw the background
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

        // Call the base class OnPaint to draw the text
        base.OnPaint(pevent);
    }
}
