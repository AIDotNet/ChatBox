
import { DEFAULT_AVATAR } from '@/const/meta';
import { DEFAULT_AGENT_LOBE_SESSION } from '@/const/session';
import { MetaData } from '@/types/meta';
import { Session } from '@/types/session';

export const getSessionPinned = (session: Session) => session.pinned;

const getAvatar = (s: MetaData) => s.avatar || DEFAULT_AVATAR;
const getTitle = (s: MetaData) => s.title || '未命名';

const getSessionById = (id: string, sessions: Session[]): Session => {
  const session = sessions.find((s) => s.id === id);

  if (!session) return DEFAULT_AGENT_LOBE_SESSION;

  return session;
};

export const sessionHelpers = {
  getAvatar,
  getSessionById,
  getSessionPinned,
  getTitle,
};
