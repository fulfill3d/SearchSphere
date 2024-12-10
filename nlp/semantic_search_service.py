import logging
import numpy as np
import os
from typing import Dict, List

import faiss

from azure_app_config_client import AppConfigClient
from azure_cosmos_db_client import AzureCosmosDbClient
from azure_openai_client import AzureOpenAIClient

from transformers import pipeline


class SemanticSearchService:
    def __init__(self, cosmos_client: AzureCosmosDbClient, openai_client: AzureOpenAIClient):
        self.cosmos_client = cosmos_client  # Initialize Cosmos DB client
        self.openai_client = openai_client  # Initialize Azure OpenAI client
        self.index = faiss.IndexFlatL2(1536)  # FAISS index with vector dimension 1536
        self.data_store = []
        self.blob_name = None
        self.question = None
        self.closest_text = None
        self.qa_pipeline = pipeline(
            "question-answering",
            model="bert-base-uncased",
            tokenizer="bert-base-uncased",
            device=0)

    def add_texts(self, documents: List[dict]):
        if not documents:
            raise ValueError("Document list cannot be empty.")
        # Extract content fragments
        embeddings = [doc["embedding"] for doc in documents if "embedding" in doc]
        embeddings = np.array(embeddings).astype('float32')  # Convert to float32 for FAISS
        self.index.add(embeddings)  # Add embeddings to FAISS
        text_contents = [doc["text-chunk"] for doc in documents if "text-chunk" in doc]
        self.data_store.extend(text_contents) # Store embeddings for reference

    def search(self, query_vector: List[float], k: int = 3):
        if not self.data_store:
            raise ValueError("The index is empty. Add vectors before searching.")
        query_embedding = np.array([query_vector]).astype('float32')
        distances, indices = self.index.search(query_embedding, k)
        self.closest_text = self.data_store[indices[0][0]]

    def generate_answer(self):
        if not self.closest_text or not self.question:
            raise ValueError("Both `closest_text` and `question` must be set before generating an answer.")
        logging.info(f"Generating answer for question: {self.question} with context: {self.closest_text}")
        answer = self.qa_pipeline(question=self.question, context=self.closest_text)
        return answer["answer"]

    def process_message(self, message: Dict[str, str]):
        """
        Process a message containing `blob_name` and `question`, perform semantic search, and return results.

        :param message: A dictionary containing `blob_name` and `question`.
        :return: List of search results.
        """
        # Extract blob_name and question from the message
        self.blob_name = message.get("blob_name")
        self.question = message.get("question")

        if not self.blob_name or not self.question:
            raise ValueError("Message must contain both `blob_name` and `question` fields.")

        # Retrieve content fragments from Cosmos DB
        logging.info(f"Retrieving content fragments for blob_name: {self.blob_name}")
        document_embeddings = self.cosmos_client.get_documents_by_blob_name(self.blob_name)

        if not document_embeddings:
            raise ValueError(f"No content fragments found for blob_name: {self.blob_name}")

        logging.info(f"Retrieved {len(document_embeddings)} content fragments for blob_name: {self.blob_name}")

        # Add fragments to the FAISS index
        self.add_texts(document_embeddings)
        logging.info("Content fragments added to the FAISS index.")

        # Perform semantic search
        logging.info(f"Performing semantic search for question: {self.question}")
        question_embedding = self.openai_client.get_embeddings(self.question)
        self.search(question_embedding)
        answer = self.generate_answer()
        logging.info(f"Semantic search completed. Answer:\n\n{answer}")

        return answer


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

    # Process the message
    semantic_service = SemanticSearchService(
        cosmos_client=cosmos_client,
        openai_client=openai_client
    )

    # Example message
    message = {
        "blob_name": "a12ba5a0-2b64-401e-8c9f-373fcc4427f9",
        "question": "What are the effects of climate change on agriculture?"
    }

    answer = semantic_service.process_message(message)

    # Print the results
    print("Search Results:", answer)