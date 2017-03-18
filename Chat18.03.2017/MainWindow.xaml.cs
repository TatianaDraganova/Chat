using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helper;

namespace Chat18._03._2017
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			new Thread(UpdateUsers).Start();
			new Thread(UpdateChat).Start();
		}

		private void UpdateChat()
		{
			
		}

		private void UpdateUsers()
		{
			while (true)
			{
				var tcpClient = new TcpClient(@"192.168.2.94", 7777);
				var stream = tcpClient.GetStream();
				IFormatter formatter = new BinaryFormatter();
				var users = formatter.Deserialize(stream) as Dictionary<string, string>;
				listView.Dispatcher.Invoke(() => listView.ItemsSource = users?.Select((user) => user.Key));
				tcpClient.Close();
				Thread.Sleep(1000);
			}
		}


		private void button_Click(object sender, RoutedEventArgs e)
		{
			var text = new TextRange(Rtbwriting.Document.ContentStart, Rtbwriting.Document.ContentEnd).Text;
			var from = "Tatiana";
			var to = "Tatiana";
			var msg = new Message(text, from, to, DateTime.Now);

			var tcpClient = new TcpClient(@"192.168.2.94", 4444);
			IFormatter formatter = new BinaryFormatter();
			var stream = tcpClient.GetStream();
			formatter.Serialize(stream, msg);

		}
	}
}
