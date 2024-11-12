import { createStyles } from 'antd-style';
import { rgba } from 'polished';
import { memo } from 'react';
import { Flexbox } from 'react-layout-kit';

const useStyles = createStyles(({ css, token }) => {
  return {
    desc: css`
      font-size: min(24px, 4vw);
      font-weight: 400;
      color: ${rgba(token.colorText, 0.8)};
      text-align: center;
      text-wrap: balance;
    `,
    title: css`
      margin-block-end: 0;

      font-size: min(56px, 7vw);
      font-weight: 800;
      line-height: 1;
      text-align: center;
      text-wrap: balance;
    `,
  };
});

const Hero = memo(() => {
  const { styles } = useStyles();

  return (
    <>
      <Flexbox
        align={'center'}
        as={'h1'}
        className={styles.title}
        gap={16}
        horizontal
        style={{
            overflow: 'hidden',
            position: 'relative',
            // 自动换行
            whiteSpace: 'pre-wrap',
            wordWrap: 'break-word',
            
        }}
        justify={'center'}
        wrap={'wrap'}
      >
        <strong style={{ fontSize: 'min(56px, 8vw)' }}>
            Chat Bot
        </strong>
      </Flexbox>
      <Flexbox
        align={'center'}
        as={'h2'}
        className={styles.desc}
        horizontal
        justify={'center'}
        wrap={'wrap'}
      >
        开启您的智能聊天体验，让我们一起探索智能体的世界！
      </Flexbox>
    </>
  );
});

export default Hero;
