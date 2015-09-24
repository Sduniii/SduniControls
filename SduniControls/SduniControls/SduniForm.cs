using System;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SduniControls
{
    public class SduniForm : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcfont);


        FontFamily ff;
        Font font;
        private Panel titlePanel;
        private SduniButton closeButton;
        private SduniButton minimizeButton;
        private SduniButton maximizeButton;
        private Label title;
        private Boolean mouseDown;
        private Point lastLocation;

        public SduniForm() : base()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.TextChanged += new EventHandler(_TextChanged);
            initTitlePanel();
        }

        private void _TextChanged(object sender, EventArgs e)
        {
            this.title.Text = this.Text;
        }

        private void initTitlePanel()
        {
            loadFont();
            //TitleBAR
            this.titlePanel = new Panel();
            this.titlePanel.Dock = DockStyle.Top;
            this.titlePanel.SetBounds(0, 0, this.Width, 26);
            this.titlePanel.BackColor = this.BackColor;
            this.titlePanel.Resize += new EventHandler(_Res);
            this.titlePanel.MouseDown += new MouseEventHandler(_MouseDown);
            this.titlePanel.MouseUp += new MouseEventHandler(_MouseUp);
            this.titlePanel.MouseMove += new MouseEventHandler(_MouseMove);
            this.titlePanel.MouseLeave += new EventHandler(_MouseLeave);

            //Title
            this.title = new Label();
            this.title.SetBounds(0, 0, 100, this.titlePanel.Height);
            this.title.ForeColor = Color.FromArgb(241, 241, 241);
            this.title.TextAlign = ContentAlignment.MiddleLeft;
            this.title.MouseDown += new MouseEventHandler(_MouseDown);
            this.title.MouseUp += new MouseEventHandler(_MouseUp);
            this.title.MouseMove += new MouseEventHandler(_MouseMove);
            this.title.MouseLeave += new EventHandler(_MouseLeave);

            //CloseButton
            this.closeButton = new SduniButton();
            this.closeButton.BackColor = this.BackColor;
            this.closeButton.BorderRadius = 0;
            this.closeButton.BorderWidth = 0;
            this.closeButton.BackgroundHoverColor = Color.FromArgb(63, 63, 65);
            this.closeButton.BackgroundPushColor = Color.FromArgb(0, 122, 204);
            this.closeButton.Text = "X";
            this.closeButton.TextHoverColor = Color.FromArgb(241, 241, 241);
            this.closeButton.TextPushColor = Color.FromArgb(241, 241, 241);
            this.closeButton.ForeColor = Color.FromArgb(241, 241, 241);
            this.closeButton.MouseClick += new MouseEventHandler(_MouseClickCloseButton);
            this.closeButton.TextAlign = ContentAlignment.MiddleCenter;

            //maximizeButton
            this.minimizeButton = new SduniButton();
            this.minimizeButton.BackColor = this.BackColor;
            this.minimizeButton.BorderRadius = 0;
            this.minimizeButton.BorderWidth = 0;
            this.minimizeButton.BackgroundHoverColor = Color.FromArgb(63, 63, 65);
            this.minimizeButton.BackgroundPushColor = Color.FromArgb(0, 122, 204);
            this.minimizeButton.Text = "A";
            this.minimizeButton.TextHoverColor = Color.FromArgb(241, 241, 241);
            this.minimizeButton.TextPushColor = Color.FromArgb(241, 241, 241);
            this.minimizeButton.ForeColor = Color.FromArgb(241, 241, 241);
            this.minimizeButton.MouseClick += new MouseEventHandler(_MouseClickMinimizeButton);
            allocFont(this.font, this.minimizeButton, 12);
            this.minimizeButton.TextAlign = ContentAlignment.MiddleCenter;

            //minimizeButton
            this.maximizeButton = new SduniButton();
            this.maximizeButton.BackColor = this.BackColor;
            this.maximizeButton.BorderRadius = 0;
            this.maximizeButton.BorderWidth = 0;
            this.maximizeButton.BackgroundHoverColor = Color.FromArgb(63, 63, 65);
            this.maximizeButton.BackgroundPushColor = Color.FromArgb(0, 122, 204);
            this.maximizeButton.Text = "B";
            this.maximizeButton.TextHoverColor = Color.FromArgb(241, 241, 241);
            this.maximizeButton.TextPushColor = Color.FromArgb(241, 241, 241);
            this.maximizeButton.ForeColor = Color.FromArgb(241, 241, 241);
            this.maximizeButton.MouseClick += new MouseEventHandler(_MouseClickMaximizeButton);
            allocFont(this.font, this.maximizeButton, 8);
            this.maximizeButton.TextAlign = ContentAlignment.MiddleCenter;

            this.titlePanel.Controls.Add(this.title);
            this.titlePanel.Controls.Add(closeButton);
            this.titlePanel.Controls.Add(minimizeButton);
            this.titlePanel.Controls.Add(maximizeButton);
            this.Controls.Add(this.titlePanel);
        }

        private void _MouseLeave(object sender, EventArgs e)
        {
            if (this.mouseDown)
            {
                this.mouseDown = false;
            }
        }

        private void _MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mouseDown)
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Location = new Point(
                (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                    this.Update();
                }
            }
        }

        private void _MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseDown = false;
            
        }

        private void _MouseDown(object sender, MouseEventArgs e)
        {
            this.mouseDown = true;
            lastLocation = e.Location;
        }

        private void _MouseClickMaximizeButton(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void _MouseClickMinimizeButton(object sender, MouseEventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                this.minimizeButton.Text = "A";
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.minimizeButton.Text = "@";
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void _Res(object sender, EventArgs e)
        {
            this.closeButton.SetBounds(this.titlePanel.Width - 34, 0, 34, this.titlePanel.Height);
            this.minimizeButton.SetBounds(this.titlePanel.Width - this.closeButton.Width - 34, 0, 34, this.titlePanel.Height);
            this.maximizeButton.SetBounds(this.titlePanel.Width - this.closeButton.Width - this.maximizeButton.Width - 34, 0, 34, this.titlePanel.Height);
        }

        private void _MouseClickCloseButton(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void loadFont()
        {
            byte[] fontArray = Properties.Resources.win;
            int dataLength = Properties.Resources.win.Length;
            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontArray, 0, ptrData, dataLength);
            uint cFonts = 0;
            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddMemoryFont(ptrData, dataLength);

            Marshal.FreeCoTaskMem(ptrData);

            ff = pfc.Families[0];
            font = new Font(ff, 15f, FontStyle.Bold);
        }

        private void allocFont(Font f, Control c, float size)
        {
            FontStyle fontstyle = FontStyle.Regular;

            c.Font = new Font(ff, size, fontstyle);
        }


    }
}
