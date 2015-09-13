using System.Drawing;
using System.Windows.Forms;

namespace SduniControls
{
    public class SduniForm : Form
    {
        private Panel titlePanel;
        public SduniForm() : base()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(0, 122, 204);

            initTitlePanel();
        }

        private void initTitlePanel()
        {
            this.titlePanel = new Panel();
            this.titlePanel.Height = 30;
            this.titlePanel.BackColor = Color.White;
            SduniButton closeButton = new SduniButton();
            closeButton.Text = "X";
            closeButton.Font = new Font(closeButton.Font, FontStyle.Bold);
            closeButton.TextAlign = ContentAlignment.MiddleCenter;

            this.titlePanel.Controls.Add(closeButton);
        }
    }
}
