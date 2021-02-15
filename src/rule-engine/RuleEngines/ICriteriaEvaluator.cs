using rule_engine.Rules;
using System.Collections.Generic;

namespace rule_engine.RuleEngines
{
    public interface ICriteriaEvaluator
    {
        bool IsCriteriaFulfilled(Criteria criteria, IDictionary<string, string> state);
    }
}