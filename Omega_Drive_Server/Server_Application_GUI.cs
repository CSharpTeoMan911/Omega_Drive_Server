using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Drive_Server
{
    class Server_Application_GUI:Server_Application_Variables
    {
        public static string Main_Menu()
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
            Console.Write("\t\t :-----------:|||\t|| Enter [ S ] to start or stop the server, [ O ] to open the ||\n");
            Console.Write("\t\t :===========:|||\t|| options menu, or [ E ] to exit.                            ||\n");
            Console.Write("\t\t :===========:|||\t||                                                            ||\n");
            Console.Write("\t\t :===========:|||\t||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\n");
            Console.Write("\t\t !!!!!!!!!!!!!|| \n");
            Console.Write("\t\t !!!!!!!!!!!!!/  \n\n");

            Console.Write("\n\n\t\t\t                [ - ] Input: ");

            Console.ForegroundColor = ConsoleColor.White;

            string input = Console.ReadLine();
            string return_value = String.Empty;


            if (input == "S")
            {
                if (server_opened == false)
                {
                    server_opened = true;
                    return_value = "Start";
                }
                else
                {
                    server_opened = false;
                    return_value = "Stop";
                }
            }
            else if (input == "O")
            {
                return_value = input;
            }
            else if (input == "E")
            {
                return_value = input;
            }
            else
            {
                Main_Menu();
            }

            return return_value;
        }
    }
}
