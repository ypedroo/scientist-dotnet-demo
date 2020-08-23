using System;

namespace scientist_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new AtendeeRepository();
            var smtpGateway = new SmtpEmailGateway();

            var atendeesToNotify = repository.GetAll();

            foreach (var atendee in atendeesToNotify)
            {
                if (smtpGateway.IsValidEmail(atendee.Email))
                {
                    smtpGateway.Send(atendee.Email, "Hello, our event will be held at Youtube on the 29th August");
                }
                else
                {
                    Console.WriteLine($"Cannot send mail to {atendee.FirstName}, the email addres is invalid");
                }
            }

        }
    }
}