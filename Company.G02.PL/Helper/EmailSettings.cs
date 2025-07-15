using Company.G02.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helper
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			// Mail Server: gmail.com
			// SMTP

			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;

			// wyeqzcvivkewvxnz

			client.Credentials = new NetworkCredential("hasan.aly.dev@gmail.com", "wyeqzcvivkewvxnz");

			client.Send("hasan.aly.dev@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
