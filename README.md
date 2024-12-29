# blockchain-signing-jwt

## Description

This is adds an endpoint to your project that will support the use of blockchain signatures/public key to generate a JWT. This JWT can then be used to login to your app.

## Currently Supported Signings

- Ethereum,Polygon (EVM based) -> "Evm"
- Solana -> "Solana"
- Bitcoin -> "Bitcoin" [COMING SOON]
- Ton -> "Ton" [COMING SOON - has some bugs]

## Extensibility

- Create a new class that implements ISignatureService
- Add to DI as a keyed service ie.
```
// MyCustomNetworkSignatureService implements ISignatureService
builder.Services.AddCustomSignatureService<MyCustomNetworkSignatureService>("MyCustomNetwork");
```

Note: "MyCustomNetwork" will be what you use for "network" in the token request outlined below.

## Usage

```
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddBlockchainSignatureVerification(options => 
{
    // Setting options from config
    builder.Configuration.GetSection("TokenGenerationOptions").Bind(options);

    // Add event to add additional claims
    options.Events.OnSignatureValidation = (context) =>
    {
        var address = context.Address;

        context.Claims.Add(new System.Security.Claims.Claim("test", "test1"));

        return Task.CompletedTask;
    };
});

.....
```

```
var app = builder.Build();

// Expose endpoint for getting token
app.AddBlockchainTokenIssuanceEndpoint(options => builder.Configuration.GetSection("TokenEndpointOptions").Bind(options));

....
```

## Request

### Signed Message

The signed message must be a UTC date time string in the format 
'''
yyyy-MM-ddTHH:mm:ss.fffZ
'''

### Request Body

```
GET ../blockchain/token
{
    // The message must be a datetime in this format. This (NOW) must be sent within the threshold mentioned in config below. 
    "raw_message":"2024-12-23T11:20:20.682Z",

    // A signed message (the plain text sent above). 
    "signature": "0x8b93726dc41f8a704b12f93349c471c605e9c1ea60a4302d85128b486e4723d94be330bc6b0238cf246e6ebfe7e95c532bf4b461ad424900b110f87f952a89c81c",

    // The network type to check signing for
    "network": "Evm"

    // If the networks algorithm does NOT support recoverable public keys then this should be populated.
    // EVM signings support recoverable keys so we do NOT need to pass with this request
    "address": null
}
```

## Response

```
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhZGRyZXNzIjoiMHgxRkJFM0RmN0VlQmY4OTRmODNjMGVFMmIxYTEwZDFCMWE1Y2RhNDU0IiwibmV0d29yayI6IkV2bSIsInRlc3QiOiJ0ZXN0MSIsIm5iZiI6MTczNTQ2NDY1MiwiZXhwIjoxNzM1NDY4MjUyLCJpYXQiOjE3MzU0NjQ2NTIsImlzcyI6Imh0dHBzOi8vc2V0LW1lIiwiYXVkIjoiaHR0cHM6Ly9zZXQtbWUtdG9vIn0.OFpjY6XxilHHz7ei6hbTW50VF-DfurDysNC06DMzTig",
    "refresh_token": null // W.I.P
    "expires_at": 3599
}
```

### Decoded Token
```
{
  "alg": "HS256",
  "typ": "JWT"
}
{
  "address": "0x1FBE3Df7EeBf894f83c0eE2b1a10d1B1a5cda454",
  "network": "Evm",
  "test": "test1", // Added custom via "OnSignatureValidation" event
  "nbf": 1735464652,
  "exp": 1735468252,
  "iat": 1735464652,
  "iss": "https://set-me",
  "aud": "https://set-me-too"
}
```

## Config

### Token Generation Options
```
{
    // This is the key used to generate the token (private key).
    "Secret": "8a1dd1f0-dc55-4e0e-91a0-81fe44417661",

    // This is the token expiry time. 
    "ExpiresInMinutes": 60,

    // This is threshold for the current time - when a message has been generated (message contents).
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

## Testing

- Unit tests
- A test Javascript file has been added [here](https://github.com/mshrm-studio/blockchain-signing-jwt/blob/main/Blockchain.Signing.Auth/Blockchain.Signing.Auth/JS/get-signature.js) to generate a signature using Metamask
