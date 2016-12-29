using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Photobooth_PPTIK.Pages
{
    /// <summary>
    /// Interaction logic for ShareEmail.xaml
    /// </summary>
    public partial class ShareEmail : Page
    {
        private string emailSubject;
        private string msgBody;
        private string path;
        private string pswd;
        private string senderEmail;

        public ShareEmail(string path)
        {
            this.InitializeComponent();
            this.path = path;
            EmailTextBox.Focus();
            ConfigSettings config = new ConfigSettings();
            string configPath = @"" + Directory.GetCurrentDirectory() + "/config.json";
            using (StreamReader file = File.OpenText("config.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                config = serializer.Deserialize<ConfigSettings>(reader);

                senderEmail = config.smtp.email;
                pswd = config.smtp.password;
                emailSubject = config.smtp.emailSubject;
                msgBody = config.smtp.msgBody;
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //put your SMTP address and port here.
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //Put the email address
                mail.From = new MailAddress(senderEmail);
                //Put the email where you want to send.
                String sendTo = EmailTextBox.Text;
                mail.To.Add(sendTo);

                mail.Subject = emailSubject;

                StringBuilder sbBody = new StringBuilder();
                sbBody.AppendLine(msgBody);
                sbBody.AppendLine("");
                sbBody.AppendLine("Thank you,");
                sbBody.AppendLine("PPTIK ITB");
                mail.Body = sbBody.ToString();

                //Your log file path
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(@"D:\screenshot.jpg");
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), this.path));

                mail.Attachments.Add(attachment);

                //Your username and password!
                SmtpServer.Credentials = new System.Net.NetworkCredential(senderEmail, pswd);
                //Set Smtp Server port
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                MessageBox.Show("Terkirim :)");

                var page = new CapturePage();
                this.NavigationService.Navigate(page);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                 MessageBox.Show("Ulangi Beberapa Saat Lagi");
            }
        }

        private void btnTake_Click(object sender, RoutedEventArgs e)
        {
            var page = new CapturePage();
            this.NavigationService.Navigate(page);
        }
    }
}
