using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces.Services
{
    public interface IMailerService
    {
        Task SendResetPasswordEmail(string emailTo, string fullname, string resetPasswordUrl);
        Task SendGoogleRegistrationSuccessEmail(
            string emailTo,
            string fullname,
            string username,
            string password,
            string changePasswordUrl
        );

        Task SendWelcomeNewAdminEmail(string emailTo, string fullname, string username, string password, string changePasswordUrl);
    }
}
