using System;
using System.Threading;

namespace scientist_demo
{
    public class CloudEmailServiceGateway
    {
        public bool ValidateEmailAddres(string emailAddress)
            => emailAddress.Contains(".com");

        public void Send(string emailAddres, string message)
        {
            Console.WriteLine($"Sending email to: {emailAddres}");
            Thread.Sleep(450);
        }
    }
}
