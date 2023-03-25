using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_GUI:Server_Application_Variables
    {
        internal static async void Main_Menu()
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


        internal static void Settings_Menu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                      SETTINGS                       |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  Enter one of the following options:                |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ P  ]  Set the server's port number               |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ PR ]  Set the server's choosen SSL protocol      |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ S  ]  Set the SMTPS service                      |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ SV ]  Set Cloudmersive virus scan                |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ DV ]  Disable Cloudmersive virus scan            |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ SM ]  Set MySQL database connection              |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ G  ]  Generate a x509 ( SSL ) certificate        |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||  [ EX ]  Exit the settings menu                     |||");
            Console.WriteLine("\t\t|||                                                     |||");
            Console.WriteLine("\t\t|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t                [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


















        /// <summary>
        /// 
        /// Port number setup
        /// 
        /// </summary>


        internal static void Port_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                     Port number setup                  |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the port number on which the server will listen |||");
            Console.WriteLine("\t\t|||  for client connections                                |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Port_Setup_Successful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                     Port number setup                  |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SMTPS service settup successful                       |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Port_Setup_Cancelled()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                     Port number setup                  |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SMTPS service settup cancelled                        |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }









        /// <summary>
        /// 
        /// SLL protocol setup
        /// 
        /// </summary>


        internal static void SSL_Protocol_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                     SSL protocol setup                 |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SSL Protocol: " + available_connection_ssl_protocol[current_connection_ssl_protocol] + "                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ N ] Next protocol                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ P ] Previous protocol                               |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }



        internal static void SSL_Protocol_Setup_Finsished()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                     SSL protocol setup                 |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SSL protocol setup finished                           |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

















        /// <summary>
        /// 
        /// SMTPS service setup
        /// 
        /// </summary>



        internal static void SMTPS_Service_Email_Setup()
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

        internal static void SMTPS_Service_Password_Setup()
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

        internal static void SMTPS_Service_Provider_Setup()
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



        internal static void SMTPS_Error_Message()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                         SMTPS                          |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Invalid email address format                         |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void SMTPS_Service_Provider_Setup_Successful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                           SMTPS                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SMTPS service settup successful                       |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void SMTPS_Service_Provider_Setup_Cancelled()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                           SMTPS                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  SMTPS service settup cancelled                        |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }














        /// <summary>
        /// 
        /// Cloudmersive API setup
        /// 
        /// </summary>

        internal static void Cloudmersive_API_Key_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                      Cloudmersive                    |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t|||  Enter the cloudmersive API key. If you do not have  |||");
            Console.WriteLine("\t\t|||  one go to https://cloudmersive.com/virus-api and    |||");
            Console.WriteLine("\t\t|||  create an account and then create an API key.       |||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                        |||");
            Console.WriteLine("\t\t|||                                                      |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Virus Total API key: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void Cloudmersive_API_Key_Setup_Error_Message()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                      Cloudmersive                      |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  The API key inserted is not valid. Enter a valid API  |||");
            Console.WriteLine("\t\t|||  key.                                                  |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void Cloudmersive_API_Key_Setup_Successful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                       Cloudmersive                     |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Cloudmersive API settup successful                    |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void Cloudmersive_API_Key_Setup_Cancelled()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                       Cloudmersive                     |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Cloudmersive API settup cancelled                     |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void Cloudmersive_API_Disabled()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                       Cloudmersive                     |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Cloudmersive virus scan disabled                      |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }










        /// <summary>
        /// 
        /// MySQL setup
        /// 
        /// </summary>

        internal static void MySQL_Username_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                         MySQL                          |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the username of the MySQL database account      |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void MySQL_Password_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                         MySQL                          |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the password of the MySQL database account      |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void MySQL_Server_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                         MySQL                          |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the server name on which the database operates  |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }



        internal static void MySQL_Server_Authentification_Error()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                         MySQL                          |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Invalid credentials or server name                    |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void MySQL_Server_Authentification_Successful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                           MySQL                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  MySQL authentification successful                     |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }










        /// <summary>
        /// 
        /// X509 certificate generation
        /// 
        /// </summary>

        internal static void X509_Certificate_Generation_Password_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                 SERVER X509 CERTIFICATE                |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the choosen certificate password                |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void X509_Certificate_Generation_Valid_Time_Period_Setup()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                 SERVER X509 CERTIFICATE                |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  Enter the choosen valid time period in days           |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ E ] Exit the configuration                          |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void X509_Certificate_Generation_Successful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                 SERVER X509 CERTIFICATE                |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  X509 certificate generated sucessfully and loaded     |||");
            Console.WriteLine("\t\t|||  in the server application's directory.                |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }


        internal static void X509_Certificate_Generation_Unsuccessful()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                 SERVER X509 CERTIFICATE                |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  X509 certificate generation error                     |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                   |||");
            Console.WriteLine("\t\t|||                                                        |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }






        /// <summary>
        /// 
        /// X509 certificate generation
        /// 
        /// </summary>

        internal static void X509_Certificate_Loadup_Error()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\n\n\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                           X509 CERTIFICATE                         |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("\t\t|||                                                                    |||");
            Console.WriteLine("\t\t|||  X509 SSL certificate loadup unsuccessful.                         |||");
            Console.WriteLine("\t\t|||                                                                    |||");
            Console.WriteLine("\t\t|||  Either the ssl certificate is not valid, or it does not exist,    |||");
            Console.WriteLine("\t\t|||  or the password set for the certificate is not valid. If a        |||");
            Console.WriteLine("\t\t|||  certificate already exists in the server application's directory  |||");
            Console.WriteLine("\t\t|||  delete it, go to the settings menu and select the option          |||");
            Console.WriteLine("\t\t|||  [ Generate a x509(SSL) certificate ]. After the certificate is    |||");
            Console.WriteLine("\t\t|||  generated, give this certificate and its password to all          |||");
            Console.WriteLine("\t\t|||  Omega Drive clients that will connect to this server              |||");
            Console.WriteLine("\t\t|||  application to install this certificate on their machines.        |||");
            Console.WriteLine("\t\t|||  This is required in order for both the client and server to       |||");
            Console.WriteLine("\t\t|||  communicate securely.                                             |||");
            Console.WriteLine("\t\t|||                                                                    |||");
            Console.WriteLine("\t\t|||  [ X ] Press any key                                               |||");
            Console.WriteLine("\t\t|||                                                                    |||");
            Console.WriteLine("\t\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");


            Console.Write("\n\n\t\t\t      [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
