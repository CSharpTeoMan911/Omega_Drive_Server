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


        internal async Task<byte[]> Server_Function_Selection(Client_WSDL_Payload payload)
        {
            byte[] function_payload = Encoding.UTF8.GetBytes("Connection error");

            
            MySqlConnector.MySqlConnection connection = new MySqlConnector.MySqlConnection("Server = " + my_sql_database_server +"; User ID = " + my_sql_database_username + "; Password = " + my_sql_database_password + "; Database = " + my_sql_database_database_name);

            
            try
            {
                await connection.OpenAsync();

                switch(payload.Function)
                {
                    case "Register":
                        function_payload = await Authentification_Functions.Register_Account(connection, payload);
                        break;

                    case "Account validation":
                        function_payload = await Authentification_Functions.Validate_Account(connection, payload);
                        break;

                    case "Log in":
                        function_payload = await Authentification_Functions.Log_In_Account(connection, payload);
                        break;

                    case "Account authentification":
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
