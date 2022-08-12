# Team Rocket API using Microsoft Visual Studio 2022

#### If you don't have Microsoft Visul Studio, the API CRUD has been published [here](https://teamrocketapi20220811192015.azurewebsites.net/swagger/index.html) 
---
**NOTE:** The CSV `pokemon.csv` file has been already upload.

### Steps to run in the computer
Intructions:

- Clone the [repository](https://github.com/wllanos/TeamRocketAPI.git).
- Open the solution file `TeamRocketAPI.sln`
- To avoid configurating the database, a portable database has been added:
		`PokemonUniverse.sqlite`
- To re-upload the CSV file, it is necessary to uncomment the line ***#35*** in `Startup.cs` file (See below). 
```
        //services.AddHostedService<CSV>();//service on charge of loading CSV file into db and deleting data
``` 
- Run the solution .
- Swagger UI will be showed and you can test the CRUD using Swagger or Postman.
- To enable Authorization security in CRUD actions and use JWT, uncomment the line **#14** in `PokemonController.cs` and proceed to use **Accounts** actions listed in Swagger UI to get a **Bearer Token**.
```
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
```
**NOTE:** If there is an error related to **Read Only database**, you must uncheck the **Read-only** attribute in `wwwroot` folder.

>>If there are any questions, send an email to wladimir_llanos@hotmail.com
