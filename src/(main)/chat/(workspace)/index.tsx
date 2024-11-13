import { Flexbox } from 'react-layout-kit';
// import Portal from './features/Portal';
import Conversation from './(conversation)';
import ChatHeader from './features/ChatHeader';

// import Portal from './Portal';

const Layout = () => {
  return (
    <Flexbox style={{
        width: '100%',
    }}>
      <ChatHeader />
      <Flexbox
        height={'100%'}
        horizontal
        style={{ overflow: 'hidden', position: 'relative' }}
        width={'100%'}
      >
        <Flexbox
          height={'100%'}
          style={{ overflow: 'hidden', position: 'relative' }}
          width={'100%'}
        >
          <Conversation/>
        </Flexbox>
        {/* {children} */}
        {/* <Portal>
            
        </Portal> */}
      </Flexbox>
    </Flexbox>
  );
};

Layout.displayName = 'DesktopConversationLayout';

export default Layout;
