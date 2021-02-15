using rule_engine.RuleEngines;
using System;
using System.IO.Abstractions;

namespace rule_engine
{
    /// <summary>
    /// The entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method.
        /// Prints the rule engine output to console based on the initial record and rules files.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("[Error] Please pass in the InitialRecordState file, then the Rules file as the arguments.");
                Environment.Exit(-1);
            }

            Console.WriteLine("Starting rule engine...");

            try
            {
                RuleEngine ruleEngine = new RuleEngine(new JsonOperator(new FileSystem()), new BasicCriteriaEvaluator(), args[0], args[1]);
                Console.WriteLine(ruleEngine.CreateOutput());
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"[Error] {e.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
