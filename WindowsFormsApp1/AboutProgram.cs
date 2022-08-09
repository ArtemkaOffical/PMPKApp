using System.Windows.Forms;

namespace PMPK
{
    public partial class AboutProgram : Form
    {
        public AboutProgram()
        {
            InitializeComponent();
        }

        private void AboutProgram_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
