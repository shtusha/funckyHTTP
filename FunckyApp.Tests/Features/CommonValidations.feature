Feature: CommonValidations
	In order to keep it funcky 
	I want to run common validations on ALL THE THINGS.

	@xml.namespaces.keep

	Background: 
	Given base url is 'http://localhost:37580/'
	And xml namespace aliases are
	| alias | namespace                                                |
	| a     | http://schemas.datacontract.org/2004/07/FunckyApp.Models |


Scenario Outline: CommonValidations

	Given url is 'Services/ScriptCompilerService.svc'
	And request content is <content>
	

	And request headers are
	| name         | value                                                                                      |
	| Accept       | application/xml                                                                            |
	| Content-Type | text/xml; charset=utf-8                                                                    |
	| SOAPAction   | http://schemas.datacontract.org/2004/07/FunckyApp.Services/CompilerServices/GetScriptStats |

	#use xslt to transform request content into a SOAP request to CompilerServices
	And xslt is FILE(XSLT\ScriptToSOAPGetScriptStatsRequest.xslt)
	When request content is transformed
	And I submit a post request

	Then response Status Code should be 200
	And the following assertions against response should pass:
	| name              | expected             | query                                                                         |
	| valid script      | <isValid>            | 'boolean(//a:IsValid)'                                                        |
	| no invalid tokens | <invalidTokensCount> | 'count(//a:InvalidTokens/child::node())'                                      |
	| # of identifiers  | <identifierCount>    | 'count(//a:IdentifierStats)'                                                  |
	| # of literals     | <literalCount>       | 'count(//a:NumericLiteralStatistics[1]/a:LiteralStats)'                       |
	| # of literals '3' | <3Count>             | 'number(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value='3']/a:Count)' |

	Scenarios: 
	| name  | content                  | isValid | invalidTokensCount | identifierCount | literalCount | 3Count |
	| 1_2   | FILE(Requests\1+2.xml)   | true    | 0                  | 1               | 2            | N/A    |
	| 2_3   | FILE(Requests\2+3.xml)   | true    | 0                  | 1               | 2            | 1      |
	| 3_4_5 | FILE(Requests\3+4+5.xml) | true    | 0                  | 1               | 3            | 1      |
	| b_c   | FILE(Requests\abc.xml)   | true    | 0                  | 3               | 0            | N/A    |
