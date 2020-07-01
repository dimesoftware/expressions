# Dime.Expressions

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/dimenics.dime-expressions?branchName=master)

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

![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)

Pull requests are welcome. Please check out the contribution and code of conduct guidelines.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)