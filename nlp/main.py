from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import os
from typing import List
import numpy as np
from azure_app_config_client import AppConfigClient
from azure_cosmos_db_client import AzureCosmosDbClient
from azure_openai_client import AzureOpenAIClient
from semantic_search_service import SemanticSearchService
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

app = FastAPI()

# Initialize dependencies
config_client = AppConfigClient(os.getenv("APP_CONFIG_URL"))

cosmos_db_client = AzureCosmosDbClient(
    endpoint=config_client.fetch_configuration_value("Fulfill3dCosmosEndpointUri"),
    key=config_client.fetch_configuration_value("Fulfill3dCosmosPrimaryKey"),
    database_name=config_client.fetch_configuration_value("SearchSphere_CosmosDbDatabaseId"),
    container_name=config_client.fetch_configuration_value("SearchSphere_CosmosDbContainerId")
)

# Initialize Azure OpenAI Client
openai_endpoint = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiEndpoint")
openai_key = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiApiKey")
openai_deployment_name = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiDeploymentName")
openai_api_version = "2023-05-15"  # Set the appropriate API version

openai_client = AzureOpenAIClient(
    endpoint=openai_endpoint,
    key=openai_key,
    model=openai_deployment_name,
    api_version=openai_api_version
)

search_service = SemanticSearchService(
    cosmos_client=cosmos_db_client,
    openai_client=openai_client
)


class SearchRequest(BaseModel):
    blob_name: str
    question: str


class SearchResponse(BaseModel):
    answer: str


@app.post("/search", response_model=SearchResponse)
async def perform_semantic_search(request: SearchRequest):
    """
    Perform semantic search based on the provided blob_name and question.

    :param request: Request body containing blob_name and question.
    :return: List of search results.
    """
    try:
        # Process the message and perform semantic search
        answer = search_service.process_message(request.model_dump())
        return SearchResponse(answer=answer)
    except ValueError as ve:
        raise HTTPException(status_code=400, detail=str(ve))
    except Exception as e:
        raise HTTPException(status_code=500, detail="An error occurred during processing")


@app.get("/")
async def root():
    """
    Root endpoint to confirm the service is running.
    """
    return {"message": "Semantic Search Service is running"}
