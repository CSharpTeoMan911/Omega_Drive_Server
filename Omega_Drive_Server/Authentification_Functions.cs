using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class Authentification_Functions
    {
        internal async Task<byte[]> Register_Account(MySqlConnector.MySqlConnection connection)
        {
            byte[] registration_result = Encoding.UTF8.GetBytes("Connection failed");

            MySqlConnector.MySqlCommand command = new MySqlConnector.MySqlCommand("", connection);

            try
            {

            }
            catch
            {
                
            }
            finally
            {
                if (command != null)
                {
                    await command.DisposeAsync();
                }
            }

            return registration_result;
        }
    }
}
