import { Skeleton } from 'antd';
import { PanelLeftClose, PanelLeftOpen } from 'lucide-react';
import { parseAsBoolean, useQueryState } from 'nuqs';
import { Suspense, memo } from 'react';
import { Flexbox } from 'react-layout-kit';

import { useGlobalStore } from '@/store/global';
import { systemStatusSelectors } from '@/store/global/selectors';
import { useSessionStore } from '@/store/session';
import { sessionMetaSelectors, sessionSelectors } from '@/store/session/selectors';

import Tags from './Tags';
import { ActionIcon, Avatar } from '@lobehub/ui';

const Main = memo(() => {

    const [ isInbox, title, description, avatar, backgroundColor] = useSessionStore((s) => [
        sessionSelectors.isInboxSession(s),
        sessionMetaSelectors.currentAgentTitle(s),
        sessionMetaSelectors.currentAgentDescription(s),
        sessionMetaSelectors.currentAgentAvatar(s),
        sessionMetaSelectors.currentAgentBackgroundColor(s),
    ]);

    // const openChatSettings = useOpenChatSettings();

    const displayTitle = isInbox ? '默认会话' : title;
    const displayDesc = isInbox ? '默认会话' : description;
    const showSessionPanel = useGlobalStore(systemStatusSelectors.showSessionPanel);
    const updateSystemStatus = useGlobalStore((s) => s.updateSystemStatus);

    return <Flexbox align={'center'} gap={4} horizontal>

        <ActionIcon
            icon={showSessionPanel ? PanelLeftClose : PanelLeftOpen}
            onClick={() => {
                console.log('click',showSessionPanel);
                
                updateSystemStatus({
                    sessionsWidth: showSessionPanel ? 0 : 320,
                    showSessionPanel: !showSessionPanel,
                });
            }}
            title={'会话列表'}
        />
        <Avatar
            avatar={avatar}
            background={backgroundColor}
            onClick={() => {

            }}
            size={40}
            title={title}
        />
        {/* <ChatHeaderTitle desc={displayDesc} tag={<Tags />} title={displayTitle} /> */}
    </Flexbox>
});

export default () => (
    <Suspense
        fallback={
            <Skeleton
                active
                avatar={{ shape: 'circle', size: 'default' }}
                paragraph={false}
                title={{ style: { margin: 0, marginTop: 8 }, width: 200 }}
            />
        }
    >
        <Main />
    </Suspense>
);
