# Houston Message Board
This is a sample AspNetCore minimal Web API application using HTML and JavaScript UI.  To run this application, you will need the following environment variables set:

- CONNECTION_STRING -- Data Source=LOCALHOST;Initial Catalog=MessageBoard;User ID=sa;Password=Welcome123;TrustServerCertificate=Yes

## Design Principals
The application is broken into a few major components.  The objective is to keeps this as simple as possible, and not incorporate any technologies that are not absolutely necessary.

### Database Project
We are taking a database first approach.  This is codified in a Visual Studio Database project.  No changes should be made to the database directly - make changes to the database project then publish.

### WebAPI Project
This is a ASPNET Core application.  This uses simple WebApi.  The "Program.cs" file loads the Endpoints.  

#### Endpoints
The Endpoints bind to the web application object and adds the GET/POST/DELETE methods.  It also registers OpenApi.  The endpoint should contain no business or data logic.  The business and data logic should be the responsibility of the entities.

#### Response
Endpoints will return a Response object and a status 200.  The response object will either have content, or will have an error, trace, and inner errors.  

### Entity Framework
For data access we have utilized Entity Framework.  These classes are not generated, but hand coded to match the tables.  They do not utilize lazy loading, and do not contain lists/enumerable of child relations.  This was decided to keep saving simple - old EF had issues with parent/child updates and I wanted to avoid these complexities.

The "Entities" have methods for loading, saving, and deleting the entity of it's type using the DatabaseContext.  This was decided so that the Endpoint contain no business or data logic.

### wwwroot
The application handles static HTML, CSS, and JavaScript.  This is essentially it's own web application.  

#### Screens
Each screen is designed to be a standalone single page application.  The screen model is to have a "list" and "edit" screen.  Each screen named "screen.html" will have a "screen.js" file.  This JavaScript file will bind event to all controls and handle all logic associated with loading and control events.

Screens should not contain business logic.  They should simply respond to control events, load controls from entities, load entities from controls, and pass all calls for business logic and data access to the associated entities.

#### Entities & Libraries
We are using the ability for the browser to "import" JS files to load modules.  The "lib" folder has the entities that match the C# entities, but call back to the WebAPIs for data access.  This also contains a corresponding "Response" object that is used by the WebAPI endpoints.

For business logic that's needed in both places, the preference is to not duplicate the efforts and code in both JS and C#.  This logic should be stored/enforced on the server side and WebAPIs calls used to process and get results of business logic.