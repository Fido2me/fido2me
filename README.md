# fido2me

TLDR: FIDO + OIDC Server. A user authentication gateway to a passwordless world.
Try at [Fido2me.com](https://fido2me.com)

## What
Inspired by [Webauthn.io](https://webauthn.io) and based on [Fido2-net-lib](https://github.com/passwordless-lib/fido2-net-lib) it is an attempt to bring closer passwordless future.
The end goal is to build a new incarnation of ["OpenID"](https://openid.net/start-using-your-openid/) social provider but based on modern authention protocols - [FIDO2](https://fidoalliance.org/fido2/).

## Why
Big corporations are already providing passwordless experience in addition to old passwords. However, there are two problems with that:
- (B2B) FIDO2 feature is not available at the basic tier. You will most likely need to go with the most expensive Enterprise edition.
- (B2C) Although FIDO2 is provided to you, you cannot disable a fallback to less secure options (passwords, phone, and text MFA).
Accessibility is a good excuse, but first, they need to solve account recovery options without using passwords. 
[Passkeys](https://fidoalliance.org/multi-device-fido-credentials/) can be a solution, but the same key is duplicated across multiple devices.

The device-bound credentials are the next logical step. But the recovery challenge should be solved too. It is impractical to think that users will buy, test, and use multiple roaming authenticators, e.g. Yubikey. Not all of them. It's too complicated and expensive for the majority of the users.

The project Fido2Me will try to provide a user-friendly way to add multiple device authenticators to your account. Windows, Mac, Security Key, IOS, Android. Use any combination of them to have enough redundancy for your authentication. Fido2Me will not support password-based flows. No traditional recovery. Just add more authenticators. But Fido2Me is not a library. It is a service. The second piece is a full-sized OAuth server (based on [Duende](https://duendesoftware.com/products/identityserver)). Once it's ready, anyone should be able to create an app (confidential, private, or device flow OAuth client). OAuth is great; you can use existing OAuth libraries to integrate into Fido2Me and provide a new social login experience.

## Why should I support / try this project?
OpenID social provider was a great attempt to reduce the number of accounts. Unfortunately, it didn't become very popular. Facebook, Google and MS social providers have many more users. I guess I'm crazy enough to try again and provide a new independent social login. The main project is available under the AGPL license.
I do support privacy. Something that you will never get from big companies. More information will be available later. In short, I don't need a lot to provide authentication functionality. Everything else will be optional to create a proper OpenID profile to be used by other applications (OAuth apps).

## Is it ready?
It's ready to try and feel a new passwordless experience. Username and usernameless flows are combined in a way it will work on most platforms. The storage is permanent, not just in memory.

## What is stored?
You need to provide a nickname only. If you want to be "fluffybunny", so be it. An email address is optional; it will only be used for important notifications.
Device attestation is required - to prepopulate a device name to manage multiple authenticators. All attestations are accepted. 
In the future, an optional OpenID profile will be supported.

## How to integrate?
Create an OAuth application (confidential or private), save `client id` and `client secret`. Use existing OAuth knowledge and get identity from fido2me. Try Confidential client (with or without PKCE), only authorization code grant is supoorted so far. CIBA and Device flow can be added later.

## Technologies
- ASP.NET Core 6 (backend)
- Vanilla JS / Fetch API (frontend). JQuery is gone.
- Bootstrap 5
- Cosmos DB (database)
- Application Insights (tracing)
- Github (Actions + Dependabot)
- Azure (hosting)
- Cloudflare (CDN, protection)
Soon will be added: Github Code Scanning / SonarCloud

## Need to run locally
You will need to install the Azure Cosmos DB emulator or use a cloud database instance along with created an Application Insight service instance. I'm using Azure Key Vault to get access to secrets. More instructions are needed here!
