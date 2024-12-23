# blockchain-signing-jwt

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
