using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Scliente
{
    public partial class Form1 : Form
    {
        static private NetworkStream corr;
        static private StreamWriter esc;
        static private StreamReader lec;
        static private TcpClient User = new TcpClient();
        static private string name = "unknown";

        private delegate void DAddItem(String i);

        private void AddItem(String i)
        {
            listBox1.Items.Add(i);
        }

        void Listen()
        {
            while (User.Connected)
            {
                try
                {
                    this.Invoke(new DAddItem(AddItem), lec.ReadLine());

                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
      

        private void btnEn_Click(object sender, EventArgs e)
        {

            esc.WriteLine(txtMensaje.Text);
            esc.Flush();
            txtUs.Clear();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            btnIn.Visible = false;
            label1.Visible = false;
            txtUs.Visible = false;
            name = txtUs.Text;

            try
            {
                User.Connect("127.0.0.1", 8000);
                switch (User.Connected)
                {
                    case true:
                        Thread hilo = new Thread(Listen);

                        corr = User.GetStream();
                        esc = new StreamWriter(corr);
                        lec = new StreamReader(corr);

                        esc.WriteLine(name);
                        esc.Flush();

                        hilo.Start();

                        break;
                    default:

                        MessageBox.Show("Fue imposible conectar con el servidor");
                        Application.Exit();
                        break;
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Fue imposible conectar con el servidor");
                Application.Exit();
            }
        }
    }
}
