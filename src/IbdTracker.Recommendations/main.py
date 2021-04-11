from fastapi import FastAPI

app = FastAPI()


@app.get("/recommendations")
async def root():
    return {"isOnline": True}
