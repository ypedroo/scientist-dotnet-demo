using System.Collections.Generic;

namespace scientist_demo
{
    public class AtendeeRepository
    {
        public List<Atendee> GetAll() => new List<Atendee>
            {
                new Atendee("Pedro", "ynoa.pedro@outlook.com"),
                new Atendee("Gesiel", "gesielchaves@gmail.com"),
                new Atendee("Mateus", "mateussantosgmail.com")
            };
    }
}