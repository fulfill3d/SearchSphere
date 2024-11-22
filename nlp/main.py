import os

from azure.servicebus import ServiceBusClient
from azure_app_config_client import AppConfigClient
from semantic_search_service import SemanticSearchService

from dotenv import load_dotenv

# Load environment variables
load_dotenv()


def main():
    # Fetch configuration settings from Azure App Configuration
    config_client = AppConfigClient(os.getenv('APP_CONFIG_URL'))

    service_bus_connection_str = config_client.fetch_configuration_value("ServiceBusConnectionString")
    queue_name = config_client.fetch_configuration_value("SS_QueueName")

    search_service = SemanticSearchService()

    # Initialize Azure Service Bus Client
    with ServiceBusClient.from_connection_string(conn_str=service_bus_connection_str) as client:
        with client.get_queue_receiver(queue_name=queue_name) as receiver:
            for message in receiver:
                search_service.set_message(message)


if __name__ == "__main__":
    while True:
        main()