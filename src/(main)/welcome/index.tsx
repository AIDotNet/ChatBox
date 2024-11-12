import { memo } from "react";
import { Flexbox } from 'react-layout-kit';
import Hero from "./features/Hero";

const Welcome = memo(() => {
    const COPYRIGHT = `© ${new Date().getFullYear()} Chat Bot`;

    return (
        <Flexbox
            align={'center'}
            height={'100%'}
            justify={'space-between'}
            padding={16}
            style={{ overflow: 'hidden', position: 'relative' }}
            width={'100%'}
        >
            <div />
            <Hero />
            <Flexbox align={'center'} horizontal justify={'space-between'}>
                <span style={{ opacity: 0.5 }}>{COPYRIGHT}</span>
            </Flexbox>
        </Flexbox>
    );
});

Welcome.displayName = "Welcome";

export default Welcome;