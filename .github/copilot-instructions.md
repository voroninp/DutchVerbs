---
applyTo: "**"
---
# Project general coding standards

## Error reporting

Ensure Errors, Warnings, and Exception messages provide enough context to understand what is going wrong. This includes:

- A human-friendly title of the **business operation** being executed. (Some code may be reused in different contexts, so it's better to give more specificity.)
- Details of the state or input (while accounting for security and regulations such as GDPR).
- A clear explanation of **why** this is an error or warning.
- **Who** is affected (user classes, tenants, dev teams, systems) and how they are affected.
- The possible **actions and consequences** of taking no action.
- Categories/Tags (e.g., Security, Usability, Infrastructure).

To provide the title of the business operation, you can use `Mbs.Common.Utils.Helpers.BusinessOperation.Start(...)` method.
The log message with `Error` or `Warning` level will be automatically enriched with the business operation title.

# Code Review recommendations

- Evaluate the changes using Cognitive Dimensions of Notations (CDN) framework.
- Highlight the most confusing parts of the code.