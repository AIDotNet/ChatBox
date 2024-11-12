import { DEFAULT_AGENT_META } from "../meta";

export const DEFAULT_MODEL = 'gpt-4o-mini';
export const DEFAULT_EMBEDDING_MODEL = 'text-embedding-3-small';

export const DEFAULT_AGENT_CONFIG = {
    model: DEFAULT_MODEL,
    params: {
        frequency_penalty: 0,
        presence_penalty: 0,
        temperature: 1,
        top_p: 1,
    },
    plugins: [],
    systemRole: '',
};

export const DEFAULT_AGENT = {
    config: DEFAULT_AGENT_CONFIG,
    meta: DEFAULT_AGENT_META,
};
