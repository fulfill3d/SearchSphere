import {DocumentQuery} from "@/models/document-query";
import {searchDocument} from "@/services/chat-service";

export const useSearchDocuments = () => {

    const handleSearchDocuments = async (
        query: DocumentQuery,
        onSuccess: (response: DocumentQuery) => void,
        onError: (message: string) => void,
    ) => {
        try {
            const response = await searchDocument(query, undefined);
            onSuccess(response);
        } catch (err: any) {
            onError(err.message);
        }
    }

    return { handleSearchDocuments }
}
