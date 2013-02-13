using SIPComm.Properties;
using Sipek.Common;
using System.IO;
using System.Media;

namespace SIPComm
{
	public class CMediaPlayerProxy : IMediaProxyInterface
	{
		SoundPlayer player = new SoundPlayer();
		

		#region Methods

		public int playTone(ETones toneId)
		{

		    Stream snd = Resources.congestion;

			switch (toneId)
			{
				case ETones.EToneDial:
				   snd = Resources.dial;
					break;
				case ETones.EToneCongestion:
					snd = Resources.congestion;
					break;
				case ETones.EToneRingback:
					snd = Resources.ringback;
					break;
				case ETones.EToneRing:
					snd = Resources.ring;
					break;
				default:
					break;
			}
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
