using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory.Objects
{
  public class Beanie
  {
    private int _id;
    private string _name;
    private string _rarity;
    private int _cost;

    public Beanie(string name, string rarity, int cost, int id = 0)
    {
      _id = id;
      _name = name;
      _rarity = rarity;
      _cost = cost;
    }

    public override bool Equals(System.Object otherBeanie)
    {
      if(!(otherBeanie is Beanie))
      {
        return false;
      }
      else
      {
        Beanie newBeanie = (Beanie) otherBeanie;
        bool nameEquality = (this.GetName() == newBeanie.GetName());
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

    public static List<Beanie> GetAll()
    {
      List<Beanie> allBabies = new List<Beanie>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM beanie_babies;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int beanieId = rdr.GetInt32(0);
        string beanieName = rdr.GetString(1);
        string beanieRarity = rdr.GetString(2);
        int beanieCost = rdr.GetInt32(3);
        Beanie newBeanie = new Beanie(beanieName, beanieRarity, beanieCost, beanieId);
        allBabies.Add(newBeanie);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO beanie_babies (name, rarity, cost) OUTPUT INSERTED.id VALUES (@BeanieName, @BeanieRarity, @BeanieCost);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@BeanieName";
      nameParameter.Value = this.GetName();

      SqlParameter rarityParameter = new SqlParameter();
      rarityParameter.ParameterName = "@BeanieRarity";
      rarityParameter.Value = this.GetRarity();

      SqlParameter costParameter = new SqlParameter();
      costParameter.ParameterName = "@BeanieCost";
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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM beanie_babies", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
