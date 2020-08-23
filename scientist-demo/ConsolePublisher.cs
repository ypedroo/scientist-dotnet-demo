﻿using System;
using System.Threading.Tasks;
using GitHub;

namespace scientist_demo
{
    class ConsolePublisher : IResultPublisher
    {
        public Task Publish<T, TClean>(Result<T, TClean> result)
        {
            Console.ForegroundColor = result.Mismatched ? ConsoleColor.Red : ConsoleColor.Green;

            Console.WriteLine($"Experiment name: '{result.ExperimentName}'");
            Console.WriteLine($"Result: '{(result.Matched ? "Control value matched candidate value" : "Control value DIDN'T matched candidate value")}'");
            Console.WriteLine($"Control Value: '{result.Control.Value}'");

            foreach (var candidate in result.Candidates)
            {
                Console.WriteLine($"Candidate name: {candidate.Name}");
                Console.WriteLine($"Candidate value: {candidate.Value}");
            }

            if (result.Mismatched)
            {

            }

            Console.ForegroundColor = ConsoleColor.White;

            return Task.FromResult(0);
        }
    }
}