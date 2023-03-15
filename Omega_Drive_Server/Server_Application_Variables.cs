using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_Variables
    {
        protected static System.Net.Sockets.Socket server_socket;
        protected static System.Timers.Timer server_functionality_timer;

        protected static bool server_opened;
        protected static int port_number = 1024;
        protected static int number_of_clients_backlog = 100;

        protected static System.Security.Cryptography.X509Certificates.X509Certificate2 server_certificate;
    }
}
