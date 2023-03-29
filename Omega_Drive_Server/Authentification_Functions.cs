using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class Authentification_Functions:Server_Application_Variables
    {

        private Server_Cryptographic_Functions Server_Cryptographic_Functions = new Server_Cryptographic_Functions();



        internal async Task<byte[]> Register_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            byte[] registration_result = Encoding.UTF8.GetBytes("Connection failed");

            MySqlConnector.MySqlCommand user_email_selection_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM user_accounts WHERE user_email = @user_email;", connection);

            try
            {
                user_email_selection_command.Parameters.AddWithValue("user_email", client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                

                MySqlConnector.MySqlDataReader user_email_selection_command_reader = await user_email_selection_command.ExecuteReaderAsync();

                try
                {
                    if (await user_email_selection_command_reader.ReadAsync() == false)
                    {
                       

                    }
                    else
                    {
                        registration_result = Encoding.UTF8.GetBytes("Email already in use");
                    }
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + E.InnerException.Message);
                    if (user_email_selection_command_reader != null)
                    {
                        await user_email_selection_command_reader.CloseAsync();
                    }
                }
                finally
                {
                    if(user_email_selection_command_reader != null)
                    {
                        await user_email_selection_command_reader.CloseAsync();
                        await user_email_selection_command_reader.DisposeAsync();
                    }
                }
            }
            catch (Exception E)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + E.Message);
            }
            finally
            {
                if (user_email_selection_command != null)
                {
                    await user_email_selection_command.DisposeAsync();
                }
            }

            return registration_result;
        }
    }
}
