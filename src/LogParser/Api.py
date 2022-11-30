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
        self.results = None # if result-query before new result is ready, ensure none is available
        shell = Shell(searchSet) 
        shell.do_find(searchSet.KeyWordList)
        shell.do_sort('True')
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
        searchPeriod = SearchPeriod(Item1='2020-12-10T15:49', Item2='2020-12-10T15:50')
        sourceSystem1 = SourceSystem(ID=10, Name='Galaxy5', SourceFolder='C:\temp\logfiles')
        sourceSystem2 = SourceSystem(ID=12, Name='Galaxy6', SourceFolder='C:\temp\logfiles2')
        keywordList = 'executing'
        searchSet = SearchSet(SourceSystems=[sourceSystem1, sourceSystem2], KeyWordList=keywordList, SearchPeriod=searchPeriod)

        shell = Shell(searchSet) #parameter
        shell.do_find(searchSet.KeyWordList)
        shell.do_sort('true')
        self.results = shell.do_query() # api test
        # self.results = shell.do_run('') # cmd test
        print(self.results)

app = FastAPI()
api = Api()
app.include_router(api.router)

if __name__ == "__main__":
    api = Api()
    api.Test() 