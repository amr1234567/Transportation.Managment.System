using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Interfaces.IHelpersServices
{
    public interface ISmsSevices
    {
        MessageResource Send(string Message, string PhoneNumber);
        string GenerateCode();
    }
}
