using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MandatoryAdmission
{
    class Profile
    {

        public string userName { get; set; }

        public int accessLevel { get; set; }

        public Profile()
        {
            this.userName = "undifiend";
            this.accessLevel = -1;
        }

        public Profile(string userName, int accessLevel)
        {
            this.userName = userName;
            this.accessLevel = accessLevel;
        }

    }

    class Authorization
    {
        private static string pathToUsersFile = @"..\..\..\users.txt";

        private static string getHashMd5(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);

            // формирование строки из бассива байт 
            string hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x}", b);

            return hash;
        }

        private static Profile auth(string name, string pass)
        {
            // В C# есть такой способ
            FileInfo fileInfo = new(pathToUsersFile);

            if (!fileInfo.Exists)
            {
                //throw new Exception("Нет доступа к файлу с данными пользователей!");
                Console.WriteLine("Invalid path to users file!");
                return new Profile();
            }

            // using определяет область, по завершении которой объект удаляется.
            // не совсем понимаю, зачем его используют
            using (StreamReader sr = new StreamReader(pathToUsersFile))
            {
                string line;
                string[] splitLine;

                while ((line = sr.ReadLine()) != null)
                {
                    splitLine = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (splitLine[0] == name && splitLine[1] == pass)
                    {
                        sr.Close();
                        return new Profile(splitLine[0], Convert.ToInt32(splitLine[2]));
                    }
                }
            }

            return new Profile();
        }

        public static void login()
        {
            Console.WriteLine("Command 'exit' for exit in programm");
            bool exit;
            Profile userProfile;

            do
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();
                if (CommandInterpreter.isExit(name)) return;

                Console.Write("Password: ");
                string pass = Console.ReadLine();
                if (CommandInterpreter.isExit(name)) return;

                string hashPass = getHashMd5(pass);
                userProfile = auth(name, hashPass);
            } while (userProfile.accessLevel < 0);

            CommandInterpreter commandInterpreter = new(userProfile);
            commandInterpreter.WriteCommand();
        }
    }
}
