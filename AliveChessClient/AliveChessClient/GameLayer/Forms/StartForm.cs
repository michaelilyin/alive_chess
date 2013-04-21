using System;
using System.Threading;
using System.Windows.Forms;
using AliveChessClient.NetLayer.Main;
using AliveChessLibrary.Commands.RegisterCommand;

namespace AliveChessClient.GameLayer.Forms
{
    public partial class StartForm : Form
    {
        private Game context;
        private object sync = new object();
        //private bool isObserver = false;

        public StartForm(Game context)
        {
            this.context = context;
            InitializeComponent();
            this.comboBox1.SelectedItem = comboBox1.Items[0];
            //textBox1.Text = "5.64.64.149";
        }

        public void EnterToGame()
        {
            lock (sync)
            {
                context.Player.Login = textBox2.Text;
                context.Player.King.Name = textBox2.Text;
                Monitor.Pulse(sync);
            }

            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientApplication.CreateClient(context, textBox1.Text);
            ClientApplication.Instance.StartClient();

            AuthorizeRequest authorize = new AuthorizeRequest();
            authorize.Login = textBox2.Text;
            authorize.Password = "Secret";
            if (comboBox1.Text == "Super user")
                authorize.IsSuperUser = true;
            else authorize.IsSuperUser = false;

            //IPEndPoint ip = (IPEndPoint)ClientApplication.Instance.Transport.Socket.LocalEndPoint;
            //authorize.IpAddress = ip.Address.ToString();
            //authorize.Port = ip.Port;
            ClientApplication.Instance.Transport.Send<AuthorizeRequest>(authorize);

            lock (sync)
            {
                Monitor.Wait(sync, 1000);
            }
        }
    }
}
