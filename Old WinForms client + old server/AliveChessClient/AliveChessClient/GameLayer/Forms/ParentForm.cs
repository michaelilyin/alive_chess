using System;
using System.Windows.Forms;

namespace AliveChessClient.GameLayer.Forms
{
    public partial class ParentForm : Form
    {
        private Game context;

        public ParentForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.context = new Game();

            this.context.GameForm.StartPosition = FormStartPosition.CenterParent;
            this.context.StartForm.StartPosition = FormStartPosition.CenterParent;

            if (context.StartForm.ShowDialog() == DialogResult.OK)
            {
                Hide();
                context.Run();
            }
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
