﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
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
    public partial class WebAPIToWCFFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "WebAPIToWCF.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WebAPIToWCF", "In order to keep it funcky I want to support calls \r\nto various types of http end" +
                    "points.", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "WebAPIToWCF")))
            {
                FunckyApp.Tests.Features.WebAPIToWCFFeature.FeatureSetup(null);
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
 #line 6
 testRunner.Given("base url is \'http://localhost:37580/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "alias",
                        "namespace"});
            table1.AddRow(new string[] {
                        "a",
                        "http://schemas.datacontract.org/2004/07/FunckyApp.Models"});
#line 7
 testRunner.And("xml namespace aliases are", ((string)(null)), table1, "And ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("JSON post to SOAP service")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "WebAPIToWCF")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("xml.namespaces.keep")]
        public virtual void JSONPostToSOAPService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("JSON post to SOAP service", new string[] {
                        "xml.namespaces.keep"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 5
 this.FeatureBackground();
#line 17
 testRunner.Given("url is \'api/scripts/\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table2.AddRow(new string[] {
                        "Accept",
                        "application/json"});
            table2.AddRow(new string[] {
                        "Content-Type",
                        "application/json"});
#line 18
 testRunner.And("request headers are", ((string)(null)), table2, "And ");
#line hidden
#line 22
 testRunner.And("request content is", "{\r\n\t\"Name\" : \"Foo\",\r\n\t\"Program\": \"var foo = 3 + 3 + 4;\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.When("I submit a post request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.Then("response Status Code should be 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.When("the following query is run against response: \'concat(\"api/scripts/\", //Id/text())" +
                    "\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.Then("all is cool", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.Given("url is query result", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table3.AddRow(new string[] {
                        "Accept",
                        "application/xml"});
            table3.AddRow(new string[] {
                        "Content-Type",
                        "application/xml"});
#line 38
 testRunner.And("request headers are", ((string)(null)), table3, "And ");
#line 42
 testRunner.When("I submit a get request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("response Status Code should be OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "expected",
                        "query"});
            table4.AddRow(new string[] {
                        "Id Exists",
                        "1",
                        "\'count(//a:Id)\'"});
            table4.AddRow(new string[] {
                        "Name matches",
                        "\'Foo\'",
                        "\'//a:Name/text()\'"});
            table4.AddRow(new string[] {
                        "Program matches",
                        "\'var foo = 3 + 3 + 4;\'",
                        "\'//a:Program/text()\'"});
#line 45
 testRunner.And("the following assertions against response should pass:", ((string)(null)), table4, "And ");
#line 53
 testRunner.Given("url is \'Services/ScriptCompilerService.svc\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "value"});
            table5.AddRow(new string[] {
                        "Accept",
                        "application/xml"});
            table5.AddRow(new string[] {
                        "Content-Type",
                        "text/xml; charset=utf-8"});
            table5.AddRow(new string[] {
                        "SOAPAction",
                        "http://schemas.datacontract.org/2004/07/FunckyApp.Services/CompilerServices/GetSc" +
                            "riptStats"});
#line 54
 testRunner.And("request headers are", ((string)(null)), table5, "And ");
#line 61
 testRunner.And("xslt is FILE(XSLT\\ScriptToSOAPGetScriptStatsRequest.xslt)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.When("response is transformed into request content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.And("I submit a post request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.Then("response Status Code should be 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "expected",
                        "query"});
            table6.AddRow(new string[] {
                        "valid script",
                        "true",
                        "\'//a:IsValid\'"});
            table6.AddRow(new string[] {
                        "no invalid tokens",
                        "0",
                        "\'count(//a:InvalidTokens/child::node())\'"});
            table6.AddRow(new string[] {
                        "has identifier foo",
                        "true",
                        "\'count(//a:IdentifierStats[a:Name=\'foo\'])>0\'"});
            table6.AddRow(new string[] {
                        "1 identifier foo",
                        "1",
                        "\'//a:IdentifierStats[a:Name=\'foo\']/a:Count\'"});
            table6.AddRow(new string[] {
                        "has literal(s) 3",
                        "true",
                        "\'count(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value=\'3\'])>0\'"});
            table6.AddRow(new string[] {
                        "2 literals 3",
                        "2",
                        "\'//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value=\'3\']/a:Count\'"});
            table6.AddRow(new string[] {
                        "has literal(s) 4",
                        "true",
                        "\'count(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value=\'4\'])>0\'"});
            table6.AddRow(new string[] {
                        "1 literal 4",
                        "1",
                        "\'//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value=\'4\']/a:Count\'"});
#line 66
 testRunner.And("the following assertions against response should pass:", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
