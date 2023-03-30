using Newtonsoft.Json.Serialization;
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


        private byte[] connection_failed_message = Encoding.UTF8.GetBytes("Connection failed");
        private byte[] email_already_in_use_message = Encoding.UTF8.GetBytes("Email already in use");
        private byte[] code_verification_error_message = Encoding.UTF8.GetBytes("Code verification error");







        internal async Task<byte[]> Register_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {

            byte[] registration_result = connection_failed_message;



            try
            {
                System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);

                string password_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<byte[]>(client_WSDL_Payload.Password___Or___Binary_Content);







            Code_Generation:

                string code = await Server_Cryptographic_Functions.Random_Alphanumeric_Code_Generator();

                string code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(code);







                MySqlConnector.MySqlCommand verify_if_code_exists_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM accounts_pending_for_validation WHERE one_time_account_validation_code = @one_time_account_validation_code;", connection);

                try
                {
                    verify_if_code_exists_command.Parameters.AddWithValue("one_time_account_validation_code", code_hashing_result);

                    MySqlConnector.MySqlDataReader verify_if_code_exists_command_reader = await verify_if_code_exists_command.ExecuteReaderAsync();

                    try
                    {
                        if(await verify_if_code_exists_command_reader.ReadAsync() == true)
                        {

                            if (verify_if_code_exists_command != null)
                            {
                                await verify_if_code_exists_command.DisposeAsync();
                            }

                            if (verify_if_code_exists_command_reader != null)
                            {
                                await verify_if_code_exists_command_reader.CloseAsync();
                                await verify_if_code_exists_command_reader.DisposeAsync();
                            }

                            goto Code_Generation;
                        }
                    }
                    catch (Exception E)
                    {
                        registration_result = code_verification_error_message;
                    }
                    finally
                    {
                        if (verify_if_code_exists_command_reader != null)
                        {
                            await verify_if_code_exists_command_reader.CloseAsync();
                            await verify_if_code_exists_command_reader.DisposeAsync();
                        }
                    }
                }
                catch (Exception E)
                {
                    registration_result = code_verification_error_message;
                }
                finally
                {
                    if(verify_if_code_exists_command != null)
                    {
                        await verify_if_code_exists_command.DisposeAsync();
                    }
                }












                if(Encoding.UTF8.GetString(registration_result) != "Code verification error")
                {
                    if (await SMTPS_Service(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key, code, client_WSDL_Payload.Function) == true)
                    {


                        if (password_hashing_result != "Error occured" && code_hashing_result != "Error occured")
                        {

                            MySqlConnector.MySqlCommand account_insertion_command = new MySqlConnector.MySqlCommand("INSERT INTO user_accounts VALUES(@user_email, @user_password, FALSE);", connection);

                            try
                            {
                                account_insertion_command.Parameters.AddWithValue("user_email", client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                                account_insertion_command.Parameters.AddWithValue("user_password", password_hashing_result);
                                await account_insertion_command.ExecuteNonQueryAsync();
                            }
                            catch (Exception E)
                            {
                                registration_result = email_already_in_use_message;
                            }
                            finally
                            {
                                if (account_insertion_command != null)
                                {
                                    await account_insertion_command.DisposeAsync();
                                }
                            }








                            if (Encoding.UTF8.GetString(registration_result) != "Email already in use")
                            {

                                MySqlConnector.MySqlCommand pending_account_validation_session_insertion_command = new MySqlConnector.MySqlCommand("INSERT INTO accounts_pending_for_validation VALUES(@user_email, @one_time_account_validation_code, NOW() + INTERVAL 2 HOUR);", connection);

                                try
                                {
                                    pending_account_validation_session_insertion_command.Parameters.AddWithValue("user_email", client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                                    pending_account_validation_session_insertion_command.Parameters.AddWithValue("one_time_account_validation_code", code_hashing_result);
                                    await pending_account_validation_session_insertion_command.ExecuteNonQueryAsync();
                                }
                                catch (Exception E)
                                {
                                    registration_result = connection_failed_message;
                                }
                                finally
                                {
                                    if (pending_account_validation_session_insertion_command != null)
                                    {
                                        await pending_account_validation_session_insertion_command.DisposeAsync();
                                    }
                                }

                            }


                        }

                    }
                    else
                    {
                        registration_result = connection_failed_message;
                    }
                }


            }
            catch (Exception E)
            {
                registration_result = Encoding.UTF8.GetBytes("Invalid email address");
            }



            return registration_result;
        }









        internal async Task<byte[]> Validate_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {

            byte[] validation_result = Encoding.UTF8.GetBytes("Connection error");
            string user_email = String.Empty;





            MySqlConnector.MySqlCommand account_validation_code_verification_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM accounts_pending_for_validation WHERE one_time_account_validation_code = @one_time_account_validation_code", connection);

            try
            {
                account_validation_code_verification_command.Parameters.AddWithValue("one_time_account_validation_code", await Server_Cryptographic_Functions.Content_Hasher<string>(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key));

                MySqlConnector.MySqlDataReader account_validation_code_verification_command_reader = await account_validation_code_verification_command.ExecuteReaderAsync();

                try
                {
                    if(await account_validation_code_verification_command_reader.ReadAsync() == true)
                    {
                        user_email = (string)account_validation_code_verification_command_reader["user_email"];
                    }
                }
                catch (Exception E)
                {
                    
                }
                finally
                {
                    if (account_validation_code_verification_command_reader != null)
                    {
                        await account_validation_code_verification_command_reader.CloseAsync();
                        await account_validation_code_verification_command_reader.DisposeAsync();
                    }
                }

            }
            catch (Exception E)
            {

            }
            finally
            {
                if(account_validation_code_verification_command != null)
                {
                    await account_validation_code_verification_command.DisposeAsync();
                }
            }








            MySqlConnector.MySqlCommand remove_pending_account_validation_command = new MySqlConnector.MySqlCommand("DELETE FROM accounts_pending_for_validation WHERE user_email = @user_email;", connection);

            try
            {
                remove_pending_account_validation_command.Parameters.AddWithValue("user_email", user_email);
                await remove_pending_account_validation_command.ExecuteNonQueryAsync();
            }
            catch
            {

            }
            finally
            {
                if(remove_pending_account_validation_command != null)
                {
                    await remove_pending_account_validation_command.DisposeAsync();
                }
            }





            MySqlConnector.MySqlCommand account_validation_command = new MySqlConnector.MySqlCommand("UPDATE user_accounts SET user_account_validated = TRUE WHERE user_email = @user_email;", connection);

            try
            {
                account_validation_command.Parameters.AddWithValue("user_email", user_email);
                await account_validation_command.ExecuteNonQueryAsync();
            }
            catch
            {

            }
            finally
            {
                if (account_validation_command != null)
                {
                    await account_validation_command.DisposeAsync();
                }
            }


            return validation_result;
        }
    }
}
