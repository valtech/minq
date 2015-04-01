# Micro Caching #

Sitecore can make a lot of requests to the database for a cold cache. If a lot of visitors hit your site (hundreds per second) then it is important to prevent multiple SQL requests for the same data. This solution combines fetch locking and micro caching to prevent cold start SQL flooding.

To use the caching features of MINQ you will need the following nuget package:

```
PM> Install-Package Minq.Caching
```

You will then need to register the following as singletons with your DI container. Here's how to do this with Castle Windsor:

```
_container.Register(Component.For<SItemComposer>().ImplementedBy<CachedItemComposer>()).Forward<CachedItemComposer>();

_container.Register(Component.For<CachedItemRepository>().UsingFactoryMethod(kernel =>
{
   return new CachedItemRepository(kernel.Resolve<ISitecoreItemGateway>(),
      new MicroCachedRespositoryDecorator<SitecoreItemKey, SItem>(new FetchLockedRepository<SitecoreItemKey, SItem>(),
         TimeSpan.FromMinutes(3))).LifestyleSingleton());
}));

_container.Register(Component.For<CachedTemplateRepository>().UsingFactoryMethod(kernel =>
{
   return new CachedTemplateRepository(kernel.Resolve<ISitecoreTemplateGateway>(),
      new MicroCachedRespositoryDecorator<SitecoreTemplateKey, STemplate>(new FetchLockedRepository<SitecoreTemplateKey, STemplate>(),
         TimeSpan.FromMinutes(30))).LifestyleSingleton());
}));

_container.Register(Component.For<CachedMediaRepository>().UsingFactoryMethod(kernel =>
{
   return new CachedMediaRepository(kernel.Resolve<ISitecoreMediaGateway>(),
      new MicroCachedRespositoryDecorator<SitecoreItemKey, SMedia>(new FetchLockedRepository<SitecoreItemKey, SMedia>(),
         TimeSpan.FromMinutes(3))).LifestyleSingleton());
}));
```

To clear the cache programatically use the ClearCaches method on the CachedItemComposer singleton instance.

The caching replaces the normal SItemComposer with CachedItemComposer, so remove the old registration for SItemComposer for existing projects.