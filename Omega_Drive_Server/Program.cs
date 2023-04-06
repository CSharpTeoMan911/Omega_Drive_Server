using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Program:Server_Application_Variables
    {
        private static Client_Connections client_connections = new Client_Connections();
        private static Server_Cryptographic_Functions server_cryptographic_functions = new Server_Cryptographic_Functions();


        static void Main(string[] args)
        {
            // Force the [ Main ] thread to wait after the [ Task<bool> Application_Main_Thread ] thread
            // in order for the application not to finish its execution prematurely, by making the
            // [ Main ] thread to wait for a return value that the Task<bool> Application_Main_Thread ]
            // must give at the end of its execution, which is set to happen when the program is
            // scheduled for exit.
            try
            {
                bool return_value = Application_Main_Thread().Result;
            }
            catch(Exception E)
            {
                System.Diagnostics.Debug.WriteLine(E.Message);
            }
        }


        private static async Task<bool> Application_Main_Thread()
        {
            server_socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            server_socket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port_number));
            server_socket.ReceiveTimeout = 1000;
            server_socket.SendTimeout = 1000;

            await Load_Server_Application_Settings_File();


        x509_Certificate_Generation:
            bool server_certificate_load_up_successful = await server_cryptographic_functions.Load_Server_Certificate_In_Application_Memory();

            if (server_certificate_load_up_successful == false)
            {

                Server_Application_GUI.X509_Certificate_Loadup_Error();
                Console.ReadLine();
                  
                bool x509_Certificate_Generation_Result_Is_Successful = await Generate_X509_Certificate();

                if (x509_Certificate_Generation_Result_Is_Successful == true)
                {
                    Server_Application_GUI.X509_Certificate_Generation_Successful();
                }
                else
                {
                    Server_Application_GUI.X509_Certificate_Generation_Unsuccessful();
                }

                Console.ReadLine();

                goto x509_Certificate_Generation;
            }
            


            server_functionality_timer = new System.Timers.Timer();
            server_functionality_timer.Elapsed += Server_functionality_timer_Elapsed;
            server_functionality_timer.Interval = 10000;
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

                    System.Threading.Thread server_operation_thread = new System.Threading.Thread(() =>
                    {
                        Server_Operation();
                    });

                    if (OperatingSystem.IsWindows() == true)
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        server_operation_thread.SetApartmentState(System.Threading.ApartmentState.MTA);
                        #pragma warning restore CA1416 // Validate platform compatibility
                    }
                    server_operation_thread.Priority = System.Threading.ThreadPriority.Highest;
                    server_operation_thread.IsBackground = false;
                    server_operation_thread.Start();

                    goto Main_Menu;
                }
                else
                {
                    server_opened = false;

                    if(server_socket.Connected == true)
                    {
                        await server_socket.DisconnectAsync(true, System.Threading.CancellationToken.None);
                    }

                    goto Main_Menu;
                }
            }
            else if (input == "ST")
            {

            Settings_Menu:

                Server_Application_GUI.Settings_Menu();

                string settings_input = Console.ReadLine();

                if (settings_input == "P")
                {
                    bool Server_Port_Setup_Cancelled = await Server_Port_Number_Setup();

                    if (Server_Port_Setup_Cancelled == true)
                    {
                        Server_Application_GUI.Port_Setup_Cancelled();
                    }
                    else
                    {
                        Server_Application_GUI.Port_Setup_Successful();
                    }

                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "PR")
                {
                    await Server_SSL_Protocol_Setup();

                    Server_Application_GUI.SSL_Protocol_Setup_Finsished();
                    Console.ReadLine();

                    goto Settings_Menu;
                }
                else if (settings_input == "S")
                {
                    bool SMTPS_Service_Setup_Cancelled = await SMTPS_Settings();

                    if (SMTPS_Service_Setup_Cancelled == true)
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
                    bool Cloudmersive_Setup_Cancelled = await Set_Cloudmersive_Scan_API_Key();

                    if (Cloudmersive_Setup_Cancelled == true)
                    {
                        Server_Application_GUI.Cloudmersive_API_Key_Setup_Cancelled();
                    }
                    else
                    {
                        Server_Application_GUI.Cloudmersive_API_Key_Setup_Successful();
                    }

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

                    
                    if (x509_Certificate_Generation_Result_Is_Successful == true)
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
            server_opened = false;

            if (server_certificate != null)
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
                server_socket.Dispose();
            }
        }



        private static async void Server_functionality_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if(server_opened == true)
            {
                // SET THE APPLICATION'S DEFAULT THREAD POOL MAXIMUM SIZE IF THE NUMBER OF AVAILABLE
                // THREADS WITHIN THE THREAD POOL IS LESS THAN 1000. THE THREAD POOL IS USED BY 
                // ASYNCHRONOUS TASKS AND THESE THREADS ARE THREADS WITHOUT CERTAIN SET 
                // CHARACTERISTICS, LIKE PRIORITY, BACKGROUND OPERATION, FOREGROUND
                // OPERATION, OR APARTMENT STATE.
                //
                // [ BEGIN ]

                int max_worker_threads = 0;
                int max_port_threads = 0;

                int current_available_worker_threads = 0;
                int current_available_port_threads = 0;

                System.Threading.ThreadPool.GetMaxThreads(out max_worker_threads, out max_port_threads);
                System.Threading.ThreadPool.GetAvailableThreads(out current_available_worker_threads, out current_available_port_threads);

                if (current_available_worker_threads < 1000)
                {
                    System.Threading.ThreadPool.SetMaxThreads(max_worker_threads + 1000, max_port_threads);
                }
                else
                {
                    System.Threading.ThreadPool.SetMaxThreads(max_worker_threads - (current_available_worker_threads - 1000), max_port_threads);
                }


                if (current_available_port_threads < 1000)
                {
                    System.Threading.ThreadPool.SetMaxThreads(max_worker_threads, max_port_threads + 1000);
                }
                else
                {
                    System.Threading.ThreadPool.SetMaxThreads(max_worker_threads, max_port_threads - (current_available_port_threads - 1000));
                }

                // [ END ]


                await Delete_Expired_Database_Items();
            }
        }
















        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async void Server_Operation()
        {
            try
            {
                server_socket.Listen(number_of_clients_backlog);

                while (true)
                {
                    if(server_opened == true)
                    {
                        await client_connections.Secure_Client_Connection(server_socket.Accept());
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch(Exception E) 
            {
                System.Diagnostics.Debug.WriteLine(E.Message);
            }
        }






        private static async Task<bool> Server_Port_Number_Setup()
        {
            bool Server_Port_Setup_Cancelled = true;

            Server_Application_GUI.Port_Setup();

            try
            {
                string input = Console.ReadLine();


                if (input != "E")
                {
                    double port_number_buffer = Convert.ToDouble(input);
                    port_number = Convert.ToInt32(input);
                    await Update_Server_Application_Settings_File();

                    Server_Port_Setup_Cancelled = false;
                }
            }
            catch
            {
                await Server_Port_Number_Setup();
            }

            return Server_Port_Setup_Cancelled;
        }




        private static async Task<bool> Server_SSL_Protocol_Setup()
        {
            Server_Application_GUI.SSL_Protocol_Setup();
            string input = Console.ReadLine();

            if(input == "N")
            {
                if (current_connection_ssl_protocol < 1)
                {
                    current_connection_ssl_protocol++;
                }

                await Update_Server_Application_Settings_File();

                await Server_SSL_Protocol_Setup();
            }
            else if(input == "P")
            {
                if (current_connection_ssl_protocol > 0)
                {
                    current_connection_ssl_protocol--;
                }

                await Update_Server_Application_Settings_File();

                await Server_SSL_Protocol_Setup();
            }
            else if(input == "E")
            {
                return true;
            }
            else
            {
                await Server_SSL_Protocol_Setup();
            }

            return true;
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

                    lock (smtps_service_email_address)
                    {
                        smtps_service_email_address = smtps_email;
                    }

                    Server_Application_GUI.SMTPS_Service_Password_Setup();
                    string smtps_password = Console.ReadLine();

                    if (smtps_password != "E")
                    {

                        lock (smtps_service_email_password)
                        {
                            smtps_service_email_password = smtps_password;
                        }

                    SMTPS_Service_Provider_Setup:

                        Server_Application_GUI.SMTPS_Service_Provider_Setup();

                        string smtps_service_provider = Console.ReadLine();

                        if (smtps_service_provider == "E")
                        {
                            return false;
                        }
                        else if (smtps_service_provider == "Google")
                        {
                            lock (smtps_service_provider_name)
                            {
                                smtps_service_provider_name = smtps_service_provider;
                            }

                            await Update_Server_Application_Settings_File();

                            return true;
                        }
                        else if (smtps_service_provider == "Microsoft")
                        {
                            lock (smtps_service_provider_name)
                            {
                                smtps_service_provider_name = smtps_service_provider;
                            }

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
            bool Cloudmersive_Setup_Cancelled = true;

            Server_Application_GUI.Cloudmersive_API_Key_Setup();

            string input = Console.ReadLine();

            string Cloudmersive_Api_Key_Buffer = String.Empty;

            lock(Cloudmersive_Api_Key)
            {
                Cloudmersive_Api_Key_Buffer = Cloudmersive_Api_Key;
                Cloudmersive_Api_Key = input;
            }

            if (await Verify_If_Cloudmersive_API_Is_Valid() == false)
            {
                if (input != "E")
                {
                    Server_Application_GUI.Cloudmersive_API_Key_Setup_Error_Message();
                    Console.ReadLine();

                    lock (Cloudmersive_Api_Key)
                    {
                        Cloudmersive_Api_Key = Cloudmersive_Api_Key_Buffer;
                    }

                    await Set_Cloudmersive_Scan_API_Key();
                }
            }
            else
            {
                Cloudmersive_Setup_Cancelled = false;
                await Update_Server_Application_Settings_File();
            }


            return Cloudmersive_Setup_Cancelled;
        }


        private static async Task<bool> Cloudmersive_Scan_Disabled()
        {
            lock (Cloudmersive_Api_Key)
            {
                Cloudmersive_Api_Key = String.Empty;
            }
            
            return await Update_Server_Application_Settings_File();
        }

        private static async Task<bool> Verify_If_Cloudmersive_API_Is_Valid()
        {
            bool api_key_is_valid = false;

            byte[] eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

            string result = await server_cryptographic_functions.Scan_File_With_Cloudmersive(eicar);


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

                            lock(my_sql_database_username)
                            {
                                lock(my_sql_database_password)
                                {
                                    lock(my_sql_database_server)
                                    {
                                        my_sql_database_username = my_sql_database_username_buffer;
                                        my_sql_database_password = my_sql_database_password_buffer;
                                        my_sql_database_server = my_sql_database_server_buffer;
                                    }
                                }
                            }

                            await Update_Server_Application_Settings_File();
                        }
                        catch(Exception E)
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

            if(certificate_password != "E")
            {
            Time_Period_Setup:
                int certificate_valid_time_period = 0;
                Server_Application_GUI.X509_Certificate_Generation_Valid_Time_Period_Setup();
                string time_period = Console.ReadLine();


                if(time_period != "E")
                {
                    try
                    {
                        certificate_valid_time_period = (int)Convert.ToDouble(time_period);
                    }
                    catch
                    {
                        goto Time_Period_Setup;
                    }


                    if(certificate_valid_time_period > 0)
                    {
                        x509_Certificate_Generation_Result_Is_Successful = await server_cryptographic_functions.Create_X509_Server_Certificate(certificate_password, certificate_valid_time_period);

                        if (x509_Certificate_Generation_Result_Is_Successful == false)
                        {
                            await Generate_X509_Certificate();
                        }
                        else
                        {
                            lock (server_ssl_certificate_password)
                            {
                                server_ssl_certificate_password = certificate_password;
                            }

                            await Update_Server_Application_Settings_File();
                        }
                    }
                }
                else
                {
                    x509_Certificate_Generation_Result_Is_Successful = false;
                }
            }
            else
            {
                x509_Certificate_Generation_Result_Is_Successful = false;
            }

            return x509_Certificate_Generation_Result_Is_Successful;
        }

    }
}
