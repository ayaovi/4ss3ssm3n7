using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SalesApi.Persistence;

namespace SalesApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      SalesRepository.CreateDatabase();
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .Build();
  }
}
