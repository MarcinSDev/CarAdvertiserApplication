using CarAdvertiser.DTO;
using System.Net.Mail;

namespace CarAdvertiser.Helpers
{
    public static class Email
    {
        private static string _emailFrom = "admin@caradvertiser.co.uk";
        private static string _subject = "Car Advertiser";

        public static void Send(string[] emailTo, string body)
        {
            using (SmtpClient client = new SmtpClient())
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFrom);
                    foreach (string to in emailTo)
                    {
                        mail.To.Add(to);
                    }

                    mail.Subject = _subject;
                    mail.Body = body;
                    client.Send(mail);
                }
            }
        }

        public static void InterestedAdvertAdded(string[] emailTo, Advertisement advert)
        {
            using (SmtpClient client = new SmtpClient())
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFrom);
                    foreach (string to in emailTo)
                    {
                        mail.To.Add(to);
                    }

                    mail.Subject = _subject;
                    mail.Body = $"A car of your interest has been added to the database, please check!";
                    mail.Body += $"<br/>Make: {advert.CarModel.CarManufacturer.Value}";
                    mail.Body += $"<br/>Model: {advert.CarModel.Value}";
                    mail.Body += $"<br/>Reg. year: {advert.RegYear}";
                    client.Send(mail);
                }
            }
        }
    }
}