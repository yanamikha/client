using System;
using System.Net.Sockets;
using System.IO;

namespace ClientFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("tcp client started");
            Console.WriteLine("Input command 'pagesaver', address filename for saving of site's text");
            string inputLine = Console.ReadLine();
            inputLine = inputLine.Replace("http://", "");
            string address = string.Empty;
            string page = string.Empty;
            string filename = string.Empty;

            if (inputLine.Substring(0, 9) == "pagesaver")
            {
                for (int i = "pagesaver".Length+1; inputLine[i] != '/'; i++)
                    address += inputLine[i];
            } 
            else
            {
                for (int i = 0; inputLine[i] != '/'; i++)
                    address += inputLine[i];
            }
                for (int i = inputLine.IndexOf('/'); inputLine[i] != ' '; i++)
                    page += inputLine[i];

                for (int i = inputLine.LastIndexOf(' '); i < inputLine.Length; i++)
                    filename += inputLine[i];

              
            string msg = "GET "+page+" HTTP/1.0\nHost: " + address + "\n\n";
            try
            {
                int port = 80;
                string server = address;
                TcpClient client = new TcpClient(server, port);
                var data = System.Text.Encoding.ASCII.GetBytes(msg);
                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent {0}", msg);

                var responseData = new byte[2048];
                int bytesRead = stream.Read(responseData, 0, responseData.Length);
                string responseMessage = System.Text.Encoding.ASCII.GetString(responseData, 0, bytesRead);
                Console.WriteLine("Received {0}", responseMessage);
                stream.Close();
                client.Close();
                var saveStr = string.Empty;
                for (int i = responseMessage.IndexOf("<body>")+6; i< responseMessage.Length-14; i++)
                    saveStr += responseMessage[i];
                using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    fstream.Write(System.Text.Encoding.Default.GetBytes(saveStr), 0, saveStr.Length);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0}",e);
            }
            Console.Read();
        }
    }

}
