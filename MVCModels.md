# Creating Models for use with Razor #

MINQ uses the POCO approach so models are just plain old C# classes. You must decorate your classes with attributes in certain circumstances.

By default, if the property name is the same as the field name, MINQ will auto-map Sitecore fields to your POCO class. Otherwise you can override the field that supplies the property value:

```
public class HomeModel
{
	[SitecoreItemKey]
	public SitecoreItemKey Key
	{
		get;
		set;
	}

	[SitecoreChildren]
	public ICollection<SampleItemModel> SampleItems
	{
		get;
		set;
	}

	[SitecoreField("Title")]
	public string Title
	{
		get;
		set;
	}
}
```

The `SitecoreItemKey` attribute will allow page editor and "For" helpers to work when you display fields from your model in a view:

```
@Sitecore.FieldFor(c => c.Title)
```

The `SitecoreChildren` attribute tells MINQ to load the children into the property. The children are lazy loaded so they are only loaded if you access that property (making it possible to define your entire tree using a single inheritance graph with zero initial loading overhead).

You can reference image fields directly using:

```
[SitecoreMediaField]
public string ImageUrl
{
	get;
	set;
}
```


If you have a `SitecoreChildren` property and only want it to be filled with items of a specific template type, simply mark the class with a `SitecoreTemplate` attribute:

```
[SitecoreTemplate("{0E67A2A7-087D-418A-8058-E298D5F9C8E3}")]
public class GridCellModel
{
	[SitecoreItemKey]
	public SitecoreItemKey Key
	{
		get;
		set;
	}

	[SitecoreField("Cell Image")]
	public GridCellImageModel Image
	{
		get;
		set;
	}
}
```

If this class is declared in a property:

```
[SitecoreChildren]
public ICollection<GridCellModel> SampleItems
{
	get;
	set;
}
```

Then any children not of template type "{0E67A2A7-087D-418A-8058-E298D5F9C8E3}" will be ignored.

Also MINQ queries will read this attribure to do template based filtering:

```
request.Item.Items().Where(item => item.Template.IsBasedOn<GridCellModel>());
```

MINQ can fill in a POCO class from a MINQ item:

```
GridCellModel cell = item.ToType<GridCellModel>();
```

This can be combined with more advanced queries:

```
GridCellModel cell = request.Item.Ancestors().First().ToType<GridCellModel>();
```