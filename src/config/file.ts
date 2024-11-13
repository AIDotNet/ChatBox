import { z } from 'zod';

const DEFAULT_S3_FILE_PATH = 'files';

export const getFileConfig = () => {
  return {
    client: {
      NEXT_PUBLIC_S3_DOMAIN: z.string().url().optional(),
      NEXT_PUBLIC_S3_FILE_PATH: z.string().optional(),
    },
    runtimeEnv: {
    },
    server: {
      // S3
      S3_ACCESS_KEY_ID: z.string().optional(),
      S3_BUCKET: z.string().optional(),
      S3_ENDPOINT: z.string().url().optional(),

      S3_REGION: z.string().optional(),
      S3_SECRET_ACCESS_KEY: z.string().optional(),
    },
  };
};

export const fileEnv = getFileConfig();
