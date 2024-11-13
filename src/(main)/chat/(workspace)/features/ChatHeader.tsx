import { ChatHeader } from '@lobehub/ui';
import Main from './Main';

const Header = () => {

    return (
        <ChatHeader
            data-tauri-drag-region
            className="titlebar"
            left={<Main />}
            //   right={<HeaderAction />}
            style={{ minHeight: 64, position: 'initial', zIndex: 11 }}
        />
    );
};

export default Header;
