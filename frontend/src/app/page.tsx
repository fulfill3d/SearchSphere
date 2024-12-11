import ChatBox from "@/components/chat-box";
import Document from "@/components/document";

export default function Home() {
  return (
      <div className="flex h-screen">
        <div className="w-3/4">
           <ChatBox />
        </div>
        <div className="w-1/4">
          <Document />
        </div>
      </div>
  );
}
