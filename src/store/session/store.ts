import { subscribeWithSelector } from 'zustand/middleware';
import { shallow } from 'zustand/shallow';
import { createWithEqualityFn } from 'zustand/traditional';
import { StateCreator } from 'zustand/vanilla';

import { createDevtools } from '../middleware/createDevtools';
import { type SessionActionSlice, sessionActionSlice } from './action';
import { type SessionState, initialSessionState } from './initialState';

//  ===============  聚合 createStoreFn ============ //

export type SessionStore = SessionState & SessionActionSlice;

const createStore: StateCreator<SessionStore, [['zustand/devtools', never]]> = (...parameters) => ({
  ...initialSessionState,
  ...sessionActionSlice(...parameters),
});

//  ===============  实装 useStore ============ //

const devtools = createDevtools('global');

export const useSessionStore = createWithEqualityFn<SessionStore>()(
  subscribeWithSelector(devtools(createStore)),
  shallow,
);
