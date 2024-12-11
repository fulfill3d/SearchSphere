import {DocumentQuery} from "@/models/document-query";
import {httpRequest} from "@/services/common/http-request";
import {Api} from "@/utils/endpoints";

export const searchDocument = async (query: DocumentQuery, accessToken: string | undefined) => {
    try {
        const response = await httpRequest(
            Api.QueryDocument.Uri,
            Api.QueryDocument.Method,
            query,
            undefined,
            accessToken
        );
        return DocumentQuery.fromJSON(response);
    } catch (error) {
        throw new Error(`Failed to send question. ${error}`);
    }
}
