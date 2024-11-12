import { memo } from "react";
import { Flexbox } from "react-layout-kit";
import ChatList from "./ChatList";
import ChatInput from "./ChatInput";


const Conversation = memo(()=>{
    return (
        <Flexbox style={{
            height: '100%',
            width: '100%',
        }}>
        <ChatList />
        <ChatInput />
        </Flexbox>
    )
})

Conversation.displayName = 'Conversation';

export default Conversation;