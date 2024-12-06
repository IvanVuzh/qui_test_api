To run this project, make sure that you have .NET SDK and Docker installed.

After clonning the repository, run ```docker-compose up -d``` in `Database` folder. Than change the connection string in `appsetting.json` to connect to said database. 
Run ```dotnet ef database update``` from cli or ```update-database``` from Package Manager Console to apply migrations.

You may wan to change `AllowReactApp` Cors policy.

Now you can start the project.
