using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowControl
{
    class BioApp
    {        
        event OutputEvent output;

        public BioApp()
        {           

        }

        // länkar ihop med IView
        public void ConnectView(IView view)
        {
            output+=view.Connect(new InputEvent(OnInput));            
        }

        public void Run()
        {
            // todo an epic gameloop
            
        }
                
        private int GetAgeCategory(int age)
        {
            switch (age)
            {
                case int x when (age < 5 || age >100):
                    return 0;
                case int x when age < 20:
                    return 2;
                    
                case int x when age > 64:
                    return 3;                
                default:
                    return 1;                    
            }
        }

        private int GetCost(int category)
        {
            switch (category)
            {
                case 0:
                    return 0;
                case 1:
                    return 120;
                case 2:
                    return 80;
                case 3:
                    return 90;
            }
            return 0;
        }
              

        // skickar 2 länkade eventmedelande, categori+pris 
        private void SendCost(int category, int cost)
        {
            
            output?.Invoke(new CommandMessage(CommandType.sendPriceCategory, category, new CommandMessage(CommandType.sendPrize, cost, null)));            
        }

        // skickar 2 olänkade eventmedelande, antalpersoner och totalsumma
        private void CompundCost(CommandMessage msg)
        {
            int counter = 0;
            int sum = 0;
            do
            {                
                sum+=GetCost(GetAgeCategory(msg.data));
                msg = msg.extraMessage;
                counter++;
            }
            while (msg!=null);

            output?.Invoke(new(CommandType.antalPersoner, counter, null));
            output?.Invoke(new(CommandType.totalSum, sum, null));
        }

        // hanterar events
        private void OnInput(CommandMessage msg)
        {
            switch (msg.type)
            {                
                case CommandType.kill:
                    output?.Invoke(new CommandMessage(CommandType.kill, 0 ,null));
                    break;
                case CommandType.requestPrice:
                    if (msg.extraMessage != null)
                    {
                        CompundCost(msg);
                        return;  // för att extra eventen hanteras i CompountCost
                    }
                    else
                    {
                        int cat = GetAgeCategory(msg.data);
                        SendCost(cat, GetCost(cat));                        
                    }
                    break;
            }

            if (msg.extraMessage != null)
            {
                this.OnInput(msg.extraMessage);
            }
        }
    }
}
