using System;
using System.IO;
using System.Threading;
using GitHub;
using Microsoft.Extensions.Configuration;

namespace scientist_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var connectionstrings = new ConnectionStrings();
            configuration.GetSection("ConnectionStrings").Bind(connectionstrings);

            var repository = new AtendeeRepository();
            var smtpGateway = new SmtpEmailGateway();
            var cloudGateway = new CloudEmailServiceGateway();
            var alternateCloudGateway = new AlternativeCloudEmailService();
            Scientist.ResultPublisher = new SqlServerPublisher(connectionstrings);

            var atendeesToNotify = repository.GetAll();

            foreach (var atendee in atendeesToNotify)
            {
                bool isValidEmail;

                isValidEmail = Scientist.Science<bool>("Cloud-email-gatway", experiment =>
                {
                    experiment.Use(() => smtpGateway.IsValidEmail(atendee.Email));

                    experiment.AddContext("email address", atendee.Email);

                    experiment.Try("Cloud gateway", () => cloudGateway.ValidateEmailAddres(atendee.Email));
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