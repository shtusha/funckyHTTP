Feature: JSON Tests
	In order to keep it funcky
	I want to make sure I can handle JSON

Background:
	Given request headers are
	| name         | value            |
	| Accept       | application/json |
	| Content-Type | application/json |


@rootElementName @Script
Scenario: Insert script (JSON)

	Given url is 'scripts/'
	And request content is FILE(Requests\3+4.js)

	When I submit a post request
	
	Then response Status Code should be 200
	And the following assertions against response should pass:
	| name            | expected         | query                         |
	| Id Exists       | 1                | 'count(//Id)'                 |
	| Name matches    | 'Test'           | 'string(/Script/Name/text())' |
	| Program matches | 'var a = 3 + 4;' | 'string(//Program/text())'    |
	
	#repeats last test from table above
	When the following query is run against response: 'string(//Program/text())'
	Then query result should match 'var a = \d \+ \d;'

Scenario: Put/Delete script (JSON)

#First put a document
	Given url is 'scripts/3plus4'
	And request content is FILE(Requests\3+4.js)
	When I submit a put request
	Then response Status Code should be 204
	
#Get the document..content should match
	Given url is 'scripts/3plus4'
	When I submit a get request
	Then response Status Code should be 200
	And the following assertions against response should pass:
	| name            | expected         | query                      |
	| Id matches      | '3plus4'         | 'string(//Id/text())'      |
	| Name matches    | 'Test'           | 'string(//Name/text())'    |
	| Program matches | 'var a = 3 + 4;' | 'string(//Program/text())' |

#Delete the document
	Given url is 'scripts/3plus4'
	When I submit a delete request
	Then response Status Code should be 204

#Try getting the document, should be not found.
	Given url is 'scripts/3plus4'
	When I submit a get request
	Then response Status Code should be 404