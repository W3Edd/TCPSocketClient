namespace TCPSocketClient {
	using W3Tools;

	class Program {
		static void Main(string[] args) {
			TCPSocket socket = new TCPSocket("localhost", 12345);
			socket.AddMessage("hello");
			P.PrintLine(socket.Chat());
		}
	}
}