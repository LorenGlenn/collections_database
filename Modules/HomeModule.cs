using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Inventory.Objects;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Post["/mybeaniebuddies"] = _ =>{
        string name = Request.Form["beanie-name"];
        string rarity = Request.Form["beanie-rarity"];
        int cost = int.Parse(Request.Form["beanie-cost"]);

        Beanie userBeanie = new Beanie(name, rarity, cost);
        userBeanie.Save();
        List<Beanie> allMyBabies = Beanie.GetAll();
        return View["beanies.cshtml", allMyBabies];
      };
    }
  }
}
