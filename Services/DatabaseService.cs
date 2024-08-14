using Digident_Group3.Interfaces;
using System;
using System.Data.SqlClient;

namespace Digident_Group3.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int RegisterUser(string email, string password, string firstName, string lastName, DateTime dateOfBirth, string address, string phoneNumber)
        {
            // This is where you generate the password hash before saving it to the database
            string hashedPassword = HashPassword(password);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    int userId = (int)command.ExecuteScalar(); // This returns the UserID
                    return userId; // Return the new UserID to be used in the application
                }
            }
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here, using something like SHA-256
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    

    public int ValidateCredentials(string email, string password)
        {
            int userId = -1;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT UserID FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", HashPassword(password)); // Assume HashPassword is a method that hashes the password

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        userId = (int)reader["UserID"]; // Retrieve the UserID if credentials match
                    }
                }
            }

            return userId; // Return the UserID if credentials are valid, otherwise -1
        }

        
    }
}