using System.Collections;
using System.Collections.Generic;
//in Extensions download "NuGet Package Manager GUI"
// First open your terminal and make sure you are CD to your directory
// then type in "dotnet add package Microsoft.Data.Sqlite"
//Open terminal from your Applications folder
//  type: "brew install sqlite"
//
using Mono.Data.Sqlite; //calls the library of SQLite nuget package MySql.Data -Version 8.0.32

//https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=net-cli
using UnityEngine;
using System.Data;



public class SimpleDB : MonoBehaviour
{
    private string dbName = "URI=file:Inventory.db";

    // Start is called before the first frame update
    void Start()
    {
        CreateDB();
        AddWeapon("Phaser", 100);
        DisplayWeapons();
        Debug.Log("start worked");
    }

    // Update is called once per frame
    void Update()
    {
        // Empty update method
    }

    private void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS weapons (name VARCHAR(20), qte INT);";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void AddWeapon(string weaponName, int weaponQte)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO weapons (name, qte) VALUES (@name, @qte);";
                command.Parameters.AddWithValue("@name", weaponName);
                command.Parameters.AddWithValue("@qte", weaponQte);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void DisplayWeapons()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM weapons;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("Name: " + reader["name"] + "\tQuantity: " + reader["qte"]);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public void GetWeapon(string weaponName)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM weapons WHERE name = @name;";
                command.Parameters.AddWithValue("@name", weaponName);
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log("Name: " + reader["name"] + "\tQuantity: " + reader["qte"]);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }

    public void UpdateWeaponQuantity(string weaponName, int newQuantity)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE weapons SET qte = @qte WHERE name = @name;";
                command.Parameters.AddWithValue("@name", weaponName);
                command.Parameters.AddWithValue("@qte", newQuantity);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void DeleteWeapon(string weaponName)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM weapons WHERE name = @name;";
                command.Parameters.AddWithValue("@name", weaponName);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
