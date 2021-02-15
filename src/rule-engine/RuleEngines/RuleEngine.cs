using rule_engine.Rules;
using System.Collections.Generic;

namespace rule_engine.RuleEngines
{
    /// <summary>
    /// The rule engine class.
    /// Contains all the methods to create the output to be printed to console.
    /// </summary>
    public class RuleEngine
    {
        private readonly IJsonOperator _jsonOperator;
        private readonly ICriteriaEvaluator _criteriaEvaluator;
        private readonly IDictionary<string, string> _state;
        private readonly IEnumerable<Rule> _rules;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="jsonOperator"></param>
        /// <param name="initialStateFilePath"></param>
        /// <param name="rulesFilePath"></param>
        public RuleEngine(IJsonOperator jsonOperator, ICriteriaEvaluator criteriaEvaluator, string initialStateFilePath, string rulesFilePath)
        {
            _jsonOperator = jsonOperator;
            _criteriaEvaluator = criteriaEvaluator;
            _state = _jsonOperator.CreateInitialState(initialStateFilePath);
            _rules = _jsonOperator.CreateRules(rulesFilePath);
        }

        /// <summary>
        /// Updates the current record state.
        /// Continously evaluating the rules in order until the record state is no longer modified.
        /// Creates a JSON serialized string to be printed to console. 
        /// </summary>
        /// <returns>The output string.</returns>
        public string CreateOutput()
        {
            UpdateStateWithRules();
            return _jsonOperator.CreateOutput(_state);
        }

        private void UpdateStateWithRules()
        {
            bool isStateModified = false;
            foreach (var rule in _rules)
            {
                if (CanExecuteRuleAction(rule.Criteria))
                {
                    isStateModified |= UpdateState(rule.Actions);
                }
            }

            if (isStateModified)
            {
                UpdateStateWithRules();
            }
        }

        private bool CanExecuteRuleAction(IEnumerable<Criteria> criterias)
        {
            foreach (var criteria in criterias)
            {
                if (!_state.ContainsKey(criteria.Key) 
                    || !_criteriaEvaluator.IsCriteriaFulfilled(criteria, _state))
                {
                    return false;
                }
            }
            return true;
        }

        private bool UpdateState(IEnumerable<Action> actions)
        {
            bool isStateUpdated = false;
            foreach (var action in actions)
            {
                if (!_state.ContainsKey(action.Key) || _state[action.Key] != action.Value)
                {
                    isStateUpdated = true;
                    _state[action.Key] = action.Value;
                }
            }
            return isStateUpdated;
        }
    }
}
