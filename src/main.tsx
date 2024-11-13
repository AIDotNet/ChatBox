import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

import { TrayIcon, TrayIconOptions } from '@tauri-apps/api/tray';
import { Menu } from "@tauri-apps/api/menu";
import { getCurrentWindow } from '@tauri-apps/api/window';
import { defaultWindowIcon } from '@tauri-apps/api/app';

try {
  // @ts-ignore
  if (!window.tray) {

    function onTrayClick(item: string) {
      if (item === "quit") {
        getCurrentWindow().close();
      }
    }

    const menu = await Menu.new({
      items: [
        {
          id: "quit",
          text: "退出程序",
          action: onTrayClick
        },
      ],
    });


    const options = {
      title: "Chat 智能体",
      menu: menu,
      icon: await defaultWindowIcon(),
      action: (e) => {
        if (e.type === 'Click') {
          getCurrentWindow().show();
        }
      }
    } as TrayIconOptions;

    const tray = await TrayIcon.new(options);

    //@ts-ignore
    window.tray = tray;
  }

} catch (e) {
}

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
);
