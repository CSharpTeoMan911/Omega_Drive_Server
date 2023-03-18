using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_Variables
    {

        private static string server_settings_file_name = "application_settings.json";


        protected static System.Net.Sockets.Socket server_socket;
        protected static System.Timers.Timer server_functionality_timer;

        protected static bool server_opened;
        protected static int port_number = 1024;
        protected static int number_of_clients_backlog = 1000;

        protected static System.Security.Cryptography.X509Certificates.X509Certificate2 server_certificate;
        protected static string server_ssl_certificate_password = String.Empty;
        protected static System.Security.Authentication.SslProtocols connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls13;
        protected static List<string> available_connection_ssl_protocol = new List<string>() { "Tls13", "Tls12", "Tls11", "Tls", "Ssl3", "Ssl2"};
        protected static int current_connection_ssl_protocol = 0;

        protected static bool Cloudmersive_Virus_Scan_Enabled;
        protected static string Cloudmersive_Api_Key = String.Empty;
        protected static string smtps_service_provider_name = "Google";
        protected static string smtps_service_email_address = String.Empty;
        protected static string smtps_service_email_password = String.Empty;

        protected static string my_sql_database_username = String.Empty;
        protected static string my_sql_database_password = String.Empty;
        protected static string my_sql_database_server = String.Empty;
        protected static string my_sql_database_name = String.Empty;



        protected static async Task<bool> Create_Server_Application_Settings_File()
        {
            bool settings_file_creation_is_successful = false;

            Server_Settings settings = new Server_Settings();
            settings.port_number = port_number;
            settings.ssl_certificate_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(server_ssl_certificate_password));
            settings.ssl_protocol_index = current_connection_ssl_protocol;
            settings.smtps_service_provider = smtps_service_provider_name;
            settings.smtps_sevice_email = Convert.ToBase64String(Encoding.UTF8.GetBytes(smtps_service_email_address));
            settings.smtps_service_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(smtps_service_email_password));
            settings.cloudmersive_scan_enabled = Cloudmersive_Virus_Scan_Enabled;
            settings.cloudmersive_api_key = Convert.ToBase64String(Encoding.UTF8.GetBytes(Cloudmersive_Api_Key));
            settings.my_sql_username = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_username));
            settings.my_sql_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_password));
            settings.my_sql_server = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_server));
            settings.my_sql_database = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_name));


            byte[] json_serialised_server_application_settings = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented));

            System.IO.FileStream server_application_settings_file_stream = System.IO.File.Create(server_settings_file_name);

            try
            {
                await server_application_settings_file_stream.WriteAsync(json_serialised_server_application_settings, 0, json_serialised_server_application_settings.Length);
                await server_application_settings_file_stream.FlushAsync();

                settings_file_creation_is_successful = true;
            }
            catch
            {
                if(server_application_settings_file_stream != null)
                {
                    await server_application_settings_file_stream.FlushAsync();
                    server_application_settings_file_stream.Close();
                }
            }
            finally
            {
                if (server_application_settings_file_stream != null)
                {
                    await server_application_settings_file_stream.FlushAsync();
                    server_application_settings_file_stream.Close();
                    await server_application_settings_file_stream.DisposeAsync();
                }
            }

            return settings_file_creation_is_successful;
        }


        protected static async Task<bool> Load_Server_Application_Settings_File()
        {
            bool server_settings_load_is_successful = false;

            if(System.IO.File.Exists(server_settings_file_name))
            {

                System.IO.StreamReader server_application_settings_stream_reader = new System.IO.StreamReader(server_settings_file_name);

                try
                {
                    string server_application_settings = await server_application_settings_stream_reader.ReadToEndAsync();

                    System.Diagnostics.Debug.WriteLine(server_application_settings);



                    Server_Settings settings = new Server_Settings();
                 
                    port_number = settings.port_number;
                    server_ssl_certificate_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.ssl_certificate_password));
                    current_connection_ssl_protocol = settings.ssl_protocol_index;
                    smtps_service_provider_name = settings.smtps_service_provider;
                    smtps_service_email_address = Encoding.UTF8.GetString(Convert.FromBase64String(settings.smtps_sevice_email));
                    smtps_service_email_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.smtps_service_password));
                    Cloudmersive_Virus_Scan_Enabled = settings.cloudmersive_scan_enabled;
                    Cloudmersive_Api_Key = Encoding.UTF8.GetString(Convert.FromBase64String(settings.cloudmersive_api_key));
                    my_sql_database_username = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_username));
                    my_sql_database_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_password));
                    my_sql_database_server = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_server));
                    my_sql_database_name = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_database));



                    server_settings_load_is_successful = true;
                }
                catch
                {
                    if(server_application_settings_stream_reader != null)
                    {
                        server_application_settings_stream_reader.Close();
                    }
                }
                finally
                {
                    if (server_application_settings_stream_reader != null)
                    {
                        server_application_settings_stream_reader.Close();
                        server_application_settings_stream_reader.Dispose();
                    }
                }

            }
            else
            {
                await Create_Server_Application_Settings_File();
            }

            return server_settings_load_is_successful;
        }


        protected static async Task<bool> Update_Server_Application_Settings_File()
        {
            bool server_settings_update_is_successful = false;

            if (System.IO.File.Exists(server_settings_file_name))
            {
                try
                {
                    System.IO.File.Delete(server_settings_file_name);
                    server_settings_update_is_successful = true;
                }
                catch
                {

                }
            }

            await Create_Server_Application_Settings_File();

            return server_settings_update_is_successful;
        }
    }
}
