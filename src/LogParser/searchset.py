from pydantic import BaseModel
from typing import List

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

