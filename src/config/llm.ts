export const getLLMConfig = () => {
  // region format: iad1,sfo1
  let regions: string[] = [];
  if (process.env.OPENAI_FUNCTION_REGIONS) {
    regions = process.env.OPENAI_FUNCTION_REGIONS.split(',');
  }

  return  {
  };
};

export const llmEnv = getLLMConfig();
