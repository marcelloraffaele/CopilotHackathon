# Project creation
```
mkdir EShopAPI
cd EShopAPI
dotnet new webapi -controllers -f net9.0

cd ..
mkdir EShopAPI.Tests
cd EShopAPI.Tests
dotnet new xunit -f net9.0

dotnet new sln -n EShopAPI
dotnet sln EShopAPI.sln add EShopAPI/EShopAPI.csproj
dotnet sln EShopAPI.sln add EShopAPI.Tests/EShopAPI.Tests.csproj
```

To run the app, open a terminal in the dotnet folder and run:

Windows environments
```
dotnet run --project .\EShopAPI\EShopAPI.csproj
```
Codespaces, Linux & Unix environments
```
dotnet run --project ./EShopAPI/EShopAPI.csproj
```