using rule_engine.Rules;
using System.Collections.Generic;

namespace rule_engine.RuleEngines
{
    public interface IJsonOperator
    {
        IDictionary<string, string> CreateInitialState(string initialStateFilePath);
        IEnumerable<Rule> CreateRules(string rulesFilePath);
        string CreateOutput(IDictionary<string, string> state);
    }
}