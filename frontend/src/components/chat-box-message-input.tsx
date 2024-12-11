'use client'

import React, { useState } from 'react';
import { useSearchDocuments } from "@/hooks/use-search-documents";
import { DocumentQuery } from "@/models/document-query";

const ChatBoxMessageInput: React.FC = () => {
    const { handleSearchDocuments } = useSearchDocuments();
    const [inputValue, setInputValue] = useState('');

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(event.target.value);
    };

    const handleSendClick = async () => {
        const query = new DocumentQuery('a12ba5a0-2b64-401e-8c9f-373fcc4427f9', inputValue); // Replace 'example_blob_name' with actual blob name
        await handleSearchDocuments(
            query,
            (response) => console.log(JSON.stringify(response)),
            (message) => console.error('Search error:', message)
        );
    };

    return (
        <div className="p-4 bg-white border-t border-gray-200">
            <div className="flex items-center space-x-4">
                <input
                    type="text"
                    className="flex-1 p-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="Type your message..."
                    value={inputValue}
                    onChange={handleInputChange}
                />
                <button
                    className="bg-blue-500 text-white p-2 rounded-lg shadow-md hover:bg-blue-600"
                    onClick={handleSendClick}
                >
                    Send
                </button>
            </div>
        </div>
    );
};

export default ChatBoxMessageInput;
