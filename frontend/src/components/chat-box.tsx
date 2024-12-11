import React from 'react';
import ChatBoxMessageInput from "@/components/chat-box-message-input";

const ChatBox: React.FC = () => {
    return (
        <div className="flex flex-col h-screen bg-gray-100">
            <header className="bg-blue-600 text-white p-4 text-center font-bold">
                Chatbox
            </header>
            <div className="flex-1 overflow-y-auto p-4">
                <div className="space-y-4">
                    <div className="flex justify-start">
                        <div className="bg-white p-3 rounded-lg shadow-md max-w-xs">
                            Hello! How can I help you today?
                        </div>
                    </div>
                    <div className="flex justify-end">
                        <div className="bg-blue-500 text-white p-3 rounded-lg shadow-md max-w-xs">
                            I need some information about your services.
                        </div>
                    </div>
                    {/* Add more messages here */}
                </div>
            </div>
            <ChatBoxMessageInput/>
        </div>
    );
};

export default ChatBox;
