---
marp: true
title: Practical Pulumi
paginate: true
theme: gaia
author: Al Rodriguez
---

# Pracical Pulumi

with AL Rodriguez

![bg right 80%](presentation-images/presentation_link_qrcode.png)

---

# Me (AL)

- @ProgrammerAL
- programmerAL.com
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

# Demo Diagram

![bg contain 80%](diagrams/demo-app.svg)

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
- `Output<string> url = func.Endpoint.Apply(x => $"https://{x}")`
  - Doesn't run the lambda until `.Endpoint` has a value

---

# Config

---

# Config Suggestions

- Load Objects when possible
- Safely load config
  - Load everything at the start
  - Load values as `required`
  - Avoids Exceptions mid-run

---

# Config Runs Only from YAML

- No Input Parameters
- Can edit YAML before running
  - As part of CI/CD pipeline
- Can load from external source with custom code
  - The config values are not tracked

---

# Secrets

- `pulumi config set mysecret myvalue --secret`
  - Encrypted into YAML
  - Accessible to anyone signed in with permissions
- Load using provider
  - 1Password, etc
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

# Demo Diagram

![bg contain 80%](diagrams/demo-app.svg)

---

# Stack References

- Load Outputs from other Stacks
  - Stack Outputs, not resources
- Order Matters

---

# Get Functions

- Load cloud resources that already exist
- Read-Only
- Not necessarily managed by Pulumi

---

# Automation API

- API to run Pulumi outside of console
  - Don't have to use `pulumi up`
- Common Use Cases: CI/CD or internal portal

---

# Debate: Deploy App with Pulumi?

- Can also deploy app when deploying cloud infra with Pulumi
  - Totally your choice
- Same amount of dependencies
- Either Way - Can set app config values with Pulumi
  - More code if separate
- My Take: Depends on Organization

---

# Automated Testing

- Unit Tests
  - API can be mocked in code to let unit tests run
  - Useful if your Pulumi code has custom logic
- Integration Tests
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

![bg right 80%](presentation-images/presentation_link_qrcode.png)
