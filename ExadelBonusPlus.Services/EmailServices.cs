using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using ExadelBonusPlus.Services.Models.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace ExadelBonusPlus.Services
{
    public class EmailServices : IEmailService
    {
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly IBonusService _bonusService;

        public EmailServices(IOptions<EmailSettings> emailSettings,
                             IUserService userService,
                             IVendorService vendorService,
                             IBonusService bonusService)
        {
            _emailSettings = emailSettings;
            _vendorService = vendorService;
            _userService = userService;
            _bonusService = bonusService;
        }
        public async Task SendEmailAsync(History history)
        {

            var user = await _userService.GetUserAsync(history.CreatorId.ToString());

            var bonus = await _bonusService.FindBonusByIdAsync(history.BonusId);
            var vendor =
                await _vendorService.GetVendorByIdAsync(bonus.Company.Id);

            string promoCode = vendor.Name + RandomString(5);
            var strToUser = String.Format("Hi {0} {1} your order recived. Your promo code is {2}",
                user.FirstName, user.LastName, promoCode);
            var strToVendor = String.Format("Dear {0},  {1} {2} will come to you with promo. PromoCode is {3}",
                vendor.Name, user.FirstName, user.LastName, promoCode);



            MimeMessage messageToUser = new MimeMessage();
            messageToUser.From.Add(new MailboxAddress(vendor.Name, vendor.Email));
            messageToUser.To.Add(new MailboxAddress(user.Email));
            messageToUser.Subject = "Get bonus";
            messageToUser.Body = new BodyBuilder()
            {
                HtmlBody = String.Format("<div>{0}</div>", strToUser)
            }.ToMessageBody();


            MimeMessage messageToVendor = new MimeMessage();
            messageToVendor.From.Add(new MailboxAddress(user.FirstName, user.Email));
            messageToVendor.To.Add(new MailboxAddress(vendor.Email));
            messageToVendor.Subject = "Get bonus";
            messageToVendor.Body = new BodyBuilder()
            {
                HtmlBody = String.Format("<div>{0}</div>", strToVendor)
            }.ToMessageBody();

            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_emailSettings.Value.SMTPServer, _emailSettings.Value.SMTPServerPort, true);
                client.Authenticate(_emailSettings.Value.EmailAddress, _emailSettings.Value.Password);
                client.Send(messageToUser);
                client.Send(messageToVendor);
                client.Disconnect(true);
            }

        }
        public string RandomString(int length)
        {
            Random _random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
