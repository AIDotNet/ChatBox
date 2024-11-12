import { DEFAULT_AGENT_META, DEFAULT_INBOX_AVATAR } from '@/const/meta';
import { DEFAULT_AGENT_CONFIG } from '@/const/settings';
import { Session } from '@/types/session';
import { merge } from '@/utils/merge';

export const INBOX_SESSION_ID = 'inbox';

export const WELCOME_GUIDE_CHAT_ID = 'welcome';

export const DEFAULT_AGENT_LOBE_SESSION = {
  config: DEFAULT_AGENT_CONFIG,
  createdAt: new Date(),
  id: '',
  meta: DEFAULT_AGENT_META,
  model: DEFAULT_AGENT_CONFIG.model,
  updatedAt: new Date(),
};

export const DEFAULT_INBOX_SESSION: Session = merge(DEFAULT_AGENT_LOBE_SESSION, {
  id: 'inbox',
  meta: {
    avatar: DEFAULT_INBOX_AVATAR,
  },
});
