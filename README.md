# blockchain-signing-jwt

## Description

This is adds an endpoint to your project that will support the use of blockchain signatures as username/password to generate a JWT. This JWT can then be used to login to your app.

## Usage

```
var builder = WebApplication.CreateBuilder(args);

// Add services/config to the container.
builder.AddBlockchainSignatureVerification(options => builder.Configuration.GetSection("TokenGenerationOptions").Bind(options));

....

var app = builder.Build();

// Expose endpoint for getting token
app.AddBlockchainTokenIssuanceEndpoint(x => { });

....
```

## Request Body

```
GET ../blockchain/token
{
    "raw_message":"2024-12-23T11:20:20.682Z",
    "signature": "0x8b93726dc41f8a704b12f93349c471c605e9c1ea60a4302d85128b486e4723d94be330bc6b0238cf246e6ebfe7e95c532bf4b461ad424900b110f87f952a89c81c",
    "network": "Ethereum"
}
```

## Response

```
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwdWJLZXkiOiIweDUwMTE2Y0E2MDg0OUNCZmMxRDMwYjUwZEVkOGIyZGJCMThEMjFBRkEiLCJuYmYiOjE3MzQ5NTI4MzcsImV4cCI6MTczNDk1NjQzNywiaWF0IjoxNzM0OTUyODM3LCJpc3MiOiJodHRwczovL3NldC1tZSIsImF1ZCI6Imh0dHBzOi8vc2V0LW1lLXRvbyJ9.RNiEdAtaZYRzOUkifLc8nCAUZEGJskwrQLVwFx-X9zY",
    "expires_at": 3599
}
```

## Config

### Token Generation Options
```
{
    // This is the key used to generaate the token (private key). The default is set below: <b>CHANGE THIS!</b>
    "Secret": "8a1dd1f0-dc55-4e0e-91a0-81fe44417661",

    // This is the token expiry time. The default is set below:
    "ExpiresInMinutes": 60,

    // This is threshold for the current time - when a message has been generated (message contents). The default is set below:
    "ExpiresThresholdInSeconds": 30,

    // The issuer of the token
    "Issuer": "https://set-me",

    // The audience of the token
    "Audience": "https://set-me-too",
}
```

### Token Endpoint Options
```
{
    // This is the extension of your app where the endpoint to generate this token will live. The default is set below:
    "Extension": "blockchain/token"
}
```
