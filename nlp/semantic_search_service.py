from typing import List
import numpy as np
from sentence_transformers import SentenceTransformer
import faiss


class SemanticSearchService:
    def __init__(self):
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

    def set_message(self, message):
        # self.add_texts(message_model)
        # self.search(message_model)
        pass

    def send_results(self):
        pass