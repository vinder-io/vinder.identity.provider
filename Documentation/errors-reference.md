# Errors Reference

This document provides a comprehensive catalog of error codes, messages, and their meanings within the system. It is intended to help developers, testers, and support teams quickly understand the possible errors that may occur, the scenarios in which they arise, and guidance on how to handle or resolve them.

Each error entry includes:  
- A unique error code for easy identification  
- A clear description of the error  
- Contextual information about when and why the error might occur

# Authentication Errors

Errors in this category are related to user authentication and token validation processes. Each error code starting with `#VINDER-IDP-ERR-AUT-XXX` indicates an issue encountered during authentication flows such as login, token generation, validation, refresh, and logout.


| Error Code                | Description                                                     | When it Occurs                                                       | How to Handle / Fix                                              |
|---------------------------|-----------------------------------------------------------------|---------------------------------------------------------------------|-----------------------------------------------------------------|
| `#VINDER-IDP-ERR-AUT-401` | Invalid credentials                                             | When the username or password provided do not match any user       | Verify the credentials and retry login                          |
| `#VINDER-IDP-ERR-AUT-404` | User not found                                                 | When the username does not exist in the system                      | Confirm the username exists or register a new account           |
| `#VINDER-IDP-ERR-AUT-405` | Invalid refresh token                                          | When the refresh token is expired, invalid, or already used        | Request a new login and refresh token                           |
| `#VINDER-IDP-ERR-AUT-409` | Logout failed                                                 | When revoking a refresh token fails due to invalid or expired token | Verify token validity before logout                             |
| `#VINDER-IDP-ERR-AUT-410` | Invalid token format                                          | When a token is malformed or incorrectly formatted                  | Ensure tokens are correctly generated and transmitted          |
| `#VINDER-IDP-ERR-AUT-411` | Token expired                                                | When the access or refresh token has passed its expiry time        | Request a new token via refresh flow or login                   |
| `#VINDER-IDP-ERR-AUT-412` | Invalid token signature                                      | When the token's signature validation fails (token tampering)      | Verify the token source and signing key                         |

---

- Contextual information about when and why the error might occur

# Tenant Errors

Errors in this category are related to issues with tenant identification and validation in the HTTP request context. Each error code starting with `#VINDER-IDP-ERR-TNT-XXX` indicates a problem encountered during tenant resolution and middleware processing.

| Error Code                | Description                                      | When it Occurs                                          | How to Handle / Fix                                             |
|---------------------------|------------------------------------------------|--------------------------------------------------------|----------------------------------------------------------------|
| `#VINDER-IDP-ERR-TNT-400` | Tenant header is missing from the HTTP request | When the incoming request does not include the tenant header (`X-Tenant`) | Ensure the client sends the tenant header in each request       |
| `#VINDER-IDP-ERR-TNT-404` | The specified tenant does not exist             | When the tenant header value does not match any tenant in the system | Verify tenant name correctness or register the tenant           |
| `#VINDER-IDP-ERR-TNT-500` | No HTTP context available to retrieve tenant information | When code runs outside of an HTTP context (e.g., background jobs like Hangfire) where `HttpContext` is not available | Ensure tenant resolution logic accounts for non-HTTP scenarios, e.g., pass tenant info explicitly in background job parameters |
