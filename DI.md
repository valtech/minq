# Dependency Injection #

MINQ was designed to be used with a dependency injection container. This core design philosophy was intended to avoid the problem associated with Sitecore's ambient context being tied to a HTTP request. Sitecore makes it hard to unobtrusively unit test Sitecore, and MINQ fixes that using dependency injection.

## Using MINQ with popular dependency containers ##

[Castle Windsor](DIWindsor.md)