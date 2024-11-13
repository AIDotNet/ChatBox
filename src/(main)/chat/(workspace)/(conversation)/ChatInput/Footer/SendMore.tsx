import { Icon } from '@lobehub/ui';
import { Button, Dropdown } from 'antd';
import { createStyles } from 'antd-style';
import { BotMessageSquare, LucideCheck, LucideChevronDown, MessageSquarePlus } from 'lucide-react';
import { memo } from 'react';
import { useHotkeys } from 'react-hotkeys-hook';
import { Flexbox } from 'react-layout-kit';

import HotKeys from '@/components/HotKeys';
import { ALT_KEY } from '@/const/hotkeys';
// import { useSendMessage } from '@/features/ChatInput/useSend';
// import { useChatStore } from '@/store/chat';
// import { useUserStore } from '@/store/user';
// import { preferenceSelectors } from '@/store/user/selectors';

const useStyles = createStyles(({ css, prefixCls }) => {
    return {
        arrow: css`
      &.${prefixCls}-btn.${prefixCls}-btn-icon-only {
        width: 28px;
      }
    `,
    };
});

interface SendMoreProps {
    disabled?: boolean;
    isMac?: boolean;
}

const SendMore = memo<SendMoreProps>(({ disabled, isMac }) => {

    const { styles } = useStyles();

    //   const [useCmdEnterToSend, updatePreference] = useUserStore((s) => [
    //     preferenceSelectors.useCmdEnterToSend(s),
    //     s.updatePreference,
    //   ]);
    //   const addAIMessage = useChatStore((s) => s.addAIMessage);
    const useCmdEnterToSend = false;
    // const { send: sendMessage } = useSendMessage();

    const hotKey = [ALT_KEY, 'enter'].join('+');
    useHotkeys(
        hotKey,
        (keyboardEvent, hotkeysEvent) => {
            console.log(keyboardEvent, hotkeysEvent);
            // sendMessage({ onlyAddUserMessage: true });
        },
        {
            enableOnFormTags: true,
            preventDefault: true,
        },
    );

    return (
        <Dropdown
            disabled={disabled}
            menu={{
                items: [
                    {
                        icon: !useCmdEnterToSend ? <Icon icon={LucideCheck} /> : <div />,
                        key: 'sendWithEnter',
                        label: 'Enter',
                        onClick: () => {
                            //   updatePreference({ useCmdEnterToSend: false });
                        },
                    },
                    {
                        icon: useCmdEnterToSend ? <Icon icon={LucideCheck} /> : <div />,
                        key: 'sendWithCmdEnter',
                        label: 'Ctrl',
                        onClick: () => {
                            //   updatePreference({ useCmdEnterToSend: true });
                        },
                    },
                    { type: 'divider' },
                    {
                        icon: <Icon icon={BotMessageSquare} />,
                        key: 'addAi',
                        label: '添加一条AI消息',
                        onClick: () => {
                            //   addAIMessage();
                        },
                    },
                    {
                        icon: <Icon icon={MessageSquarePlus} />,
                        key: 'addUser',
                        label: (
                            <Flexbox gap={24} horizontal>
                                添加一条用户消息
                                <HotKeys keys={hotKey} />
                            </Flexbox>
                        ),
                        onClick: () => {
                            // sendMessage({ onlyAddUserMessage: true });
                        },
                    },
                ],
            }}
            placement={'topRight'}
            trigger={['hover']}
        >
            <Button
                aria-label={'更多'}
                className={styles.arrow}
                icon={<Icon icon={LucideChevronDown} />}
                type={'primary'}
            />
        </Dropdown>
    );
});

SendMore.displayName = 'SendMore';

export default SendMore;
