import { ChatHeader } from '@lobehub/ui';

import HeaderAction from './HeaderAction';
import Main from './Main';

const Header = () => <ChatHeader
    data-tauri-drag-region
    className="titlebar"
    left={<Main />} right={<HeaderAction />} style={{ zIndex: 11,userSelect:'none' }} />;

export default Header;
