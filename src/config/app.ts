declare global {
  // eslint-disable-next-line @typescript-eslint/no-namespace
  namespace NodeJS {
    interface ProcessEnv {
      ACCESS_CODE?: string;
    }
  }
}


declare global {
  interface Window {
    thor: {
      DEFAULT_MODEL: string;
      DEFAULT_AVATAR: string;
      DEFAULT_INBOX_AVATAR: string;
      DEFAULT_USER_AVATAR: string;
    };
  }
}

export const getAppConfig = () => {

  return {
  };
};

export const appEnv = getAppConfig();
