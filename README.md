<p align="center">
  <img src="https://i.imgur.com/MmamZlQ.png" alt="vinder.logo" />
</p>

<h1 align="center">IDENTITY PROVIDER</h1>

### About This Project

Yes, we're yet another Identity Provider. There are much more robust options out there like **Keycloak**, **Auth0**, **Okta**, and **IdentityServer**, but our focus is simplicity.

With our Identity Provider, you can quickly spin up your identity service — all you need is a MongoDB instance, which can easily be obtained for free with 512MB on [MongoDB Atlas](https://www.mongodb.com/cloud/atlas) (at least as of today).

Our solution is also fully multi-tenant: each tenant you create can have its own users, permissions, and more.

We focus on what really matters in most cases: user identity and permissions.

Have you ever stopped to look at a JWT token issued by Keycloak? How many of those claims do you actually use? Maybe I’m being ignorant or naive, but this project was born from the need to keep it simple — focusing only on what is essential.

Ideal for small to medium projects, startups, or teams that need a customizable identity solution without heavy overhead. Not intended to replace enterprise-grade providers in large-scale scenarios, but designed for teams focused on delivering value quickly with minimal complexity.

Specifically, this provider is ideal for server-to-server scenarios, where one application needs to authenticate and consume APIs of another application. By only supporting the `client_credentials` flow, we are intentionally limiting the supported flows. This is fully valid for server-to-server use cases (e.g., microservices consuming APIs protected by the identity provider).