docker create ^
--name htown-msg ^
--network houston ^
-p 8080:8080 ^
-e CONNECTION_STRING="Data Source=htown-mssql;Initial Catalog=MessageBoard;User ID=sa;Password=Welcome123;TrustServerCertificate=Yes" ^
szmsg
pause