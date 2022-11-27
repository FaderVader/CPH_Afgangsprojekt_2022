from fastapi import FastAPI, APIRouter
from searchset import SearchSet
from Database import Database
from Shell import Shell

class Api():
    def __init__(self):
        self.router = APIRouter()
        self.router.add_api_route("/search", self.Search, methods=["POST"]) #
        self.dataBase = Database()

    def Search(self, search: SearchSet):
        id = search.SourceSystems[0].ID 
        sourceSystem = self.dataBase.GetSourceSystemById(id)
        # print(sourceSystem)
        self.GetLogFiles(search)

    def GetLogFiles(self, search: SearchSet):
        sourceSystems = {}
        for sourceSystem in search.SourceSystems:
            sourceSystems[sourceSystem.ID] = sourceSystem
            logFiles = self.dataBase.GetLogFileBySSId(sourceSystem.ID)
            for file in logFiles:
                print(file['ID'], file['FileName'])


    # def GetLinesFromFile(self, logFileId: int):

app = FastAPI()
api = Api()
app.include_router(api.router)

