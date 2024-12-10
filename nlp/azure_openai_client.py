import logging
import os
from openai import AzureOpenAI
from azure_app_config_client import AppConfigClient

class AzureOpenAIClient:
    """A client to interact with Azure OpenAI services."""

    def __init__(self, endpoint: str, key: str, model: str, api_version: str):
        """
        Initialize the Azure OpenAI Client with the provided connection details.
        """
        self.endpoint = endpoint
        self.key = key
        self.model = model
        self.client = AzureOpenAI(azure_endpoint=endpoint, api_key=key, api_version=api_version)
        logging.info("Azure OpenAI Client initialized.")

    def get_embeddings(self, text: str) -> list:
        """
        Get embeddings for the provided text.

        :param text: The text to convert into embeddings.
        :return: The embeddings as a list of floats.
        """
        try:
            logging.info(f"Getting embeddings for text: {text}")
            response = self.client.embeddings.create(
                input=text,
                model="text-embedding-3-small"  # Specify the model to use
            )
            embeddings = response.data[0].embedding
            logging.info("Embeddings retrieval successful.")
            return embeddings
        except Exception as e:
            logging.error(f"Failed to get embeddings: {e}")
            return []

if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)

    # Initialize AppConfigClient
    config_client = AppConfigClient(os.getenv('APP_CONFIG_URL'))
    openai_endpoint = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiEndpoint")
    openai_key = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiApiKey")
    openai_deployment_name = config_client.fetch_configuration_value("SearchSphere_AzureOpenAiDeploymentName")
    openai_api_version = "2023-05-15"  # Set the appropriate API version

    # Initialize Azure OpenAI Client
    openai_client = AzureOpenAIClient(
        endpoint=openai_endpoint,
        key=openai_key,
        model=openai_deployment_name,
        api_version=openai_api_version)

    # Example usage
    text = "Once upon a time"
    embeddings = openai_client.get_embeddings(text)
    print("Embeddings:", embeddings)