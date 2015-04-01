# Supply content to Razor Views #

Content is supplied to razor views using Sitecore controller renderings (a new type of rendering in Sitecore 7).

An example controller is as follows (the hard coded GUIDs are just for demo purposes - MINQ gives you full LINQ query support over the Sitecore tree):

```
public class DemoController : Controller
{
	private SRequest _request;
	private SContext _context;

	public DemoController(SRequest request, SContext context)
	{
		_request = request;
		_context = context;
	}

	public ActionResult Index()
	{
		DemoModel demoModel = new DemoModel();

		demoModel.GridCells = _context.Item("{31368B31-01A9-42CD-BD60-40FA86C2F2DC}")
			.Items().ToType<GridCellModel>().ToArray();

		demoModel.Home = _context.Item("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
			.ToType<HomeModel>();

		SItem item = _context.Item("{BA1E57ED-EBCE-42D5-B36B-C77140C1A24B}");

		return View(demoModel);
	}

	public ActionResult About()
	{
		return View();
	}
}
```

It is trongly recomended you set up Dependency Injection to inject the necessary SContent and SRequest object into your controller.