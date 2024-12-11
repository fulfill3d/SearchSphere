import React from "react";
import DocumentList from "@/components/document-list";
import DocumentUploadSection from "@/components/document-upload-section";

const Document: React.FC = () => {

    return (
        <div className="flex flex-col h-screen bg-gray-100">
            <header className="bg-blue-600 text-white p-4 text-center font-bold">
                Document List
            </header>
            <DocumentList/>
            <DocumentUploadSection/>
        </div>
    );
}

export default Document;
