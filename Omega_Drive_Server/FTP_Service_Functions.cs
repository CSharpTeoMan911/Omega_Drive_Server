﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class FTP_Service_Functions:Server_Application_Variables
    {
        private Authentification_Functions Authentification_Functions = new Authentification_Functions();





        internal async Task<byte[]> Retrieve_User_Files_Data(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            Tuple<byte[], string> log_in_session_key_verification_result = await Authentification_Functions.Verify_Log_In_Session_Key(connection, client_WSDL_Payload);

            byte[] retrieve_user_files_data_result = log_in_session_key_verification_result.Item1;




            if(retrieve_user_files_data_result == log_in_session_key_valid)
            {
                MySqlConnector.MySqlCommand user_files_data_retrieval_command = new MySqlConnector.MySqlCommand("SELECT file_id, file_name, file_size, file_upload_date, is_directory FROM user_files_data WHERE user_email = @user_email;", connection);

                try
                {
                    user_files_data_retrieval_command.Parameters.AddWithValue("user_email", log_in_session_key_verification_result.Item2);

                    MySqlConnector.MySqlDataReader user_files_data_retrieval_command_reader = await user_files_data_retrieval_command.ExecuteReaderAsync();

                    List<long> file_id_info = new List<long>();
                    List<byte[]> file_name_info = new List<byte[]>();
                    List<int> file_size_info = new List<int>();
                    List<byte[]> file_upload_date_info = new List<byte[]>();
                    List<bool> is_directory_info = new List<bool>();


                    try
                    {
                        while(await user_files_data_retrieval_command_reader.ReadAsync() == true)
                        {
                            file_id_info.Add((long)user_files_data_retrieval_command_reader["file_id"]);
                            file_name_info.Add(Encoding.UTF8.GetBytes((string)user_files_data_retrieval_command_reader["file_name"]));
                            file_size_info.Add((int)user_files_data_retrieval_command_reader["file_size"]);
                            file_upload_date_info.Add(Encoding.UTF8.GetBytes(((DateTime)user_files_data_retrieval_command_reader["file_upload_date"]).ToString()));
                            is_directory_info.Add((bool)user_files_data_retrieval_command_reader["is_directory"]);
                        }

                        try
                        {
                            User_Files_Info user_Files_Info = new User_Files_Info();

                            user_Files_Info.FILE_IDS = file_id_info.ToArray();
                            user_Files_Info.FILE_NAMES = file_name_info.ToArray();
                            user_Files_Info.FILE_SIZES = file_size_info.ToArray();
                            user_Files_Info.FILE_UPLOAD_DATES = file_upload_date_info.ToArray();
                            user_Files_Info.IS_DIRECTORY = is_directory_info.ToArray();

                            retrieve_user_files_data_result = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(user_Files_Info));
                        }
                        catch(Exception E)
                        {
                            retrieve_user_files_data_result = connection_failed_message;
                        }
                    }
                    catch(Exception E)
                    {
                        System.Diagnostics.Debug.WriteLine(E.Message);
                        retrieve_user_files_data_result = connection_failed_message;
                    }
                    finally
                    {
                        if(user_files_data_retrieval_command_reader != null)
                        {
                            await user_files_data_retrieval_command_reader.DisposeAsync();
                        }
                    }
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine(E.Message);
                    retrieve_user_files_data_result = connection_failed_message;
                }
                finally
                {
                    if(user_files_data_retrieval_command != null)
                    {
                        await user_files_data_retrieval_command.DisposeAsync();
                    }
                }
            }

            return retrieve_user_files_data_result;
        }






        internal async Task<byte[]> Delete_User_File(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            Tuple<byte[], string> log_in_session_key_verification_result = await Authentification_Functions.Verify_Log_In_Session_Key(connection, client_WSDL_Payload);

            byte[] delete_user_file_result = log_in_session_key_verification_result.Item1;



            if(log_in_session_key_verification_result.Item1 == log_in_session_key_valid)
            {
                MySqlConnector.MySqlCommand file_deletion_command = new MySqlConnector.MySqlCommand("DELETE FROM users_files WHERE file_id = @file_id;", connection);

                try
                {
                    file_deletion_command.Parameters.AddWithValue("file_id", Encoding.UTF8.GetString(client_WSDL_Payload.Password___Or___Binary_Content));
                    await file_deletion_command.ExecuteNonQueryAsync();
                }
                catch(Exception E)
                {
                    delete_user_file_result = connection_failed_message;
                }
                finally
                {
                    if (file_deletion_command != null)
                    {
                        await file_deletion_command.DisposeAsync();
                    }
                }
            }

            return delete_user_file_result;
        }



        internal async Task<byte[]> Download_User_File(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            Tuple<byte[], string> log_in_session_key_verification_result = await Authentification_Functions.Verify_Log_In_Session_Key(connection, client_WSDL_Payload);

            byte[] download_user_file_result = log_in_session_key_verification_result.Item1;



            if(log_in_session_key_verification_result.Item1 == log_in_session_key_valid)
            {
                MySqlConnector.MySqlCommand download_user_file_command = new MySqlConnector.MySqlCommand("SELECT file_binaries FROM ftp_service_file_system WHERE file_id = @file_id;", connection);

                try
                {
                    download_user_file_command.Parameters.AddWithValue("file_id", Encoding.UTF8.GetString(client_WSDL_Payload.Password___Or___Binary_Content));

                    MySqlConnector.MySqlDataReader download_user_file_command_reader = await download_user_file_command.ExecuteReaderAsync();

                    try
                    {
                        if (await download_user_file_command_reader.ReadAsync() == true)
                        {
                            download_user_file_result = (byte[])download_user_file_command_reader["file_binaries"];
                        }
                        else
                        {
                            download_user_file_result = connection_failed_message;
                        }
                    }
                    catch (Exception E)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + E.Message);
                        download_user_file_result = connection_failed_message;
                    }
                    finally
                    {
                        if (download_user_file_command_reader != null)
                        {
                            await download_user_file_command_reader.DisposeAsync();
                        }
                    }
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + E.Message);
                    download_user_file_result = connection_failed_message;
                }
                finally
                {
                    if (download_user_file_command != null)
                    {
                        await download_user_file_command.DisposeAsync();
                    }
                }
            }

            return download_user_file_result;
        }



        internal async Task<byte[]> Upload_User_File(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            Tuple<byte[], string> log_in_session_key_verification_result = await Authentification_Functions.Verify_Log_In_Session_Key(connection, client_WSDL_Payload);

            byte[] upload_user_file_result = log_in_session_key_verification_result.Item1;

            long user_files_count = 0;


            if (log_in_session_key_verification_result.Item1 == log_in_session_key_valid)
            {
                MySqlConnector.MySqlCommand user_files_count_command = new MySqlConnector.MySqlCommand("SELECT COUNT(*) FROM users_files AS Temporary_Derived_Table;", connection);

                try
                {
                    MySqlConnector.MySqlDataReader user_files_count_command_reader = await user_files_count_command.ExecuteReaderAsync();

                    try
                    {
                        if(await user_files_count_command_reader.ReadAsync() == true)
                        {
                            user_files_count = (long)user_files_count_command_reader["COUNT(*)"];
                        }
                    }
                    catch(Exception E)
                    {
                        if (user_files_count_command_reader != null)
                        {
                            await user_files_count_command_reader.DisposeAsync();
                        }

                        if (user_files_count_command != null)
                        {
                            await user_files_count_command.DisposeAsync();
                        }

                        upload_user_file_result = connection_failed_message;

                        System.Diagnostics.Debug.WriteLine("Error: " + E.Message);

                        goto User_File_Upload_Failed;
                    }
                    finally
                    {
                        if (user_files_count_command_reader != null)
                        {
                            await user_files_count_command_reader.DisposeAsync();
                        }
                    }
                }
                catch
                {
                    if (user_files_count_command != null)
                    {
                        await user_files_count_command.DisposeAsync();
                    }

                    upload_user_file_result = connection_failed_message;

                    goto User_File_Upload_Failed;
                }
                finally
                {
                    if (user_files_count_command != null)
                    {
                        await user_files_count_command.DisposeAsync();
                    }
                }


          

                MySqlConnector.MySqlCommand user_file_upload_insert_user_files_command = new MySqlConnector.MySqlCommand("INSERT INTO users_files VALUES(@file_id + 1, @user_email);", connection);

                try
                {
                    user_file_upload_insert_user_files_command.Parameters.AddWithValue("file_id", user_files_count);
                    user_file_upload_insert_user_files_command.Parameters.AddWithValue("user_email", log_in_session_key_verification_result.Item2);

                    await user_file_upload_insert_user_files_command.ExecuteNonQueryAsync();
                }
                catch (Exception E)
                {
                    upload_user_file_result = connection_failed_message;

                    if (user_file_upload_insert_user_files_command != null)
                    {
                        await user_file_upload_insert_user_files_command.DisposeAsync();
                    }

                   

                    goto User_File_Upload_Failed;
                }
                finally
                {
                    if (user_file_upload_insert_user_files_command != null)
                    {
                        await user_file_upload_insert_user_files_command.DisposeAsync();
                    }
                }
                

                


                MySqlConnector.MySqlCommand user_file_upload_insert_ftp_service_file_system_command = new MySqlConnector.MySqlCommand("INSERT INTO ftp_service_file_system VALUES(@file_id + 1, @file_name, @file_size, NOW(), @file_binaries, FALSE);", connection);

                try
                {

                    User_File user_file = Newtonsoft.Json.JsonConvert.DeserializeObject<User_File>(Encoding.UTF8.GetString(client_WSDL_Payload.Password___Or___Binary_Content));


                    user_file_upload_insert_ftp_service_file_system_command.Parameters.AddWithValue("file_id", user_files_count);
                    user_file_upload_insert_ftp_service_file_system_command.Parameters.AddWithValue("file_name", user_file.File_Name);
                    user_file_upload_insert_ftp_service_file_system_command.Parameters.AddWithValue("file_size", user_file.File_Size);
                    user_file_upload_insert_ftp_service_file_system_command.Parameters.AddWithValue("file_binaries", user_file.File_Binaries);

                    await user_file_upload_insert_ftp_service_file_system_command.ExecuteNonQueryAsync();
                }
                catch (Exception E)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + E.Message);
                    upload_user_file_result = connection_failed_message;
                }
                finally
                {
                    if (user_file_upload_insert_ftp_service_file_system_command != null)
                    {
                        await user_file_upload_insert_ftp_service_file_system_command.DisposeAsync();
                    }
                }
                
            }


        User_File_Upload_Failed:
            return upload_user_file_result;
        }
    }
}
