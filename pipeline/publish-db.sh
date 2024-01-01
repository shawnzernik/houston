#! /bin/sh

dotnet build ../htown-msg/database/database.sqlproj

# https://learn.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage?view=sql-server-ver16
sqlpackage /a:publish \
	/sf:../htown-msg/database/bin/Debug/database.dacpac \
	/tsn:messageboard-database.cjmkgk4ys7so.us-east-2.rds.amazonaws.com \
	/tdn:MessageBoard \
	/tu:admin \
	/tp:Welcome123 \
	/ttsc:true