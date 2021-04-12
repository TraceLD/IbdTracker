import concurrent.futures
import asyncio

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from models import FoodItemRecommendationData

app = FastAPI()

# Only ASP.NET Core should be able to call this microservice over localhost.
# Users should not be able to call it directly as it does not have auth.
origins = [
    "http://localhost"
    "http://localhost:5000",
    "https://localhost:5001"
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.get("/")
async def root():
    return {"isOnline": True}


@app.post("/recommendations")
async def recommendations(data: list[FoodItemRecommendationData]):
    for d in data:
        print(d.foodItemPainInfo)

    return []


@app.get("/test")
async def test():
    with concurrent.futures.ProcessPoolExecutor() as pool:
        pil_args = ["hello"]
        print(await asyncio.get_event_loop().run_in_executor(pool, test_some_long_method, *pil_args))


def test_some_long_method(some_string: str):
    return some_string
