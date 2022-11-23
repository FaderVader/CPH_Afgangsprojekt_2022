SELECT * FROM SourceSystems;
SELECT * FROM LogFiles;
SELECT * FROM LogLines;

DELETE FROM LogLines --WHERE SourceSystemID = 1 AND LogFileID = 1;
DELETE FROM LogFiles;
DELETE FROM SourceSystems;

--DROP TABLE LogLines;
--DROP TABLE LogFiles;
--DROP TABLE SourceSystems;

DBCC CHECKIDENT (SourceSystems, RESEED, 0);
DBCC CHECKIDENT (LogFiles, RESEED, 0);
DBCC CHECKIDENT (LogLines, RESEED, 0);

DELETE FROM LogFiles WHERE SourceSystemID = 1 AND FileName = 'GalaxySiteSelector-AX76707-20201210.log';