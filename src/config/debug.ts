declare global {
  // eslint-disable-next-line @typescript-eslint/no-namespace
  namespace NodeJS {
    interface ProcessEnv {
      NEXT_PUBLIC_DEVELOPER_DEBUG: string;
      NEXT_PUBLIC_I18N_DEBUG: string;
      NEXT_PUBLIC_I18N_DEBUG_BROWSER: string;

      NEXT_PUBLIC_I18N_DEBUG_SERVER: string;
    }
  }
}

export const getDebugConfig = () => ({
});
