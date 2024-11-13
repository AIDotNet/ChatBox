import { ActionIcon } from '@lobehub/ui';
import { Popconfirm } from 'antd';
import { Eraser } from 'lucide-react';
import { memo, useCallback, useState } from 'react';


const Clear = memo(() => {

    const [confirmOpened, updateConfirmOpened] = useState(false);

    const resetConversation = useCallback(async () => {

    }, []);

    const actionTitle: any = confirmOpened ? void 0 : '清除消息';

    const popconfirmPlacement = 'topRight';

    return (
        <Popconfirm
            arrow={false}
            okButtonProps={{ danger: true, type: 'primary' }}
            onConfirm={resetConversation}
            onOpenChange={updateConfirmOpened}
            open={confirmOpened}
            placement={popconfirmPlacement}
            title={
                <div style={{ marginBottom: '8px', whiteSpace: 'pre-line', wordBreak: 'break-word' }}>
                    确认清除当前消息？
                </div>
            }
        >
            <ActionIcon
                icon={Eraser}
                overlayStyle={{ maxWidth: 'none' }}
                placement={'bottom'}
                title={actionTitle}
            />
        </Popconfirm>
    );
});

export default Clear;
