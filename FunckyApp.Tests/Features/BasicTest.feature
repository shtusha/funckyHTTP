Feature: BasicTest
	In order to keep it funcky
	I want to make sure proper response codes are returned

Background:
	Given request headers are
	| name         | value           |
	| Accept       | application/xml |
	| Content-Type | text/xml        |
	
Scenario: Edge cases

#Get non existent script
	Given url is scripts/InvalidIdentifier
	When I submit a get request
	Then response Status Code should be NotFound

#Send unsupported verb
	Given url is scripts/
	When I submit a delete request
	Then response Status Code should be 405

#Send empty post
	Given url is scripts/
	When I submit a post request
	And add headers
	| name           | value |
	| Content-Length | 0     |
	Then response Status Code should be 400
	
Scenario: Mime Type handling

#When accept header is xml response should be xml
	Given url is scripts/
	And request headers are
	| name   | value           |
	| Accept | application/xml |
	When I submit a get request
	Then response header Content-Type should contain 'application/xml'

#When accept header is xml response should be json	
	Given url is scripts/
	And request headers are
	| name   | value           |
	| Accept | application/json |
	When I submit a get request
	Then response header Content-Type should be 'application/json; charset=utf-8'

Scenario: Insert script

	Given url is scripts/
	And xml namespace aliases are
	| alias | namespace                                                |
	| a     | http://schemas.datacontract.org/2004/07/FunckyApp.Models |
	And request content is
	"""
	<Script xmlns="http://schemas.datacontract.org/2004/07/FunckyApp.Models">
		<Name>Test</Name>
		<Program>var a = 1+2;</Program>
	</Script>
	"""

	When I submit a post request
	Then response Status Code should be 200

	When the following query is run against response: 'count(a:Script/a:Id)'
	Then the result should be 1

	When the following query is run against response: 'string(//a:Name/text())'
	Then the result should be 'Test'

	When the following query is run against response: 'string(//a:Program/text())'
	Then the result should be 'var a = 1+2;'

	And the following assertions against response should pass:
	| name            | expected       | query                        |
	| Id Exists       | 1              | 'count(a:Script/a:Id)'       |
	| Name matches    | 'Test'         | 'string(//a:Name/text())'    |
	| Program matches | 'var a = 1+2;' | 'string(//a:Program/text())' |

Scenario: Put/Delete script
	
	Given xml namespace aliases are
	| alias | namespace                                                |
	| a     | http://schemas.datacontract.org/2004/07/FunckyApp.Models |


#First put a document
	Given url is scripts/1234567890
	And request content is
	"""
	<Script xmlns="http://schemas.datacontract.org/2004/07/FunckyApp.Models">
		<Id>1234567890</Id>
		<Name>Test</Name>
		<Program>var a = 2+3;</Program>
	</Script>
	"""
	When I submit a put request
	Then response Status Code should be 204
	
#Get the document..content should match
	Given url is scripts/1234567890
	When I submit a get request
	Then response Status Code should be 200
	And the following assertions against response should pass:
	| name            | expected       | query                        |
	| Id matches      | '1234567890'   | 'string(//a:Id/text())'      |
	| Name matches    | 'Test'         | 'string(//a:Name/text())'    |
	| Program matches | 'var a = 2+3;' | 'string(//a:Program/text())' |

#Delete the document
	Given url is scripts/1234567890
	When I submit a delete request
	Then response Status Code should be 204

#Try getting the document, should be not found.
	Given url is scripts/1234567890
	When I submit a get request
	Then response Status Code should be 404