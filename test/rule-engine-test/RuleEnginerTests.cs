using Newtonsoft.Json;
using rule_engine.RuleEngines;
using rule_engine.Rules;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace rule_engine_test
{
    public class RuleEnginerTests
    {
        [Fact]
        public void CreateOutput_ValidInputs_CreateOutputSuccessfully()
        {
            //Arrange
            List<rule_engine.Rules.Action> initialStates = new List<rule_engine.Rules.Action>
            {
                new rule_engine.Rules.Action
                {
                    Key = "asleep",
                    Value = "true"
                }
            };
            List<Rule> rules = new List<Rule>
            {
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "get-ready",
                            Operator = "=",
                            Value = "true"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "work",
                            Value = "true"
                        }
                    }
                },
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "=",
                            Value = "false"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "stretch",
                            Value = "true"
                        },
                        new rule_engine.Rules.Action
                        {
                            Key = "check-phone",
                            Value = "true"
                        }
                    }
                },
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "=",
                            Value = "true"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "asleep",
                            Value = "false"
                        }
                    }
                },
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "=",
                            Value = "false"
                        },
                        new Criteria
                        {
                            Key = "check-phone",
                            Operator = "=",
                            Value = "true"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "get-ready",
                            Value = "true"
                        }
                    }
                }
            };
            List<rule_engine.Rules.Action> expected = new List<rule_engine.Rules.Action>
            {
                new rule_engine.Rules.Action
                {
                    Key = "asleep",
                    Value = "false"
                },
                new rule_engine.Rules.Action
                {
                    Key = "stretch",
                    Value = "true"
                },
                new rule_engine.Rules.Action
                {
                    Key = "check-phone",
                    Value = "true"
                },
                new rule_engine.Rules.Action
                {
                    Key = "get-ready",
                    Value = "true"
                },
                new rule_engine.Rules.Action
                {
                    Key = "work",
                    Value = "true"
                }
            };
            string initialStateFilePath = @"c:\initialState.json";
            string rulesFilePath = @"c:\rules.json";
            MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(JsonConvert.SerializeObject(initialStates)) },
                { rulesFilePath, new MockFileData(JsonConvert.SerializeObject(rules)) }
            });
            JsonOperator jsonOperator = new JsonOperator(mockFileSystem);
            BasicCriteriaEvaluator criteriaEvaluator = new BasicCriteriaEvaluator();
            RuleEngine underTest = new RuleEngine(jsonOperator, criteriaEvaluator, initialStateFilePath, rulesFilePath);
            //Act
            string result = underTest.CreateOutput();
            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expected, Formatting.Indented), result);
        }

        [Fact]
        public void CreateOutput_InvalidCriteriaOperator_ThrowsArgumentException()
        {
            //Arrange
            List<rule_engine.Rules.Action> initialStates = new List<rule_engine.Rules.Action>
            {
                new rule_engine.Rules.Action
                {
                    Key = "asleep",
                    Value = "true"
                }
            };
            List<Rule> rules = new List<Rule>
            {
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "+",
                            Value = "true"
                        },
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "=",
                            Value = "true"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "asleep",
                            Value = "false"
                        }
                    }
                },
                new Rule
                {
                    Criteria = new List<Criteria>
                    {
                        new Criteria
                        {
                            Key = "asleep",
                            Operator = "=",
                            Value = "true"
                        }
                    },
                    Actions = new List<rule_engine.Rules.Action>
                    {
                        new rule_engine.Rules.Action
                        {
                            Key = "is-8-am",
                            Value = "false"
                        }
                    }
                }
            };
            List<rule_engine.Rules.Action> expected = new List<rule_engine.Rules.Action>
            {
                new rule_engine.Rules.Action
                {
                    Key = "asleep",
                    Value = "true"
                },
                new rule_engine.Rules.Action
                {
                    Key = "is-8-am",
                    Value = "false"
                }
            }; ;
            string initialStateFilePath = @"c:\initialState.json";
            string rulesFilePath = @"c:\rules.json";
            MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(JsonConvert.SerializeObject(initialStates)) },
                { rulesFilePath, new MockFileData(JsonConvert.SerializeObject(rules)) }
            });
            JsonOperator jsonOperator = new JsonOperator(mockFileSystem);
            BasicCriteriaEvaluator criteriaEvaluator = new BasicCriteriaEvaluator();
            RuleEngine underTest = new RuleEngine(jsonOperator, criteriaEvaluator, initialStateFilePath, rulesFilePath);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateOutput());
        }
    }
}
