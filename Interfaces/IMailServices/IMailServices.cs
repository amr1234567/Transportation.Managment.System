using Core.Dto.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IMailServices
{
    public interface IMailServices
    {
        void SendEmail(Message message);
    }
}
