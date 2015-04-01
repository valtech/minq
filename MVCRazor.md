# Using in Razor Views #

```
@using Minq.Mvc;

@model MinqDemo.Model.HomeModel

Simple field:
<div>
@Sitecore.FieldFor(c => c.SampleItem.Title)
</div>

Image:
<div>
@Sitecore.ImageFor(c => c.SampleItem.Graphic, new { width = "40px" })
</div>

Link:
<div>
@Sitecore.LinkFor(c => c.SampleItem.Goto).IfEmpty(() =>
   Sitecore.FieldFor(c => c.SampleItem.Title, new { @class = "fred" }))
</div>

Edit frame:
<div>
@Sitecore.Editor(
@<div>
   Hello @Sitecore.FieldFor(c => c.SampleItem.Title)
</div>, new { title = "hello" }
)
</div>
```