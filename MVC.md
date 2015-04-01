# MVC #

MINQ has been designed to work seamlessly with MVC, it supports the same model syntax you will be familiar with in normal MVC such as:

```
@Html.TextBoxFor(model => model.Title)
```

Can be used for common Sitecore outputting requirements:

```
@Sitecore.LinkFor(model => model.Faq)
```

Which can be tailored using more advanced syntax than normal MVC:

```
@Sitecore.LinkFor(c => c.SampleItem.Goto).IfEmpty(() =>
   Sitecore.FieldFor(c => c.SampleItem.Title, new { @class = "fred" }))
```