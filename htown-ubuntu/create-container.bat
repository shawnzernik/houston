mkdir c:\code\deloitte\htown-ubuntu\root
docker create ^
--name htown-ubuntu ^
--network houston ^
-v "c:\code\deloitte\htown-ubuntu\root":/root ^
-it ^
ubuntu

pause