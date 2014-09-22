﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASUControl
{
	public class Serial
	{
		public static String DEFAULT_PORT_NAME = "COM26";
		public static int BAUD_RATE = 9600;

		private SerialPort serialPort;

		public String getPortName()
		{
			return serialPort.PortName;
		}

		public void setupPort(string portName = null, int baudRate = 9600)
		{
			if (portName == null)
				portName = DEFAULT_PORT_NAME;
			if (serialPort != null)
				serialPort.Close();
			serialPort = new SerialPort(portName, baudRate);
			serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
			serialPort.Open();
		}
		public void closePort()
		{
			if (serialPort != null)
				serialPort.Close();
		}
		public void sendCommand(String s)
		{
			var form = Form.ActiveForm as Form1;
			try
			{
				form.logPrint("Sending: " + s + "\r\n");
				serialPort.Write(s);
			}
			catch (InvalidOperationException e)
			{
				e.ToString();
				MessageBox.Show("No Connection!\r\nPlease reconnect and try again.\r\n");
			}
		}

		private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			receivedDataQueue.Enqueue(serialPort.ReadLine());
		}

		public Queue<String> receivedDataQueue = new Queue<String>();
		public bool hasMsg()
		{
			return receivedDataQueue.Count != 0;
		}
		public String nextMsg()
		{
			if (hasMsg())
				return receivedDataQueue.Dequeue();
			return null;
		}

	}
}
