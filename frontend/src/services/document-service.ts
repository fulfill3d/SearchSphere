import { httpRequest } from './common/http-request';
import { Document, IDocument } from '@/models/document';
import {Api} from "@/utils/endpoints";

export const getDocuments = async (oid: string, accessToken: string | undefined): Promise<Document[]> => {

    const response = await httpRequest(
        Api.GetDocuments(oid).Uri,
        Api.GetDocuments(oid).Method,
        null,
        undefined,
        accessToken
    );
    return response.map((doc: IDocument) => Document.fromJSON(doc));
};

export const uploadDocuments = async (documents: File[], accessToken: string | undefined): Promise<void> => {
    const formData = new FormData();
    documents.forEach(doc => formData.append('documents', doc));
    await httpRequest(
        Api.UploadDocuments.Uri,
        Api.UploadDocuments.Method,
        formData,
        undefined,
        accessToken
    );
}
