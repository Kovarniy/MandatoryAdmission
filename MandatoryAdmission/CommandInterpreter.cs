using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandatoryAdmission
{
    class CommandInterpreter
    {
        private Profile userProfile;
        public CommandInterpreter(Profile userProfile)
        {
            this.userProfile = userProfile;
            Console.WriteLine($"Welcome, {userProfile.userName}! You have level {userProfile.accessLevel}.");
            Console.WriteLine("Create file - create filename newtext");
            Console.WriteLine("Delete file - delete filename");
            Console.WriteLine("Change file - change filename newtext");
            Console.WriteLine("Read file - read filename");
        }

        public static string[] SplitComand(string commandLine)
        => commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        public static bool isExit(string command) => command == "exit" ? true : false;

        private bool isValidCommand(string commandLine)
        {
            string[] splitCommand = SplitComand(commandLine);

            if (splitCommand.Length == 2)
            {
                if (splitCommand[0] == "read" || splitCommand[0] == "delete")
                {
                    return true;
                }
            } else if (splitCommand.Length == 3) {
                if (splitCommand[0] == "create" || splitCommand[0] == "change")
                {
                    return true;
                }
            }

           return false;
        }

        public void WriteCommand()
        {
            bool validateResult;
            string command;
            FileWorker fileWorker = new (userProfile.accessLevel);

            do
            {
                command = Console.ReadLine();
                command.Trim();
                if (isExit(command)) return;
                validateResult = isValidCommand(command);
            } while (!validateResult);

            string[] splitCommand = SplitComand(command);
            switch (splitCommand[0])
            {
                case "create":
                    fileWorker.CreateFile(splitCommand[1], splitCommand[2]);
                    break;
                case "delete":
                    fileWorker.DeleteFile(splitCommand[1]);
                    break;
                case "change":
                    fileWorker.CnageFile(splitCommand[1], splitCommand[2]);
                    break;
                case "read":
                    fileWorker.ReadFile(splitCommand[1]);
                    break;
            }

        }
    }
}
