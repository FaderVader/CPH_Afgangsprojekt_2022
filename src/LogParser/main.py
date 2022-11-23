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


server = "localhost"  ##log-store-db ##localhost
database = "LogStore"
username = "sa"
pwd = "Jakob12345!"

cnxn = pymssql.connect(server, username, pwd, database)
cursor = cnxn.cursor()

cursor.execute("SELECT * FROM LogFiles WHERE Id = 1;") ##SELECT * FROM LogFiles WHERE Id = 1; ##SELECT @@version;
row = cursor.fetchone() 
while row: 
    print(row[2])
    row = cursor.fetchone()

cnxn.close()