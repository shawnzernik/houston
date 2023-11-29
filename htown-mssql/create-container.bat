docker create ^
-e "ACCEPT_EULA=Y" ^
-e "MSSQL_SA_PASSWORD=Welcome123" ^
-p 1433:1433 ^
--network houston ^
--name htown-mssql ^
mcr.microsoft.com/mssql/server:2022-latest
pause
