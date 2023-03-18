using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    internal class Server_Settings
    {
        public int port_number;
        public int ssl_protocol_index;
        public string ssl_certificate_password;
        public string smtps_service_provider;
        public string smtps_sevice_email;
        public string smtps_service_password;
        public bool cloudmersive_scan_enabled;
        public string cloudmersive_api_key;
        public string my_sql_username;
        public string my_sql_password;
        public string my_sql_server;
        public string my_sql_database;
    }
}
