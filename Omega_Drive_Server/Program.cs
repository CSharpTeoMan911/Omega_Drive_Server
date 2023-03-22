﻿using System;
using System.Runtime.ConstrainedExecution;
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
                    bool SMTPS_Service_Setup_Cancelled = await SMTPS_Settings();

                    if(SMTPS_Service_Setup_Cancelled == true)
                    {
                        Server_Application_GUI.SMTPS_Service_Provider_Setup_Successful();
                    }
                    else
                    {
                        Server_Application_GUI.SMTPS_Service_Provider_Setup_Cancelled();
                    }
                    
                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "SV")
                {
                    await Set_Cloudmersive_Scan_API_Key();

                    Server_Application_GUI.Cloudmersive_API_Key_Setup_Successful();
                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "DV")
                {
                    await Cloudmersive_Scan_Disabled();

                    Server_Application_GUI.Cloudmersive_API_Disabled();
                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "SM")
                {
                    await Set_MySQL_Database_Connection();

                    Server_Application_GUI.MySQL_Server_Authentification_Successful();
                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "G")
                {
                    bool x509_Certificate_Generation_Result_Is_Successful = await Generate_X509_Certificate();

                    if(x509_Certificate_Generation_Result_Is_Successful == true)
                    {
                        Server_Application_GUI.X509_Certificate_Generation_Successful();
                    }
                    else
                    {
                        Server_Application_GUI.X509_Certificate_Generation_Unsuccessful();
                    }

                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "SC")
                {
                    bool x509_Certificate_Setup_Result_Is_Cancelled = await Set_X509_Certificate();

                    if (x509_Certificate_Setup_Result_Is_Cancelled == true)
                    {
                        Server_Application_GUI.X509_Certificate_Setup_Cancelled();
                        Console.ReadLine();
                    }
                    else
                    {
                        Server_Application_GUI.X509_Certificate_Setup_Successful();
                        Console.ReadLine();
                    }

                    Console.ReadLine();

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




        private static async Task<bool> SMTPS_Settings()
        {
            Server_Application_GUI.SMTPS_Service_Email_Setup();

            string smtps_email = Console.ReadLine();

            if (smtps_email != "E")
            {
                try
                {
                    System.Net.Mail.MailAddress valid_email = new System.Net.Mail.MailAddress(smtps_email);

                    smtps_service_email_address = smtps_email;

                    Server_Application_GUI.SMTPS_Service_Password_Setup();

                    string smtps_password = Console.ReadLine();

                    if (smtps_password != "E")
                    {
                    SMTPS_Service_Provider_Setup:

                        Server_Application_GUI.SMTPS_Service_Provider_Setup();

                        smtps_service_email_password = smtps_password;

                        string smtps_service_provider = Console.ReadLine();

                        if (smtps_service_provider == "E")
                        {
                            return false;
                        }
                        else if (smtps_service_provider == "Google")
                        {
                            smtps_service_provider_name = smtps_service_provider;

                            await Update_Server_Application_Settings_File();

                            return true;
                        }
                        else if (smtps_service_provider == "Microsoft")
                        {
                            smtps_service_provider_name = smtps_service_provider;

                            await Update_Server_Application_Settings_File();

                            return true;
                        }
                        else
                        {
                            goto SMTPS_Service_Provider_Setup;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    Server_Application_GUI.SMTPS_Error_Message();
                    Console.ReadLine();

                    await SMTPS_Settings();
                }
            }
            else
            {
                return false;
            }


            return true;
        }


        private static async Task<bool> Set_Cloudmersive_Scan_API_Key()
        {
            Server_Application_GUI.Cloudmersive_API_Key_Setup();

            string input = Console.ReadLine();

            string Cloudmersive_Api_Key_Buffer = Cloudmersive_Api_Key;
            Cloudmersive_Api_Key = input;

            if (await Verify_If_Cloudmersive_API_Is_Valid() == false)
            {
                if (input != "E")
                {
                    Server_Application_GUI.Cloudmersive_API_Key_Setup_Error_Message();
                    Console.ReadLine();

                    Cloudmersive_Api_Key = Cloudmersive_Api_Key_Buffer;

                    await Set_Cloudmersive_Scan_API_Key();
                }
            }
            else
            {
                await Update_Server_Application_Settings_File();
            }


            return true;
        }


        private static async Task<bool> Cloudmersive_Scan_Disabled()
        {
            Cloudmersive_Api_Key = String.Empty;
            
            return await Update_Server_Application_Settings_File();
        }

        private static async Task<bool> Verify_If_Cloudmersive_API_Is_Valid()
        {
            bool api_key_is_valid = false;

            byte[] eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

            string result = await Server_Cryptographic_Functions_Mitigator.Scan_File_With_Cloudmersive_Initiator(eicar);


            if (result.Contains("Virus found"))
            {
                api_key_is_valid = true;
            }

            return api_key_is_valid;
        }


        private static async Task<bool> Set_MySQL_Database_Connection()
        {
            bool invalid_credentials = false;

            Server_Application_GUI.MySQL_Username_Setup();
            string my_sql_database_username_buffer = Console.ReadLine();

            if(my_sql_database_username_buffer != "E")
            {
                Server_Application_GUI.MySQL_Password_Setup();
                string my_sql_database_password_buffer = Console.ReadLine();

                if (my_sql_database_username_buffer != "E")
                {
                    Server_Application_GUI.MySQL_Server_Setup();
                    string my_sql_database_server_buffer = Console.ReadLine();

                    if (my_sql_database_username_buffer != "E")
                    {
                        MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection("Server=" + my_sql_database_server_buffer + ";" + "User ID=" + my_sql_database_username_buffer + ";" + "Password=" + my_sql_database_password_buffer + ";" + "Database=" + my_sql_database_database_name);

                        try
                        {
                            await connection.OpenAsync();

                            my_sql_database_username = my_sql_database_username_buffer;
                            my_sql_database_password = my_sql_database_password_buffer;
                            my_sql_database_server = my_sql_database_server_buffer;

                            await Update_Server_Application_Settings_File();
                        }
                        catch
                        {
                            Server_Application_GUI.MySQL_Server_Authentification_Error();
                            Console.ReadLine();

                            invalid_credentials = true;

                            if (connection != null)
                            {
                                await connection.CloseAsync();
                            }
                        }
                        finally
                        {
                            if (connection != null)
                            {
                                await connection.CloseAsync();
                                await connection.DisposeAsync();
                            }
                        }

                        if(invalid_credentials == true)
                        {
                            await Set_MySQL_Database_Connection();
                        }
                    }
                }
            }

            return true;
        }


        private static async Task<bool> Generate_X509_Certificate()
        {
            bool x509_Certificate_Generation_Result_Is_Successful = false;

            Server_Application_GUI.X509_Certificate_Generation_Password_Setup();
            string certificate_password = Console.ReadLine();

        Time_Period_Setup:
            int certificate_valid_time_period = 0;
            Server_Application_GUI.X509_Certificate_Generation_Valid_Time_Period_Setup();

            try
            {
                certificate_valid_time_period = (int)Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                goto Time_Period_Setup;
            }


            x509_Certificate_Generation_Result_Is_Successful = await Server_Cryptographic_Functions_Mitigator.Create_X509_Server_Certificate_Initiator(certificate_password, certificate_valid_time_period);

            if(x509_Certificate_Generation_Result_Is_Successful == false)
            {
                await Generate_X509_Certificate();
            }

            return true;
        }



        private static async Task<bool> Set_X509_Certificate()
        {
            bool X509_Certificate_Cancelled = false;

            Server_Application_GUI.X509_Certificate_Setup();

            string certificate_password = Console.ReadLine();

            if(certificate_password != "E")
            {
                bool X509_Certificate_Setup_Result = await Server_Cryptographic_Functions_Mitigator.Load_Server_Certificate_In_Application_Memory_Initiator(certificate_password);

                if(X509_Certificate_Setup_Result == false)
                {
                    Server_Application_GUI.X509_Certificate_Setup_Error();
                    Console.ReadLine();

                    await Set_X509_Certificate();
                }
                else
                {
                    server_ssl_certificate_password = certificate_password;
                    await Update_Server_Application_Settings_File();
                }
            }
            else
            {
                X509_Certificate_Cancelled = true;
            }

            return X509_Certificate_Cancelled;
        }

    }
}
