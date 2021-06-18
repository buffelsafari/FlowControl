using System;

namespace FlowControl
{

    // programmet har en en app class och en view class
    // dessa är länkade via Events (lite exprimenterande)
    // Jag inser naturligtvis att designen är en smula knasig men jag ville prova lite.

    // 4. jag vill träna lite på att använda delegats.


    delegate void InputEvent(CommandMessage msg);
    delegate void OutputEvent(CommandMessage msg);

    class Program
    {
        static void Main()
        {
            ConsoleView view = new ConsoleView();            
            BioApp app = new BioApp();
            app.ConnectView(view);            
            app.Run();
        }
    }
}
