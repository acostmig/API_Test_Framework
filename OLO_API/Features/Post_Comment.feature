Feature: Post_Comment
	Feature will test for the 
	functionality of all the provided endpoints
	to be behaving as expected


Scenario: Comment GET
	When user performs "GET" from "/comments?postId=1"
	Then the status code should be 200
	And response body should be a valid JSON object

Scenario: Comment POST
	Given comment object has been loaded with:
	| postId | name            | email                | body                               |
	| 1      | "Miguel Acosta" | "acostmig@gmail.com" | "This is the actual comment body!" |
	When user performs "POST" from "/posts/1/comments"
	Then the status code should be 201
	And response body should match the provided object data

#Failing because is returning 304 rather than 404 for invalid post ID 
Scenario: Comment GET - Invalid ID
	When user performs "GET" from "/comments?postId=999"
	Then the status code should be 404
	And response body should be a valid JSON object

#Failing because a failure should happen for invalid email
Scenario: Comment POST - Invalid Email
	Given comment object has been loaded with:
	| postId | name            | email                | body                               |
	| 1      | "Miguel Acosta" | "Expecting failure" | "This is the actual comment body!" |
	When user performs "POST" from "/posts/1/comments"
	Then the status code should be 400
	And response body should match the provided object data