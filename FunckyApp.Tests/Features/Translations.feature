Feature: Translations
	In order to use Inflationary fiveum APIs
	As an api consumer
	I need to make sure phrases get properly translated

	
	Scenario Outline: Translations
	
	Given url is 'api/posts/preview'
	And request headers are
	| name         | value              |
    | Content-Type | 'application/json' |
	
	And request content is 
	"""
	{
	 message: "<request content>",
	 inflationRate: <inflRate>
	}
	"""
	
	When I submit a POST request
	
	Then response Status Code should be 200

	When the following query is run against response: '//inflated'
	Then the result should be <expected>

	Scenarios: 
	| request content                               | inflRate | expected                                                     |
	| It took us forever to get home                | 1        | 'It threek us fivever three get home'                        |
	| inflated messages tend to be complicated      | 1        | 'inflnined messages elevend three be complicnined'           |
	| To be or not to be?                           | 3        | 'Five be or not five be?'                                    |
	| Once upon a time someone ate five tenderloins | 2        | 'Three times upon a time somethree ten seven twelvederloins' |


	#If a phrase cannot be inflated, then nflation rate is irrelevant
	Scenario Outline: Non Inflatable
	
	Given url is 'api/posts/preview'
	And request headers are
	| name         | value              |
    | Content-Type | 'application/json' |
	
	And request content is 
	"""
	{
	 message: "platinum, gold, silver",
	 inflationRate: <inflation rate>
	}
	"""
	
	When I submit a POST request
	
	Then response Status Code should be 200

	When the following query is run against response: '//inflated'
	Then the result should be 'platinum, gold, silver'

	Scenarios: 
	| inflation rate |
	| 1              |
	| 2              |
	| 3              |
	| 4              |
	| 5              |