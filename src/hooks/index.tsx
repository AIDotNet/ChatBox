import { useLocation } from 'react-router-dom';
import { SidebarTabKey } from '@/store/global/initialState';

/**
 * Returns the active tab key (chat/market/settings/...)
 */
export const useActiveTabKey = () => {
    const { pathname } = useLocation();

  return pathname.split('/').find(Boolean)! as SidebarTabKey ?? SidebarTabKey.Welcome;
};
