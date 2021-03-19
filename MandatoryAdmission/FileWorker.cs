using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MandatoryAdmission
{
    class FileWorker
    {
        
        private static string pathToFles = @"..\..\..\files.txt";

        private FileInfo fileInfo = new (pathToFles);

        public int accessLevel { get; set; }

        public FileWorker(int accessLevel)
        {
            this.accessLevel = accessLevel;
        }

        private bool CurrectPathToFS()
        {
            if (!fileInfo.Exists)
            {
                //throw new Exception("nvalid path to file-system file!");
                Console.WriteLine("Invalid path to file-system file!");
                return false;
            }

            return true;
        }

        public bool CanChangeFile(int fileAccessLvl)
        {
            if (fileAccessLvl == accessLevel) return true;

            Console.WriteLine($"File has level {fileAccessLvl}, your level {accessLevel}... Access denied!");
            return false;
        }

        public bool CanReadFile(int fileAccessLvl)
        {
            if (fileAccessLvl <= accessLevel) return true;

            Console.WriteLine($"File has level {fileAccessLvl}, your level {accessLevel}... Access denied!");
            return false;
        }

        public async void CreateFile(string filename, string text)
        {
            StreamWriter sw = new StreamWriter(pathToFles, true, System.Text.Encoding.Default);
            
            await sw.WriteLineAsync($"{filename} {text} {accessLevel}");
            sw.Close();
            Console.WriteLine($"File {filename} was created!");
        }

        public void DeleteFile(string filename)
        {
            StreamReader reader = new StreamReader(pathToFles);
            string allLines = "", line;

            while ((line = reader.ReadLine()) != null)
            {

                // Раньше тут был StartWith, но так красивее
                string[] command = CommandInterpreter.SplitComand(line);
                if (command[0] == filename)
                {
                    if (!CanChangeFile(Convert.ToInt32(command[2]))) return;

                    continue;
                }

                allLines += $"{line}\n";
            }

            reader.Close();

            StreamWriter writer = new StreamWriter(pathToFles);
            writer.Write(allLines);
            writer.Close();
        }

        public void CnageFile(string filename, string text)
        {

            StreamReader reader = new StreamReader(pathToFles);
            string allLines = "", line; 

            while ((line = reader.ReadLine()) != null)
            {
                string[] command = CommandInterpreter.SplitComand(line);
                if (command[0] == filename)
                {
                    if (!CanChangeFile(Convert.ToInt32(command[2]))) return;
      
                    line = $"{filename} {text} {accessLevel}";
                }
                
                allLines += $"{line}\n";
            }

            reader.Close();

            StreamWriter writer = new StreamWriter(pathToFles);
            writer.Write(allLines);
            writer.Close();

            Console.WriteLine($"File {filename} was changed!");
        }

        public void ReadFile(string filename)
        {
            StreamReader reader = new StreamReader(pathToFles);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] command = CommandInterpreter.SplitComand(line);
                if (command[0] == filename && CanReadFile(Convert.ToInt32(command[2])))
                {
                    Console.Write(command[1]);
                }
            }

            reader.Close();
        }

    }
}
