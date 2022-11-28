from fastapi import FastAPI, APIRouter
from Types import SearchSet, SourceSystem, SearchPeriod
from Database import Database
from Shell import Shell

class Api():
    def __init__(self):
        self.router = APIRouter()
        self.router.add_api_route("/search", self.Search, methods=["POST"]) #
        self.router.add_api_route("/retrieve", self.Retrieve, methods=["GET"]) #
        self.dataBase = Database()
        self.results = None

    def Search(self, searchSet: SearchSet):
        shell = Shell(searchSet) 
        shell.do_find(searchSet.KeyWordList)
        self.results = shell.do_query()
        print(self.results)

    def Retrieve(self):
        return self.results

    # test
    def GetLogFiles(self, search: SearchSet):
        sourceSystems = {}
        for sourceSystem in search.SourceSystems:
            sourceSystems[sourceSystem.ID] = sourceSystem
            logFiles = self.dataBase.GetLogFileBySSId(sourceSystem.ID)
            for file in logFiles:
                print(file['ID'], file['FileName'])

    # test
    def Test(self):
        searchPeriod = SearchPeriod(Item1='2022-11-24T00:00:00', Item2='2022-11-25T00:00:00')
        sourceSystem1 = SourceSystem(ID=4, Name='Galaxy1', SourceFolder='C:\temp\logfiles')
        sourceSystem2 = SourceSystem(ID=6, Name='Galaxy2', SourceFolder='C:\temp\logfiles2')
        keywordList = 'test hest'
        searchSet = SearchSet(SourceSystems=[sourceSystem1, sourceSystem2], KeyWordList=keywordList, SearchPeriod=searchPeriod)

        shell = Shell(searchSet) #parameter
        shell.do_find("setupsession")
        # self.results = shell.do_query()
        self.results = shell.do_run('')
        print(self.results)

app = FastAPI()
api = Api()
app.include_router(api.router)

if __name__ == "__main__":
    api = Api()
    api.Test() 