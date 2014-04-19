Feature: WebAPIToWCF
	In order to keep it funcky I want to support calls 
	to various types of http endpoints.

	Background: 
	Given base url is 'http://localhost:37580/'
	And xml namespace aliases are
	| alias | namespace                                                |
	| a     | http://schemas.datacontract.org/2004/07/FunckyApp.Models |


@xml.namespaces.keep

Scenario: JSON post to SOAP service

	#first post new script
	Given url is 'api/scripts/'
	And request headers are
	| name         | value            |
	| Accept       | application/json |
	| Content-Type | application/json |
	And request content is 
	"""
	{
		"Name" : "Foo",
		"Program": "var foo = 3 + 3 + 4;"
	}
	"""

	When I submit a post request
	Then response Status Code should be 200

	#build url for next request by extracting Id from response
	When the following query is run against response: 'concat("api/scripts/", //Id/text())'
	Then all is cool

	Given url is query result
	And request headers are
	| name         | value           |
	| Accept       | application/xml |
	| Content-Type | application/xml |
	When I submit a get request

	Then response Status Code should be OK
	And the following assertions against response should pass:
	| name            | expected               | query                        |
	| Id Exists       | 1                      | 'count(//a:Id)'              |
	| Name matches    | 'Foo'                  | 'string(//a:Name/text())'    |
	| Program matches | 'var foo = 3 + 3 + 4;' | 'string(//a:Program/text())' |
	
	
	#ready to build the SOAP call
	Given url is 'Services/ScriptCompilerService.svc'
	And request headers are
	| name         | value                                                                                      |
	| Accept       | application/xml                                                                            |
	| Content-Type | text/xml; charset=utf-8                                                                    |
	| SOAPAction   | http://schemas.datacontract.org/2004/07/FunckyApp.Services/CompilerServices/GetScriptStats |

	#use xslt to transform response from previous request into a SOAP request to CompilerServices
	And xslt is FILE(XSLT\ScriptToSOAPGetScriptStatsRequest.xslt)
	When response is transformed into request content
	And I submit a post request

	Then response Status Code should be 200
	And the following assertions against response should pass:
	| name               | expected | query                                                                         |
	| valid script       | true     | 'boolean(//a:IsValid)'                                                        |
	| no invalid tokens  | 0        | 'count(//a:InvalidTokens/child::node())'                                      |
	| has identifier foo | true     | 'count(//a:IdentifierStats[a:Name='foo'])>0'                                  |
	| 1 identifier foo   | 1        | 'number(//a:IdentifierStats[a:Name='foo']/a:Count)'                           |
	| has literal(s) 3   | true     | 'count(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value='3'])>0'        |
	| 2 literals 3       | 2        | 'number(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value='3']/a:Count)' |
	| has literal(s) 4   | true     | 'count(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value='4'])>0'        |
	| 1 literal 4        | 1        | 'number(//a:NumericLiteralStatistics[1]/a:LiteralStats[a:Value='4']/a:Count)' |