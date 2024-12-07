import logging
import os
from azure.cosmos import CosmosClient, exceptions
from azure_app_config_client import AppConfigClient


class AzureCosmosDbClient:
    """A Cosmos DB Client that can query and retrieve documents from a container."""

    def __init__(self, endpoint: str, key: str, database_name: str, container_name: str):
        """
        Initialize the Cosmos DB Client with the provided connection details.
        """
        self.endpoint = endpoint
        self.key = key
        self.database_name = database_name
        self.container_name = container_name
        self.client = CosmosClient(endpoint, key)
        self.database = self.client.get_database_client(database_name)
        self.container = self.database.get_container_client(container_name)
        logging.info(f"Azure Cosmos DB Client initialized for container '{container_name}'.")

    def query_documents(self, query: str, parameters: list = None):
        """
        Query documents in the Cosmos DB container using SQL-like syntax.

        :param query: The SQL query string to execute.
        :param parameters: List of parameters for the query.
        :return: List of documents matching the query.
        """
        try:
            logging.info(f"Executing query: {query} with parameters: {parameters}")
            results = self.container.query_items(
                query=query,
                parameters=parameters,
                enable_cross_partition_query=True
            )
            documents = list(results)
            logging.info(f"Query executed successfully. Retrieved {len(documents)} documents.")
            return documents
        except exceptions.CosmosHttpResponseError as e:
            logging.error(f"Failed to execute query: {query}. Error: {e}")
            return []

    def get_document_by_id(self, document_id: str, partition_key: str):
        """
        Retrieve a document by its ID and partition key.

        :param document_id: The ID of the document to retrieve.
        :param partition_key: The partition key of the document.
        :return: The document if found, or None.
        """
        try:
            logging.info(f"Retrieving document with ID '{document_id}' and PartitionKey '{partition_key}'.")
            document = self.container.read_item(item=document_id, partition_key=partition_key)
            logging.info(f"Document retrieved successfully: {document}")
            return document
        except exceptions.CosmosResourceNotFoundError:
            logging.warning(f"Document with ID '{document_id}' not found.")
            return None
        except exceptions.CosmosHttpResponseError as e:
            logging.error(f"Failed to retrieve document with ID '{document_id}': {e}")
            return None

    def close(self):
        """Close the Cosmos DB Client."""
        logging.info("Azure Cosmos DB Client cleanup handled by garbage collection.")


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

    # Query documents
    query = "SELECT * FROM c WHERE c.partitionKey = @partitionKey"
    parameters = [{"name": "@partitionKey", "value": "text-content"}]
    documents = cosmos_client.query_documents(query, parameters)
    print("Documents:", documents)

    # Get a specific document by ID
    document = cosmos_client.get_document_by_id(document_id="12345", partition_key="text-content")
    print("Document:", document)
