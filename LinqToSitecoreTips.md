# LINQ to Sitecore Tips #

## Collections ##

The LINQ to Sitecore API offers some strong performance advantages if certain conventions are followed when accessing collections.

For instance:

```
item.Descendants().Take(5);
```

The above code will be fast as it will stop iterating after five items.

## Getting the first item in a collection ##

Always use the following approach when getting the first item in a collection:

```
SItem first = items.FirstOrDefault();
```

Ensure you check whether the item is null or not:

```
SItem first = items.FirstOrDefault();

if (first == null)
{
    throw new Exception(“Expected item XYZ to exist”);
}
```

Try to avoid First as it will fail when the collection is empty.

## Use descendants sparingly ##

The Descendants function on the SItem will recursively trawl all items under the owning item:

```
SItem item = item.Descendents().FirstOrDefault();
```

The Descendants function can be very slow – use with caution. The most likely alternative is the Items function which just returns the children of the owning item:

```
SItem item = item.Items().FirstOrDefault();
```

## Use ordering sparingly ##

The LINQ extensions for ordering will force the LINQ iterators to visit all items.

For instance:

```
item.Descendants().OrderBy(item => item.Field("Modified").Value<DateTime>());
```

The above code may be very slow as the full tree of descendants will be visited to perform the ordering.

Also:

```
item.Descendants().OrderBy(item => item.Field("Modified").Value<DateTime>().Take(5);
```

The above code will still be slow as the ordering will still cause all items to be visited before it knows which first five items to finally take.

## Filtering items by template ##

Filtering items by template is a common task in Sitecore, the following code shows how to get all an item’s children based on a particular template’s GUID:

```
item.Items().Where(item => item.Template.IsBasedOn(TemplateId));
```