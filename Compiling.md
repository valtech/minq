No Sitecore libraries, files, or assets are supplied with this library as these are the property of Sitecore, so to build the solution you will need to copy some Sitecore DLLs to the Source/Library folder. If you follow the information in the Source/Libraries/!Instructions.txt file, the project will build just fine.

MINQ uses NUnit for testing. The NUnit testing framework is referenced via nuget but you will still need the NUnit runner to run the tests.

## MINQ assemblies ##

```
Minq.dll
Minq.Sitecore.dll
Minq.Mocks.dll
```

## Sitecore dependencies ##

```
Sitecore.Kernel.dll
Sitecore.Mvc.dll
```