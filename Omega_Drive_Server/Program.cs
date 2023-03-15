using System;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Program:Server_Application_Variables
    {

        private sealed class Server_Cryptographic_Functions_Mitigator:Server_Cryptographic_Functions
        {
            internal static async Task<bool> Create_X509_Server_Certificate_Initiator(string password, int certificate_valid_time_period_in_days)
            {
                return await Create_X509_Server_Certificate(password, certificate_valid_time_period_in_days);
            }

            internal static async Task<bool> Delete_X509_Server_Certificate_Initiator()
            {
                return await Delete_X509_Server_Certificate();
            }
        }

        private sealed class Client_Connections_Mitigator:Client_Connections
        {
            internal static async Task<bool> Secure_Client_Connection_Initiator(System.Net.Sockets.Socket client)
            {
                return await Secure_Client_Connection(client);
            }
        }


        static void Main(string[] args)
        {
            server_functionality_timer = new System.Timers.Timer();
            server_functionality_timer.Elapsed += Server_functionality_timer_Elapsed;
            server_functionality_timer.Interval = 100;
            server_functionality_timer.Start();

            System.AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;



        Main_Menu:
            string option = Server_Application_GUI.Main_Menu();

            if (option == "Start")
            {
                Server_Operation();

                goto Main_Menu;
            }
            else if (option == "Stop")
            {
                if(server_socket != null)
                {
                    server_socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    server_socket.Close();
                    server_socket.Dispose();
                }

                goto Main_Menu;
            }
            else if (option == "O")
            {
            //Options_Menu:;
            }
            else if (option == "E")
            {
                Environment.Exit(0);
            }
            else
            {
                goto Main_Menu;
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if(server_certificate != null)
            {
                server_certificate.Dispose();
            }

            if(server_functionality_timer != null)
            {
                server_functionality_timer.Stop();
                server_functionality_timer.Dispose();
            }

            if(server_socket != null)
            {
                server_socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                server_socket.Close();
                server_socket.Dispose();
            }
        }

        private static void Server_functionality_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        private static void Server_Operation()
        {
            try
            {
                server_socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                server_socket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, port_number));

                server_socket.Listen(number_of_clients_backlog);

                while (server_opened == true)
                {
                    System.Threading.Thread client_connection_thread = new System.Threading.Thread(async () =>
                    {
                        await Client_Connections_Mitigator.Secure_Client_Connection_Initiator(server_socket.Accept());
                    });

                    if (OperatingSystem.IsWindows() == true)
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        client_connection_thread.SetApartmentState(System.Threading.ApartmentState.MTA);
#pragma warning restore CA1416 // Validate platform compatibility
                    }
                    client_connection_thread.Priority = System.Threading.ThreadPriority.Highest;
                    client_connection_thread.IsBackground = false;
                    client_connection_thread.Start();
                }
            }
            catch { }
        }
    }
}
