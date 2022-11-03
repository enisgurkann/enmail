using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EB2B.CORE.Service;

public class MailService : IMailService
{
    private readonly SmtpClient _client;
    private readonly string _userName;
    private IConfiguration _configuration;
    public MailService(IConfiguration configuration)
    {
        _userName = configuration.GetSection("Mail:Username").Value;

        _client = new SmtpClient()
        {
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Host = configuration.GetSection("Mail:Smtp").Value,
            EnableSsl = bool.Parse(configuration.GetSection("Mail:IsSSL").Value),
            Credentials = new NetworkCredential(_userName, configuration.GetSection("Mail:Password").Value)
        };
    }
   
    public async Task<string> SendAsync(string Baslik, string Icerik, string Email, string Email2 = null, string Email3 = null, string Email4 = null)
    {
        try
        {
            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress(_userName),
                Subject = Baslik,
                Body = Icerik,
                BodyEncoding = Encoding.UTF8,
                BodyTransferEncoding = System.Net.Mime.TransferEncoding.Base64,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(Email));
            if (Email2 is not null)
                mail.To.Add(new MailAddress(Email2));
            if (Email3 is not null)
                mail.To.Add(new MailAddress(Email3));
            if (Email4 is not null)
                mail.To.Add(new MailAddress(Email3));

            await _client.SendMailAsync(mail);

            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "OK";
    }
    
}