using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExadelBonusPlus.Services.Models.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(History history);
    }
}
