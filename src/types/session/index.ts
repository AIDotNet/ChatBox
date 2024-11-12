import { LLMParams } from "../llm";
import { MetaData } from "../meta";

export interface Session {
    config: SessionConfig;
    createdAt: Date;
    id: string;
    meta: MetaData;
    model: string;
    pinned?: boolean;
    tags?: string[];
    updatedAt: Date;
}

export interface SessionConfig{
    id?: string;
    /**
     * 角色所使用的语言模型
     * @default gpt-4o-mini
     */
    model: string;
    /**
     * 语言模型参数
     */
    params: LLMParams;
    /**
     * 启用的插件
     */
    plugins?: string[];
    /**
     *  模型供应商
     */
    provider?: string;
    /**
     * 系统角色
     */
    systemRole: string;
}