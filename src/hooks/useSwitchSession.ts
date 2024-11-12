import { useLocation } from 'react-router-dom';
import { useCallback } from 'react';

import { useQueryRoute } from '@/hooks/useQueryRoute';
import { useSessionStore } from '@/store/session';

export const useSwitchSession = () => {
  const switchSession = useSessionStore((s) => s.switchSession);
  const router = useQueryRoute();
  const { pathname } = useLocation();

  return useCallback(
    (id: string) => {
      switchSession(id);
      const chatPath = '/chat';
      if (pathname !== chatPath) {
        setTimeout(() => {
          router.push(chatPath, {
            query: { session: id, showMobileWorkspace: 'true' },
          });
        }, 50);
      }
    },
    [],
  );
};
