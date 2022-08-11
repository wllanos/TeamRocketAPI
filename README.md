# Team Rocket API using Microsoft Visual Studio 2022

**NOTE:** The CSV `pokemon.csv` file has been already upload.

Intructions:

- Clone the [repository](https://github.com/wllanos/TeamRocketAPI.git).
- Open the solution file `TeamRocketAPI.sln`
- To avoid configurating the database, a portable database has been added:
		`PokemonUniverse.sqlite`
- To reset the data is if you wish, is necessary to uncomment the line ***#35*** in `Startup.cs` file (See below). 
```
        //services.AddHostedService<CSV>();//service on charge of loading CSV file into db and deleting data
``` 

- Run the solution 
- Swagger UI will be showed and you can test the CRUD using it or Postman

>>If you have any questions, please let me know wladimir_llanos@hotmail.com

