import React from 'react';

interface ErrorProps {
    message: string;
}

const Error: React.FC<ErrorProps> = ({ message }) => {
    return (
        <div className="flex items-center justify-center h-screen">
            <div className="text-red-600">{message}</div>
        </div>
    );
};

export default Error;
