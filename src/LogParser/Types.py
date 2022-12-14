from collections import namedtuple
from datetime import datetime
from pydantic import BaseModel
from typing import List

## LogWatcher types
class SourceSystem(BaseModel):
    ID: int
    Name: str
    SourceFolder: str
    LineTemplate: str | None = None

class SearchPeriod(BaseModel):
    Item1: str
    Item2: str

class SearchSet(BaseModel):
  SourceSystems: List[SourceSystem]
  KeyWordList: str | None = None
  SearchPeriod: SearchPeriod

## LogParser type
class LogLine():
    """
    This class is used for encapsulating a single line from a log-file.
    """
    def __init__(self, logLine):
        self.lineElements = self.readLineElements(logLine)

    def GetTimeStamp(self):
        return self.lineElements[0]

    def GetPayLoad(self):
        return self.lineElements[1]

    def readLineElements(self, logLine):
        elements = logLine.split()
        try:
            timeStamp = self.ConvertStringToTime(elements[0]) 
        except IndexError:
            timeStamp = ''

        try:
            startIndex = logLine.find(' ', 0)
            payload = logLine[startIndex:]
        except IndexError:
            payload = ''

        return (timeStamp, payload)

    @staticmethod
    def ConvertStringToTime(date_args):  # date_args = '2020-09-04-18:16:12.1515421' '2020-09-29T08:42:42.0346299+02:00'
        """
        Convert a standard date-argument to Epoch timestamp.
        """
        try:
            timeString = date_args.replace('T', '-')
            timeString = timeString[0:26]  # remove '+02:00' + trim milisec part down

            date = datetime.strptime(timeString, "%Y-%m-%d-%H:%M:%S.%f")
            return date.timestamp()  # convert to UNIX time
        except:
            return ''

    @staticmethod
    def ConvertTimestampToString(timestamp):
        """
        Convert an epoch timestamp to standard date. 
        """
        time = datetime.fromtimestamp(timestamp).strftime('%Y-%m-%d-%H:%M:%S.%f')  # convert from UNIX time
        return time

"""
Public namedtuples 
"""
# pointer to occurence of node in source-file
Terminator = namedtuple("Terminator", "client date linenumber payload")

# used in Query.StartEnd
IntervalPair = namedtuple("IntervalPair", "delta pointer_A pointer_B")

if __name__ == "__main__":
    epoch = LogLine.ConvertStringToTime('2020-10-01T09:12:02.7398274+02:00')
    std_time = LogLine.ConvertTimestampToString(epoch)
    print(f'IN: {epoch} -> OUT: {std_time}')
