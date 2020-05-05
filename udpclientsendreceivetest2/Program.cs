using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace udptest
{

    public class UDPProgram
    {
        public static int port = 30000;
        public static IPAddress ipAddress = IPAddress.Parse("10.156.1.208");
        public static IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);
        public static Boolean done = false;
        public static Boolean exception_thrown = false;
        static public UdpClient udpclient;
        static public UdpClient udpclient2;

        public static void method1()
        {
            Console.WriteLine("UDP client1 ");
            //udpclient.ExclusiveAddressUse = false;
            int localport = 40000;
            udpclient = new UdpClient();
            udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpclient.Client.Bind(new IPEndPoint(IPAddress.Any, localport));

            string text_to_send = "UDP Client1 message";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text_to_send);
            try
            {
                //send data to port 30000 i.e. server from client1
                udpclient.Send(send_buffer, send_buffer.Length, ipEndpoint);
                Console.WriteLine("    UDP client1 --- after sending");

                //receive data from port 40000 i.e. client1

                //IPEndPoint remoteEP1 = new IPEndPoint(IPAddress.Any, 300000);
                IPEndPoint remoteEP1 = new IPEndPoint(IPAddress.Any, 40000);
                //                        Byte[] receiveBytes = udpclient.Receive(ref remoteEP);

                var receivedData = udpclient.Receive(ref ipEndpoint);
                //var receivedData = udpclient.Receive()
                string returnData = Encoding.ASCII.GetString(receivedData);
                Console.WriteLine("This is the message that you received: ");
                Console.WriteLine(returnData.ToString());
                Console.WriteLine("This message was sent from " + ipEndpoint.Address.ToString() + " on their port number " + ipEndpoint.Port.ToString() + "\n");

                Thread.Sleep(5000);
                udpclient.Close();
            }
            catch (Exception send_exception)
            {
                exception_thrown = true;
                Console.WriteLine(" Exception {1}", send_exception.Message);
            }
            if (exception_thrown == false)
            {
                Console.WriteLine("Message has been sent to the broadcast address");
            }
            else
            {
                exception_thrown = false;
                Console.WriteLine("The exception indicates the message was not sent. \n");
            }
            Console.WriteLine("    UDP client1 ");
        }


        public static void method2()
        {
            Console.WriteLine("UDP client2 ");

            int localport2 = 40000;
            UdpClient udpclient2 = new UdpClient();
            udpclient2.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpclient2.Client.Bind(new IPEndPoint(IPAddress.Any, localport2));
            Thread.Sleep(1000);

            //udpclient2.ExclusiveAddressUse = false;
            string text_to_send2 = "UDP Client2 message";
            byte[] send_buffer2 = Encoding.ASCII.GetBytes(text_to_send2);
            try
            {
                //send data to port 30000 i.e. server from client2
                udpclient2.Send(send_buffer2, send_buffer2.Length, ipEndpoint);
                Console.WriteLine("    UDP client2 --- after sending");

                var receivedData2 = udpclient2.Receive(ref ipEndpoint);
                string returnData2 = Encoding.ASCII.GetString(receivedData2);
                Console.WriteLine("This is the message that you received in udpclient2: ");
                Console.WriteLine(returnData2.ToString());
                Console.WriteLine("This message was sent from " + ipEndpoint.Address.ToString() + " on their port number " + ipEndpoint.Port.ToString() + "\n");
                udpclient2.Close();
            }
            catch (Exception send_exception)
            {
                exception_thrown = true;
                Console.WriteLine(" Exception {2}", send_exception.Message);
            }
            if (exception_thrown == false)
            {
                Console.WriteLine("Message has been sent to the broadcast address");
            }
            else
            {
                exception_thrown = false;
                Console.WriteLine("The exception indicates the message was not sent. \n");
            }
            Console.WriteLine("    UDP client2 ");
        }

        public class GFG
        {
            private static readonly object Program;

            static void Main(string[] args)
            {
                Boolean done = false;
                Boolean exception_thrown = false;

                #region comments
                // create an address object and populate it with the IP address that we will use
                // in sending at data to. This particular address ends in 255 meaning we will send
                // to all devices whose address begins with 192.168.2.
                // However, objects of class IPAddress have other properties. In particular, the
                // property AddressFamily. Does this constructor examine the IP address being
                // passed in, determine that this is IPv4 and set the field. If so, the notes
                // in the help file should say so.
                #endregion
                // IPEndPoint appears (to me) to be a class defining the first or final data object in the process of sending or 
                //receiving a communications packet. It holds the address to send to or receive from and the port to be used. We create
                // this one using the address just built in the previous line, and adding in the
                // port number.
                #region comments
                // The below three lines of code will not work. They appear to load
                // the variable broadcast_string witha broadcast address. But that
                // address causes an exception when performing the send.
                //
                //string broadcast_string = IPAddress.Broadcast.ToString();
                //Console.WriteLine("broadcast_string contains {0}", broadcast_string);

                //send_to_address = IPAddress.Parse(broadcast_string);
                #endregion

                Console.WriteLine("UDP client has begun --- 1");
                Console.WriteLine("Program will now send message to UDP server with port 30000.");
                Console.WriteLine("sending to address: {0} port: {1}", ipEndpoint.Address, ipEndpoint.Port);

                //while (!done)
                {
                    //Console.WriteLine("Enter text to send, blank line to quit");
                    string text_to_send0 = "this is dali";
                    if (text_to_send0.Length == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        Thread t1 = new Thread(method1)
                        {

                        };

                        Thread t2 = new Thread(method2)
                        {

                        };
                        t1.Start();
                        t2.Start();

                        t1.Join();

                        //wait for t2 to finish
                        t2.Join();
                    }
                } // end of while (!done)
            }
        }
    }
}
