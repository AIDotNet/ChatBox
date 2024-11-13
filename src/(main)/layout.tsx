import { memo } from "react";
import { Flexbox } from 'react-layout-kit';
import { Avatar, Divider } from "antd";
import { Github, MessageSquare } from "lucide-react";
import { useGlobalStore } from "../store/global/store";
import { SidebarTabKey } from "@/store/global/initialState";
import { Outlet, useNavigate } from "react-router-dom";
import { useActiveTabKey } from "@/hooks";
import {GITHUB} from "@/const/url.ts";
import { ActionIcon, SideNav } from "@lobehub/ui";

const MainLayout = memo(() => {
    const [activeTabKey, switchActiveTabKey] = useGlobalStore(state => [useActiveTabKey(), state.switchActiveTabKey]);

    const navigate = useNavigate();
    function switchTab(tab: SidebarTabKey) {
        navigate(`/${tab}`);
        switchActiveTabKey(tab);
    }

    return (
        <>
            <SideNav
                id="side-nav"
                data-tauri-drag-region
                className="titlebar"
                style={{
                    height: 'inherit',
                }}
                avatar={<Avatar style={{
                }} size={45} src="https://chat.ai-v1.cn/icons/icon-192x192.png" />}
                topActions={<>
                    <ActionIcon
                        active={activeTabKey === 'chat'}
                        icon={MessageSquare}
                        onClick={() =>
                            switchTab(SidebarTabKey.Chat)
                        }
                        size="large"
                    />
                </>}
                bottomActions={<>
                    <ActionIcon
                        icon={Github}
                        onClick={() => {
                            navigate(GITHUB)
                        }}
                        size="large"
                    />
                </>}>

            </SideNav>
            <Divider type="vertical" style={{
                height: '100vh',
                margin: '0'
            }} />
            <Flexbox style={{
                width: 'calc(100% - 60px)',
            }}>
                <Outlet />
            </Flexbox>
        </>
    )
});

MainLayout.displayName = "MainLayout";

export default MainLayout;