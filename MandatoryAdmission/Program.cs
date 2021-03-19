using System;


namespace MandatoryAdmission
{
  
    class Program
    {
       
        static void Main(string[] args)
        {
            Profile userProfile = Authorization.login();

            while (userProfile.accessLevel < 0)
            {
                Console.WriteLine("Wrong name or password. Try again...");
                userProfile = Authorization.login();
            }

            CommandInterpreter commandInterpreter = new (userProfile);
            commandInterpreter.WriteCommand();

        }
    }
}
