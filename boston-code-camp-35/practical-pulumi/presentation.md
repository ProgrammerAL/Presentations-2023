---
marp: false
title: Practical Pulumi
paginate: true
theme: gaia
author: Al Rodriguez
---

# Pracical Pulumi

with AL Rodriguez

---

# Me (AL)

- @ProgrammerAL
- programmerAL.com
- youtube.com/@ProgrammerAL
- NOT affiliated with Pulumi

![bg right 80%](presentation-images/presentation_link_qrcode.png)

---

# What this session is

- Intermediate session to Pulumi
- Assumes you know the basics
  - Maybe you've used it already
- Uses C# and Azure
  - But the concepts apply to other languages and clouds supported by Pulumi

---

#

![bg contain](presentation-images/pulumi-state-flow.png)

---

# Dependencies

---

# Inputs/Outputs

- Dependencies
- Usually values don't exist yet
  - Ex: Azure Function Endpoint
- Get `Output<string> myEndpoint = myResource.Endpoint.Apply(endpoint => $"https://{endpoint}")`
  - Doesn't run the lambda until `.Endpoint` has a value

---

# Config

---

# Config

- Load Objects when possible
- Avoid Exceptions when possible
  - Load everything at once
  - Load values as `required`

---

# Config Inputs

- Spoiler: Doesn't exist!
- Edit YAML before running
  - As part of CI/CD pipeline
- Load from external source with custom code
  - The config values are not tracked

---

# Secrets

- Pulumi Config Secrets
- Load using provider
  - 1Password, Azure Key Vault, etc
- Load from external source with custom code
  - The config values are not tracked

---

# Pulumi ESC

- ESC = Environments, Secrets, and Configuration
- Separate tool, bundled with the Pulumi CLI
- Consume config from anywhere

---

# Other Stuff

---

# Get Functions

- Load cloud resources that already exist
- Read-Only
- Not necessarily managed by Pulumi

---

# Stack References

- Load Outputs from other Stacks
- Order Matters

---

# Automation API

- API to run Pulumi outside of console
  - Don't have to use `pulumi up`
- Common Use CAses: CI/CD or internal portal

---

# Concept: Deploy App with Pulumi

- Can also deploy app when deploying cloud infra with Pulumi
  - Totally your choice
- Same amount of dependencies
- If together
  - Can set app config values with Pulumi
- If separate
  - Can use Automation API to set app config values

---

# Automated Testing

- Unit Tests
  - API can be mocked in code to let unit tests run
  - Useful if your Pulumi code has custom logic
- Automation Tests
  - Probably more useful overall

---

# Custom Resources

- Wrapper around existing resources
- Can create in 1 language, export to others

---

# Pulumi AI

- Generate Pulumi Code using that thing everyone's talking about
- pulumi.com/ai

---

# Online Info

- @ProgrammerAL
- programmerAL.com
- youtube.com/@ProgrammerAL

![bg right 80%](presentation-images/presentation_link_qrcode.png)
