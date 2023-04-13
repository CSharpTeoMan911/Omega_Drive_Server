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







        internal async Task<byte[]> Register_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {

            byte[] registration_result = connection_failed_message;
            bool email_already_in_use = false;





            if (Encoding.UTF8.GetString(client_WSDL_Payload.Password___Or___Binary_Content).Length >= 6)
            {

                try
                {
                    System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                }
                catch
                {
                    registration_result = invalid_email_address;
                    goto Registration_Error;
                }


                string password_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<byte[]>(client_WSDL_Payload.Password___Or___Binary_Content);









                string code = await Server_Cryptographic_Functions.Random_Alphanumeric_Code_Generator();

                string code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(code);









                if (password_hashing_result != content_hashing_error.ToString() && code_hashing_result != content_hashing_error.ToString())
                {
                    MySqlConnector.MySqlCommand verify_if_code_exists_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM accounts_pending_for_validation WHERE one_time_account_validation_code = @one_time_account_validation_code;", connection);

                    try
                    {
                        verify_if_code_exists_command.Parameters.AddWithValue("one_time_account_validation_code", code_hashing_result);

                        MySqlConnector.MySqlDataReader verify_if_code_exists_command_reader = await verify_if_code_exists_command.ExecuteReaderAsync();

                        try
                        {
                            if (await verify_if_code_exists_command_reader.ReadAsync() == true)
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

                                registration_result = connection_failed_message;
                                goto Registration_Error;
                            }
                        }
                        catch (Exception E)
                        {
                            if (verify_if_code_exists_command_reader != null)
                            {
                                await verify_if_code_exists_command_reader.CloseAsync();
                                await verify_if_code_exists_command_reader.DisposeAsync();
                            }

                            registration_result = connection_failed_message;
                            goto Registration_Error;
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
                        if (verify_if_code_exists_command != null)
                        {
                            await verify_if_code_exists_command.DisposeAsync();
                        }

                        registration_result = connection_failed_message;
                        goto Registration_Error;
                    }
                    finally
                    {
                        if (verify_if_code_exists_command != null)
                        {
                            await verify_if_code_exists_command.DisposeAsync();
                        }
                    }
                }
                else
                {
                    registration_result = connection_failed_message;
                    goto Registration_Error;
                }














                if (await SMTPS_Service(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key, code, "Account validation") == true)
                {


                    if (password_hashing_result != content_hashing_error.ToString() && code_hashing_result != content_hashing_error.ToString())
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
                            if (account_insertion_command != null)
                            {
                                await account_insertion_command.DisposeAsync();
                            }

                            email_already_in_use = true;
                            registration_result = email_already_in_use_message;

                            goto Registration_Error;
                        }
                        finally
                        {
                            if (account_insertion_command != null)
                            {
                                await account_insertion_command.DisposeAsync();
                            }
                        }








                        if (email_already_in_use == true)
                        {
                            MySqlConnector.MySqlCommand pending_account_validation_session_insertion_command = new MySqlConnector.MySqlCommand("INSERT INTO accounts_pending_for_validation VALUES(@user_email, @one_time_account_validation_code, NOW() + INTERVAL 2 HOUR);", connection);

                            try
                            {
                                pending_account_validation_session_insertion_command.Parameters.AddWithValue("user_email", client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                                pending_account_validation_session_insertion_command.Parameters.AddWithValue("one_time_account_validation_code", code_hashing_result);
                                await pending_account_validation_session_insertion_command.ExecuteNonQueryAsync();

                                registration_result = account_registration_successful;
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
            else
            {
                registration_result = invalid_password_length;
            }


        Registration_Error:
            return registration_result;
        }









        internal async Task<byte[]> Validate_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {

            byte[] validation_result = connection_failed_message;

            string account_validation_code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
            string user_email = String.Empty;




            
            if(account_validation_code_hashing_result != content_hashing_error.ToString())
            {
                MySqlConnector.MySqlCommand account_validation_code_verification_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM accounts_pending_for_validation WHERE one_time_account_validation_code = @one_time_account_validation_code", connection);

                try
                {
                    account_validation_code_verification_command.Parameters.AddWithValue("one_time_account_validation_code", account_validation_code_hashing_result);

                    MySqlConnector.MySqlDataReader account_validation_code_verification_command_reader = await account_validation_code_verification_command.ExecuteReaderAsync();

                    try
                    {
                        if (await account_validation_code_verification_command_reader.ReadAsync() == true)
                        {
                            user_email = (string)account_validation_code_verification_command_reader["user_email"];
                        }
                        else
                        {
                            validation_result = invalid_account_validation_code;

                            if (account_validation_code_verification_command_reader != null)
                            {
                                await account_validation_code_verification_command_reader.CloseAsync();
                                await account_validation_code_verification_command_reader.DisposeAsync();
                            }

                            goto Account_Validation_Error;
                        }
                    }
                    catch (Exception E)
                    {
                        if (account_validation_code_verification_command_reader != null)
                        {
                            await account_validation_code_verification_command_reader.CloseAsync();
                            await account_validation_code_verification_command_reader.DisposeAsync();
                        }

                        goto Account_Validation_Error;
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
                    if (account_validation_code_verification_command != null)
                    {
                        await account_validation_code_verification_command.DisposeAsync();
                    }

                    goto Account_Validation_Error;
                }
                finally
                {
                    if (account_validation_code_verification_command != null)
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
                    if (remove_pending_account_validation_command != null)
                    {
                        await remove_pending_account_validation_command.DisposeAsync();
                    }

                    goto Account_Validation_Error;
                }
                finally
                {
                    if (remove_pending_account_validation_command != null)
                    {
                        await remove_pending_account_validation_command.DisposeAsync();
                    }
                }





                MySqlConnector.MySqlCommand account_validation_command = new MySqlConnector.MySqlCommand("UPDATE user_accounts SET user_account_validated = TRUE WHERE user_email = @user_email;", connection);

                try
                {
                    account_validation_command.Parameters.AddWithValue("user_email", user_email);
                    await account_validation_command.ExecuteNonQueryAsync();

                    validation_result = account_validation_successful;
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
            }


        Account_Validation_Error:
            return validation_result;
        }












        internal async Task<byte[]> Log_In_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            byte[] log_in_account_result = connection_failed_message;

            string password_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<byte[]>(client_WSDL_Payload.Password___Or___Binary_Content);

            if (password_hashing_result != content_hashing_error.ToString())
            {
                MySqlConnector.MySqlCommand verify_user_credentials_command = new MySqlConnector.MySqlCommand("SELECT user_email, user_account_validated FROM user_accounts WHERE user_password = @user_password;", connection);

                try
                {
                    verify_user_credentials_command.Parameters.AddWithValue("user_password", password_hashing_result);

                    MySqlConnector.MySqlDataReader verify_user_credentials_command_reader = await verify_user_credentials_command.ExecuteReaderAsync();

                    try
                    {
                        if (await verify_user_credentials_command_reader.ReadAsync() == true)
                        {


                            if ((string)verify_user_credentials_command_reader["user_email"] == client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key)
                            {


                                if ((bool)verify_user_credentials_command_reader["user_account_validated"] == false)
                                {


                                    if (verify_user_credentials_command != null)
                                    {
                                        await verify_user_credentials_command.DisposeAsync();
                                    }

                                    if (verify_user_credentials_command_reader != null)
                                    {
                                        await verify_user_credentials_command_reader.CloseAsync();
                                        await verify_user_credentials_command_reader.DisposeAsync();
                                    }

                                    log_in_account_result = account_not_validated;
                                    goto Log_In_Error;
                                }


                            }
                            else
                            {


                                if (verify_user_credentials_command != null)
                                {
                                    await verify_user_credentials_command.DisposeAsync();
                                }

                                if (verify_user_credentials_command_reader != null)
                                {
                                    await verify_user_credentials_command_reader.CloseAsync();
                                    await verify_user_credentials_command_reader.DisposeAsync();
                                }

                                log_in_account_result = invalid_email_address;
                                goto Log_In_Error;


                            }
                        }
                        else
                        {


                            if (verify_user_credentials_command != null)
                            {
                                await verify_user_credentials_command.DisposeAsync();
                            }

                            if (verify_user_credentials_command_reader != null)
                            {
                                await verify_user_credentials_command_reader.CloseAsync();
                                await verify_user_credentials_command_reader.DisposeAsync();
                            }

                            log_in_account_result = invalid_password;
                            goto Log_In_Error;


                        }
                    }
                    catch (Exception E)
                    {
                        if (verify_user_credentials_command_reader != null)
                        {
                            await verify_user_credentials_command_reader.CloseAsync();
                            await verify_user_credentials_command_reader.DisposeAsync();
                        }

                        log_in_account_result = connection_failed_message;
                        goto Log_In_Error;


                    }
                    finally
                    {


                        if (verify_user_credentials_command_reader != null)
                        {
                            await verify_user_credentials_command_reader.CloseAsync();
                            await verify_user_credentials_command_reader.DisposeAsync();
                        }


                    }

                }
                catch (Exception E)
                {
                    if (verify_user_credentials_command != null)
                    {
                        await verify_user_credentials_command.DisposeAsync();
                    }

                    log_in_account_result = connection_failed_message;
                    goto Log_In_Error;


                }
                finally
                {


                    if (verify_user_credentials_command != null)
                    {
                        await verify_user_credentials_command.DisposeAsync();
                    }


                }















                string code = await Server_Cryptographic_Functions.Random_Alphanumeric_Code_Generator();
                string code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(code);


                if (code_hashing_result != content_hashing_error.ToString())
                {
                    if (await SMTPS_Service(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key, code, client_WSDL_Payload.Function) == true)
                    {
                        MySqlConnector.MySqlCommand delete_pending_account_validation_session = new MySqlConnector.MySqlCommand("INSERT INTO pending_log_in_sessions VALUES(@one_time_log_in_session_code, @user_email, NOW() + INTERVAL 5 MINUTE)", connection);


                        try
                        {
                            delete_pending_account_validation_session.Parameters.AddWithValue("one_time_log_in_session_code", code_hashing_result);
                            delete_pending_account_validation_session.Parameters.AddWithValue("user_email", client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);
                            await delete_pending_account_validation_session.ExecuteNonQueryAsync();

                            log_in_account_result = login_successful;
                        }
                        catch (Exception E)
                        {
                            log_in_account_result = connection_failed_message;
                        }
                        finally
                        {
                            if (delete_pending_account_validation_session != null)
                            {
                                await delete_pending_account_validation_session.DisposeAsync();
                            }
                        }
                    }
                }
                else
                {
                    log_in_account_result = connection_failed_message;
                }
            }

        Log_In_Error:
            return log_in_account_result;
        }








        internal async Task<byte[]> Authentificate_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            byte[] authentificate_account_result = connection_failed_message;

            string user_email = String.Empty;
            string authentification_code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);


            if(authentification_code_hashing_result != content_hashing_error.ToString())
            {
                MySqlConnector.MySqlCommand verify_log_in_code_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM pending_log_in_sessions WHERE one_time_log_in_session_code = @one_time_log_in_session_code;", connection);

                try
                {
                    verify_log_in_code_command.Parameters.AddWithValue("one_time_log_in_session_code", authentification_code_hashing_result);

                    MySqlConnector.MySqlDataReader verify_log_in_code_command_reader = await verify_log_in_code_command.ExecuteReaderAsync();

                    try
                    {

                        if (await verify_log_in_code_command_reader.ReadAsync() == true)
                        {
                            user_email = (string)verify_log_in_code_command_reader["user_email"];
                        }
                        else
                        {
                            if (verify_log_in_code_command_reader != null)
                            {
                                await verify_log_in_code_command_reader.DisposeAsync();
                            }

                            if (verify_log_in_code_command != null)
                            {
                                await verify_log_in_code_command.DisposeAsync();
                            }

                            authentificate_account_result = invalid_log_in_code;

                            goto Authentification_Error;
                        }
                    }
                    catch (Exception E)
                    {
                        if (verify_log_in_code_command_reader != null)
                        {
                            await verify_log_in_code_command_reader.DisposeAsync();
                        }

                        if (verify_log_in_code_command != null)
                        {
                            await verify_log_in_code_command.DisposeAsync();
                        }

                        authentificate_account_result = invalid_log_in_code;

                        goto Authentification_Error;
                    }
                    finally
                    {
                        if (verify_log_in_code_command_reader != null)
                        {
                            await verify_log_in_code_command_reader.DisposeAsync();
                        }
                    }

                }
                catch (Exception E)
                {
                    if (verify_log_in_code_command != null)
                    {
                        await verify_log_in_code_command.DisposeAsync();
                    }

                    authentificate_account_result = invalid_log_in_code;

                    goto Authentification_Error;
                }
                finally
                {
                    if (verify_log_in_code_command != null)
                    {
                        await verify_log_in_code_command.DisposeAsync();
                    }
                }





                MySqlConnector.MySqlCommand authentificate_user_command = new MySqlConnector.MySqlCommand("DELETE FROM pending_log_in_sessions WHERE one_time_log_in_session_code = @one_time_log_in_session_code;", connection);

                try
                {
                    authentificate_user_command.Parameters.AddWithValue("one_time_log_in_session_code", authentification_code_hashing_result);
                    await authentificate_user_command.ExecuteNonQueryAsync();

                    authentificate_account_result = login_successful;
                }
                catch
                {
                    if (authentificate_user_command != null)
                    {
                        await authentificate_user_command.DisposeAsync();
                    }

                    authentificate_account_result = invalid_log_in_code;

                    goto Authentification_Error;
                }
                finally
                {
                    if (authentificate_user_command != null)
                    {
                        await authentificate_user_command.DisposeAsync();
                    }
                }



                string code = await Server_Cryptographic_Functions.Random_Alphanumeric_Code_Generator();
                string code_hashing_result = await Server_Cryptographic_Functions.Content_Hasher<string>(code);


                if (code_hashing_result != content_hashing_error.ToString())
                {
                    MySqlConnector.MySqlCommand log_in_key_insertion_command = new MySqlConnector.MySqlCommand("INSERT INTO active_log_in_sessions VALUES(@log_in_session_key, @user_email, NOW() + INTERVAL 42 HOUR);", connection);

                    try
                    {
                        log_in_key_insertion_command.Parameters.AddWithValue("log_in_session_key", code_hashing_result);
                        log_in_key_insertion_command.Parameters.AddWithValue("user_email", user_email);
                        await log_in_key_insertion_command.ExecuteNonQueryAsync();

                        authentificate_account_result = Encoding.UTF8.GetBytes(code);
                    }
                    catch
                    {
                        authentificate_account_result = invalid_log_in_code;

                        if (log_in_key_insertion_command != null)
                        {
                            await log_in_key_insertion_command.DisposeAsync();
                        }
                    }
                    finally
                    {
                        if (log_in_key_insertion_command != null)
                        {
                            await log_in_key_insertion_command.DisposeAsync();
                        }
                    }
                }
            }

        Authentification_Error:
            return authentificate_account_result;
        }




        internal async Task<Tuple<byte[], string>> Verify_Log_In_Session_Key(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            string user_email = String.Empty;

            byte[] log_in_session_key_verification_result = connection_failed_message;

            string log_in_session_key_hashing__result = await Server_Cryptographic_Functions.Content_Hasher<string>(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);


            if(log_in_session_key_hashing__result != content_hashing_error.ToString())
            {
                MySqlConnector.MySqlCommand verify_if_log_in_session_key_is_valid_command = new MySqlConnector.MySqlCommand("SELECT user_email FROM active_log_in_sessions WHERE log_in_session_key = @log_in_session_key;", connection);

                try
                {
                    verify_if_log_in_session_key_is_valid_command.Parameters.AddWithValue("log_in_session_key", log_in_session_key_hashing__result);

                    MySqlConnector.MySqlDataReader verify_if_log_in_session_key_is_valid_command_reader = await verify_if_log_in_session_key_is_valid_command.ExecuteReaderAsync();

                    try
                    {
                        if (await verify_if_log_in_session_key_is_valid_command_reader.ReadAsync() == true)
                        {
                            user_email = (string)verify_if_log_in_session_key_is_valid_command_reader["user_email"];
                            log_in_session_key_verification_result = log_in_session_key_valid;
                        }
                        else
                        {
                            log_in_session_key_verification_result = log_in_session_key_invalid;
                        }
                    }
                    catch (Exception E)
                    {
                        log_in_session_key_verification_result = connection_failed_message;
                    }
                    finally
                    {
                        if (verify_if_log_in_session_key_is_valid_command_reader != null)
                        {
                            await verify_if_log_in_session_key_is_valid_command_reader.DisposeAsync();
                        }
                    }

                }
                catch (Exception E)
                {
                    log_in_session_key_verification_result = connection_failed_message;
                }
                finally
                {
                    if (verify_if_log_in_session_key_is_valid_command != null)
                    {
                        await verify_if_log_in_session_key_is_valid_command.DisposeAsync();
                    }
                }
            }

            return new Tuple<byte[], string>(log_in_session_key_verification_result, user_email);
        }



        internal async Task<byte[]> Log_Out_Account(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            byte[] log_out_account_result = connection_failed_message;

            string log_in_session_key_hashing__result = await Server_Cryptographic_Functions.Content_Hasher<string>(client_WSDL_Payload.Email___Or___Log_In_Session_Key___Or___Account_Validation_Key);

            if(log_in_session_key_hashing__result != content_hashing_error.ToString())
            {
                MySqlConnector.MySqlCommand log_out_account = new MySqlConnector.MySqlCommand("DELETE FROM active_log_in_sessions WHERE log_in_session_key = @log_in_session_key;", connection);

                try
                {
                    log_out_account.Parameters.AddWithValue("log_in_session_key", log_in_session_key_hashing__result);
                    await log_out_account.ExecuteNonQueryAsync();

                    log_out_account_result = log_out_successful;
                }
                catch
                {

                }
                finally
                {
                    if (log_out_account != null)
                    {
                        await log_out_account.DisposeAsync();
                    }
                }
            }

            return log_out_account_result;
        }
    }
}
