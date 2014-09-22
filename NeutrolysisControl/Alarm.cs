using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace NeutrolysisControl
{
	class Alarm
	{
		SoundPlayer soundPlayer;
		System.IO.UnmanagedMemoryStream alarmFile = null;
		bool isPlaying = false;
		public Alarm()
		{
			setAlarmFile(NeutrolysisControl.Properties.Resources.fire_alert);
			open();
		}
		public Alarm(System.IO.UnmanagedMemoryStream file)
		{
			setAlarmFile(file);
			open();
		}

		public void play()
		{
			if (!isPlaying)
			{
				soundPlayer.PlayLooping();
				isPlaying = true;
			}
		}
		public void stop()
		{
			if (isPlaying)
			{
				soundPlayer.Stop();
				isPlaying = false;
			}
		}
		void open()
		{
			if (alarmFile != null)
				soundPlayer = new SoundPlayer(alarmFile);
		}
		public void setAlarmFile(System.IO.UnmanagedMemoryStream file)
		{
			alarmFile = file;
		}
	}
}
