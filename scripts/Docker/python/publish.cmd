REM source C:\GIT_REPOS\cph\CPH_Afgangsprojekt_2022\src\LogParser
REM target C:\GIT_REPOS\cph\CPH_Afgangsprojekt_2022\scripts\Docker\python\code

SET source="%~dp0%\..\..\..\src\LogParser\"
SET destination="%~dp0%\code\"

XCOPY %source% %destination% /Y