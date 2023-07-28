## Overview

Welcome to the E-Commerce API! This API is a simplified e-commerce platform with a single endpoint that performs a checkout action. It takes a list of watch IDs and returns the total cost based on the provided watch catalog, including associated prices and discounts.

## Technology Stack

- Programming Language: C# (built with .NET Core)
- Testing Framework: NUnit
- Source Control: Git

## Project Structure

The project is organized into the following components:

1. `ECommerceCheckoutAPI`: Main API project with the `CheckoutController` and startup configurations.
2. `ECommerceCheckout.Domain`: Contains the business logic for calculating the total cost of watches.
3. `ECommerceCheckout.Utilities`: Includes custom exceptions and utility classes.
4. `ECommerceCheckout.Tests`: Contains unit and integration tests for the application.

## Automated Testing

Automated testing is an essential part of this API. The project contains unit and integration tests to ensure the correct functionality of the API.

## Improvements and Future Considerations

- Implement a CI/CD pipeline for automated builds and deployments.
- Enhance error handling and response messages for improved user feedback.
- Extend the API to support additional e-commerce functionalities like adding items to the cart and processing payments.
- Utilize a database to manage the watch catalog dynamically.
- Implement rate limiting and authentication mechanisms for enhanced security.
- Deploy the API to a cloud service for scalability and reliability.
