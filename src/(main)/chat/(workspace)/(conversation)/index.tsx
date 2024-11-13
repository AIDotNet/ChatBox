import { memo } from "react";
import { Flexbox } from "react-layout-kit";
import ChatList from "./ChatList";
import ChatInput from "./ChatInput";
import ChatTool from "./ChatTool";
import { Divider } from "antd";


const Conversation = memo(() => {
    return (
        <Flexbox style={{
            flex: 1,
        }}>
            <ChatList />
            <Divider style={{
                margin: 0,
            }} />
            <ChatInput />
        </Flexbox>
    )
})

Conversation.displayName = 'Conversation';

export default Conversation;