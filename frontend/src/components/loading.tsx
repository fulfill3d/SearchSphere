import React from 'react';
import {useLoadingStateMessage} from "@/hooks/use-loading-state-message";

const Loading: React.FC = () => {
    const message = useLoadingStateMessage();

    return (
        <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="flex flex-col items-center">
                {/* Loader Animation */}
                <svg
                    className="animate-spin h-10 w-10 text-coral"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                >
                    <circle
                        className="opacity-25"
                        cx="12"
                        cy="12"
                        r="10"
                        stroke="currentColor"
                        strokeWidth="4"
                    ></circle>
                    <path
                        className="opacity-75"
                        fill="currentColor"
                        d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"
                    ></path>
                </svg>

                {/* Animated Loading Text */}
                <p className="mt-4 text-lg font-medium text-gray-700">
                    {message}
                </p>
            </div>
        </div>
    );
};

export default Loading;
