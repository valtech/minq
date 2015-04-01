# MVC configuration #

To get the Sitecore helper object, pages must inherit from SitecorePage`<TModel>`:

```
<system.web.webPages.razor>
   <pages pageBaseType="Minq.Sitecore.Mvc.SitecorePage">
      <namespaces>
         <!-- namespaces removed for clarity -->
      </namespaces>
   </pages>
</system.web.webPages.razor>
```

# Binding Redirects #

Depending on your verison of MVC you may need to add some binding redirects. This shows the redirects for MVC 5:

```
<dependentAssembly>
   <assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" xmlns="urn:schemas-microsoft-com:asm.v1" />
      <bindingRedirect oldVersion="1.0.0.0-5.0.0.0" newVersion="5.1.0.0" xmlns="urn:schemas-microsoft-com:asm.v1" />
</dependentAssembly>
<dependentAssembly>
   <assemblyIdentity name="System.Web.WebPages" publicKeyToken="31bf3856ad364e35" xmlns="urn:schemas-microsoft-com:asm.v1" />
      <bindingRedirect oldVersion="1.0.0.0-3.0.0.0" newVersion="3.0.0.0" xmlns="urn:schemas-microsoft-com:asm.v1" />
</dependentAssembly>
<dependentAssembly>
   <assemblyIdentity name="System.Web.WebPages.Razor" publicKeyToken="31bf3856ad364e35" xmlns="urn:schemas-microsoft-com:asm.v1" />
      <bindingRedirect oldVersion="1.0.0.0-2.0.0.0" newVersion="3.0.0.0" xmlns="urn:schemas-microsoft-com:asm.v1" />
</dependentAssembly>
```