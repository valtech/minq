# Using MINQ with Castle Windsor #

Registering the core Sitecore dependencies:

```
container.Register(Component.For<ISitecoreItemGateway>()
   .ImplementedBy<SitecoreItemGateway>().LifestylePerWebRequest());

container.Register(Component.For<ISitecoreTemplateGateway>()
   .ImplementedBy<SitecoreTemplateGateway>().LifestylePerWebRequest());

container.Register(Component.For<ISitecoreMediaGateway>()
   .ImplementedBy<SitecoreMediaGateway>().LifestylePerWebRequest());

container.Register(Component.For<ISitecoreContext>()
   .ImplementedBy<SitecoreContext>().LifestylePerWebRequest());

container.Register(Component.For<ISitecoreRequest>()
   .ImplementedBy<SitecoreRequest>().LifestylePerWebRequest());
```

Registering the LINQ to Sitecore dependencies:

```
container.Register(Component.For<SRequest>().LifestylePerWebRequest());

container.Register(Component.For<SContext>().LifestylePerWebRequest());

container.Register(Component.For<SItemComposer>().LifestylePerWebRequest());
```