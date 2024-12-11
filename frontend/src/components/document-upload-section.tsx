'use client'

import React from 'react';

const DocumentUploadSection: React.FC = () => {
    return (
        <div className="p-4 bg-gray-200 border-t border-gray-300">
            <div
                className="flex items-center space-x-4 p-4 border-2 border-dashed border-gray-400 rounded-lg"
                onDragOver={(e) => e.preventDefault()}
                onDrop={(e) => {
                    e.preventDefault();
                    // Handle file drop
                }}
            >
                <input
                    type="file"
                    className="hidden"
                    id="file-upload"
                />
                <label
                    htmlFor="file-upload"
                    className="flex-1 p-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 cursor-pointer"
                >
                    Drag and drop files here or click to upload
                </label>
                <button className="bg-blue-600 text-white p-2 rounded-lg shadow-md hover:bg-blue-700">
                    Upload
                </button>
            </div>
        </div>
    );
};

export default DocumentUploadSection;
