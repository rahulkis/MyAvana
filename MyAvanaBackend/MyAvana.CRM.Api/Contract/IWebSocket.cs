using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IWebSocket
    { 
        bool UpdateUserLastPing(string userid);
    }
}
