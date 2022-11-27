import pymssql

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

    def GetAllLogLinesByFileId(self, logFileId:int):
        """
        [item['ID', 'SourceSystemID', 'LogFileID', 'TimeOfEvent', 'EventDescription', 'SourceModule', 'RawText']]
        """
        with pymssql.connect(self.server, self.username, self.pwd, self.database) as connection:
            with connection.cursor(as_dict =True) as cursor:
                cursor.execute('SELECT * FROM LogLines WHERE LogFileID=%s', logFileId)
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