using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowControl
{
    interface IView
    {
        public OutputEvent Connect(InputEvent inp);
    }
}
