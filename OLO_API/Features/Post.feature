Feature: Post
	Feature will test for the 
	functionality of all the provided endpoints
	to be behaving as expected


Scenario: Post GET
	When user performs "GET" from "/posts"
	Then the status code should be 200
	And response body should be a valid JSON object

Scenario: Post POST
	Given post object has been loaded with:
	| userId | title            | body                                        |
	| 1      | "OLO Test Title" | "OLO body is a really big string ho ho ho!" |
	When user performs "POST" from "/posts"
	Then the status code should be 201
	And response body should match the provided object data

Scenario: Post PUT
	Given post object has been loaded with:
	| userId | id | title            | body                                        |
	| 1      | 1  | "OLO Test Title" | "OLO body is a really big string ho ho ho!" |
	When user performs "PUT" from "/posts/1"
	Then the status code should be 200
	And response body should match the provided object

#failing because patch is setting null properties
#Including ID
Scenario: Post PATCH
	Given post object has been loaded with:
	| userId | body                                        |
	| 1      | "OLO body is a really big string ho ho ho!" |
	And original object is loaded performing "GET" from "/posts/1"
	When user performs "PATCH" from "/posts/1"
	Then the status code should be 200
	And response body should match original object except for the provided properties


Scenario: Post DELETE
	Given post object has been loaded with:
	| userId | title            | body                                        |
	| 1      | "OLO Test Title" | "OLO body is a really big string ho ho ho!" |
	When user performs "POST" from "/posts"
	Then response body should be a valid object
	When user performs "DELETE" from "/posts/{newlyCreatedId}"
	Then the status code should be 200
	When user performs "GET" from "/posts/{newlyCreatedId}"
	Then the status code should be 404



#failing because this is returning 500 for an invalid ID
Scenario: Post PUT - Invalid ID
	Given post object has been loaded with:
	| userId | id | title            | body                                        |
	| 1      | 1  | "OLO Test Title" | "OLO body is a really big string ho ho ho!" |
	When user performs "PUT" from "/posts/999"
	Then the status code should be 404


#failing because this is returning 200 for an invalid ID
Scenario: Post PATCH - Invalid ID
	Given post object has been loaded with:
	| userId | id |  body                                       |
	| 1      | 1  | "OLO body is a really big string ho ho ho!" |
	When user performs "PATCH" from "/posts/999"
	Then the status code should be 404



Scenario: Post GET - Invalid ID
	When user performs "GET" from "/posts/999"
	Then the status code should be 404

#failing because this is returning 200 for an invalid ID
Scenario: Post DELETE - Invalid ID
	When user performs "DELETE" from "/posts/9999"
	Then the status code should be 404