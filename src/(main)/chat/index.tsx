import { memo } from "react";
import { Flexbox } from 'react-layout-kit';
import Session from "./@session";
import Workspace from "./(workspace)";

const Chat = memo(() => {
    return (
        <Flexbox horizontal>
            <Session/>
            <Workspace/>
        </Flexbox>
    );
});

Chat.displayName = "Chat";

export default Chat;