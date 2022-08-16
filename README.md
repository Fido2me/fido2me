# fido2me

## What
Inspired by [Webauthn.io](https://webauthn.io) and based on [Fido2-net-lib](https://github.com/passwordless-lib/fido2-net-lib) it is an attempt to bring closer passwordless future.
The end goal is to build a new incarnation of ["OpenID"](https://openid.net/start-using-your-openid/) social provider but based on modern authention protocols - [FIDO2](https://fidoalliance.org/fido2/).

## Why
Big corporations are already providing passwordless experience in additional to old passwords. There are 2 problems with that:
- (B2B) FIDO2 feature is not available at basic tier. Most likely you will need to go with the most expensive Enterprise edition.
- (B2C) Although FIDO2 is provided to you, you cannot disable a fallback to less secure options (passwords, phone and text MFA).

Accessibility is an honorable excuse, but first they need to solve account recovery option without using passwords.
[Passkeys](https://fidoalliance.org/multi-device-fido-credentials/) can be a solution, but the same key is duplicated across multiple devices.

Device-bound credential is the next logical step. But the recovery challenge should be solved for it too. It is impractical to think that users will buy, test, and use multiple roaming authenticators, e.g. Yubikey. Not all of them, it's too complicated and expensive for the majority of the users.

The project **Fido2Me** will try to provide a user-friendly way to add multiple device authenticators to your account. Windows, Mac, Security Key, IOS, Android. Use any combination of them to have enough redundancy for your authentication. Fido2Me will not support password-based flows. No traditional recovery. Just add more authenticators.
But Fido2Me is not a library. It is a **service**. The second piece is a full-sized OAuth server (based on [Duende](https://duendesoftware.com/products/identityserver)). Once it's ready, anyone should be able to create an app (confidential, private, or device flow OAuth client).
OAuth is great, you can use exisiting OAuth libraries to integrate to Fido2Me and provide a new social login experience.

## Why should I support / try this project?
OpenID provider was a great attempt to reduce the number of accounts. Unfortunatelly it didn't become very popular. Facebook, Google and MS social providers have many more users. I guess I'm crazy enough to try again and provide a new independent social login. The main project is available under the AGPL license.

I do support privacy. Something that you will never get from big companies. More information will be available later. In short, I don't need a lot to provide authentication functionality. Everything else will be optional to create a proper OpenID profile to be used by other applications (OAuth apps).

## Is it ready?
It's ready to be tested. I'm trying to combine username and usernameless flows that will work on the majority of platforms.
The storage is not temporary, however the data can be purged in some rare conditions, it is still Beta.

## Technologies
- ASP.NET Core 6 (backend)
- Vanilla JS / Fetch API (frontend). JQuery is referenced but lightly used and soon will be gone.
- Bootstrap 5
- Cosmos DB (database)
- Application Insights (tracing)
- Github (Actions + Dependabot)
- Azure (hosting)
- Cloudflare (CDN, protection)

Soon will be added: Github Code Scanning / SonarCloud

## Need to run locally

You will need to install the Azure Cosmos DB emulator or use a cloud database instance along with created an Application Insight service instance.
I'm using Azure Key Vault to get access to secrets. More instructions are needed here!
