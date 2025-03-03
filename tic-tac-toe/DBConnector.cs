using MySqlConnector;
using System;

namespace tic_tac_toe
{
    public class DBConnector
    {
        private static readonly string SERVER_HOST = "host.docker.internal"; // <- line that needs to be changed if put on a distant server
        private static readonly string DATABASE_NAME = "tictactoe_online";
        private static readonly string USERNAME = "tictactoe_user";
        private static readonly string PASSWORD = "tictactoe_password";

        private static readonly Lazy<DBConnector> _instance = new(() => new DBConnector());
        private static readonly object _lock = new();

        private DBConnector() { }

        public static DBConnector Instance => _instance.Value;

        private static string GetConnectionString()
        {
            return $"Server={SERVER_HOST};Port=3306;Database={DATABASE_NAME};Uid={USERNAME};Pwd={PASSWORD};";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }

        /// <summary>
        /// Checks if the database connection is successful.
        /// </summary>
        /// <returns>True if the connection is successful, otherwise false.</returns>
        public static bool TestConnection()
        {
            try
            {
                using var conn = new MySqlConnection(GetConnectionString());
                conn.Open();
                return conn.State == System.Data.ConnectionState.Open;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Inserts a user into the database.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="pwdHash">Hash of the password</param>
        public void InsertUser(string username, string pwdHash)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand("INSERT INTO users (username, pwdHash) VALUES (@username, @pwdHash)", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@pwdHash", pwdHash);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error inserting user: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a user exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if user exists, otherwise false.</returns>
        public bool UserExists(string username)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking user existence: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets a user's password hash from the database.
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The hashed password if found; otherwise, null.</returns>
        public string? GetUserPasswordHash(string username)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand("SELECT pwdHash FROM users WHERE username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                using var reader = cmd.ExecuteReader();
                return reader.Read() ? reader.GetString("pwdHash") : null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching user password hash: {ex.Message}");
                return null;
            }
        }
    }
}