using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_Variables
    {
        // SERVER FUNCTIONS NOTIFICATIONS AND ERROR MESSAGES
        //
        // [ BEGIN ]

        protected static byte[] connection_failed_message = Encoding.UTF8.GetBytes("Connection failed");
        protected static byte[] email_already_in_use_message = Encoding.UTF8.GetBytes("Email already in use");
        protected static byte[] account_registration_successful = Encoding.UTF8.GetBytes("Registration successful");
        protected static byte[] invalid_email_address = Encoding.UTF8.GetBytes("Invalid email address");
        protected static byte[] invalid_password = Encoding.UTF8.GetBytes("Invalid password");
        protected static byte[] account_not_validated = Encoding.UTF8.GetBytes("Un-validated account");
        protected static byte[] login_successful = Encoding.UTF8.GetBytes("Log in successful");
        protected static byte[] invalid_account_validation_code = Encoding.UTF8.GetBytes("Invalid account validation code");
        protected static byte[] invalid_log_in_code = Encoding.UTF8.GetBytes("Invalid log in code");
        protected static byte[] account_validation_successful = Encoding.UTF8.GetBytes("Account validation successful");
        protected static byte[] invalid_password_length = Encoding.UTF8.GetBytes("Invalid password length");
        protected static byte[] account_authentification_successful = Encoding.UTF8.GetBytes("Account authentification successful");
        protected static byte[] log_in_session_key_valid = Encoding.UTF8.GetBytes("Log in session key is valid");
        protected static byte[] log_in_session_key_invalid = Encoding.UTF8.GetBytes("Log in session key is invalid");
        protected static string content_hashing_error = "Error occured";
        // [ END ]










        // SERVER APPLICATION MAIN FUNCTIONALITIES VARIABLES
        //
        // [ BEGIN ]

        private static string server_settings_file_name = "application_settings.json";


        protected static System.Net.Sockets.Socket server_socket;
        protected static System.Timers.Timer server_functionality_timer;



        protected static bool server_opened;
        protected static int port_number = 1024;
        protected static int number_of_clients_backlog = 1000;



        protected static System.Security.Cryptography.X509Certificates.X509Certificate2 server_certificate;
        protected static string server_ssl_certificate_password = String.Empty;
        protected static System.Security.Authentication.SslProtocols connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls13;


        protected static readonly Dictionary<string, string> smtps_provider_and_server = new Dictionary<string, string>()
        {
            { "Google", "smtp.gmail.com" },
            { "Microsoft", "smtp-mail.outlook.com" }
        };

        protected static int current_connection_ssl_protocol = 0;



        protected static string Cloudmersive_Api_Key = String.Empty;
        protected static string smtps_service_provider_name = "Google";
        protected static string smtps_service_email_address = String.Empty;
        protected static string smtps_service_email_password = String.Empty;



        protected static string my_sql_database_username = String.Empty;
        protected static string my_sql_database_password = String.Empty;
        protected static string my_sql_database_server = String.Empty;
        protected static string my_sql_database_database_name = "omega_drive_db";

        // [ END ]






        internal enum SslProtocol
        {
            Tls13 = 0,
            Tls12 = 1
        }




        // SETS THE SSL PROTOCOL THAT IS USED BY THE SERVER DURING ANY CONNECTION INITIATED WITH ANY CLIENT
        protected static Task<bool> Get_And_Set_SSL_Protocol()
        {
            if (current_connection_ssl_protocol == (int)SslProtocol.Tls13)
            {
                connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls13;
            }
            else
            {
                connection_ssl_protocol = System.Security.Authentication.SslProtocols.Tls12;
            }

            return Task.FromResult(true);
        }










        // CREATE THE SERVER APPLICATION SETTINGS FILE IN JSON FORMAT
        private static async Task<bool> Create_Server_Application_Settings_File()
        {
            bool settings_file_creation_is_successful = false;

            Server_Settings settings = new Server_Settings();



            // LOCKS ARE USED FOR ALL THE VARIABLES ACCESSED BY THIS METHOD BECAUSE THEY CAN BE ACCESSED BY MULTIPLE THREADS.
            // IN ORDER TO AVOID RACE CONDITIONS, THESE OBJECTS MUST BE TAKEN FROM THE HEAP AND LOCKED ON THE STACK UNTIL THE
            // PROCEDURES ARE FINISHED.

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

            await Get_And_Set_SSL_Protocol();


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












        // IF THE SERVER APPLICATION'S SETTINGS FILE EXISTS, THE VARIABLES WILL BE DESERIALIZED FROM
        // JSON FORMAT AND LOADED WITHIN THE APPLICATION'S MEMORY AS VARIABLES.
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


                    // LOCKS ARE USED FOR ALL THE VARIABLES ACCESSED BY THIS METHOD BECAUSE THEY CAN BE ACCESSED BY MULTIPLE THREADS.
                    // IN ORDER TO AVOID RACE CONDITIONS, THESE OBJECTS MUST BE TAKEN FROM THE HEAP AND LOCKED ON THE STACK UNTIL THE
                    // PROCEDURES ARE FINISHED.

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

                    await Get_And_Set_SSL_Protocol();


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















        // WHEN THE SERVER'S SETTINGS ARE UPDATED THE CURRENT SETTINGS FILE IS DELETED AND A NEW SETTINGS
        // FILE IS CREATED WITH THE UPDATED VALUES. THIS IS DONE INSTEAD OF OVERRIDING THE FILE TO AVOID
        // ANY ERRORS THAT COULD BE CAUSED BY MULTIPLE FACTORS.
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












        // SERVER APPLICATION'S FUNCTIONALITIES
        //
        // [ BEGIN ]



        // THE SMTPS SERVER FUNCTIONALITY IS USED TO SEND CODES TO THE USER FOR REGISTRATION AND LOG IN
        // THE SMTPS FUNCTIONALITY IS CONFIGURED TO USE MICROSOFT OR GOOGLE SERVICES, IN ACCORDANCE
        // WITH THE SETTINGS SPECIFIED BY THE SERVER'S ADMINISTRATOR
        protected static async Task<bool> SMTPS_Service(string user_email, string code, string function)
        {
            bool smtps_operation_is_successful = true;


            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            try
            {
                lock (smtps_service_email_address)
                {
                    lock (smtps_service_email_password)
                    {
                        message.From.Add(new MimeKit.MailboxAddress("Omega Drive", smtps_service_email_address));
                        message.To.Add(new MimeKit.MailboxAddress("User", user_email));
                        message.Subject = function + " code";
                        message.Body = new MimeKit.TextPart("plain") { Text = "One time " + function + " code: " + code };


                        string smtp_server = String.Empty;
                        int smtp_server_port = 587;

                        smtps_provider_and_server.TryGetValue(smtps_service_provider_name, out smtp_server);

                        client.Connect(smtp_server, smtp_server_port, MailKit.Security.SecureSocketOptions.StartTls);

                        client.Authenticate(smtps_service_email_address, smtps_service_email_password);
                    }
                }
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

            }
            catch (Exception E)
            {
                smtps_operation_is_successful = false;

                if (client != null)
                {
                    await client.DisconnectAsync(true);
                }

                if (message != null)
                {
                    message.Dispose();
                }
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }

                if (message != null)
                {
                    message.Dispose();
                }
            }

            return smtps_operation_is_successful;
        }



        // [ END ]







        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static async Task<bool> Delete_Expired_Database_Items()
        {
            MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection(("Server=" + my_sql_database_server + ";" + "User ID=" + my_sql_database_username + ";" + "Password=" + my_sql_database_password + ";" + "Database=" + my_sql_database_database_name));

            try
            {
                await connection.OpenAsync();

                MySqlConnector.MySqlCommand delete_expired_accounts_pending_for_validation = new MySqlConnector.MySqlCommand("DELETE FROM accounts_pending_for_validation WHERE account_validation_expiry_date < NOW();", connection);

                try
                {
                    await delete_expired_accounts_pending_for_validation.ExecuteNonQueryAsync();
                }
                catch (Exception E)
                {

                }
                finally
                {
                    if (delete_expired_accounts_pending_for_validation != null)
                    {
                        await delete_expired_accounts_pending_for_validation.DisposeAsync();
                    }
                }




                MySqlConnector.MySqlCommand delete_expired_pending_log_in_sessions = new MySqlConnector.MySqlCommand("DELETE FROM pending_log_in_sessions WHERE log_in_session_request_expiry_date < NOW();", connection);

                try
                {
                    await delete_expired_pending_log_in_sessions.ExecuteNonQueryAsync();
                }
                catch (Exception E)
                {

                }
                finally
                {
                    if (delete_expired_pending_log_in_sessions != null)
                    {
                        await delete_expired_pending_log_in_sessions.DisposeAsync();
                    }
                }




                MySqlConnector.MySqlCommand delete_expired_active_log_in_sessions = new MySqlConnector.MySqlCommand("DELETE FROM active_log_in_sessions WHERE active_log_in_session_expiry_date < NOW();", connection);

                try
                {
                    await delete_expired_active_log_in_sessions.ExecuteNonQueryAsync();
                }
                catch (Exception E)
                {

                }
                finally
                {
                    if (delete_expired_active_log_in_sessions != null)
                    {
                        await delete_expired_active_log_in_sessions.DisposeAsync();
                    }
                }

            }
            catch (Exception E)
            {
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

            return true;
        }
    }
}
