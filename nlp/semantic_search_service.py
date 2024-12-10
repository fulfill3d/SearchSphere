import logging
import numpy as np
import os
from typing import Dict, List

import faiss

from azure_app_config_client import AppConfigClient
from azure_cosmos_db_client import AzureCosmosDbClient


class SemanticSearchService:
    def __init__(self, cosmos_client: AzureCosmosDbClient):
        self.cosmos_client = cosmos_client  # Initialize Cosmos DB client
        self.index = faiss.IndexFlatL2(768)  # FAISS index with vector dimension 768

    def add_texts(self, embeddings: List[float]):
        if not embeddings:
            raise ValueError("Embeddings list cannot be empty.")
        embeddings = np.array(embeddings).astype('float32')  # Convert to float32 for FAISS
        self.index.add(embeddings)  # Add embeddings to FAISS

    def search(self, query_vector: List[float], k: int = 3):
        if not self.data_store:
            raise ValueError("The index is empty. Add vectors before searching.")
        query_embedding = np.array([query_vector]).astype('float32')
        distances, indices = self.index.search(query_embedding, k)
        results = [
            {"text": self.data_store[i], "distance": float(distances[0][j])}
            for j, i in enumerate(indices[0])
        ]
        return results

    def process_message(self, message: Dict[str, str]):
        """
        Process a message containing `blob_name` and `question`, perform semantic search, and return results.

        :param message: A dictionary containing `blob_name` and `question`.
        :return: List of search results.
        """
        # Extract blob_name and question from the message
        blob_name = message.get("blob_name")
        question = message.get("question")

        if not blob_name or not question:
            raise ValueError("Message must contain both `blob_name` and `question` fields.")

        # Retrieve content fragments from Cosmos DB
        logging.info(f"Retrieving content fragments for blob_name: {blob_name}")
        embeddings = self.cosmos_client.get_embeddings_by_blob_name(blob_name)

        if not embeddings:
            raise ValueError(f"No content fragments found for blob_name: {blob_name}")

        logging.info(f"Retrieved {len(embeddings)} content fragments for blob_name: {blob_name}")

        # Add fragments to the FAISS index
        self.add_texts(embeddings)
        logging.info("Content fragments added to the FAISS index.")

        # Perform semantic search
        logging.info(f"Performing semantic search for question: {question}")
        results = self.search(question)
        logging.info(f"Semantic search completed. Found {len(results)} results.")

        return results


if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)

    # Initialize AppConfigClient
    config_client = AppConfigClient(os.getenv('APP_CONFIG_URL'))
    cosmos_db_endpoint = config_client.fetch_configuration_value("Fulfill3dCosmosEndpointUri")
    cosmos_db_key = config_client.fetch_configuration_value("Fulfill3dCosmosPrimaryKey")
    cosmos_db_database_name = config_client.fetch_configuration_value("SearchSphere_CosmosDbDatabaseId")
    cosmos_db_container_name = config_client.fetch_configuration_value("SearchSphere_CosmosDbContainerId")

    # Initialize Cosmos DB Client
    cosmos_client = AzureCosmosDbClient(
        endpoint=cosmos_db_endpoint,
        key=cosmos_db_key,
        database_name=cosmos_db_database_name,
        container_name=cosmos_db_container_name
    )

    # Process the message
    semantic_service = SemanticSearchService(cosmos_client=cosmos_client)

    # Example message
    message = {
        "blob_name": "b3111a7e-cfa1-49d0-b34e-22ea227dec11",
        "question": "What are the effects of climate change on agriculture?"
    }

    results = semantic_service.process_message(message)

    # Print the results
    print("Search Results:", results)