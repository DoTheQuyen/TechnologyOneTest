\# Design Document



\## Requirement



\- Convert a dollar amount to words, via a web page.

\- 123.45 → ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS



\## Structure



\- TechOneCore — conversion logic. No web dependencies.

\- TechOneAPI — endpoint plus the web page in wwwroot.

\- TechOneTest — NUnit tests.

\- Logic stays in the service. Controller just passes the request through.

\- TechOneCore registers itself via AddSharedServices(), so Program.cs never names a concrete class.

\- Rejected — one project. Logic would mix with controller code, and tests would need a web host.



\## Algorithm



\- Split the dollars into groups of three digits from the right.

\- Convert each group, add a scale word by position (THOUSAND, MILLION, …).

\- Three cases inside a group, because English names them differently: 0-9, 10-19 — irregular (ELEVEN, TWELVE), 20-99 — tens word plus optional hyphenated unit

\- English repeats every three digits, so the group code is written once and reused. Adding a scale is one dictionary entry.

\- Rejected — recursion. Works, but the position-to-scale link is hidden in the call stack instead of visible as an index.



\## Lookup tables



\- Dictionary of int to string, keyed by the number the word spells. 20 maps to TWENTY.

\- Teens\[13] reads as the word for thirteen. No counting positions when editing.

\- Rejected — arrays. Developer has to remember what each index means. Easy to get wrong.



\## Input type



\- Requirement says numerical twice, so string and char are out of scope.

\- Requirement does not ban nullable numeric, so it's allowed by default.

\- Decimal because it is exact for money. Double gives 28 cents for 1.29.

\- Nullable because plain decimal turns bad input into 0, so abc would return ZERO DOLLARS.



\## Cases not in the requirement, assume allowed by default



\- Zero — allowed, returns ZERO DOLLARS.

\- Null — throws ArgumentNullException. A missing parameter should error, not default.

\- Negative — allowed, prefixed MINUS.



\## Validation



\- Browser: regex, optional minus, up to 19 digits, max 2 decimals. Fast, clear message.

\- Model binding: string to decimal. Bad input returns 400 automatically.

\- Service: null and range guards.

\- Browser checks are for the message only. Endpoint can be called directly, so the server is the real check. Never trust the inputs



\## Errors and response



\- Service throws typed exceptions.

\- HandleRequest in BaseApiController maps ArgumentException to 400, everything else to 500 with a log.

\- Every response uses ResponseDTO:



```json

{ "isSuccess": true, "result": "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", "errMsg": null }

```



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

