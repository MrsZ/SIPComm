using Microsoft.Win32;
using System.Collections.Generic;
using System.Security.AccessControl;
using WaveLib.AudioMixer;

namespace SIPComm
{
	class Audio
	{
		List<MixerDetail> _outputList;
		List<MixerDetail> _inputList;
		List<string> _outputStringList;
		List<string> _inputStringList;

		public Audio()
		{
			Mixers mMixers = new Mixers();
			_inputList = new List<MixerDetail>();
			_inputStringList = new List<string>();
			MixerDetail mixerDetailDefault = new MixerDetail();
			mixerDetailDefault.DeviceId = -1;
			mixerDetailDefault.MixerName = "Default";
			mixerDetailDefault.SupportWaveOut = true;
			_inputList.Add(mixerDetailDefault);
			_inputStringList.Add(mixerDetailDefault.MixerName);
			foreach (MixerDetail mixerDetail in mMixers.Recording.Devices)
			{
				_inputList.Add(mixerDetail);
				_inputStringList.Add(mixerDetail.MixerName);
			}
			
			_outputList = new List<MixerDetail>();
			mixerDetailDefault = new MixerDetail();
			_outputStringList = new List<string>();
			mixerDetailDefault.DeviceId = -1;
			mixerDetailDefault.MixerName = "Default";
			mixerDetailDefault.SupportWaveOut = true;
			_outputList.Add(mixerDetailDefault);
			_outputStringList.Add(mixerDetailDefault.MixerName);
			foreach (MixerDetail mixerDetail in mMixers.Playback.Devices)
			{
				_outputList.Add(mixerDetail);
				_outputStringList.Add(mixerDetail.MixerName);
			}



		}


		public List<string> StringOutput
		{
			get
			{
				return _outputStringList;
			}
		}

		public List<string> StringInput
		{
			get
			{
				return _inputStringList;
			}
		}
		
		public List<MixerDetail> Output
		{
			get
			{
				return _outputList;
			}
		}

		public List<MixerDetail> Input
		{
			get
			{				
				return _inputList;
			}
		}
	}

	
}
