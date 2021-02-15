using rule_engine.Rules;
using System;
using System.Collections.Generic;

namespace rule_engine.RuleEngines
{
    public class BasicCriteriaEvaluator : ICriteriaEvaluator
    {
        public BasicCriteriaEvaluator()
        {
        }

        public bool IsCriteriaFulfilled(Criteria criteria, IDictionary<string, string> state)
        {
            return criteria.Operator switch
            {
                "=" => state[criteria.Key] == criteria.Value,
                "!=" => state[criteria.Key] != criteria.Value,
                _ => throw new ArgumentException($"The operator {criteria.Operator} is not supported. Please verify the rules file content.")
            };
        }
    }
}
