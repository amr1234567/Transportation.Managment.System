using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Email
{
    public class Message
    {
        public IEnumerable<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message()
        {

        }
        public Message(IEnumerable<string> addresses, string subject, string content)
        {
            To = [.. addresses.Select(x => new MailboxAddress("email", x))];
            Subject = subject;
            Content = content;
        }
    }
}
