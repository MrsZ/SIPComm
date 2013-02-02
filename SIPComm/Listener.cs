using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SIPComm
{
	public class Listener
	{
		private const int Port = 31462;
		private IPAddress IP = IPAddress.Loopback;
		private UdpClient _instance;
		private IPEndPoint _endPoint;

		public delegate void DIncomingListenerMessageHandler(string message);
		public event DIncomingListenerMessageHandler OnReceiveCommand;

		public Listener()
		{
			Initialise();
		}

		public void Initialise()
		{
			if (null != _instance) return;
			try
			{			
				_instance = new UdpClient(Port);
			}
			catch (SocketException e)
			{
				throw e;
			}
			_endPoint = new IPEndPoint(IP, Port);
		}

		public void Start()
		{
			try
			{
				while (true)
				{
					byte[] bytes = _instance.Receive(ref _endPoint);
					string s = (Encoding.ASCII.GetString(bytes, 0, bytes.Length));
					if (null != OnReceiveCommand)
					{
						OnReceiveCommand.Invoke(s);
					}
				}
			}
			catch (Exception e)
			{
				return;
			}
			finally
			{
				_instance.Close();
			}
		}
	}
}