using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_GUI:Server_Application_Variables
    {
        public static async void Main_Menu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.Write("\n\n\n\t\t" + @"    ////////////|" + "\n");
            Console.Write("\t\t" + @"  /////////////||" + "\n");
            Console.Write("\t\t !!!!!!!!!!!!!|||\n");
            Console.Write("\t\t :===========:|||\n");
            Console.Write("\t\t :===========:|||\t\n");
            Console.Write("\t\t :===========:|||\t\n");
            Console.Write("\t\t :-----------:|||\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\n");
            Console.Write("\t\t :[[[[[[[[[");

            if (server_opened == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.Write("O");

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.Write("]:|||\t||                                                            ||\n");
            Console.Write("\t\t :-----------:|||\t|| Enter [ S ] to start or stop the server, [ ST ] to open    ||\n");
            Console.Write("\t\t :===========:|||\t|| the settings menu, or [ E ] to exit.                       ||\n");
            Console.Write("\t\t :===========:|||\t||                                                            ||\n");
            Console.Write("\t\t :===========:|||\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\n");
            Console.Write("\t\t !!!!!!!!!!!!!|| \n");
            Console.Write("\t\t !!!!!!!!!!!!!/  \n\n");

            Console.Write("\n\n\t\t\t                [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        
        public static void Settings_Menu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                      SETTINGS                       |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  Enter one of the following options:                |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ S ]  Set the SMTPS service                       |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ EV ]  Enable Cloudmersive virus scan             |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ DV ]  Disable Cloudmersive virus scan            |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ SV ]  Set Cloudmersive API key                   |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ SM ]  Set MySQL database connection              |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ G ]  Generate a x509 ( SSL ) certificate         |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ EX ]  Exit the settings menu                     |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t                [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }










        public static void SMTPS_Service_Email_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                       SMTPS                         |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                       |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t     [ - ] Email account address: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void SMTPS_Service_Password_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                       SMTPS                         |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                       |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Email account password: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void SMTPS_Service_Provider_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                        SMTPS                         |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t|||  [ - ]  SMTPS service provider [ Google, Microsoft ] |||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                        |||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] SMTPS service provider: ");

            Console.ForegroundColor = ConsoleColor.White;
        }



        public static void Virus_Total_API_Key_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                      Cloudmersive                    |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                        |||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Virus Total API key: ");

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
