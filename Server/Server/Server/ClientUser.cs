using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientUser
    {
        int state;
        public ClientUser()
        {
            state = 0;
        }
        public void setState(int i)
        {
            state = i;
        }
        public int getState()
        {
            return state;
        }
    }
}
