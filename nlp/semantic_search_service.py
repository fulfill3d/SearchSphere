import logging
import numpy as np
import os
from typing import Dict, List

import faiss
from sentence_transformers import SentenceTransformer

from azure_app_config_client import AppConfigClient
from azure_cosmos_db_client import AzureCosmosDbClient


class SemanticSearchService:
    def __init__(self, cosmos_client: AzureCosmosDbClient):
        self.cosmos_client = cosmos_client  # Initialize Cosmos DB client
        self.model = SentenceTransformer('all-MiniLM-L6-v2')  # Pre-trained model
        self.index = faiss.IndexFlatL2(384)  # FAISS index with vector dimension 384
        self.data_store = []  # Store the texts corresponding to the embeddings

    def add_texts(self, texts: List[str]):
        if not texts:
            raise ValueError("Text list cannot be empty.")
        embeddings = self.model.encode(texts)
        embeddings = np.array(embeddings).astype('float32')  # Convert to float32 for FAISS
        self.index.add(embeddings)  # Add embeddings to FAISS
        self.data_store.extend(texts)  # Update the data store

    def search(self, query: str, k: int = 3):
        if not self.data_store:
            raise ValueError("The index is empty. Add texts before searching.")
        query_embedding = self.model.encode([query])
        query_embedding = np.array(query_embedding).astype('float32')
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
        content_fragments = self.cosmos_client.get_text_fragments_by_blob_name(blob_name)

        if not content_fragments:
            raise ValueError(f"No content fragments found for blob_name: {blob_name}")

        logging.info(f"Retrieved {len(content_fragments)} content fragments for blob_name: {blob_name}")

        # Add fragments to the FAISS index
        self.add_texts(content_fragments)
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