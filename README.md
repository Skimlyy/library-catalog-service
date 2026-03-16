library-catalog-service

CatalogService är ett ASP.NET Core Web API som hanterar katalogen över objekt som kan lånas i ett bibliotekssystem.
Tjänsten ansvarar för att lagra och tillhandahålla information om låneobjekt (Items) och deras kategorier (ItemTypes), till exempel böcker och teknisk utrustning.
API:t används av andra delar av systemet, exempelvis LoanService och MVC-klienten, för att hämta information om vilka objekt som finns tillgängliga i biblioteket. 
Detta är en del av en tjänsteorienterad arkitektur (SOA) där varje tjänst ansvarar för sin egen funktionalitet och databas.

Datamodell
API:t innehåller två tabeller: ItemTypes och Items. ItemTypes representerar kategorier av objekt, till exempel Bok och Utrustning. 
Items representerar de faktiska objekten som kan lånas.
Relationen mellan tabellerna är att en ItemType kan ha många Items, medan varje Item tillhör exakt en ItemType. 
Relationen implementeras via ItemTypeId och en främmande nyckel med DeleteBehavior.Restrict, vilket förhindrar att en kategori raderas om den används av ett eller flera objekt.

Säkerhet
POST, PUT och DELETE-endpoints är skyddade med API-nyckel via en egen ApiKeyFilter i Security-mappen. Anrop till dessa endpoints kräver headern X-Api-Key med korrekt nyckel. 
GET-endpoints är öppna så att andra tjänster kan läsa katalogen utan nyckel.
API-nyckeln lagras som en miljövariabel i Azure och finns inte i källkoden.

API-endpoints
För objekt (Items) finns möjligheten att hämta alla objekt genom GET /api/items. Ett specifikt objekt kan hämtas med GET /api/items/{id}. 
Nya objekt kan skapas via POST /api/items (kräver API-nyckel), befintliga objekt kan uppdateras via PUT /api/items/{id} (kräver API-nyckel), och objekt kan tas bort genom DELETE /api/items/{id} (kräver API-nyckel).
För kategorier (ItemTypes) finns liknande funktionalitet. Alla kategorier kan hämtas genom GET /api/itemtypes, medan en specifik kategori kan hämtas med GET /api/itemtypes/{id}.
Det är också möjligt att skapa nya kategorier via POST /api/itemtypes (kräver API-nyckel), uppdatera befintliga kategorier med PUT /api/itemtypes/{id} (kräver API-nyckel), samt ta bort kategorier via DELETE /api/itemtypes/{id} (kräver API-nyckel).

Teknik
Projektet är byggt med ASP.NET Core Web API, Entity Framework Core och SQLite för databashantering. Swagger/OpenAPI används för testning av API:t, och DTOs används för API-respons. Tjänsten är driftsatt på Microsoft Azure.

Testning av API
Projektet innehåller en .http-testfil som kan användas för att testa API-endpoints direkt från JetBrains Rider. Filen innehåller exempel på att skapa kategori, skapa item, uppdatera item, hämta items och ta bort item.
Detta demonstrerar full CRUD-funktionalitet.

Arkitektur
Systemet är designat enligt en tjänsteorienterad arkitektur (SOA) där varje tjänst ansvarar för en specifik del av systemet. CatalogService ansvarar för katalogdata, objekt och kategorier.

Användning av AI
AI har använts som stöd under utvecklingen för att diskutera arkitektur och design av API:t, få förslag på implementation av controllers, DTOs och säkerhetslösningar samt felsöka problem i kod och konfiguration. 
All kod har granskats, anpassats och förståtts innan den inkluderats i projektet.