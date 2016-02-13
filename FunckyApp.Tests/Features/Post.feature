Feature: Posts
	In order to keep it funcky
	As an API consumer
	I want to create and retriveve inflationary messages

Scenario: Create and get Post
	#Login
	Given url is '/Token'
	And request header Content-Type is 'application/x-www-form-urlencoded'
	And request content is 'grant_type=password&username=FunckyUser&password=1234567'
	When I submit a POST request
	Then response Status Code should be OK

	#Construct auth header
	When the following query is run against response: 'concat("Bearer ", //access_token/text())'
	Then all is cool
	
	Given url is 'api/posts'
	And GLOBAL request headers are
	| name          | value              |
	| Accept        | 'application/xml'  |
	| Content-Type  | 'application/json' |
	| Authorization | query result       |

	And request content is '{ message: "one plus two equals three", inflationRate: 1 }'
	When I submit a POST request
	
	Then response Status Code should be 201
	And response header Location should exist

	Given url is response header Location
	When I submit a GET request
	Then response Status Code should be OK

	And the following assertions against response should pass:
	| expected     | query                                     |
	| 'one'        | '//TextFragmentViewModel[1]/OriginalText' |
	| 'two'        | '//TextFragmentViewModel[1]/InflatedText' |
	| true         | '//TextFragmentViewModel[1]/IsInflated'   |
	| ' plus '     | '//TextFragmentViewModel[2]/OriginalText' |
	| false        | '//TextFragmentViewModel[2]/IsInflated'   |
	| 'two'        | '//TextFragmentViewModel[3]/OriginalText' |
	| 'three'      | '//TextFragmentViewModel[3]/InflatedText' |
	| true         | '//TextFragmentViewModel[3]/IsInflated'   |
	| ' equals '   | '//TextFragmentViewModel[4]/OriginalText' |
	| false        | '//TextFragmentViewModel[4]/IsInflated'   |
	| 'three'      | '//TextFragmentViewModel[5]/OriginalText' |
	| 'four'       | '//TextFragmentViewModel[5]/InflatedText' |
	| true         | '//TextFragmentViewModel[5]/IsInflated'   |
	| 'FunckyUser' | '//Author'                                |
	
	When the following query is run against response: '//Link[Rel/text() = "self"]/Href/text()'
	Then all is cool

	#Get resource url from self link 
	Given url is query result
	And request headers are
    | name   | value              |
    | Accept | 'application/json' |
	When I submit a GET request

	Then response Status Code should be 200
	And the following assertions against response should pass:
	| expected     | query                         |
	| 'one'        | '//fragments[1]/originalText' |
	| 'two'        | '//fragments[1]/inflatedText' |
	| true         | '//fragments[1]/isInflated'   |
	| ' plus '     | '//fragments[2]/originalText' |
	| false        | '//fragments[2]/isInflated'   |
	| 'two'        | '//fragments[3]/originalText' |
	| 'three'      | '//fragments[3]/inflatedText' |
	| true         | '//fragments[3]/isInflated'   |
	| ' equals '   | '//fragments[4]/originalText' |
	| false        | '//fragments[4]/isInflated'   |
	| 'three'      | '//fragments[5]/originalText' |
	| 'four'       | '//fragments[5]/inflatedText' |
	| true         | '//fragments[5]/isInflated'   |
	| 'FunckyUser' | '//author'                    |


