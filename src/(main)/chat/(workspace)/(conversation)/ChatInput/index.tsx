import { Divider } from "antd";
import { memo } from "react";
import { Flexbox } from 'react-layout-kit';


const ChatInput = memo(() => {
    return (
        <Flexbox style={{
            height: 220,
        }}>
            <Divider />
            <Flexbox>
                ChatInput
            </Flexbox>
        </Flexbox>
    )
})

ChatInput.displayName = 'ChatInput';

export default ChatInput;