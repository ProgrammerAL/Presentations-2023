using System.Diagnostics;

namespace ProgrammerAl.Presentations.OTel.PurchasesService;

public static class ActivitySources
{
    public static ActivitySource PurchasesServiceSource = new ActivitySource("purchases-service", "1.0.0");
}
