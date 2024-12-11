import {useEffect, useState} from "react";

export const useLoadingStateMessage = () => {

    const loadingMessages = [
        "Loading, please wait...",
        "This might take a few moments...",
        "Our microservices are waking up...",
        "It could be a cold start, hang tight...",
        "Thanks for your patience, almost there..."
    ];

    const [currentMessageIndex, setCurrentMessageIndex] = useState(0);

    useEffect(() => {
        const intervalId = setInterval(() => {
            setCurrentMessageIndex((prevIndex) => (prevIndex + 1) % loadingMessages.length);
        }, 4000);

        return () => clearInterval(intervalId); // Cleanup interval on component unmount
    }, [loadingMessages.length]);

    return loadingMessages[currentMessageIndex]
}
