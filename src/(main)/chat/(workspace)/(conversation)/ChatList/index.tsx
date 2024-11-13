import { memo } from "react";

import { Flexbox } from 'react-layout-kit';

const ChatList = memo(()=>{
    return (
        <Flexbox style={{
            flex: 1,
        }}>
            ChatList
        </Flexbox>
    )    
})

ChatList.displayName = 'ChatList';

export default ChatList;
