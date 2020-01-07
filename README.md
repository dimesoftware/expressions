# Dime.Expressions

[![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/Expressions-%20MAIN%20-%20CI?branchName=master)](https://dev.azure.com/dimenicsbe/Utilities/_build/latest?definitionId=81&branchName=master)

## Introduction

Powerful expression builder.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

Generates expressions on the fly

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

public Expression<Func<MyClass, bool>> Get(string property, string operation, string value)
{
    ExpressionBuilder builder = new ExpressionBuilder();
    builder.WithDateTimeParser(new DateTimeParser("Europe/London", new CultureInfo("en-GB")));
    builder.WithDoubleParser(new DoubleParser("en-GB"));
    builder.WithDecimalParser(new DecimalParser("en-GB"));

    return ((IFilterExpressionBuilder)builder).GetExpression<MyClass>(property, operation, value);
}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

MIT