import { Icon } from '@lobehub/ui';
import { createStyles } from 'antd-style';
import { isString } from 'lodash-es';
import { Command, Delete, Option } from 'lucide-react';
import { rgba } from 'polished';
import { memo, useEffect, useState } from 'react';
import { Flexbox } from 'react-layout-kit';

import { ALT_KEY, CLEAN_MESSAGE_KEY, META_KEY } from '@/const/hotkeys';

const useStyles = createStyles(
    ({ css, token }, inverseTheme: boolean) => css`
    font-size: 12px;

    kbd {
      min-width: 16px;
      height: 22px;
      padding-inline: 8px;

      line-height: 22px;
      color: ${inverseTheme ? token.colorTextTertiary : token.colorTextSecondary};
      text-align: center;

      background: ${inverseTheme ? rgba(token.colorTextTertiary, 0.15) : token.colorFillTertiary};
      border-radius: ${token.borderRadius}px;
    }
  `,
);

export interface HotKeysProps {
    desc?: string;
    inverseTheme?: boolean;
    keys: string;
}

const HotKeys = memo<HotKeysProps>(({ keys, desc, inverseTheme }) => {
    const { styles } = useStyles(inverseTheme);
    const [keysGroup, setKeysGroup] = useState(keys.split('+'));
    const visibility = typeof window === 'undefined' ? 'hidden' : 'visible';

    useEffect(() => {
        const mapping: Record<string, any> = {
            [ALT_KEY]: <Icon icon={Option} />,
            [CLEAN_MESSAGE_KEY]: <Icon icon={Delete} />,
            [META_KEY]: <Icon icon={Command} />,
        };
        const newValue = keys
            .split('+')
            .filter(Boolean)
            .map((k) => mapping[k] ?? k);
        setKeysGroup(newValue);
    }, [keys]);

    const content = (
        <Flexbox align={'center'} className={styles} gap={4} horizontal>
            {keysGroup.map((key, index) => (
                <kbd key={index}>
                    <span style={{ visibility }}>{isString(key) ? key.toUpperCase() : key}</span>
                </kbd>
            ))}
        </Flexbox>
    );

    if (!desc) return content;
    return (
        <Flexbox gap={16} horizontal>
            {desc}
            {content}
        </Flexbox>
    );
});

export default HotKeys;
