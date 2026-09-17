\# Test Plan



\## Scope



\- In: conversion logic, validation, status codes, the web page.

\- Out: load testing, cross-browser, auth (endpoint is anonymous by design).



\## Run



```bash

dotnet test

```



\## Unit tests



Cases live in TechOneTest/NumberToWordsConverterTests.cs, one group per branch:



\- Single digits, teens, tens hyphenation, hundreds with AND.

\- All scales to QUINTILLION, empty groups skipped, AND between groups.

\- Cents and plurality, negatives, rounding away from zero, cents rolling into a dollar.

\- Boundaries — long.MaxValue converts, above it throws. Null throws.



Exception type is asserted, not just that something threw, since the controller picks 400 or 500 from the type.



\## Manual checks



App running at https://localhost:7095:



\- Valid value → 200 and correct words. Negative → MINUS prefix.

\- No value, abc, or 20 digits → 400 with a message.

\- GET / loads the page. Convert works by button and by Enter.

\- Bad input shows a client message and sends no request, and clears any old result.

\- Server stopped → could not reach the server.



\## Bugs found by testing



\- Validation errors returned 500, not 400 — the service caught its own ArgumentException and rethrew as plain Exception, losing the type.

\- Missing value threw in the controller before validation, because .Value was read off a null decimal?.

\- 0.999 gave ONE HUNDRED CENTS, and 0.125 gave 12 cents from banker's rounding. Both fixed by rounding once, away from zero.

\- Page could not call the API when opened from disk. Fixed by serving it from wwwroot.



\## Result



All unit tests pass. All manual checks behave as listed.

