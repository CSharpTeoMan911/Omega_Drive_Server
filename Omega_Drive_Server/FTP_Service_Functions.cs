using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class FTP_Service_Functions:Server_Application_Variables
    {
        internal async Task<byte[]> Retrieve_User_Files_Data(MySqlConnector.MySqlConnection connection, Client_WSDL_Payload client_WSDL_Payload)
        {
            byte[] retrieve_user_files_data_result = connection_failed_message;

            /*
             * 	file_id INT NOT NULL,
    file_name VARCHAR(254) NOT NULL,
    file_size INT NOT NULL,
    file_upload_date DATETIME NOT NULL,
             */
            MySqlConnector.MySqlCommand user_files_data_retrieval_command = new MySqlConnector.MySqlCommand("SELECT * FROM WHERE user_files_data WHERE user_email = @user_email;", connection);

            return retrieve_user_files_data_result;
        }
    }
}
