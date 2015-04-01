# Getting Started #

This section provides a quick overview of how to get going with MINQ.

## [Compiling](Compiling.md) ##

For quick guide to compiling MINQ see the [Compiling guide](Compiling.md).

## [Dependency Injection](DI.md) ##

Dependency Injection is core to MINQ and you will need to use a DI container to get up and running. For an overview of using MINQ with a DI container see the [Castle Windsor guide](DIWindsor.md).

## Installation via Nuget ##

You can install the main DLLs using:

```
PM> Install-Package Minq
```

You can install the mocking DLLs using:

```
PM> Install-Package Minq.Mocks
```

Don't forget to consult the [Configuration](MVCSetup.md) page.

## Sitecore Dependencies ##

You will also need to add references to the following Sitecore DLLs (MINQ does not distribute these as they are the property of Sitecore):

```
Sitecore.Kernel.dll
Sitecore.Mvc.dll
```