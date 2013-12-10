Feature: BasicTest
	In order to keep it funcky
	I want to make sure proper response codes are returned

Background:
	Given request headers are
	| name         | value           |
	| Accept       | application/xml |
	| Content-Type | text/xml        |
	
Scenario: Get non existent script
	
	Given url is 'scripts/InvalidIdentifier'
	
	When I submit a get request
	Then response Status Code should be NotFound
	And response Status Code should be 404

	
Scenario: Insert script

	Given url is 'scripts/'
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
	
#	Scenario: Get all the scripts

#	Given url is 'scripts/'
#	And xml namespace aliases are
#	| alias | namespace                                                 |
#	| a     | http://schemas.microsoft.com/2003/10/Serialization/Arrays |

#	When I submit a get request
#	Then response Status Code should be OK
#	When the following query is run against response: 'count(a:ArrayOfString/*)'
#	Then the result should be 1
