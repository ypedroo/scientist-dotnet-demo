using System;
using GitHub;

namespace scientist_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new AtendeeRepository();
            var smtpGateway = new SmtpEmailGateway();
            var cloudGateway = new CloudEmailServiceGateway();
            Scientist.ResultPublisher = new ConsolePublisher();

            var atendeesToNotify = repository.GetAll();

            foreach (var atendee in atendeesToNotify)
            {
                bool isValidEmail;

                isValidEmail = Scientist.Science<bool>("Cloud-email-gatway", experiment =>
                {
                    experiment.Use(() => smtpGateway.IsValidEmail(atendee.Email));

                    experiment.Try(() => cloudGateway.ValidateEmailAddres(atendee.Email));
                });

                if (isValidEmail)
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