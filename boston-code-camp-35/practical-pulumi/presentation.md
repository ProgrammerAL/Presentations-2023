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

# Online Info

- @ProgrammerAL
- https://programmerAL.com

![bg right 80%](presentation-images/presentation_link_qrcode.svg)

---

# What this session is

- Intermediate session to Pulumi
- Assumes you know the basics
  - Maybe you've used it already
- Uses C# and Azure
  - But the concepts apply to other languages and clouds supported by Pulumi

---

# Inputs/Outputs

- Dependencies
- Usually values don't exist yet
  - Ex: Azure Function Endpoint
- Get `myResource.Endpoint.Apply(x => {/*Your Code Here*/})`
  - Doesn't run the lambda until `.Endpoint` a value

---

# Config

- Avoid Exceptions partway through
  - Load everything at once
  - Load values as `required`
- Load Objects

---

# Runtime Config

- Spoiler: Doesn't exist!
- Edit YAML before running
  - As part of CI/CD pipeline
  - Load from external source

---

# Pulumi ESC

---

# Secrets

- Pulumi Config Secrets
- Load with cusom code
- Load using provider, like 1Password
- Secret Objects???

---

# Get Functions

---

# Stack References

- Load Outputs from other Stacks
- Order Matters

---

# Concept: Deploy App with Pulumi

- Pros:
  - Less code
- Cons:
  - More Complex

---

# Automation API

---

# Automated Testing

- Unit Tests
- Automation Tests

---

# Custom Resources

- Wrapper around existing resources
- Can create in 1 language, export to others

---

# Pulumi AI

---
