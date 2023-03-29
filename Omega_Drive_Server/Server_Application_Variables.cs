using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
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
        protected static List<string> available_connection_ssl_protocol = new List<string>() { "Tls13", "Tls12" };
        protected static int current_connection_ssl_protocol = 0;



        protected static string Cloudmersive_Api_Key = String.Empty;
        protected static string smtps_service_provider_name = "Google";
        protected static string smtps_service_email_address = String.Empty;
        protected static string smtps_service_email_password = String.Empty;



        protected static string my_sql_database_username = String.Empty;
        protected static string my_sql_database_password = String.Empty;
        protected static string my_sql_database_server = String.Empty;
        protected static string my_sql_database_database_name = "omega_drive_db";







        protected static Task<bool> SSL_Protocol_Selection()
        {
            if (available_connection_ssl_protocol[current_connection_ssl_protocol] == "Tls13")
            {
                connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls13;
            }
            else
            {
                connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls12;
            }

            return Task.FromResult(true);
        }

        private static async Task<bool> Create_Server_Application_Settings_File()
        {
            bool settings_file_creation_is_successful = false;

            Server_Settings settings = new Server_Settings();


            lock(server_ssl_certificate_password)
            {
                lock (smtps_service_provider_name)
                {
                    lock (smtps_service_email_address)
                    {
                        lock (smtps_service_email_password)
                        {
                            lock (Cloudmersive_Api_Key)
                            {
                                lock (my_sql_database_username)
                                {
                                    lock (my_sql_database_password)
                                    {
                                        lock (my_sql_database_server)
                                        {
                                            settings.port_number = port_number;
                                            settings.ssl_certificate_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(server_ssl_certificate_password));
                                            settings.ssl_protocol_index = current_connection_ssl_protocol;
                                            settings.smtps_service_provider = smtps_service_provider_name;
                                            settings.smtps_sevice_email = Convert.ToBase64String(Encoding.UTF8.GetBytes(smtps_service_email_address));
                                            settings.smtps_service_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(smtps_service_email_password));
                                            settings.cloudmersive_api_key = Convert.ToBase64String(Encoding.UTF8.GetBytes(Cloudmersive_Api_Key));
                                            settings.my_sql_username = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_username));
                                            settings.my_sql_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_password));
                                            settings.my_sql_server = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_sql_database_server));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            await SSL_Protocol_Selection();


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

                    Server_Settings settings = Newtonsoft.Json.JsonConvert.DeserializeObject<Server_Settings>(server_application_settings);

                    lock (server_ssl_certificate_password)
                    {
                        lock (smtps_service_provider_name)
                        {
                            lock (smtps_service_email_address)
                            {
                                lock (smtps_service_email_password)
                                {
                                    lock (Cloudmersive_Api_Key)
                                    {
                                        lock (my_sql_database_username)
                                        {
                                            lock (my_sql_database_password)
                                            {
                                                lock (my_sql_database_server)
                                                {
                                                    port_number = settings.port_number;
                                                    server_ssl_certificate_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.ssl_certificate_password));
                                                    current_connection_ssl_protocol = settings.ssl_protocol_index;
                                                    smtps_service_provider_name = settings.smtps_service_provider;
                                                    smtps_service_email_address = Encoding.UTF8.GetString(Convert.FromBase64String(settings.smtps_sevice_email));
                                                    smtps_service_email_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.smtps_service_password));
                                                    Cloudmersive_Api_Key = Encoding.UTF8.GetString(Convert.FromBase64String(settings.cloudmersive_api_key));
                                                    my_sql_database_username = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_username));
                                                    my_sql_database_password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_password));
                                                    my_sql_database_server = Encoding.UTF8.GetString(Convert.FromBase64String(settings.my_sql_server));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await SSL_Protocol_Selection();


                    server_settings_load_is_successful = true;
                }
                catch(Exception E)
                {
                    if (server_application_settings_stream_reader != null)
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




        protected static async Task<bool> SMTPS_Service(string user_email, string code, string function)
        {
            bool smtps_service_result = false;

            MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            try
            {
                message.From.Add(new MimeKit.MailboxAddress("Omega Drive", smtps_service_email_address));
                message.To.Add(new MimeKit.MailboxAddress("User", user_email));
                message.Subject = function + " code";
                message.Body = new MimeKit.TextPart("plain") { Text = "One time " + function + " code: " + code };


                MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();

                try
                {
                    string smtp_server = String.Empty;
                    int smtp_server_port = 0;


                    switch(smtps_service_provider_name)
                    {
                        case "Google":
                            smtp_server = "smtp.gmail.com";
                            smtp_server_port = 465;
                            break;

                        case "Microsoft":
                            smtp_server = "smtp-mail.outlook.com";
                            smtp_server_port = 587;
                            break;
                    }


                    client.Connect(smtp_server, smtp_server_port, MailKit.Security.SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(smtps_service_email_address, smtps_service_email_password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + E.Message);
                    if (client != null)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
                finally
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }
            }
            catch (Exception E)
            {

            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }

            return smtps_service_result;
        }
    }
}
