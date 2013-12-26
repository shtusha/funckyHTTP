﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34003
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace FunckyApp.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class BasicTestFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BasicTests.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BasicTest", "In order to keep it funcky\r\nI want to make sure proper response codes are returne" +
                    "d", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "BasicTest")))
            {
                FunckyApp.Tests.Features.BasicTestFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 5
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table1.AddRow(new string[] {
                        "Accept",
                        "application/xml"});
            table1.AddRow(new string[] {
                        "Content-Type",
                        "text/xml"});
#line 6
 testRunner.Given("request headers are", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "alias",
                        "namespace"});
            table2.AddRow(new string[] {
                        "a",
                        "http://schemas.datacontract.org/2004/07/FunckyApp.Models"});
#line 11
 testRunner.And("xml namespace aliases are", ((string)(null)), table2, "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Edge cases")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "BasicTest")]
        public virtual void EdgeCases()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edge cases", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 19
 testRunner.Given("url is \'scripts/InvalidIdentifier\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("response Status Code should be NotFound", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.Given("url is \'scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
 testRunner.When("I submit a delete request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.Then("response Status Code should be 405", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 29
 testRunner.Given("url is \'scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.When("I submit a post request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table3.AddRow(new string[] {
                        "Content-Length",
                        "0"});
#line 31
 testRunner.And("add headers", ((string)(null)), table3, "And ");
#line 34
 testRunner.Then("response Status Code should be 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Media type handling")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "BasicTest")]
        public virtual void MediaTypeHandling()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Media type handling", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 39
 testRunner.Given("url is \'scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table4.AddRow(new string[] {
                        "Accept",
                        "application/xml"});
#line 40
 testRunner.And("request headers are", ((string)(null)), table4, "And ");
#line 43
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("response header Content-Type should contain \'application/xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.Given("url is \'scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table5.AddRow(new string[] {
                        "Accept",
                        "application/json"});
#line 48
 testRunner.And("request headers are", ((string)(null)), table5, "And ");
#line 51
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.Then("response header Content-Type should be \'application/json; charset=utf-8\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Insert script")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "BasicTest")]
        public virtual void InsertScript()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Insert script", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 56
 testRunner.Given("url is \'scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 57
 testRunner.And("request content is FILE(Requests\\1+2.xml)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I submit a post request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 61
 testRunner.Then("response Status Code should be 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "expected",
                        "query"});
            table6.AddRow(new string[] {
                        "Id Exists",
                        "1",
                        "\'count(a:Script/a:Id)\'"});
            table6.AddRow(new string[] {
                        "Name matches",
                        "\'Test\'",
                        "\'string(//a:Name/text())\'"});
            table6.AddRow(new string[] {
                        "Program matches",
                        "\'var a = 1 + 2;\'",
                        "FILE(Queries\\program.q)"});
#line 62
 testRunner.And("the following assertions against response should pass:", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Put/Delete script")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "BasicTest")]
        public virtual void PutDeleteScript()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Put/Delete script", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 5
this.FeatureBackground();
#line 71
 testRunner.Given("url is \'scripts/1234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
 testRunner.And("request content is FILE(Requests\\2+3.xml)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.When("I submit a put request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.Then("response Status Code should be 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 77
 testRunner.Given("url is \'scripts/1234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 78
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
 testRunner.Then("response Status Code should be 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "expected",
                        "query"});
            table7.AddRow(new string[] {
                        "Id matches",
                        "\'1234567890\'",
                        "\'string(//a:Id/text())\'"});
            table7.AddRow(new string[] {
                        "Name matches",
                        "\'Test\'",
                        "\'string(//a:Name/text())\'"});
            table7.AddRow(new string[] {
                        "Program matches",
                        "\'var a = 2+3;\'",
                        "FILE(Queries\\program.q)"});
#line 80
 testRunner.And("the following assertions against response should pass:", ((string)(null)), table7, "And ");
#line 87
 testRunner.Given("url is \'scripts/1234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 88
 testRunner.When("I submit a delete request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 89
 testRunner.Then("response Status Code should be 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 92
 testRunner.Given("url is \'scripts/1234567890\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 94
 testRunner.Then("response Status Code should be 404", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion