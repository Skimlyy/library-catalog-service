library-catalog-service

CatalogService är ett ASP.NET Core Web API som hanterar katalogen över objekt som kan lånas i bibliotekssystemet. 
Tjänsten ansvarar för att lagra och tillhandahålla information om låneobjekt (Items) och deras kategorier (ItemTypes), till exempel böcker och teknisk utrustning.

API:t används av andra delar av systemet, exempelvis LoanService och MVC-klienten, för att hämta information om vilka objekt som finns tillgängliga i biblioteket. 
Detta är en del av en tjänsteorienterad arkitektur (SOA) där varje tjänst ansvarar för sin egen funktionalitet och databas.

Datamodell

API:t innehåller två tabeller: ItemTypes och Items. ItemTypes representerar kategorier av objekt, till exempel Bok och Utrustning. 
Items representerar de faktiska objekten som kan lånas.

Relationen mellan tabellerna är att en ItemType kan ha många Items, medan varje Item tillhör exakt en ItemType. Relationen implementeras via ItemTypeId. 
Exempel på hur objekten kopplas till sina kategorier är att boken Clean Code tillhör kategorin Bok, medan en dator som Dell XPS tillhör kategorin Utrustning.

API-endpoints

API:t innehåller flera endpoints för att hantera objekt och kategorier. För objekt (Items) finns möjligheten att hämta alla objekt genom endpointen GET /api/items. 
Ett specifikt objekt kan hämtas med GET /api/items/{id}. Nya objekt kan skapas via POST /api/items, befintliga objekt kan uppdateras via PUT /api/items/{id}, och objekt kan tas bort genom DELETE /api/items/{id}.

För kategorier (ItemTypes) finns liknande funktionalitet. Alla kategorier kan hämtas genom GET /api/itemtypes, medan en specifik kategori kan hämtas med GET /api/itemtypes/{id}. 
Det är också möjligt att skapa nya kategorier via POST /api/itemtypes, uppdatera befintliga kategorier med PUT /api/itemtypes/{id}, samt ta bort kategorier via DELETE /api/itemtypes/{id}.

Teknik

Projektet är byggt med ASP.NET Core Web API, Entity Framework Core och SQLite för databashantering. Swagger/OpenAPI används för testning av API:t, och DTOs används för API-respons.

Testning av API

Projektet innehåller en .http-testfil som kan användas för att testa API-endpoints direkt från JetBrains Rider. Filen innehåller exempel på att skapa kategori, skapa item, uppdatera item, hämta items och ta bort item. 
Detta demonstrerar full CRUD-funktionalitet.

Arkitektur

Systemet är designat enligt en tjänsteorienterad arkitektur (SOA) där varje tjänst ansvarar för en specifik del av systemet. CatalogService ansvarar för katalogdata, objekt och kategorier.

Användning av AI

AI har använts som stöd under utvecklingen för att diskutera arkitektur och design av API:t, få förslag på implementation av controllers och DTOs samt felsöka problem i kod och konfiguration.

All kod har granskats, anpassats och förståtts innan den inkluderats i projektet.