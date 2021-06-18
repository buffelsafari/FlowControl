using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowControl
{
    class ConsoleView : IView
    {
        bool isRunning = true;
        event InputEvent input;        

        public ConsoleView()
        {
            Thread thread = new Thread(Run);
            thread.Start();
        }

        public OutputEvent Connect(InputEvent inp)
        {
            input += inp;            
            return OnOutput;
        }

        public void Run()
        {            
            while (isRunning)
            {
                MainMenu();
            }
        }

        private void MainMenu()
        {
            PrintMainMenu();
            String str = Console.ReadLine();
            
            switch (str)
            {
                case "0":
                    input?.Invoke(new CommandMessage(CommandType.kill, 0, null));                        
                    break;
                case "1":                        
                    input?.Invoke(new(CommandType.requestPrice, AgeChoice(), null));
                    break;
                case "2":
                    Repeater();
                    break;
                case "3":
                    TreeWord();
                    break;
                case "4":                         
                    SendManyCost(ManyAgeChoise());
                    break;

                default:
                    PrintInvalid();
                    break;
            }            
        }

        // splittar ut det tredje ordet, beräknar resultatet direkt i view classen 
        private void TreeWord()
        {
            string[] split;
            do
            {
                Console.WriteLine("Skriv en mening med minst tre ord!");
                string line = Console.ReadLine();

                split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);                
            }
            while (split.Length<3);

            Console.WriteLine($"Det tredje ordet är '{split[2]}'");
        }

        // repeterar en sträng 10ggr direkt i view
        private void Repeater()
        {
            Console.WriteLine("Skriv något!");
            string line = Console.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(line);
            }
        }

        // skickar en länk med ålders events till app
        private void SendManyCost(int[] ages)
        {
            CommandMessage root = new CommandMessage(CommandType.requestPrice, ages[0], null);
            for (int i = 1; i < ages.Length; i++)
            {
                CommandMessage msg = root;
                root = new CommandMessage(CommandType.requestPrice, ages[i], msg);
            }
            input?.Invoke(root);
        }
                
        private void PrintMainMenu()
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("0 stäng");
            Console.WriteLine("1 beräkna priset");
            Console.WriteLine("2 Upprepa tio gånger");
            Console.WriteLine("3 Det tredje ordet");
            Console.WriteLine("4 beräkna priset för sällskap");
        }

        private void PrintInvalid()
        {
            Console.WriteLine("felaktig input");
        }

        private void PrintAgeCategory(int category)
        {
            switch (category)
            {
                case 1:
                    Console.Write("Normalpris:");
                    break;
                case 2:
                    Console.Write("Ungdomspris:");
                    break;
                case 3:
                    Console.Write("Pensionärspris:");
                    break;
            }
        }

        // retunerar åldersinput
        private int AgeChoice()
        {
            int age;
            bool success = false;
            do
            {
                Console.WriteLine("ange ålder:");
                string line = Console.ReadLine();
                success = int.TryParse(line, out age) && age>0;

                if (!success)
                {
                    PrintInvalid();
                }
            }
            while(!success);
            return age;
        }

        // retunerar en array med åldersinput
        private int[] ManyAgeChoise()
        {
            int n;
            bool success = false;
            do
            {
                Console.WriteLine("Hur många biljetter?");
                string line = Console.ReadLine();
                success = int.TryParse(line, out n) && n > 0;

                if (!success)
                {
                    PrintInvalid();
                }

            }
            while (!success);

            int[] ages = new int[n];
            for (int i = 0; i < n; i++)
            {
                ages[i]=AgeChoice();
            }
            return ages;
        }

        // hanterar events
        private void OnOutput(CommandMessage msg)
        {
            switch (msg.type)
            {                
                case CommandType.kill:                    
                    isRunning = false;
                    break;
                case CommandType.sendPriceCategory:
                    PrintAgeCategory(msg.data);
                    break;
                case CommandType.sendPrize:
                    Console.Write($"{msg.data}kr\n");
                    break;
                case CommandType.totalSum:
                    Console.WriteLine($"toala kostnaden {msg.data} kr");
                    break;
                case CommandType.antalPersoner:
                    Console.WriteLine($"antal personer {msg.data} st");
                    break;
            }
            
            if (msg.extraMessage != null)
            {                
                this.OnOutput(msg.extraMessage);
            } 
        }
    }
}
