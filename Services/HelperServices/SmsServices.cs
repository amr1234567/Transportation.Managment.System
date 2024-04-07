using Core.Helpers.Classes;
using Interfaces.IHelpersServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Services.HelperServices
{
    public class SmsServices : ISmsSevices
    {
        private readonly TwilioConfiguration _options;

        public SmsServices(IOptions<TwilioConfiguration> options)
        {
            _options = options.Value;
        }

        public string GenerateCode()
        {
            var code = "";
            for (int i = 0; i < 5; i++)
                code += new Random().Next(9);
            return code;
        }

        public MessageResource Send(string Message, string PhoneNumber)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);

            var response = MessageResource.Create(
                body: Message,
                from: new Twilio.Types.PhoneNumber(_options.PhoneNumber),
                to: PhoneNumber);

            return response;
        }


    }
}
