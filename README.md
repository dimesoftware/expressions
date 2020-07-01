# Dime.Expressions

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/dimenics.dime-expressions?branchName=master) ![Code coverage](https://img.shields.io/azure-devops/coverage/dimenicsbe/Utilities/147/master)

## Introduction

Powerful expression builder.

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

``` csharp
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
  public async Task<IEnumerable<Customer>> Get()
  {
     var filter = CustomerFilter.CreateFilter("IsActive", "eq", "true"); // x => x.IsActive == true;    
     return await dbContext.Customers.Where(filter).ToListAsync();
  }
}
```

## Contributing

![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)

Pull requests are welcome. Please check out the contribution and code of conduct guidelines.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)