# Graham Hayes #
## For the Code Challenge for FIG Talent Solutions ##

## Notes ##
Unsure of desired behavior for query parameters (i.e. 400 if invalid) & if to default, so behavior is configurable via the Constants defined in the root `Settings.cs` file:
- `QueryByCritOnInvalid`: 401 if query by parameter is specified but invalid
- `QueryByDefaultField`: if queryBy is invalid it defaults it to the specified parameter

## Seeding Data ##
If you want to seed the database with some mock data set `Settings.SeedDatabase` to true


## End Points ##
 are as follows:
- [GET] `/teams`
	- List all teams
- [GET] `/teams?orderBy=<name/location>`
	- list teams teams and order by name or location ASC
- [POST] `/teams`
	- create a team (can pass players if desired)
- [GET] `/teams/{teamId}/addPlayer/{playerId}`
	- add player to team
- [GET] `/teams/{teamId}/removePlayer/{playerId}`
	- remove player from team
- [GET] `/players`
	- list all players
- [GET] `/players?teamId=<id>`
	- list all players in the specified team id
	- `/players/query/onTeamId/{id}` gives same result
- [GET] `/players?lastName=<name>`
	- list all players with the specified last name
	- `/players/query/byLastName/{name}` gives same result
- [GET] `/players?teamName=<name>`
	- list all players in the team whos name is specified
	- `/players/query/byOnTeamName/{name}` gives same result
- [POST] `/player`
	- create player (can specify team id)	- 

## To Run: ##
`dotnet run` in `/FigApi` 
