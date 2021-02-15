using Newtonsoft.Json;
using rule_engine.Rules;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace rule_engine.RuleEngines
{
    /// <summary>
    /// The JSON operator class.
    /// Contains all methods to serialize the state and rule JSON files.
    /// Contains all methods to deserialize the objects to create the output string.
    /// </summary>
    public class JsonOperator : IJsonOperator
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="fileSystem"></param>
        public JsonOperator(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Creates the initial state as a dictionary based on the initial state file.
        /// </summary>
        /// <param name="initialStateFilePath"></param>
        /// <returns>The initial state dictionary.</returns>
        public IDictionary<string, string> CreateInitialState(string initialStateFilePath)
        {
            VerifyFile(initialStateFilePath);
            IEnumerable<Rules.Action> actions = DeserializeJsonFile<Rules.Action>(initialStateFilePath);
            return CreateInitialStateDictionary(actions);
        }

        /// <summary>
        /// Creates a list of Rule objects based on the rules file.
        /// </summary>
        /// <param name="rulesFilePath"></param>
        /// <returns>A enumerable of Rule objects.</returns>
        public IEnumerable<Rule> CreateRules(string rulesFilePath)
        {
            VerifyFile(rulesFilePath);
            IEnumerable<Rule> rules = DeserializeJsonFile<Rule>(rulesFilePath);
            return rules;
        }

        /// <summary>
        /// Creates a string based on the state dictionary.
        /// </summary>
        /// <param name="states"></param>
        /// <returns>The output string.</returns>
        public string CreateOutput(IDictionary<string, string> states)
        {
            List<Rules.Action> formattedState = new List<Rules.Action>();
            foreach(var state in states)
            {
                formattedState.Add(new Rules.Action
                {
                    Key = state.Key,
                    Value = state.Value
                });
            }
            return JsonConvert.SerializeObject(formattedState, Formatting.Indented);
        }

        private IDictionary<string, string> CreateInitialStateDictionary(IEnumerable<Rules.Action> actions)
        {
            Dictionary<string, string> intitialState = new Dictionary<string, string>();
            foreach (Rules.Action action in actions)
            {
                intitialState.Add(action.Key, action.Value);
            }
            return intitialState;
        }

        private IEnumerable<T> DeserializeJsonFile<T>(string filePath)
        {
            List<string> errors = new List<string>();
            string jsonString = _fileSystem.File.ReadAllText(filePath);
            IEnumerable<T> result = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString, new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            });
            if (errors.Any())
            {
                string errorMessage = $"An error occured while deserializing the objects in file: {filePath}{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
                throw new ArgumentException(errorMessage);
            }
            return result;
        }

        private void VerifyFile(string filePath)
        {
            if (!_fileSystem.File.Exists(filePath))
            {
                throw new ArgumentException($"File: {filePath} does not exist.");
            }
            if (_fileSystem.Path.GetExtension(filePath).ToLower() != ".json")
            {
                throw new ArgumentException($"File: {filePath} is not a .json file.");
            }
            if (string.IsNullOrWhiteSpace(_fileSystem.File.ReadAllText(filePath)))
            {
                throw new ArgumentException($@"File: {filePath} is empty. Please include ""[]"" if you intended to pass in an empty array.");
            }
        }
    }
}
