import { useState, useEffect } from 'react';
import { IDocument } from '@/models/document';
import { getDocuments } from '@/services/document-service';

export const useGetDocuments = (oid: string) => {
    const [documents, setDocuments] = useState<IDocument[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        setLoading(true);
        getDocuments(oid, undefined)
            .then(docs => setDocuments(docs))
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    }, [oid]);

    return { documents, loading, error };
};
