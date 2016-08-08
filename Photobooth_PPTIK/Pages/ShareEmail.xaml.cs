using System;
using System.Collections.Generic;
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
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace Photobooth_PPTIK.Pages
{
    /// <summary>
    /// Interaction logic for ShareEmail.xaml
    /// </summary>
    public partial class ShareEmail : Page
    {
        private string path;

        public ShareEmail(string path)
        {
            this.InitializeComponent();
            this.path = path;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //put your SMTP address and port here.
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //Put the email address
                mail.From = new MailAddress("yours");
                //Put the email where you want to send.
                String sendTo = EmailTextBox.Text;
                mail.To.Add(sendTo);

                mail.Subject = "BPG Bandung - Photo Booth PPTIK";

                StringBuilder sbBody = new StringBuilder();

                sbBody.AppendLine("Thank you,");

                sbBody.AppendLine("PPTIK ITB");
                mail.Body = sbBody.ToString();

                //Your log file path
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(@"D:\screenshot.jpg");
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), this.path));

                mail.Attachments.Add(attachment);

                //Your username and password!
                SmtpServer.Credentials = new System.Net.NetworkCredential("yours", "yours");
                //Set Smtp Server port
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                MessageBox.Show("The email has been sent! :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
