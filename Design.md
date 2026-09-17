

\## Design.md



```markdown

\# Design Document



\## Requirement



\- Convert a dollar amount to words, using a web page.

\- Example: 123.45 returns ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS.



\## Structure



\- TechOneCore: the conversion logic. No web dependencies.

\- TechOneAPI: the endpoint, and the web page in wwwroot.

\- TechOneTest: NUnit tests.

\- Every logic should be in the service layer, so the controller only passes the request through and returns the response.

\- TechOneCore registers itself with AddSharedServices(), so Program.cs does not need to know the concrete class.

\- I did not put everything in one project, because then the logic would mix with the controller code, and the tests would need a web host to run.



\## Algorithm



\- Split the dollars into groups of three digits, starting from the right.

\- Convert each group, then add the scale word based on the position of the group (THOUSAND, MILLION, and so on).

\- Inside a group there are three cases, because English names them differently:

&#x20; 0 to 9, then 10 to 19 which are irregular (ELEVEN, TWELVE), then 20 to 99 which use the tens word with an optional hyphenated unit.

\- English repeats the same pattern every three digits, so the group code is written once and reused. 

&#x20; To support a bigger number, I only need to add one more entry to the scales dictionary.

\- I did not use recursion. It also works, but then the link between the position and the scale word is hidden in the call stack, instead of being visible as an index.



\## Lookup tables



\- I used Dictionary of int to string, and the key is the number that the word spells. So 20 maps to TWENTY.

\- teens\[13] reads as the word for thirteen, so there is no need to count positions when editing the table.

\- I did not use arrays, because then the developer has to remember what each index means, and it is easy to get wrong.



\## Input type



\- The requirement says numerical twice, so string and char are out of scope.

\- The requirement does not say that nullable is not allowed, so it is allowed by default.

\- I used decimal, because it is exact for money. Double is not exact, and it gives 28 cents for 1.29.

\- I used nullable, because a plain decimal turns bad input into 0, so abc would return ZERO DOLLARS instead of an error.



\## Cases not in the requirement



The requirement does not mention about these cases, so I allowed them by default:



\- Zero: allowed, returns ZERO DOLLARS.

\- Null: throws ArgumentNullException. A missing parameter should return an error, not a default value.

\- Negative: allowed, with MINUS as the prefix.



\## Validation



\- Browser: a regex for optional minus, up to 19 digits, and maximum 2 decimals. This is fast and gives a clear message.

\- Model binding: string to decimal. If it fails, the response is 400 automatically.

\- Service: the null check and the range check.

\- The browser check is only for the message. The endpoint can also be called directly, so the server is the real check. Never trust the input from the client.



\## Errors and response



\- The service throws typed exceptions.

\- HandleRequest in BaseApiController maps ArgumentException to 400, and everything else to 500 with a log.

\- The response uses ResponseDTO:



```json

{ "isSuccess": true, "result": "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", "errMsg": null }



\- Front end or any third-party client always gets the same shape, success or failure.

\- Status codes still mean what they normally mean.

\- BaseApiController also holds the route attributes, so every endpoint behaves the same.

\- No auth, as the requirement.



\## Web page



\- Plain HTML with fetch, served from wwwroot by the same app.

\- Same origin, so no CORS and no build step.

\- Fieldset, legend and output give the result box for free.



\## Limits



\- Max is long.MaxValue, since dollars go through a long. Guarded before the cast. Scales go to QUINTILLION.

\- Rounds away from zero. The .NET default is banker's rounding, which turns 0.125 into 0.12.

\- Rounded once up front, so cents never exceed 99. 0.999 becomes ONE DOLLAR.

\- Dollars and cents only. Upper case only.

