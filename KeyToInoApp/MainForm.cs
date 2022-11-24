using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace KeyToInoApp
{
    public partial class MainForm : Form
    {
        byte lastKeyPressed = 0;
        byte[] buffer = {0, 0, 0xFF};

        public MainForm()
        {
            InitializeComponent();
            UpdatePortsComboBox(comboBoxPort);
            if(comboBoxPort.Items.Count > 0)
            {
                comboBoxPort.SelectedIndex = 0;
            }
            comboBoxBaud.SelectedIndex = 6;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxKeyPressed.Text = e.KeyData.ToString().Split(',')[0];
            if(lastKeyPressed != e.KeyValue)
            {
                Console.WriteLine(e.KeyValue + " " + 1);
                if (serialPort.IsOpen)
                {
                    buffer[0] = (byte)e.KeyValue;
                    buffer[1] = 0x01;
                    serialPort.Write(buffer, 0, buffer.Length);
                }
            }
            lastKeyPressed = (byte)e.KeyValue;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyValue + " " + 0);
            if (serialPort.IsOpen)
            {
                buffer[0] = (byte)e.KeyValue;
                buffer[1] = 0x00;
                serialPort.Write(buffer, 0, buffer.Length);
            }
            if(lastKeyPressed == e.KeyValue)
            {
                lastKeyPressed = 0;
            }
        }

        private void comboBoxPort_MouseEnter(object sender, EventArgs e)
        {
            UpdatePortsComboBox(sender);
        }

        private static void UpdatePortsComboBox(object sender)
        {
            var cb = (ComboBox)sender;
            var selected = cb.SelectedItem;
            cb.Items.Clear();
            cb.Items.AddRange(SerialPort.GetPortNames());
            cb.SelectedItem = selected;
        }

        private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            if (serialPort.IsOpen == false)
            {
                serialPort.PortName = cb.SelectedItem.ToString();
            }
        }

        private void comboBoxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            if (serialPort.IsOpen == false)
            {
                serialPort.BaudRate = int.Parse(cb.SelectedItem.ToString());
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var bt = (Button)sender;
            try
            {
                if (serialPort.IsOpen == false)
                {
                    serialPort.Open();
                    bt.Text = "Disconnect";
                    labelStatus.Text = "Connected";
                }
                else 
                {
                    serialPort.Close();
                    bt.Text = "Connect";
                    labelStatus.Text = "Disconnected";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
