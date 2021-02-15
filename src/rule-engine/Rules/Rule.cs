using Newtonsoft.Json;
using System.Collections.Generic;

namespace rule_engine.Rules
{
    /// <summary>
    /// The rule class.
    /// Represents an object in the Rules array.
    /// </summary>
    public class Rule
    {
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Criteria> Criteria
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        public IEnumerable<Action> Actions
        {
            get;
            set;
        }
    }
}
