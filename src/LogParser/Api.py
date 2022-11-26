from fastapi import FastAPI, APIRouter
from searchset import SearchSet
from Database import Database

class Api():
    def __init__(self):
        self.router = APIRouter()
        self.router.add_api_route("/search", self.Search, methods=["POST"]) #
        self.dataBase = Database()

    def Search(self, search: SearchSet):
        id = search.SourceSystems[0].ID 
        sourceSystem = self.dataBase.GetSourceSystemById(id)
        print(sourceSystem)

app = FastAPI()
api = Api()
app.include_router(api.router)