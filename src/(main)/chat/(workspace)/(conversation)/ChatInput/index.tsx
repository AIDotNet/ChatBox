import { ActionIcon, ChatInputActionBar, ChatInputArea, ChatSendButton, DraggablePanel, TokenTag } from "@lobehub/ui";
import { Eraser, Languages } from "lucide-react";
import { memo, useState } from "react";
import { Flexbox } from 'react-layout-kit';
import Header from "./Header";
import TextArea from "./TextArea";
import Footer from "./Footer";


const ChatInput = memo(() => {
    const [expand, setExpand] = useState<boolean>(false);


    return (
        <DraggablePanel
            fullscreen={expand}
            headerHeight={64}
            maxHeight={800}
            minHeight={160}
            placement="bottom"
            size={{
                width: '100%',

            }}
            style={{ zIndex: 10, paddingBottom: 20,paddingRight: 10 }}
        >
            <Flexbox
                gap={8}
                height={'100%'}
                padding={'12px 0 16px'}
                style={{
                    minHeight: 160,
                    marginRight: 15,
                    position: 'relative'
                }}
            >
                <Header expand={expand} setExpand={setExpand} />
                <TextArea setExpand={setExpand} />
                <Footer expand={expand} setExpand={setExpand} />
            </Flexbox>
        </DraggablePanel>
    )
})

ChatInput.displayName = 'ChatInput';

export default ChatInput;