using Codecool.BruteForce.Users.Model;
using Microsoft.Data.Sqlite;

namespace Codecool.BruteForce.Users.Repository;

public class UserRepository : IUserRepository
{
    private readonly string _dbFilePath;

    public UserRepository(string dbFilePath)
    {
        _dbFilePath = dbFilePath;
    }

    private SqliteConnection GetPhysicalDbConnection()
    {
        var dbConnection = new SqliteConnection($"Data Source ={_dbFilePath};Mode=ReadWrite");
        dbConnection.Open();
        return dbConnection;
    }

    private void ExecuteNonQuery(string query)
    {
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private static SqliteCommand GetCommand(string query, SqliteConnection connection)
    {
        return new SqliteCommand
        {
            CommandText = query,
            Connection = connection,
        };
    }

    public void Add(string userName, string password)
    {
        var query = @$"INSERT INTO users(user_name,password) VALUES('{userName}','{password}') ";
        ExecuteNonQuery(query);
    }

    public void Update(int id, string userName, string password)
    {
        var query = @$"UPDATE users SET user_name = '{userName}', password = '{password}' WHERE id={id}";
        ExecuteNonQuery(query);
    }

    public void Delete(int id)
    {
        var query = @$"DELETE FROM users WHERE id = {id}";
        ExecuteNonQuery(query);
    }

    public void DeleteAll()
    {
        var query = @$"DELETE FROM users";
        ExecuteNonQuery(query);
    }

    public User Get(int id)
    {
        var query = $"SELECT * FROM users WHERE id = {id}";
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);

        using var reader = command.ExecuteReader();
    
        if (reader.Read())
        {
            return new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        }
    
        // Return null (or throw an exception) if the user with the specified ID is not found.
        return null; // You can customize this behavior based on your application's requirements.
    }

    public IEnumerable<User> GetAll()
    {
        var users = new List<User>();
        var query = @$"SELECT * FROM users";
        
        using var connection = GetPhysicalDbConnection();
        using var command = GetCommand(query, connection);
        using var reader = command.ExecuteReader();
        
        /*
         while (reader.Read()): This is a loop that iterates through the rows of data retrieved from the database using the SqliteDataReader. It continues looping as long as there are more rows to read.

var id = reader.GetInt32(0);: Inside the loop, this line retrieves the value of the first column (column with index 0) in the current row of the result set and assigns it to the id variable. In this case, it assumes that the first column in the query result represents the user's ID.

var userName = reader.GetString(1);: Similarly, this line retrieves the value of the second column (column with index 1) in the current row and assigns it to the userName variable. It assumes that the second column represents the user's username.

var password = reader.GetString(2);: Likewise, this line retrieves the value of the third column (column with index 2) in the current row and assigns it to the password variable. It assumes that the third column represents the user's password.

users.Add(new User(id, userName, password));: Finally, this line constructs a new User object using the id, userName, and password values obtained from the current row of the result set, and then adds that User object to a List<User> called users. This is done for each row of data retrieved from the database, effectively building a collection of User objects representing all the users in the database.
         */
        
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var userName = reader.GetString(1);
            var password = reader.GetString(2);
            users.Add(new User(id, userName, password));
        }

        return users;
    }
}
