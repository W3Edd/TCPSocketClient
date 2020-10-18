namespace TCPSocketClient {
	using System;
	using System.Collections.Generic;
	using System.Net.Sockets;
	using W3Tools;

	public class TCPSocket {
		public string Server { get; set; }
		public int Port { get; set; }
		public List<string> Messages { get; set; }
		public TcpClient Client { get; set; }

		public TCPSocket(string server, int port) {
			this.Server = server;
			this.Port = port;
			this.Messages = new List<string>();
		}

		public void AddMessage(string message) {
			this.Messages.Add(message);
		}

		public bool SendMessage() {
			bool sent = false;
			if (this.Messages.Count <= 0) return false;
			try {
				this.Client = new TcpClient(this.Server, this.Port);
				byte[] data = System.Text.Encoding.ASCII.GetBytes(this.Messages[0]);
				NetworkStream stream = this.Client.GetStream();
				try {
					stream.Write(data, 0, data.Length);
					this.Messages.RemoveAt(0);
					sent = true;
				}
				catch (Exception e) {
					Logger.StaticLog(e);
				}
				finally {
					stream.Close();
					this.Client.Close();
				}
			}
			catch (Exception e) {
				Logger.StaticLog(e);
			}
			return sent;
		}

		public bool SendAll() {
			bool sent = false;
			while (
				this.Messages.Count > 0 &&
				(sent = this.SendMessage())
			) { }
			return sent;
		}

		public string Read() {
			byte[] received = new byte[1024];
			int bytes = 0;
			try {
				this.Client = new TcpClient(this.Server, this.Port);
				NetworkStream stream = this.Client.GetStream();
				bytes = stream.Read(received, 0, received.Length);
				stream.Close();
				this.Client.Close();
			}
			catch (Exception e) {
				Logger.StaticLog(e);
			}
			return System.Text.Encoding.ASCII.GetString(received, 0, bytes);
		}

		public string Chat() {
			string message = null;
			if (this.Messages.Count <= 0) return null;
			try {
				this.Client = new TcpClient(this.Server, this.Port);
				byte[] data = System.Text.Encoding.ASCII.GetBytes(this.Messages[0]);
				NetworkStream stream = this.Client.GetStream();
				try {
					stream.Write(data, 0, data.Length);
					this.Messages.RemoveAt(0);
					data = new byte[1024];
					int bytes = stream.Read(data, 0, data.Length);
					message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				}
				catch (Exception e) {
					Logger.StaticLog(e);
				}
				finally {
					stream.Close();
					this.Client.Close();
				}
			}
			catch (Exception e) {
				Logger.StaticLog(e);
			}
			return message;
		}
	}
}