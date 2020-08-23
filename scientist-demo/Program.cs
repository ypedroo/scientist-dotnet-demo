using System;
using System.Threading;
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
            var alternateCloudGateway = new AlternativeCloudEmailService();
            Scientist.ResultPublisher = new ConsolePublisher();

            var atendeesToNotify = repository.GetAll();

            foreach (var atendee in atendeesToNotify)
            {
                bool isValidEmail;

                isValidEmail = Scientist.Science<bool>("Cloud-email-gatway", experiment =>
                {
                    experiment.BeforeRun(() => PreapreForExperiment(atendee));

                    experiment.Use(() => smtpGateway.IsValidEmail(atendee.Email));

                    // Add context
                    experiment.AddContext("email address", atendee.Email);

                    // Add condition to experiment
                    //experiment.RunIf(() => atendee.FirstName == "Pedro" || atendee.FirstName == "Mateus");
                    //experiment.Ignore((control, candidate) => atendee.FirstName == "Mateus");

                    experiment.Try("Cloud gateway", () => cloudGateway.ValidateEmailAddres(atendee.Email));
                    experiment.Try("Alternative gateway", () => alternateCloudGateway.ValidateEmailAddres(atendee.Email));
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
            Console.ReadLine();
        }

        public static void PreapreForExperiment(Atendee atendee)
        {
            Console.WriteLine($"Preparing {atendee.FirstName} for experiment");

            Thread.Sleep(5000);
        }
    }
}