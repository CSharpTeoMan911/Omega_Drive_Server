using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class Server_Function_Selector:Server_Application_Variables
    {
        private Payload_Serialization Payload_Serialization = new Payload_Serialization();
        private Authentification_Functions Authentification_Functions = new Authentification_Functions();
        private FTP_Service_Functions  FTP_Service_Functions = new FTP_Service_Functions();




        protected const string register_function_identifier = "Register";
        protected const string account_validation_function_identifier = "Account validation";
        protected const string log_in_function_identifier = "Log in";
        protected const string log_out_function_identifier = "Log out";
        protected const string account_authentification_function_identifier = "Account authentification";
        protected const string verify_log_in_session_key_function_identifier = "Verify log in session key";
        protected const string retrieve_user_files_data_function_identifier = "Retrieve user files data";
        protected const string delete_user_file_function_identifier = "Delete user file";




        internal async Task<byte[]> Server_Function_Selection(Client_WSDL_Payload payload)
        {
            byte[] function_payload = connection_failed_message;

            
            MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection("Server = " + my_sql_database_server +"; User ID = " + my_sql_database_username + "; Password = " + my_sql_database_password + "; Database = " + my_sql_database_database_name);

            
            try
            {
                await connection.OpenAsync();

                switch(payload.Function)
                {
                    case register_function_identifier:
                        function_payload = await Authentification_Functions.Register_Account(connection, payload);
                        break;

                    case account_validation_function_identifier:
                        function_payload = await Authentification_Functions.Validate_Account(connection, payload);
                        break;

                    case log_in_function_identifier:
                        function_payload = await Authentification_Functions.Log_In_Account(connection, payload);
                        break;

                    case log_out_function_identifier:
                        function_payload = await Authentification_Functions.Log_Out_Account(connection, payload);
                        break;

                    case account_authentification_function_identifier:
                        function_payload = await Authentification_Functions.Authentificate_Account(connection, payload);
                        break;

                    case verify_log_in_session_key_function_identifier:
                        function_payload = (await Authentification_Functions.Verify_Log_In_Session_Key(connection, payload)).Item1;
                        break;

                    case retrieve_user_files_data_function_identifier:
                        function_payload = await FTP_Service_Functions.Retrieve_User_Files_Data(connection, payload);
                        break;

                    case delete_user_file_function_identifier:
                        function_payload = await FTP_Service_Functions.Delete_User_File(connection, payload);
                        break;
                }
            }
            catch(Exception E)
            {
                if(connection != null)
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

            return await Payload_Serialization.Serialize_Payload(function_payload);
        }
    }
}
