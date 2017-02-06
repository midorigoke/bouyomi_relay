using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace bouyomi_relay_cui
{
	class Program
	{
		static void Main(string[] args)
		{
			int rx_port = 0;
			string tx_host = null;
			int tx_port = 0;
			string message_prefix = null;
			string message_suffix = null;

			switch (args.Length)
			{
				case 4:
					rx_port = Int32.Parse(args[0]);
					tx_host = args[1];
					tx_port = Int32.Parse(args[2]);
					message_prefix = args[3];
					break;

				case 5:
					rx_port = Int32.Parse(args[0]);
					tx_host = args[1];
					tx_port = Int32.Parse(args[2]);
					message_prefix = args[3];
					message_suffix = args[4];
					break;

				default:
					Console.Error.WriteLine("使用法: bouyomi_relay_cui <RX Port> <TX Host> <TX Port> <Massage Prefix> [Massage Suffix]");
					return;
			}

			TcpListener tl = new TcpListener(IPAddress.Any, rx_port);

			tl.Start();

			while (true)
			{
				TcpClient rx_client = tl.AcceptTcpClient();

				NetworkStream rx_ns = rx_client.GetStream();

				BinaryReader rx_br = new BinaryReader(rx_ns);

				Int16 command = rx_br.ReadInt16();
				Int16 speed = 0;
				Int16 tone = 0;
				Int16 volume = 0;
				Int16 voice = 0;
				byte code = 0;
				int length = 0;
				byte[] message_byte;
				string message_string = null;

				switch (command)
				{
					case 1:
						speed = rx_br.ReadInt16();
						tone = rx_br.ReadInt16();
						volume = rx_br.ReadInt16();
						voice = rx_br.ReadInt16();
						code = rx_br.ReadByte();
						length = rx_br.ReadInt32();

						message_byte = new byte[length];

						for (int i = 0; i < length; i++)
						{
							message_byte[i] = rx_br.ReadByte();
						}

						switch (code)
						{
							case 0:
								message_string = Encoding.UTF8.GetString(message_byte);
								break;
								
							case 1:
								message_string = Encoding.Unicode.GetString(message_byte);
								break;

							case 2:
								message_string = Encoding.GetEncoding("shift_jis").GetString(message_byte);
								break;
						}

						string message_concated = message_prefix + message_string + message_suffix;

						byte[] message_concated_byte = Encoding.UTF8.GetBytes(message_concated);

						length = message_concated_byte.Length;

						TcpClient tc = null;

						try
						{
							tc = new TcpClient(tx_host, tx_port);
						}
						catch
						{
							Console.Error.WriteLine("接続失敗");
						}

						if (tc != null)
						{
							NetworkStream tx_ns = tc.GetStream();

							BinaryWriter tx_bw = new BinaryWriter(tx_ns);

							tx_bw.Write(command);
							tx_bw.Write(speed);
							tx_bw.Write(tone);
							tx_bw.Write(volume);
							tx_bw.Write(voice);
							tx_bw.Write(0);
							tx_bw.Write(length);
							tx_bw.Write(message_concated_byte);

							tc.Close();
						}

						break;
				}

				rx_br.Close();				
				rx_ns.Close();
				rx_client.Close();

			}
		}
	}
}
