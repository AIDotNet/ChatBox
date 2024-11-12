import { memo } from 'react';

import { DEFAULT_INBOX_AVATAR } from '@/const/meta';
import { INBOX_SESSION_ID } from '@/const/session';
import { SESSION_CHAT_URL } from '@/const/url';
import { useSwitchSession } from '@/hooks/useSwitchSession';
import { useSessionStore } from '@/store/session';

import ListItem from '../ListItem';
import { Link } from 'react-router-dom';

const Inbox = memo(() => {
    const activeId = useSessionStore((s) => s.activeId);
    const switchSession = useSwitchSession();

    return (
        <Link
            aria-label={'默认会话'}
            style={{
                textDecoration: 'none',
            }}
            to={SESSION_CHAT_URL(INBOX_SESSION_ID)}
            onClick={(e) => {
                e.preventDefault();
                switchSession(INBOX_SESSION_ID);
            }}
        >
            <ListItem
                active={activeId === INBOX_SESSION_ID}
                avatar={DEFAULT_INBOX_AVATAR}
                title={'默认会话'}
            />
        </Link>
    );
});

export default Inbox;
