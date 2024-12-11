'use client'

import React from 'react';
import {useGetDocuments} from "@/hooks/use-get-documents";
import Loading from "@/components/loading";
import Error from "@/components/error";

const DocumentList: React.FC = () => {
    const { documents, loading, error } = useGetDocuments("user-reference-id-from-jwt");

    if (loading) {
        return <Loading />;
    }

    if (error) {
        return <Error message={error} />;
    }

    return (
        <div className="flex-1 overflow-y-auto p-4">
            <div className="space-y-4">
                {documents.map((document) => (
                    <div key={document.id} className="bg-white p-3 rounded-lg shadow-md">
                        <div className="flex justify-between items-center">
                            <span>{document.name}</span>
                            <button className="bg-red-500 text-white p-2 rounded-lg shadow-md hover:bg-red-600">
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default DocumentList;
