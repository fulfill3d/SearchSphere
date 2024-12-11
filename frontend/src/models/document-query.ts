export interface IDocumentQuery {
    blob_name: string;
    question: string;
    answer?: string;
}

export class DocumentQuery implements IDocumentQuery {
    blob_name: string;
    question: string;
    answer?: string;

    constructor(blob_name: string, question: string, answer?: string) {
        this.blob_name = blob_name;
        this.question = question;
        this.answer = answer;
    }

    static fromJSON(json: any): DocumentQuery {
        return new DocumentQuery(json.blob_name, json.question, json.answer);
    }
}
