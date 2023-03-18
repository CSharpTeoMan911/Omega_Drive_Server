using System;
using System.Text;
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

            internal static async Task<bool> Load_Server_Certificate_In_Application_Memory_Initiator(string password)
            {
                return await Load_Server_Certificate_In_Application_Memory(password);
            }

            internal static async Task<string> Scan_File_With_Cloudmersive_Initiator(byte[] file)
            {
                return await Scan_File_With_Cloudmersive(file);
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
            // Force the [ Main ] thread to wait after the [ Task<bool> Application_Main_Thread ] thread
            // in order for the application not to finish its execution prematurely, by making the
            // [ Main ] thread to wait for a return value that the Task<bool> Application_Main_Thread ]
            // must give at the end of its execution, which is set to happen when the program is
            // scheduled for exit.
            bool return_value = Application_Main_Thread().Result;
        }


        private static async Task<bool> Application_Main_Thread()
        { 
            await Load_Server_Application_Settings_File();


            server_functionality_timer = new System.Timers.Timer();
            server_functionality_timer.Elapsed += Server_functionality_timer_Elapsed;
            server_functionality_timer.Interval = 100;
            server_functionality_timer.Start();

            System.AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


        Main_Menu:
            Server_Application_GUI.Main_Menu();
            string input = Console.ReadLine();

            if (input == "S")
            {
                if (server_opened == false)
                {
                    server_opened = true;

                    System.Threading.Thread client_connection_thread = new System.Threading.Thread(async () =>
                    {
                        Server_Operation();
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

                    goto Main_Menu;
                }
                else
                {
                    server_opened = false;

                    if (server_socket != null)
                    {
                        if (server_socket.Connected == true)
                        {
                            server_socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                        }
                        server_socket.Close();
                        server_socket.Dispose();
                    }

                    goto Main_Menu;
                }
            }
            else if (input == "ST")
            {

            Settings_Menu:

                Server_Application_GUI.Settings_Menu();

                string settings_input = Console.ReadLine();

                if (settings_input == "S")
                {
                    SMTPS_Settings();
                    goto Settings_Menu;
                }
                else if (settings_input == "EV")
                {
                    Cloudmersive_Scan_Enabled();
                    goto Settings_Menu;
                }
                else if (settings_input == "DV")
                {
                    Cloudmersive_Scan_Disabled();
                    goto Settings_Menu;
                }
                else if (settings_input == "SV")
                {
                    goto Settings_Menu;
                }
                else if (settings_input == "SM")
                {
                    goto Settings_Menu;
                }
                else if (settings_input == "G")
                {
                    goto Settings_Menu;
                }
                else if (settings_input == "EX")
                {
                    goto Main_Menu;
                }
                else
                {
                    goto Settings_Menu;
                }
            }
            else if (input == "E")
            {
                Environment.Exit(0);
            }
            else
            {
                goto Main_Menu;
            }


            return true;
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
                if (server_socket.Connected == true)
                {
                    server_socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                }
                server_socket.Close();
                server_socket.Dispose();
            }
        }

        private static void Server_functionality_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        private static async void Server_Operation()
        {
            try
            {
                server_socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                server_socket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, port_number));
                server_socket.ReceiveTimeout = 1000;
                server_socket.SendTimeout = 1000;
                server_socket.Listen(number_of_clients_backlog);

                while (server_opened == true)
                {
                    await Client_Connections_Mitigator.Secure_Client_Connection_Initiator(server_socket.Accept());
                }
            }
            catch(Exception E) 
            {
                
            }
        }




        private static async void SMTPS_Settings()
        {
            Server_Application_GUI.SMTPS_Service_Email_Setup();

            string smtps_email = Console.ReadLine();

            if (smtps_email == "E")
            {
                SMTPS_Settings();
            }
            else
            {
                try
                {
                    System.Net.Mail.MailAddress valid_email = new System.Net.Mail.MailAddress(smtps_email);

                    smtps_service_email_address = smtps_email;

                    Server_Application_GUI.SMTPS_Service_Password_Setup();

                    string smtps_password = Console.ReadLine();

                    if (smtps_password == "E")
                    {
                        SMTPS_Settings();
                    }
                    else
                    {

                    SMTPS_Service_Provider_Setup:

                        Server_Application_GUI.SMTPS_Service_Provider_Setup();

                        smtps_service_email_password = smtps_password;

                        string smtps_service_provider = Console.ReadLine();

                        if (smtps_service_provider == "E")
                        {
                            SMTPS_Settings();
                        }
                        else if (smtps_service_provider == "Google")
                        {
                            smtps_service_provider_name = smtps_service_provider;

                            await Update_Server_Application_Settings_File();
                        }
                        else if (smtps_service_provider == "Microsoft")
                        {
                            smtps_service_provider_name = smtps_service_provider;

                            await Update_Server_Application_Settings_File();
                        }
                        else
                        {
                            goto SMTPS_Service_Provider_Setup;
                        }
                    }
                }
                catch
                {
                    SMTPS_Settings();
                }
            }
        }


        private static async void Cloudmersive_Scan_Enabled()
        {
            byte[] eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

            string result = await Server_Cryptographic_Functions_Mitigator.Scan_File_With_Cloudmersive_Initiator(eicar);

            if(result.Contains("Error calling ScanFile:") == false)
            {
                Cloudmersive_Virus_Scan_Enabled = true;
            }
            else
            {
                
            }

            await Update_Server_Application_Settings_File();
        }

        private static async void Cloudmersive_Scan_Disabled()
        {
            Cloudmersive_Virus_Scan_Enabled = false;

            await Update_Server_Application_Settings_File();
        }


        private async void Set_Cloudmersive_API_Key()
        {
            
        }

    }
}
