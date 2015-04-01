# LINQ to Sitecore #

Classes in the Minq.Linq namespace provide an API very similar in format to Microsoft's LINQ to XML.

## Querying Sitecore ##

At the heart of LINQ to Sitecore is the SContext class which can be used as the route in to querying Sitecore:

```
var context = container.Resolve<SContext>();
```

We can then get items based on their GUID or path:

```
var item = context.Item("GUID or path");
```

Items are really easy to query (this query gets a flattened list of all descendants based on a specific template):

```
var items = item.Descendants()
   .Where(d => d.Template.IsBasedOn("template GUID"));
```

## Getting the item associated with the current request ##

A special request class handles information associated with the current HTTP request:

```
var request = container.Resolve<SRequest>();
```

Get the item associated with the current request:

```
var item = request.Item;
```