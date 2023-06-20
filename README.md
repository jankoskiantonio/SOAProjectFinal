# SOAProjectFinal

To enable the backend:

1. Open SOAProject in Visual Studio
1. Configure the `appsettings.json` file's `DefaultConnection` string to match your PostgreSQL database.
2. Enter `Update-Database` in the NuGet Package Manager Console in Visual Studio
3. Build and run the project
4. Via Swagger or Postman, use the Register API to register an account and login with it via the Login API to generate an auth token.
5. Provide the auth token to gain access to the other API endpoints

To use the frontend:

1. In the frontweb folder, open a terminal and run `npm install`, then `npm run serve`


Note: Project was published to Azure together with the frontend, but the frontend was deployed with bugs.

Participants:
Antonio Jankoski 128824
Nikola Jovanoski 128687
Mario Bojarovski 128484
