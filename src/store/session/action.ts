import { StateCreator } from "zustand";
import { SessionStore } from "./store";


export interface SessionActionSlice {
    createSession: () => void;

    /**
     * switch the session
     */
    switchSession: (sessionId: string) => void;
}

export const sessionActionSlice: StateCreator<
    SessionStore & SessionActionSlice,
    [],
    [],
    SessionActionSlice> = (set, get) => ({
        createSession() {
        },
        switchSession(sessionId: string) {
            if (get().activeId === sessionId) return;
        
            set({ activeId: sessionId }, false);
        },
    });