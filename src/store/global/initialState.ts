
export enum SidebarTabKey {
    Chat = 'chat',
    Welcome = 'welcome',
}

export interface GlobalState {
    theme: 'auto' | 'dark' | 'light';
    activeTabKey: SidebarTabKey;
    status: SystemStatus;
    isStatusInit: boolean;
}

export const INITIAL_STATUS = {
    expandSessionGroupKeys: ['pinned', 'default'],
    filePanelWidth: 320,
    hidePWAInstaller: false,
    inputHeight: 200,
    mobileShowTopic: false,
    sessionsWidth: 320,
    showChatSideBar: true,
    showFilePanel: true,
    showSessionPanel: true,
    showSystemRole: false,
    zenMode: false,
} satisfies SystemStatus;

export const initialState: GlobalState = {
    theme: localStorage.getItem('theme') as 'auto' | 'dark' | 'light' || 'auto',
    activeTabKey: SidebarTabKey.Welcome,
    status: INITIAL_STATUS,
    isStatusInit: false,
}

export interface SystemStatus {
    // which sessionGroup should expand
    expandSessionGroupKeys: string[];
    filePanelWidth: number;
    hidePWAInstaller?: boolean;
    inputHeight: number;
    mobileShowPortal?: boolean;
    mobileShowTopic?: boolean;
    sessionsWidth: number;
    showChatSideBar?: boolean;
    showFilePanel?: boolean;
    showSessionPanel?: boolean;
    showSystemRole?: boolean;
    zenMode?: boolean;
}
