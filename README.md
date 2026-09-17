\# Simple web application



\- Converts a dollar amount to words, via a web page.

\- 123.45 → ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS

\- .NET 9, C#.



\## Build



```bash

dotnet build

```



\## Run



```bash

dotnet run --project TechOneAPI --launch-profile https

```



\- Then open https://localhost:7095.

\- In Visual Studio 2022 or later, pick the https profile and press F5.

\- The page is served by the API itself, so no separate front-end build and no CORS setup.

\- Certificate warning on first run? Trust the dev cert:



```bash

dotnet dev-certs https --trust

```



\## Use



\- Enter an amount such as 123.45.

\- Click Convert, or press Enter.

\- Result shows below the input. Bad input shows a message instead.



\## Structure



\- TechOneCore — conversion logic. No web dependencies.

\- TechOneAPI — endpoint plus the web page in wwwroot.

\- TechOneTest — NUnit tests.



\## API



```

GET /api/numbertowords/convert-to-words?value=123.45

```



Every response uses the same shape:



```json

{ "isSuccess": true, "result": "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", "errMsg": null }

```



```json

{ "isSuccess": false, "result": null, "errMsg": "Input amount cannot be null. (Parameter 'amount')" }

```



\- 200 success, 400 bad input, 500 unexpected. Status codes still mean what they normally mean.



\## Tests



```bash

dotnet test

```



\## Documents



\- DESIGN.md

\- TEST-PLAN.md

