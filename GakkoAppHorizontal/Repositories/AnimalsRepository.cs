using System.Data.SqlClient;
using GakkoHorizontalSlice.Model;

namespace GakkoHorizontalSlice.Repositories;

public class AnimalsRepository : IAnimalsService
{
    private IConfiguration _configuration;
    
    public AnimalsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IEnumerable<Animal> GetAnimals(string orderBy = "name")
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        string query;
        
        switch (orderBy.ToLower())
        {
            case "name":
                query = "Name";
                break;
            case "description":
                query = "Description";
                break;
            case "category":
                query = "Category";
                break;
            case "area":
                query = "Area";
                break;
            default:
                query = "Name";
                break;
        }
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdAnimal, Name, Description, Categoty, Area FROM Animal ORDER BY " + query;
        
        var dr = cmd.ExecuteReader();
        var Animals = new List<Animal>();
        while (dr.Read())
        {
            var grade = new Animal
            {
                IdAnimal = (int)dr["IdAnimal"],
                Name = dr["FirstName"].ToString(),
                Description = dr["LastName"].ToString(),
                Category = dr["Email"].ToString(),
                Area = dr["Address"].ToString(),
            };
            Animals.Add(grade);
        }
        
        return Animals;
    }

    public Animal GetAnimal(int idAnimal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdAnimal, Name, Description, Categoty, Area FROM Animal WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
        
        var dr = cmd.ExecuteReader();
        
        if (!dr.Read()) return null;
        
        var Animal = new Animal
        {
            IdAnimal = (int)dr["IdAnimal"],
            Name = dr["FirstName"].ToString(),
            Description = dr["LastName"].ToString(),
            Category = dr["Email"].ToString(),
            Area = dr["Address"].ToString(),
        };
        
        return Animal;
    }

    public int CreateAnimal(Animal Animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Animal(Name, Description, Categoty, Area) VALUES(@Name, @Description, @Category, @Area)";
        cmd.Parameters.AddWithValue("@IdAnimal", Animal.IdAnimal);
        cmd.Parameters.AddWithValue("@FirstName", Animal.Name);
        cmd.Parameters.AddWithValue("@LastName", Animal.Description);
        cmd.Parameters.AddWithValue("@Email", Animal.Category);
        cmd.Parameters.AddWithValue("@Address", Animal.Area);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    
    public int DeleteAnimal(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", id);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }

    public int UpdateAnimal(Animal Animal)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE Animal SET Name=@Name, Description=@Description, Category=@Category, Area=@Area WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", Animal.IdAnimal);
        cmd.Parameters.AddWithValue("@FirstName", Animal.Name);
        cmd.Parameters.AddWithValue("@LastName", Animal.Description);
        cmd.Parameters.AddWithValue("@Email", Animal.Category);
        cmd.Parameters.AddWithValue("@Address", Animal.Area);
        
        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}