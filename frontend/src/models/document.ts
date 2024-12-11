export interface IDocument {
    id: string;
    name: string;
}

export class Document implements IDocument {
    id: string;
    name: string;

    constructor(uuid: string, name: string) {
        this.id = uuid;
        this.name = name;
    }

    static fromJSON(json: IDocument): Document {
        return new Document(json.id, json.name);
    }
}
