using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowControl
{
    enum CommandType
    {
        kill,
        requestPrice,
        sendPriceCategory,
        sendPrize,
        totalSum,
        antalPersoner
    }

    class CommandMessage
    {
        public CommandType type { get; }
        
        public int data { get; }

        public CommandMessage extraMessage { get; }

        public CommandMessage(CommandType type, int data, CommandMessage extraMessage)
        {
            this.type = type;
            this.data = data;
            this.extraMessage = extraMessage;
        }
    }
}
