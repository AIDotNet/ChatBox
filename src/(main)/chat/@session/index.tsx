import { useActionSWR } from "@/libs/swr";
import { useSessionStore } from "@/store/session";
import { useResponsive, createStyles } from "antd-style";
import { MessageSquarePlus } from "lucide-react";
import { lazy, memo, Suspense } from "react";
import { Flexbox } from 'react-layout-kit';
import SkeletonList from "./features/SkeletonList";
import { useGlobalStore } from "@/store/global";
import { systemStatusSelectors } from "@/store/global/selectors";
import { ActionIcon, DraggablePanel } from "@lobehub/ui";

export const useStyles = createStyles(({ css, token }) => ({
    logo: css`
      color: ${token.colorText};
      fill: ${token.colorText};
    `,
    top: css`
      position: sticky;
      inset-block-start: 0;
    `,
}));


const SessionListContent = lazy(() => import('./features/SessionListContent'));


const Session = memo(() => {
    const { styles } = useStyles();

    const [createSession] = useSessionStore(state => [state.createSession]);
    const { md = true } = useResponsive();
    const showSessionPanel = useGlobalStore((s) => s.status.showSessionPanel);

    const { mutate, isValidating } = useActionSWR('session.createSession', () => createSession());
    const switchShowSessionPanel = useGlobalStore((s) => s.switchShowSessionPanel);
    
    return (
        <DraggablePanel
            placement="left"
            mode={md ? 'fixed' : 'float'}
            expand={showSessionPanel}
            onExpandChange={(pinned) => {
                switchShowSessionPanel(pinned);
            }}
            defaultSize={{ width: 300 }}
            maxWidth={300}
            minWidth={200}
            size={{ height: '100%', width: 300 }}
        >
            <Flexbox style={{
                height: '100vh',
            }}>
                <Flexbox
                    className={styles.top} gap={16} padding={16}>
                    <Flexbox distribution={'space-between'} horizontal>
                        <Flexbox align={'center'} gap={4} horizontal>
                            <span className={styles.logo}>
                                Chat Bot
                            </span>
                        </Flexbox>
                        <ActionIcon
                            icon={MessageSquarePlus}
                            loading={isValidating}
                            onClick={() => mutate()}
                            size={{
                                fontSize: 24,
                            }}
                            style={{ flex: 'none' }}
                            title={'新建会话'}
                        />
                    </Flexbox>
                    <Suspense fallback={<SkeletonList />}>
                        <SessionListContent />
                    </Suspense>
                </Flexbox>

            </Flexbox>
        </DraggablePanel>
    );
});

Session.displayName = "Session";

export default Session;