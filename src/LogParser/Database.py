import pymssql
from Types import SearchPeriod
import datetime
from dateutil.parser import parse

class Database():
    def __init__(self):
        self.server = "localhost"
        self.database = "LogStore"
        self.username = "sa"
        self.pwd = "Jakob12345!"

    def GetSourceSystemById(self, sourceSystemId:int):
        """
        [item['ID', 'Name', 'SourceFolder', 'LineTemplate']]
        """
        with pymssql.connect(self.server, self.username, self.pwd, self.database) as connection:
            with connection.cursor(as_dict =True) as cursor:
                cursor.execute('SELECT * FROM SourceSystems WHERE ID=%s', sourceSystemId)
                for row in cursor:
                    return row

    def GetLogfileById(self, logFileId:int):
        """
        [item['ID', 'SourceSystemID', 'FileName', 'FileHash']]
        """
        with pymssql.connect(self.server, self.username, self.pwd, self.database) as connection:
            with connection.cursor(as_dict =True) as cursor:
                cursor.execute('SELECT * FROM LogFiles WHERE ID=%s', logFileId)
                for row in cursor:
                    # print("LogFileID=%d, Name=%s" % (row['ID'], row['FileName']))
                    return row

    def GetLogFileBySSId(self, sourceSystemId : int):
        """
        [item['ID', 'SourceSystemID', 'FileName', 'FileHash']]
        """
        with pymssql.connect(self.server, self.username, self.pwd, self.database) as connection:
            with connection.cursor(as_dict =True) as cursor:
                cursor.execute('SELECT * FROM LogFiles WHERE SourceSystemID=%s', sourceSystemId)
                allFiles = []
                for row in cursor:
                    allFiles.append(row)
                return allFiles

    def GetAllLogLinesByFileId(self, logFileId:int, searchPeriod: SearchPeriod=None):
        """
        [item['ID', 'SourceSystemID', 'LogFileID', 'TimeOfEvent', 'EventDescription', 'SourceModule', 'RawText']]
        """

        query = 'SELECT * FROM LogLines WHERE LogFileID={}'.format(logFileId)
        if searchPeriod != None:
            start = parse(searchPeriod.Item1).strftime("%Y-%m-%d %H:%M")
            end = parse(searchPeriod.Item2).strftime("%Y-%m-%d %H:%M")
            query += " AND TimeOfEvent BETWEEN '{}' AND '{}';".format(start, end)

        with pymssql.connect(self.server, self.username, self.pwd, self.database) as connection:
            with connection.cursor(as_dict =True) as cursor:
                cursor.execute(query)
                allLogLines = []
                for row in cursor:
                    # print("LogLineID=%d, TimeOfEvent=%s" % (row['ID'], row['TimeOfEvent']))
                    allLogLines.append(row)
                return allLogLines

# testitems
if __name__ == "__main__":
    database = Database()
    logfile = database.GetLogfileById(7)
    print(logfile)

    logfiles = database.GetLogFileBySSId(4)
    print(logfiles)

    sourceSystem = database.GetSourceSystemById(4)
    print(sourceSystem)

    lines = database.GetAllLogLinesByFileId(7)
    print(len(lines))