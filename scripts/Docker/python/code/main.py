from fastapi import FastAPI
from Item import Item
import pymssql

app = FastAPI()

@app.get("/api")
def hello():
  return {"Hello world!"}

@app.post("/items/")
async def create(item: Item): 
  return item


server = "log-store-db"
database = "LogStore"
username = "sa"
pwd = "Jakob12345!"

cnxn = pymssql.connect(server, username, pwd, database)
cursor = cnxn.cursor()

cursor.execute("SELECT @@version;") 
row = cursor.fetchone() 
while row: 
    print(row[0])
    row = cursor.fetchone()

cnxn.close()