import isEqual from 'fast-deep-equal';
import { merge } from '@/utils/merge';
import { SidebarTabKey, SystemStatus } from './initialState';
import { GlobalStore } from './store';
import type { StateCreator } from 'zustand/vanilla';


export interface GlobalStoreAction {
    switchTheme: (theme: 'auto' | 'dark' | 'light') => void;
    switchActiveTabKey: (key: SidebarTabKey) => void;
    updateSystemStatus: (status: Partial<SystemStatus>, action?: any) => void;
    switchShowSessionPanel: (show: boolean) => void;
}

export const globalActionSlice: StateCreator<
    GlobalStore & GlobalStoreAction,
    [],
    [],
    GlobalStoreAction
> = (set, get) => ({
    switchTheme: (theme: 'auto' | 'dark' | 'light') => {
        set({
            theme
        })
        localStorage.setItem('theme', theme);
    },
    switchActiveTabKey: (key: SidebarTabKey) => {
        set({
            activeTabKey: key
        });
    },
    updateSystemStatus: (status, action) => {
  
      const nextStatus = merge(get().status, status);
      if (isEqual(get().status, nextStatus)) return;
  
      set({ status: nextStatus }, false);
  
    //   get().statusStorage.saveToLocalStorage(nextStatus);
    },
    switchShowSessionPanel: (show) => {
        set({
            status: {
                ...get().status,
                showSessionPanel: show
            }
        });
    }
});