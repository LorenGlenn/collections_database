using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory.Objects
{
  public class Card
  {
    private int _id;
    private string _name;
    private string _rarity;
    private int _cost;

    public Card(string name, string rarity, int cost, int id = 0)
    {
      _id = id;
      _name = name;
      _rarity = rarity;
      _cost = cost;
    }

    public override bool Equals(System.Object otherCard)
    {
      if(!(otherCard is Card))
      {
        return false;
      }
      else
      {
        Card newCard = (Card) otherCard;
        bool nameEquality = (this.GetName() == newCard.GetName());
        return (nameEquality);
      }
    }

    public override int GetHashCode()
    {
     return this.GetName().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetRarity()
    {
      return _rarity;
    }

    public int GetCost()
    {
      return _cost;
    }

    public static List<Card> GetAll()
    {
      List<Card> allBabies = new List<Card>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM baseball_cards;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cardId = rdr.GetInt32(0);
        string cardName = rdr.GetString(1);
        string cardRarity = rdr.GetString(2);
        int cardCost = rdr.GetInt32(3);
        Card newCard = new Card(cardName, cardRarity, cardCost, cardId);
        allBabies.Add(newCard);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allBabies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO baseball_cards (name, rarity, cost) OUTPUT INSERTED.id VALUES (@CardName, @CardRarity, @CardCost);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CardName";
      nameParameter.Value = this.GetName();

      SqlParameter rarityParameter = new SqlParameter();
      rarityParameter.ParameterName = "@CardRarity";
      rarityParameter.Value = this.GetRarity();

      SqlParameter costParameter = new SqlParameter();
      costParameter.ParameterName = "@CardCost";
      costParameter.Value = this.GetCost();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(rarityParameter);
      cmd.Parameters.Add(costParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void RemoveACard(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM baseball_cards WHERE id = @CardId;", conn);
      SqlParameter cardIdParameter = new SqlParameter();
      cardIdParameter.ParameterName = "@CardId";
      cardIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cardIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

    }

    public static Card Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM baseball_cards WHERE id = @CardId;", conn);
      SqlParameter cardIdParameter = new SqlParameter();
      cardIdParameter.ParameterName = "@CardId";
      cardIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cardIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCardId = 0;
      int foundCardCost = 0;
      string foundCardName = null;
      string foundCardRarity = null;
      while(rdr.Read())
      {
        foundCardId = rdr.GetInt32(0);
        foundCardName = rdr.GetString(1);
        foundCardRarity = rdr.GetString(2);
        foundCardCost = rdr.GetInt32(3);
      }
      Card foundCard = new Card(foundCardName, foundCardRarity, foundCardCost, foundCardId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCard;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM baseball_cards;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
