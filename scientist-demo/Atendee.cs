﻿namespace scientist_demo
{
    class Atendee
    {
        public Atendee(string firstName, string email)
        {
            FirstName = firstName;
            Email = email;
        }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}