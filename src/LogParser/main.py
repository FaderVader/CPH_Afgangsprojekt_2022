from fastapi import FastAPI
from Item import Item

app = FastAPI()

@app.get("/api")
def hello():
  return {"Hello world!"}

@app.post("/items/")
async def create(item: Item): 
  return item