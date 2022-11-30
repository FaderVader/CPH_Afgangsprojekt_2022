USE LogStore;
GO

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

--DELETE FROM LogFiles WHERE SourceSystemID = 1 AND FileName = 'GalaxySiteSelector-AX76707-20201210.log';

SELECT * FROM LogFiles WHERE SourceSystemID = 6;
SELECT * FROM LogLines WHERE LogFileID = 7 ORDER BY TimeOfEvent; --AND SourceSystemID = 4; 

SELECT * FROM LogLines WHERE LogFileID=15 AND TimeOfEvent BETWEEN '2020-12-10 15:49' AND '2020-12-10 15:50';

SELECT * FROM LogLines WHERE SourceSystemID=10 AND TimeOfEvent BETWEEN '2020-12-10 15:49' AND '2020-12-10 15:50';
