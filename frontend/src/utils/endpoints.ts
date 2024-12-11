import {HttpMethod} from "@/services/common/http-request";

interface Endpoint {
    Uri: string;
    Method: HttpMethod;
}

export const Api = {
    UploadDocuments: {
        Uri: `${process.env.NEXT_PUBLIC_BASE_API_URL}`,
        Method: HttpMethod.POST,
    } as Endpoint,
    GetDocuments: (oid: string): Endpoint => ({
        Uri: `${process.env.NEXT_PUBLIC_BASE_API_URL}/${oid}`,
        Method: HttpMethod.GET,
    }) as Endpoint,
    QueryDocument: {
        Uri: `${process.env.NEXT_PUBLIC_BASE_SEARCH_URL}`,
        Method: HttpMethod.POST,
    } as Endpoint,
}
