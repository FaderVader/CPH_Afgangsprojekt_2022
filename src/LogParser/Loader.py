from Types import LogLine
from Types import SearchSet
from Database import Database
from os import listdir
from datetime import datetime, timezone


class Loader:
    def __init__(self, searchSet:SearchSet): 
        self.structure = None
        self.searchSet = searchSet
        self.database = Database()
        self.setup()

    def setup(self):
        self.loadAllLogFiles()

    def loadAllLogFiles(self):
        linesList = []
        logfileDict = {}        
        sourceSystemDict = {}
        searchPeriod = self.searchSet.SearchPeriod

        for ssID in self.searchSet.SourceSystems:   
            logfiles = self.database.GetLogFileBySSId(ssID.ID)
            for logfile in logfiles:
                logLines = self.database.GetAllLogLinesByFileId(logfile['ID'], searchPeriod)
                for logLine in logLines:

                    # create LogLines instance and add to list
                    time = logLine['RawText'].split()[0]
                    tempLine = "{time} {id} {description}".format(time=time, id=logLine['ID'], description=logLine['EventDescription'])
                    line = LogLine(tempLine)
                    linesList.append(line)
                logfileDict[logfile['ID']] = linesList
                linesList = []
            sourceSystemDict[ssID.ID] = logfileDict
            logfileDict = {}
        self.structure = sourceSystemDict

    def GetStructuredLogs(self): 
        return self.structure 