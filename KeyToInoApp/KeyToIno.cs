using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace KeyToInoApp
{
    public class KeyToIno
    {
        private readonly SerialPort port;

        private byte lastKeyPressed = 0;
        private readonly byte[] buffer = { 0, 0, 0xFF };

        public event KeyEventHandler KeyDown;

        public KeyToIno(Form form, SerialPort port)
        {
            this.port = port;
            form.KeyPreview = true;
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
            if(lastKeyPressed != e.KeyValue)
            {
                if(port.IsOpen)
                {
                    buffer[0] = (byte)e.KeyValue;
                    buffer[1] = 0x01;
                    port.Write(buffer, 0, buffer.Length);
                }
            }
            lastKeyPressed = (byte)e.KeyValue;
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (port.IsOpen)
            {
                buffer[0] = (byte)e.KeyValue;
                buffer[1] = 0x00;
                port.Write(buffer, 0, buffer.Length);
            }
            if (lastKeyPressed == e.KeyValue)
            {
                lastKeyPressed = 0;
            }
        }
    }
}
