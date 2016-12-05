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
      Post["/mycards"] = _ =>{
        string name = Request.Form["card-name"];
        string rarity = Request.Form["card-rarity"];
        int cost = int.Parse(Request.Form["card-cost"]);

        Card userCard = new Card(name, rarity, cost);
        userCard.Save();
        List<Card> allMyBabies = Card.GetAll();
        return View["cards.cshtml", allMyBabies];
      };

      Get["/throwOutBaby/{id}"] = parameters =>
      {
        Beanie newBeanie = Beanie.Find(parameters.id);
        string name = newBeanie.GetName();
        Beanie.RemoveABeanie(parameters.id);
        return View["throwOutBaby.cshtml", name];
      };

      Get["/throwOutCard/{id}"] = parameters =>
      {
        Card newCard = Card.Find(parameters.id);
        string name = newCard.GetName();
        Card.RemoveACard(parameters.id);
        return View["throwOutBaby.cshtml", name];
      };
    }
  }
}
