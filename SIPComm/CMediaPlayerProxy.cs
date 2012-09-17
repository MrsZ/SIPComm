using System.Media;
using Sipek.Common;
using System.IO;

namespace SIPComm
{
	public class CMediaPlayerProxy : IMediaProxyInterface
	{
		SoundPlayer player = new SoundPlayer();

		#region Methods

		public int playTone(ETones toneId)
		{

		    Stream snd = SIPComm.Properties.Resources.congestion;

			switch (toneId)
			{
				case ETones.EToneDial:
				   snd = SIPComm.Properties.Resources.dial;
					break;
				case ETones.EToneCongestion:
					snd = SIPComm.Properties.Resources.congestion;
					break;
				case ETones.EToneRingback:
					snd = SIPComm.Properties.Resources.ringback;
					break;
				case ETones.EToneRing:
					snd = SIPComm.Properties.Resources.ring;
					break;
				default:
					break;
			}

			//player.SoundLocation = fname;
			player.Stream = snd;
			player.Load();
			player.PlayLooping();

			return 1;
		}

		public int stopTone()
		{
			player.Stop();
			return 1;
		}

		#endregion

	}
}
