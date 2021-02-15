using Newtonsoft.Json;

namespace rule_engine.Rules
{
    /// <summary>
    /// The criteria class.
    /// Represents an object in the Criteria JSON array.
    /// </summary>
    public class Criteria
    {
        [JsonProperty(Required = Required.Always)]
        public string Key
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        public string Operator
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        public string Value
        {
            get;
            set;
        }
    }
}
