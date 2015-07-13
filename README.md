# MINQ for Sitecore and Sitecore MVC #

Mockable, LINQable support for Sitecore and Sitecore MVC

## Mockable ##

The main problems with unit testing Sitecore are:

- Sitecore is tied to a HTTP request

- Sitecore is tied to a database

To solve these problems, Minq provides first class support for unit testing Sitecore without connecting to the CMS repository or requiring a HTTP context. This is done by abstracting away the Sitecore API and providing a mocking framework to supply data for testing purposes.

## LINQable ##

Minq is a great productivity library for Sitecore that provides a LINQ to XML style interface. That can turn dozens of lines of cumbersome API query code into fluent one line LINQ statements.

## POCO objects ##

Minq also provides entity framework style POCO objects for Sitecore and a Sitecore HTML helper that supports page edtiable field rendering via a fluent syntax.

## MVC ##

Provides enhancements to Sitecore MVC such as improved POCO HTML helpers, edit frames, and page editor support. The HTML helpers are unit testable and use the same abstraction approach as the core MINQ library.
