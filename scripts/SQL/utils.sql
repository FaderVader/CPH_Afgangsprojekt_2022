USE LogStore;
GO

SELECT * FROM SourceSystems;
SELECT * FROM LogFiles;
SELECT * FROM LogLines;

--DELETE FROM LogLines --WHERE SourceSystemID = 1 AND LogFileID = 1;
--DELETE FROM LogFiles
--DELETE FROM SourceSystems;

--DBCC CHECKIDENT (SourceSystems, RESEED, 0);
--DBCC CHECKIDENT (LogFiles, RESEED, 0);
--DBCC CHECKIDENT (LogLines, RESEED, 0);

--DROP TABLE LogLines;
--DROP TABLE LogFiles;
--DROP TABLE SourceSystems;

--DELETE FROM LogFiles WHERE SourceSystemID = 1 AND FileName = 'GalaxySiteSelector-AX76707-20201210.log';

SELECT * FROM LogFiles WHERE SourceSystemID = 6;
SELECT * FROM LogLines WHERE LogFileID = 16 ORDER BY TimeOfEvent; --AND SourceSystemID = 4; 

SELECT * FROM LogLines WHERE LogFileID=15 AND TimeOfEvent BETWEEN '2020-12-10 15:49' AND '2020-12-10 15:50';
SELECT * FROM LogLines WHERE SourceSystemID=10 AND TimeOfEvent BETWEEN '2020-12-10 15:49' AND '2020-12-10 15:50';

SELECT * FROM LogLines WHERE SourceSystemID = 10 and LogFileID = 15  AND TimeOfEvent BETWEEN '10-12-2020 00:00:00' AND '11-12-2020 23:00:00';
SELECT * FROM LogLines WHERE SourceSystemID = 10 ORDER BY TimeOfEvent;
SELECT * FROM LogLines WHERE SourceSystemID = 10 AND TimeOfEvent BETWEEN '10-12-2020 15:46:00' AND '10-12-2020 16:48:00';
SELECT * FROM LogLines WHERE SourceSystemID = 10 and LogFileID = 15  AND TimeOfEvent BETWEEN '2020-09-04 18:16:00' AND '2020-09-04 18:20:00';

SELECT * FROM LogLines WHERE LEN(RawText) < 101;
SELECT * FROM LogLines ORDER BY LogFileID;
SELECT * FROM LogLines WHERE SourceSystemID = 5 ORDER BY TimeOfEvent;