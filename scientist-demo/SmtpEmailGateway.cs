using System;
using System.Threading;

namespace scientist_demo
{
    class SmtpEmailGateway
    {
        public bool IsValidEmail(string emailAddres)
            => emailAddres.Contains("@");

        public void Send(string emailAddres, string message)
        {
            Console.WriteLine($"Sending email to: {emailAddres}");
            Thread.Sleep(200);
        }
    }
}