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

# Development
## Backend
Prompt in agent mode:
```
Work in the directory EShopAPI. When run terminal remember to change the right directory, for example: `cd EShopAPI && dotnet test`.
Delete the default WeatherForecastController and WeatherForecast class.
Create a new controller named EShopController.
- Create a Rest API with methods to:
    - Get the automobile parts list with page offset and limit.
    - Get automobile part details by id. 
    - Search automobile parts by name, description, manufacturer, price.
    - Create a shop cart.
    - Add a product to the shop cart.
    - Remove a product from the shop cart.
    - Calculate the total price of the products in the shop cart.
Remember to get the data from a JSON file `automobileParts.json`.
update the client.rest file in order to have test for the api
```

## Frontend 

dotnet new webapp -n EShopUI
dotnet sln EShopAPI.sln add EShopUI/EShopUI.csproj

    - Create a list of products in the main page.
    - Create a search bar to filter the products.
    - Navigate to the description page when the user clicks on a product.
    - (Optional) Slicer to filter the products by price.
    