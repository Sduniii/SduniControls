using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SduniControls
{
    public class SduniButton : Button
    {
        private enum MouseState { None = 1, Down = 2, Up = 3, Enter = 4, Leave = 5, Move = 6 }

        private MouseState MState = MouseState.None;

        private Color bdColor = Color.Black;
        private int bdRadius = 15;
        private Color backgroundColorHover;
        private Color textColorHover;
        private Color backgroundColorPush;
        private Color textColorPush;
        private int borderWidth;

        public SduniButton() : base()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                      ControlStyles.Opaque |
                      ControlStyles.ResizeRedraw |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.CacheText, // We gain about 2% in painting by avoiding extra GetWindowText calls
                      true);

            //Set default Colors
            this.bdColor = Color.Black;
            this.bdRadius = 5;
            this.BackColor = Color.FromArgb(153, 204, 255);
            this.ForeColor = Color.FromArgb(128, 128, 128);
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.backgroundColorHover = Color.FromArgb(153, 204, 255);
            this.backgroundColorPush = Color.FromArgb(153, 204, 255);
            this.textColorHover = Color.FromArgb(128, 128, 128); ;
            this.textColorPush = Color.FromArgb(128, 128, 128);
            this.borderWidth = 1;

            this.MouseLeave += new EventHandler(_MouseLeave);
            this.MouseDown += new MouseEventHandler(_MouseDown);
            this.MouseUp += new MouseEventHandler(_MouseUp);
            this.MouseMove += new MouseEventHandler(_MouseMove);
        }

        protected void PaintTransparentBackground(Graphics g, Rectangle clipRect)
        {
            // check if we have a parent
            if (this.Parent != null)
            {
                // convert the clipRects coordinates from ours to our parents
                clipRect.Offset(this.Location);

                PaintEventArgs e = new PaintEventArgs(g, clipRect);

                // save the graphics state so that if anything goes wrong 
                // we're not fubar
                GraphicsState state = g.Save();

                try
                {
                    // move the graphics object so that we are drawing in 
                    // the correct place
                    g.TranslateTransform((float)-this.Location.X, (float)-this.Location.Y);

                    // draw the parents background and foreground
                    this.InvokePaintBackground(this.Parent, e);
                    this.InvokePaint(this.Parent, e);

                    return;
                }
                finally
                {
                    // reset everything back to where they were before
                    g.Restore(state);
                    clipRect.Offset(-this.Location.X, -this.Location.Y);
                }
            }

            // we don't have a parent, so fill the rect with
            // the default control color
            g.FillRectangle(SystemBrushes.Control, clipRect);
        }

        #region Mouse Events

        private void _MouseDown(object sender, MouseEventArgs mevent)
        {
            MState = MouseState.Down;
            Invalidate();
        }

        private void _MouseUp(object sender, MouseEventArgs mevent)
        {
            MState = MouseState.Up;
            Invalidate();
        }

        private void _MouseMove(object sender, MouseEventArgs mevent)
        {
            MState = MouseState.Move;
            Invalidate();
        }

        private void _MouseLeave(object sender, EventArgs e)
        {
            MState = MouseState.Leave;
            Invalidate();
        }

        #endregion

        public static GraphicsPath GetRoundedRect(RectangleF r, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.StartFigure();
            r = new RectangleF(r.Left, r.Top, r.Width, r.Height);
            if (radius <= 0.0F || radius <= 0.0F)
            {
                gp.AddRectangle(r);
            }
            else
            {
                gp.AddArc((float)r.X, (float)r.Y, radius, radius, 180, 90);
                gp.AddArc((float)r.Right - radius, (float)r.Y, radius - 1, radius, 270, 90);
                gp.AddArc((float)r.Right - radius, ((float)r.Bottom) - radius - 1, radius - 1, radius, 0, 90);
                gp.AddArc((float)r.X, ((float)r.Bottom) - radius - 1, radius - 1, radius, 90, 90);
            }
            gp.CloseFigure();
            return gp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.PaintTransparentBackground(e.Graphics, this.ClientRectangle);

            #region Painting

            //now let's we begin painting
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            #endregion


            Color backgroundColor = this.BackColor;
            Color textColor = this.ForeColor;
            Color borderColor = this.bdColor;


            if (!this.Enabled)
            {
                backgroundColor = Color.FromArgb((int)(backgroundColor.GetBrightness() * 255),
                    (int)(backgroundColor.GetBrightness() * 255),
                    (int)(backgroundColor.GetBrightness() * 255));
            }
            else
            {
                if (MState == MouseState.None || MState == MouseState.Leave)
                {
                    backgroundColor = this.BackColor;
                    textColor = this.ForeColor;
                    borderColor = this.bdColor;
                }
                else if (MState == MouseState.Down)
                {
                    backgroundColor = backgroundColorPush;
                    textColor = textColorPush;
                    borderColor = this.bdColor;
                }
                else if (MState == MouseState.Move || MState == MouseState.Up)
                {
                    backgroundColor = backgroundColorHover;
                    textColor = textColorHover;
                    borderColor = this.bdColor;
                }
            }

            PaintBackground(e, g, backgroundColor, borderColor);
            TEXTandIMAGE(this.ClientRectangle, g, textColor);

        }

        protected void PaintBackground(PaintEventArgs e, Graphics g, Color backgroundColor, Color borderColor)
        {

            Rectangle r = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.ClientRectangle.Height);

            //rectangle for gradient, half upper and lower
            RectangleF halfup = new RectangleF(r.Left, r.Top, r.Width, r.Height);
            RectangleF halfdown = new RectangleF(r.Left, r.Top + (r.Height / 2) - 1, r.Width, r.Height);

            //BEGIN PAINT BACKGROUND
            //for half upper, we paint using linear gradient
            using (GraphicsPath thePath = GetRoundedRect(r, this.bdRadius))
            {


                //BEGIN PAINT
                using (GraphicsPath gborderDark = thePath)
                {
                    using (Pen p = new Pen(borderColor, this.borderWidth))
                    {
                        //MessageBox.Show(this.borderWidth.ToString());
                        if (this.borderWidth > 0) {
                            g.DrawPath(p, gborderDark);
                        }
                        SolidBrush br = new SolidBrush(backgroundColor);
                        g.FillPath(br, gborderDark);
                    }
                }
                //END PAINT
            }
        }


        protected void TEXTandIMAGE(Rectangle Rec, Graphics g, Color textColor)
        {
            //BEGIN PAINT TEXT
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            #region Top

            if (this.TextAlign == ContentAlignment.TopLeft)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.TopCenter)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.TopRight)
            {
                sf.LineAlignment = StringAlignment.Near;
                sf.Alignment = StringAlignment.Far;
            }

            #endregion

            #region Middle

            else if (this.TextAlign == ContentAlignment.MiddleLeft)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.MiddleCenter)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.MiddleRight)
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Far;
            }

            #endregion

            #region Bottom

            else if (this.TextAlign == ContentAlignment.BottomLeft)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Near;
            }
            else if (this.TextAlign == ContentAlignment.BottomCenter)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Center;
            }
            else if (this.TextAlign == ContentAlignment.BottomRight)
            {
                sf.LineAlignment = StringAlignment.Far;
                sf.Alignment = StringAlignment.Far;
            }

            #endregion

            if (this.ShowKeyboardCues)
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            else
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            g.DrawString(this.Text, this.Font, new SolidBrush(textColor), Rec, sf);
        }

        [Description("BorderColor"),
            Category("Darstellung"),
            DefaultValue(typeof(Color), "Black"),
            Browsable(true)]
        public Color BorderColor
        {
            get { return this.bdColor; }
            set { this.bdColor = value; Invalidate(); }
        }

        [Description("BackgroundHoverColor"),
            Category("Darstellung"),
            DefaultValue(typeof(Color), "Black"),
            Browsable(true)]
        public Color BackgroundHoverColor
        {
            get { return this.backgroundColorHover; }
            set { this.backgroundColorHover = value; Invalidate(); }
        }

        [Description("BackgroundPushColor"),
            Category("Darstellung"),
            DefaultValue(typeof(Color), "Black"),
            Browsable(true)]
        public Color BackgroundPushColor
        {
            get { return this.backgroundColorPush; }
            set { this.backgroundColorPush = value; Invalidate(); }
        }

        [Description("TextHoverColor"),
            Category("Darstellung"),
            DefaultValue(typeof(Color), "Black"),
            Browsable(true)]
        public Color TextHoverColor
        {
            get { return this.textColorHover; }
            set { this.textColorHover = value; Invalidate(); }
        }

        [Description("TextPushColor"),
            Category("Darstellung"),
            DefaultValue(typeof(Color), "Black"),
            Browsable(true)]
        public Color TextPushColor
        {
            get { return this.textColorPush; }
            set { this.textColorPush = value; Invalidate(); }
        }

        [Description("BorderRadius"),
            Category("Darstellung"),
            DefaultValue(10),
            Browsable(true)]
        public int BorderRadius
        {
            get { return this.bdRadius; }
            set { this.bdRadius = value; }
        }

        [Description("BorderWidth"),
            Category("Darstellung"),
            DefaultValue(1),
            Browsable(true)]
        public int BorderWidth
        {
            get { return this.borderWidth; }
            set { this.borderWidth = value; }
        }


    }
}
