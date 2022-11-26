using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace KeyToInoApp
{
    public partial class MainForm : Form
    {
        readonly KeyToIno keyToIno;
        readonly SerialPort port;

        public MainForm()
        {
            InitializeComponent();

            port = new SerialPort();
            keyToIno = new KeyToIno(this, port);

            UpdatePortsComboBox(comboBoxPort);

            if(comboBoxPort.Items.Count > 0)
            {
                comboBoxPort.SelectedIndex = 0;
            }
            comboBoxBaud.SelectedIndex = 6;

            keyToIno.KeyDown += KeyToIno_KeyDown;
        }

        private void KeyToIno_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKeyPressed.Text = e.KeyData.ToString().Split(',')[0];
        }

        private void ComboBoxPort_MouseEnter(object sender, EventArgs e)
        {
            UpdatePortsComboBox(sender);
        }

        private void ComboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            if (port.IsOpen == false)
            {
                port.PortName = cb.SelectedItem.ToString();
            }
        }

        private void ComboBoxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            if (port.IsOpen == false)
            {
                port.BaudRate = int.Parse(cb.SelectedItem.ToString());
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            var bt = (Button)sender;
            try
            {
                if (port.IsOpen == false)
                {
                    port.Open();
                    bt.Text = "Disconnect";
                    labelStatus.Text = "Connected";
                }
                else 
                {
                    port.Close();
                    bt.Text = "Connect";
                    labelStatus.Text = "Disconnected";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void UpdatePortsComboBox(object sender)
        {
            var cb = (ComboBox)sender;
            var selected = cb.SelectedItem;
            cb.Items.Clear();
            cb.Items.AddRange(SerialPort.GetPortNames());
            cb.SelectedItem = selected;
        }
    }
}
