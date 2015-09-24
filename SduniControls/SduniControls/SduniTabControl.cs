using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SduniControls
{
    public class SduniTabControl : TabControl
    {
        public SduniTabControl() : base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
            this.SizeMode = TabSizeMode.Fixed;
            this.Dock = DockStyle.Fill;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Alignment = TabAlignment.Left;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            


            Bitmap b = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(b);

            g.Clear(Color.Gainsboro);

            for (int i = 0; i < this.TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                if (i == this.SelectedIndex)
                {
                    g.FillRectangle(Brushes.Red, tabRect);
                }
                else
                {
                    g.FillRectangle(Brushes.AliceBlue, tabRect);
                }

                g.DrawString(this.TabPages[i].Text, this.Font, Brushes.White, tabRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            e.Graphics.DrawImage((Bitmap)b.Clone(), 0, 0);

            g.Dispose();
            b.Dispose();
            base.OnPaint(e);
        }
    }
}
