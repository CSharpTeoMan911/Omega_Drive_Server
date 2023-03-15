using System;

namespace Omega_Drive_Server
{
    class Program
    {
        static void Main(string[] args)
        {



            Server_Cryptographic_Functions.Create_X509_Server_Certificate("OMEGA", 365);
        }
    }
}
