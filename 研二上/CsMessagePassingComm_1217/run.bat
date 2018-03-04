
cd /d %~dp0

cd Client\bin\Debug &
start Client.exe &
cd ..\..\..\RepositoryServer\bin\Debug &
start RepositoryServer.exe &
cd ..\..\..\BuildServer\bin\Debug
start BuildServer.exe &
cd ..\..\..\