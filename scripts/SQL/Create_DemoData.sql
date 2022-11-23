USE LogStore;
GO

INSERT SourceSystems(Name, SourceFolder, LineTemplate)
VALUES ('GalaxySiteSelector', 'C:\temp\logfiles', '')

INSERT SourceSystems(Name, SourceFolder, LineTemplate)
VALUES ('OtherSystem', 'C:\temp\otherfiles', '')


INSERT LogFiles(SourceSystemID, FileName, FileHash)
VALUES (1, 'GalaxySiteSelector-AX76707-20201210.log', 'ccc');

INSERT LogFiles(SourceSystemID, FileName, FileHash)
VALUES (1, 'GalaxySiteSelector-AX76707-20201211.log', 'fff');

INSERT LogFiles(SourceSystemID, FileName, FileHash)
VALUES (2, 'test.log', 'aaa');

INSERT LogFiles(SourceSystemID, FileName, FileHash)
VALUES (2, 'test2.log', 'bbb');

INSERT LogLines(SourceSystemID, LogFileID, TimeOfEvent, Severity, EventDescription, SourceModule, RawText)
VALUES (1, 1, SYSDATETIME(), 'ERROR', 'Something bad happened', 'main.executor.test', 'all the text')

INSERT LogLines(SourceSystemID, LogFileID, TimeOfEvent, Severity, EventDescription, SourceModule, RawText)
VALUES (1, 2, SYSDATETIME(), 'ERROR', 'Something bad happened', 'main.executor.test', 'all the text')

SELECT * FROM SourceSystems;
SELECT * FROM LogFiles;
SELECT * FROM LogLines;