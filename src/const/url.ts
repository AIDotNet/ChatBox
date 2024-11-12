import qs from 'query-string';
import urlJoin from 'url-join';

import pkg from '../../package.json';
import { INBOX_SESSION_ID } from './session';

export const UTM_SOURCE = 'chat_preview';

export const OFFICIAL_URL = 'https://ai-dotnet.com/';
export const OFFICIAL_PREVIEW_URL = 'https://chat.token-ai.cn/';
export const OFFICIAL_SITE = 'https://ai-dotnet.com/';

export const OG_URL = '/og/cover.png?v=1';

export const GITHUB = pkg.homepage;

export const GITHUB_ISSUES = urlJoin(GITHUB, 'issues/new/choose');

export const CHANGELOG = urlJoin(GITHUB, 'blob/main/CHANGELOG.md');

export const DOCKER_IMAGE = 'https://hub.docker.com/r/aidotnet';

export const DOCUMENTS = urlJoin(OFFICIAL_SITE, '/docs');
export const USAGE_DOCUMENTS = urlJoin(DOCUMENTS, '/usage');
export const SELF_HOSTING_DOCUMENTS = urlJoin(DOCUMENTS, '/self-hosting');
export const WEBRTC_SYNC_DOCUMENTS = urlJoin(SELF_HOSTING_DOCUMENTS, '/advanced/webrtc');
export const DATABASE_SELF_HOSTING_URL = urlJoin(SELF_HOSTING_DOCUMENTS, '/server-database');

// use this for the link
export const DOCUMENTS_REFER_URL = `${DOCUMENTS}?utm_source=${UTM_SOURCE}`;

export const WIKI = urlJoin(GITHUB, 'wiki');

export const BLOG = urlJoin(OFFICIAL_SITE, 'blog');

export const ABOUT = OFFICIAL_SITE;

export const SESSION_CHAT_URL = (id: string = INBOX_SESSION_ID, mobile?: boolean) =>
  qs.stringifyUrl({
    query: mobile ? { session: id, showMobileWorkspace: mobile } : { session: id },
    url: '/chat',
  });


export const LOBE_URL_IMPORT_NAME = 'settings';
export const EMAIL_SUPPORT = '239573049@qq.com';
export const EMAIL_BUSINESS = '239573049@qq.com';

export const RELEASES_URL = urlJoin(GITHUB, 'releases');

export const mailTo = (email: string) => `mailto:${email}`;
