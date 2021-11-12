<p align="center">
<img src="./assets/logo.svg" height="250px" />
</p>

<h1 align="center">.NET Expressions Builder</h1>

<div align="center">
<img src="https://dev.azure.com/dimesoftware/Utilities/_apis/build/status/dimenics.dime-expressions?branchName=master" /> <img src="https://img.shields.io/azure-devops/coverage/dimesoftware/Utilities/147/master" /> <img src="https://img.shields.io/badge/License-MIT-brightgreen.svg?style=flat-square"/> <img src="https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square" />
</div>

## Introduction

Powerful filter builder that converts string-based queries to expressions `Expression<Func<T,bool>>` that you can execute against your code, collections, Entity Framework, and so much more.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

Generates expressions on the fly.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Expressions:

- dotnet cli: `dotnet add package Dime.Expressions`
- Package manager: `Install-Package Dime.Expressions`

## Usage

```csharp
using System.Linq.Expressions;

public class Customer
{
  public bool IsActive { get; set; }
}

public class CustomerFilter
{
  public static Expression<Func<Customer, bool>> CreateFilter(string property, string operation, string val)
  {
    ExpressionBuilder builder = new ExpressionBuilder();
    builder.WithDateTimeParser(new DateTimeParser("Europe/London", new CultureInfo("en-GB")));
    builder.WithDoubleParser(new DoubleParser("en-GB"));
    builder.WithDecimalParser(new DecimalParser("en-GB"));

    return ((IFilterExpressionBuilder)builder).GetExpression<Customer>(property, operation, val);
  }
}

public class CustomerApiController : ControllerBase
{
  /// <summary>
  /// Example data:
  /// property = "IsActive"
  /// operatorKey = "eq"
  /// value = "true"
  /// </summary>
  public async Task<IEnumerable<Customer>> Get(string property, string operatorKey, string value)
  {
     var filter = CustomerFilter.CreateFilter(property, operatorKey, value); // x => x.IsActive == true;
     return await dbContext.Customers.Where(filter).ToListAsync();
  }
}
```

## Contributing

We welcome contributions. Please check out the contribution and code of conduct guidelines first.

To contribute:

1. Fork the project
2. Create a feature branch (`git checkout -b feature/mynewfeature`)
3. Commit your changes (`git commit -m 'Add mynewfeature'`)
4. Push to the branch (`git push origin feature/mynewfeature`)
5. Open a pull request
