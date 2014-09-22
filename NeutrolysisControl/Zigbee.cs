using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASUControl
{
	public class Zigbee
	{
		private static SerialPort serialPort;
		public const string DEFAULT_PORT_NAME = "COM26";
		public static String getPortName()
		{
			return serialPort.PortName;
		}
		public static int BAUD_RATE = 9600;
		public static void setupPort(string portName = DEFAULT_PORT_NAME, int baudRate = 9600)
		{
			if (serialPort != null)
				serialPort.Close();
			serialPort = new SerialPort(portName, baudRate);
			serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
			serialPort.Open();
		}
		public static void closePort()
		{
			if (serialPort != null)
				serialPort.Close();
		}
		public static void sendCommand(String s)
		{
			try
			{
				var form = Form.ActiveForm as Form1;
				form.logPrint("Sending: " + s + "\r\n");
				serialPort.Write(s);
			}
			catch (InvalidOperationException e)
			{ MessageBox.Show("No Connection!\r\nPlease reconnect and try again."); }
		}

		static public Queue<string> receivedDataQueue = new Queue<string>();
		private static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			receivedDataQueue.Enqueue(serialPort.ReadLine());
		}

	}
}
