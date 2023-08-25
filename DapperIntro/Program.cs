
using Dapper;
using DapperIntro.Entities;
using Microsoft.Data.SqlClient;

var connectionString = "Data Source=(localdb)\\DapperIntroServer;Initial Catalog=DapperIntro;Integrated Security=true";

// Example 1: Select
var getPeopleQuery = "Select * from People ORDER BY name desc";

using (var connection = new SqlConnection(connectionString))
{
    // var peopleDynamic = connection.Query(getPeopleQuery).ToList();
    var people = connection.Query<Person>(getPeopleQuery).ToList();
}

// Example 2: Insert
var insertPersonQuery = "INSERT INTO People (name, email) VALUES (@name, @email)";

using (var connection = new SqlConnection(connectionString))
{
    // connection.Execute(insertPersonQuery, new { name = "Russell", email = "russ@hotmail.com" });
    var people = connection.Query<Person>(getPeopleQuery).ToList();
}

// Example 3: Multiple queries

var multiQuery = "Select * from People; Select * from Addresses";

using (var connection = new SqlConnection(connectionString))
{
    using (var multi = connection.QueryMultiple(multiQuery))
    {
        var people    = multi.Read<Person>().ToList();
        var addresses = multi.Read<Address>().ToList();
    }
}

// Example 4: Inner join
var innerJoinQuery = @"Select * from People
                        INNER JOIN Addresses
                        ON People.Id = Addresses.PersonId";

var peopleDictionary =  new Dictionary<int, Person>();

using (var connection = new SqlConnection(connectionString))
{
    var list = connection.Query<Person, Address, Person>(innerJoinQuery,
        (person, address) =>
        {

            Person personTemp;

            // If person is not in the list
            if (!peopleDictionary.TryGetValue(person.Id, out personTemp))
            {
                personTemp = person;
                personTemp.Addresses = new List<Address>();
                peopleDictionary.Add(personTemp.Id, person);
            }

            if (address != null)
            {
                personTemp.Addresses.Add(address);
            }
            return personTemp;
        }).Distinct().ToList();
}