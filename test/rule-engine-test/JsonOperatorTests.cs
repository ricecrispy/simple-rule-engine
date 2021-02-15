using Newtonsoft.Json;
using rule_engine.RuleEngines;
using rule_engine.Rules;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace rule_engine_test
{
    public class JsonOperatorTests
    {
        [Fact]
        public void GeneralOperation_InvalidFileType_ThrowsArgumentException()
        {
            //Arrange
            string initialStateFilePath = @"c:\file.txt";
            string rulesFilePath = @"c:\rules.txt";
            string directoryPath = @"c:\folder\";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(string.Empty) },
                { rulesFilePath, new MockFileData(string.Empty) }
            });
            fileSystem.AddDirectory(directoryPath);
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateInitialState(initialStateFilePath));
            Assert.Throws<ArgumentException>(() => underTest.CreateRules(rulesFilePath));
            Assert.Throws<ArgumentException>(() => underTest.CreateInitialState(directoryPath));
        }

        [Fact]
        public void GeneralOperation_EmptyArray_()
        {
            //Arrange
            string initialStateFilePath = @"c:\file.json";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(string.Empty) }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateInitialState(initialStateFilePath));
        }

        [Fact]
        public void GeneralOperation_EmptyArray_CreatesEmptyDictionary()
        {
            //Arrange
            string initialStateFilePath = @"c:\file.json";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(@"[]") }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act
            var result = underTest.CreateInitialState(initialStateFilePath);
            //Assert
            Assert.IsType<Dictionary<string, string>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateInitialState_InvalidFileContent_ThrowsArgumentException()
        {
            //Arrange
            string initialStateFilePath = @"c:\file.json";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData("just some text") },
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateInitialState(initialStateFilePath));
        }

        [Fact]
        public void CreateInitialState_ValidFile_CreatesDictionarySuccessfully()
        {
            //Arrange
            string initialStateFilePath = @"c:\initialState.json";
            string initialState = @"[{ ""Key"": ""field_923"", ""Value"": ""Hello there"" },{ ""Key"": ""field_5"", ""Value"": ""98;32"" }]";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { initialStateFilePath, new MockFileData(initialState) }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act
            IDictionary<string, string> result = underTest.CreateInitialState(initialStateFilePath);
            //Assert
            Assert.Equal("Hello there", result["field_923"]);
            Assert.Equal("98;32", result["field_5"]);
        }

        [Fact]
        public void CreateRules_ValidFile_CreateRulesSuccessfully()
        {
            //Arrange
            string rulesFilePath = @"c:\rules.json";
            string rules = @"[
    {
        ""Name"": ""rule 1"",
        ""Criteria"": [
            { ""Key"": ""field_622"", ""Operator"": ""="", ""Value"": ""7/27/2019"" },
            { ""Key"": ""field_731"", ""Operator"": ""!="", ""Value"": ""Option B"" }
        ],
        ""Actions"": [
            {
                ""Key"": ""field_8792"",
                ""Value"": ""57.6294""
            }
        ]
    }
]";
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { rulesFilePath, new MockFileData(rules) }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act
            IEnumerable<Rule> result = underTest.CreateRules(rulesFilePath);
            //Assert
            Assert.Single(result);
        }

        [Fact]
        public void CreateRules_InvalidFileContent_ThrowsJsonException()
        {
            //Arrange
            string rulesFilePath = @"c:\rules.json";
            var wrongContent = new
            {
                hello = "world"
            };
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { rulesFilePath, new MockFileData(JsonConvert.SerializeObject(wrongContent)) }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateRules(rulesFilePath));
        }

        [Fact]
        public void CreateRules_InvalidFileContentList_ThrowsJsonException()
        {
            //Arrange
            string rulesFilePath = @"c:\rules.json";
            List<object> wrongContentList = new List<object>
            {
                new
                {
                    ping = "pong"
                },
                new
                {
                    good = "morning"
                }
            };
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { rulesFilePath, new MockFileData(JsonConvert.SerializeObject(wrongContentList)) }
            });
            JsonOperator underTest = new JsonOperator(fileSystem);
            //Act + Assert
            Assert.Throws<ArgumentException>(() => underTest.CreateRules(rulesFilePath));
            Assert.Throws<ArgumentException>(() => underTest.CreateInitialState(rulesFilePath));
        }

        [Fact] 
        public void CreateOutput_ValidDictionary_CreateOutputStringSuccessfully()
        {
            //Arrange
            List<rule_engine.Rules.Action> expected = new List<rule_engine.Rules.Action>
            {
                new rule_engine.Rules.Action
                {
                    Key = "field_1",
                    Value = "2/1/2020"
                },
                new rule_engine.Rules.Action
                {
                    Key = "field_2",
                    Value = "Option B"
                }
            };
            string expectedString = JsonConvert.SerializeObject(expected, Formatting.Indented);
            Dictionary<string, string> state = new Dictionary<string, string>
            {
                { "field_1", "2/1/2020" },
                { "field_2", "Option B"}
            };
            JsonOperator underTest = new JsonOperator(new MockFileSystem());
            //Act
            string result = underTest.CreateOutput(state);
            //Assert
            Assert.Equal(expectedString, result);
        }
    }
}
