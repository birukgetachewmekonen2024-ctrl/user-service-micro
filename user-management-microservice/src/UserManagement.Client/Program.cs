using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using UserManagement.Client.Protos;

namespace UserManagement.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UserService.UserServiceClient(channel);

            // Example of user registration
            var registerResponse = await client.RegisterAsync(new UserRegistrationRequest
            {
                Username = "testuser",
                Password = "password123",
                Email = "testuser@example.com"
            });

            Console.WriteLine($"Registration Success: {registerResponse.Success}");

            // Example of user login
            var loginResponse = await client.LoginAsync(new UserLoginRequest
            {
                Username = "testuser",
                Password = "password123"
            });

            Console.WriteLine($"Login Success: {loginResponse.Success}, Token: {loginResponse.Token}");
        }
    }
}