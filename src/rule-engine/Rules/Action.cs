using Newtonsoft.Json;

namespace rule_engine.Rules
{
    /// <summary>
    /// The action class.
    /// Represents an object in the Action JSON array.
    /// </summary>
    public class Action
    {
        [JsonProperty(Required = Required.Always)]
        public string Key
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
