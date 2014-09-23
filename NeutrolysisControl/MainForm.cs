using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;          // Serial stuff in here.
using System.Threading;

namespace NeutrolysisControl
{
	public partial class MainForm : Form
	{
		Serial zigbee = new Serial();

		int FIRE_ALARM_MIN_TEMP = NeutrolysisControl.Properties.Settings.Default.FIRE_ALARM_MIN_TEMP;

		public MainForm()
		{

			InitializeComponent();
			putCOMSIntoMenu();
			acTemp = (int)numericUpDown1.Value;
			try
			{
				zigbee.setupPort(Serial.DEFAULT_PORT_NAME, Serial.BAUD_RATE);
				portToolStripMenuItem.Text = "Port (" + zigbee.getPortName() + ")";
			}
			catch (System.IO.IOException error)
			{
				logPrint(error.Message);
				this.Show();
				MessageBox.Show("Please ,Select a port!");
			}
			timer1.Start();
		}
		void putCOMSIntoMenu()
		{
			portToolStripMenuItem.DropDownItems.Clear();
			if (SerialPort.GetPortNames().Count() >= 0)
			{
				foreach (string p in SerialPort.GetPortNames())
				{
					portToolStripMenuItem.DropDownItems.Add(p);
				}
			}
			else
			{
				portToolStripMenuItem.DropDownItems.Add("No Ports available, press any key to exit.");
			} portToolStripMenuItem.DropDownItems.Add("-"); portToolStripMenuItem.DropDownItems.Add("Update");

		}

		public void logPrint(String msg)
		{
			txtSerial.Text = msg + "\n\n" + txtSerial.Text;
		}

		String receivedMsg;
		Alarm fileAlarm = new Alarm();
		void timer1_Tick(object sender, EventArgs e) //Procssing Monitoring
		{
			while (zigbee.hasMsg())
			{
				receivedMsg = zigbee.nextMsg();
				logPrint(receivedMsg);

				try
				{
					//Extract key and value from a command
					char key;
					string value;

					key = receivedMsg[0];
					value = receivedMsg.Substring(1, receivedMsg.Length - 1);

					//	switching on key
					switch (key)
					{
						case 'T':
							lblTemp.Text = value;
							double receivedValue = double.Parse(value);
							if (receivedValue > FIRE_ALARM_MIN_TEMP)
							{
								lblTemp.ForeColor = Color.Red;
								fileAlarm.play();
							}
							else
							{
								lblTemp.ForeColor = Color.Green;
								fileAlarm.stop();
							}
							break;
						case 'H':
							lblHumidity.Text = value;
							break;
						case 'S':
							lblSmoke.Text = value;
							break;
					}
				}
				catch (IndexOutOfRangeException error) { logPrint(error.Message); }
				catch (ArgumentOutOfRangeException error) { logPrint(error.Message); }
			}
		}

		//Lighta
		private void chkLED1_CheckedChanged(object sender, EventArgs e)
		{
			if (chkLED1.Checked == true)
			{
				zigbee.sendCommand("0");
				picLED1.Image = NeutrolysisControl.Properties.Resources.led_ON;
			}
			else if (chkLED1.Checked == false)
			{
				zigbee.sendCommand("1");
				picLED1.Image = NeutrolysisControl.Properties.Resources.led_OFF;
			}
		}
		private void picLED1_Click(object sender, EventArgs e)
		{
			chkLED1.Checked = !chkLED1.Checked;
		}
		private void chkLED2_CheckedChanged(object sender, EventArgs e)
		{
			if (chkLED2.Checked == true)
			{
				zigbee.sendCommand("2");
				picLED2.Image = NeutrolysisControl.Properties.Resources.led_ON;
			}
			else if (chkLED2.Checked == false)
			{
				zigbee.sendCommand("3");
				picLED2.Image = NeutrolysisControl.Properties.Resources.led_OFF;
			}
		}
		private void picLED2_Click(object sender, EventArgs e)
		{
			chkLED2.Checked = !chkLED2.Checked;
		}
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			zigbee.closePort();
		}
		///////////////////////////////////////////////////////

		private void button1_Click(object sender, EventArgs e)
		{
			zigbee.sendCommand(txtSerialMessage.Text + "\n");
			txtSerialMessage.Text = "";
		}

		//Menu
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{ Close(); }

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{ Show(); }

		private void hideToolStripMenuItem_Click(object sender, EventArgs e)
		{ Hide(); }
		///////////////////////////////////////////////////////

		int acTemp;
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			trackBar1.Value = (int)numericUpDown1.Value;
			if (numericUpDown1.Value < acTemp)
			{
				zigbee.sendCommand("h");
				acTemp = (int)numericUpDown1.Value;
			}
			else if (numericUpDown1.Value > acTemp)
			{
				zigbee.sendCommand("g");
				acTemp = (int)numericUpDown1.Value;
			}
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			for (int i = 0; i < (0); i++)
			{

			}
			numericUpDown1.Value = trackBar1.Value;

		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{ MessageBox.Show("NeutrolysisControl\nBy:\n\n\n\n\n\n\n\n\n\n"); }

		private void portToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.ToString().Equals("Update"))
				putCOMSIntoMenu();
			else
			{
				zigbee.setupPort(e.ClickedItem.ToString(), Serial.BAUD_RATE);
				portToolStripMenuItem.Text = "Port (" + e.ClickedItem.ToString() + ")";
			}

		}

		private void chkAC_CheckedChanged(object sender, EventArgs e)
		{
			if (chkAC.Checked == true)
				zigbee.sendCommand("7");
			else if (chkAC.Checked == false)
				zigbee.sendCommand("8");
			trackBar1.Enabled = chkAC.Checked;
			numericUpDown1.Enabled = chkAC.Checked;
		}

		private void chkProjector_CheckedChanged(object sender, EventArgs e)
		{
			if (chkProjector.Checked == true)
				zigbee.sendCommand("9");
			else if (chkProjector.Checked == false)
				zigbee.sendCommand("a");
			btnZoomIn.Enabled = chkProjector.Checked;
			btnZoomOut.Enabled = chkProjector.Checked;
		}

		private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cmbMode.SelectedIndex)
			{
				case 0:
					chkAC.Checked = false;
					chkLED1.Checked = false;
					chkLED2.Checked = false;
					chkProjector.Checked = false;
					chkDoor1.Checked = true;
					chkDoor2.Checked = true;
					chkWindow1.Checked = true;
					chkWindow2.Checked = true;
					break;
				case 1:
					chkAC.Checked = true;
					chkLED1.Checked = false;
					chkLED2.Checked = false;
					chkProjector.Checked = true;
					chkDoor1.Checked = true;
					chkDoor2.Checked = true;
					chkWindow1.Checked = true;
					chkWindow2.Checked = true;
					break;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.MaximizeBox = false;
			picLED1.Image = NeutrolysisControl.Properties.Resources.led_OFF;
			picLED2.Image = NeutrolysisControl.Properties.Resources.led_OFF;
		}

		private void chkWindow1_CheckedChanged(object sender, EventArgs e)
		{
			if (chkWindow1.Checked == true)
				zigbee.sendCommand("4");
			else if (chkWindow1.Checked == false)
				zigbee.sendCommand("5");
		}
	}
}
