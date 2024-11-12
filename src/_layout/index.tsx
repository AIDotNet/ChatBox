import { memo, useEffect, useState } from "react";
import RootLayoutProps from "./type";
import { Flexbox } from 'react-layout-kit';
import { Button } from "antd";
import { Maximize, Minimize, Minus, Moon, Sun, X } from "lucide-react";
import { getCurrentWindow } from '@tauri-apps/api/window';
import ThemeLayout from "./ThemeLayout";
import { useGlobalStore } from "../store/global/store";

const Layout = memo<RootLayoutProps>(({
    children
}) => {
    const [theme, switchTheme] = useGlobalStore(state => [state.theme, state.switchTheme]);
    const [min, setMin] = useState<boolean>(false);
    useEffect(() => {
        try{

            getCurrentWindow().isMinimized().then((isMinimized) => {
                setMin(isMinimized);
            });
    
            getCurrentWindow().onMoved(async () => {
                const maxinmized = await getCurrentWindow().isMaximized();
                setMin(maxinmized);
            });
        }catch(e){
            console.error(e);
        }

    }, []);


    const handleMinimize = () => {
        getCurrentWindow().minimize();
    };

    const handleMaximize = () => {
        if (!min) {
            getCurrentWindow().maximize();
        } else {
            getCurrentWindow().unmaximize();
        }

        setMin(!min);
    };

    const handleClose = () => {
        getCurrentWindow().hide();
    };

    return (
        <ThemeLayout>
            <Flexbox horizontal>
                <>
                    <Button
                        type="text"
                        onClick={() => {
                            switchTheme(theme === 'dark' ? 'light' : 'dark')
                        }}
                        style={{
                            position: "absolute",
                            right: 90,
                            top: 1,
                            zIndex: 999
                        }}
                        icon={<span style={{ color: theme === 'dark' ? 'white' : 'black' }}>
                            {theme === 'dark' ? <Sun size='20' /> : <Moon size='20' />}
                        </span>
                        }
                    >

                    </Button>
                    <Button
                        type="text"
                        onClick={() => handleMinimize()}
                        icon={<Minus size='20' />}
                        style={{
                            position: "absolute",
                            right: 60,
                            top: 1,
                            zIndex: 999
                        }}>
                    </Button>
                    <Button
                        onClick={() => handleMaximize()}
                        type="text"
                        icon={!min ? <Maximize size='20' /> : <Minimize size='20' />}
                        style={{
                            position: "absolute",
                            right: 30,
                            top: 1,
                            zIndex: 999
                        }}>
                    </Button>
                    <Button
                        type="text"
                        onClick={() => handleClose()}
                        icon={<X size='20' />}
                        style={{
                            position: "absolute",
                            right: 0, // 调整第二个按钮的位置
                            top: 1,
                            zIndex: 999
                        }}>
                    </Button>
                </>
                {children}
            </Flexbox>

        </ThemeLayout>
    )
});

Layout.displayName = "Layout";

export default Layout;