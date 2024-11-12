import { memo, useEffect, useState } from "react";
import { ConfigProvider, theme } from 'antd';
import RootLayoutProps from "./type";
import 'dayjs/locale/zh-cn';
import dayjs from "dayjs";
import { useGlobalStore } from "../store/global";
dayjs.locale('zh-cn');

const ThemeLayout = ({ children }: RootLayoutProps) => {
    const [activeTheme, switchTheme] = useGlobalStore(state => [state.theme, state.switchTheme]);

    useEffect(() => {
        if (activeTheme === 'auto') {
            const media = window.matchMedia('(prefers-color-scheme: dark)');
            const listener = (e: MediaQueryListEvent) => {
                switchTheme(e.matches ? 'dark' : 'light');
            };

            if (media.matches) {
                switchTheme('dark');
            } else {
                switchTheme('light');
            }

            media.addEventListener('change', listener);
            return () => {
                media.removeEventListener('change', listener);
            };
        } else {
            switchTheme(activeTheme);
        }
    }, [activeTheme]);

    useEffect(() => {

        if (activeTheme === 'dark') {
            document.body.classList.add('dark');
        } else {
            document.body.classList.remove('dark');
        }

    }, [activeTheme]);

    return (
        <ConfigProvider
            theme={{
                algorithm: activeTheme === "dark" ? theme.darkAlgorithm : theme.defaultAlgorithm
            }} >
            {children}
        </ConfigProvider>
    );
};

ThemeLayout.displayName = "ThemeLayout";

export default memo(ThemeLayout);