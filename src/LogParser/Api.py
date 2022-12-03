from fastapi import FastAPI, APIRouter, HTTPException
from Types import SearchSet, SourceSystem, SearchPeriod
from Database import Database
from Shell import Shell

class Api():
    def __init__(self):
        self.router = APIRouter()
        self.router.add_api_route("/search", self.Search, methods=["POST"]) #
        self.router.add_api_route("/research", self.ReSearch, methods=["POST"]) #
        self.router.add_api_route("/retrieve", self.Retrieve, methods=["GET"]) #
        self.dataBase = Database()
        self.shell = None
        self.results = None

    def Search(self, searchSet: SearchSet):
        self.results = None # if result-query before new result is ready, ensure none is available
        self.shell = Shell(searchSet) 
        self.shell.do_find(searchSet.KeyWordList)
        self.shell.do_sort('True')
        self.results = self.shell.do_run()
        print(self.results)

    def ReSearch(self, searchSet: SearchSet):
        if (self.shell == None):
            raise HTTPException(status_code=400, detail="Previous search not valid") 
        self.results = None
        self.shell.do_find(searchSet.KeyWordList)
        self.shell.do_sort('True')
        self.results = self.shell.do_run()
        print(self.results)

    def Retrieve(self):
        return self.results

    # test
    def Test(self):
        searchPeriod = SearchPeriod(Item1='2020-12-09T15:49', Item2='2020-12-10T15:50')
        sourceSystem1 = SourceSystem(ID=2, Name='Galaxy5', SourceFolder='C:\temp\logfiles')
        sourceSystem2 = SourceSystem(ID=12, Name='Galaxy6', SourceFolder='C:\temp\logfiles2')
        keywordList = 'setupsession completed' #setupsession
        searchSet = SearchSet(SourceSystems=[sourceSystem1, sourceSystem2], KeyWordList=keywordList, SearchPeriod=searchPeriod)

        shell = Shell(searchSet) #parameter
        shell.do_find(searchSet.KeyWordList)
        shell.do_sort('true')
        self.results = shell.do_run() # api test
        print(self.results)

app = FastAPI()
api = Api()
app.include_router(api.router)

if __name__ == "__main__":
    api = Api()
    api.Test() 
