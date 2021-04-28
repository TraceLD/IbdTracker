import concurrent.futures
import asyncio

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from api.models import FoodItemRecommendationData
from api.fuzzy_recommendations import food_items_recommendations as fir

app = FastAPI()
firs = fir.FoodItemRecommendationsSystem()

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


@app.post("/recommendations/fi")
async def recommendations(data: list[FoodItemRecommendationData]):
    with concurrent.futures.ProcessPoolExecutor() as pool:
        pil_args = [data]
        res = await asyncio.get_event_loop().run_in_executor(pool, firs.process_all_fis, *pil_args)
        return res
