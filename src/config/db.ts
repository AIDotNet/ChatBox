import { z } from 'zod';

export const getServerDBConfig = () => {
  return {
    client: {
      NEXT_PUBLIC_ENABLED_SERVER_SERVICE: z.boolean(),
    },
    runtimeEnv: {
      DATABASE_DRIVER: 'neon',
    },
    server: {
      DATABASE_DRIVER: z.enum(['neon', 'node']),
      DATABASE_TEST_URL: z.string().optional(),
      DATABASE_URL: z.string().optional(),

      KEY_VAULTS_SECRET: z.string().optional(),
    },
  };
};

export const serverDBEnv = getServerDBConfig();
