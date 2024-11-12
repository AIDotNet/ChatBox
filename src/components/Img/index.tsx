import { ImageProps } from 'antd';
import { ElementType, createElement, forwardRef, useMemo } from 'react';

import { ImgProps } from '@/types';

const createContainer = (as: ElementType) =>
  forwardRef((props: any, ref) => createElement(as, { ...props, ref }));

const Img = forwardRef<any, ImgProps & ImageProps & { unoptimized?: boolean }>(
  ({ unoptimized, ...res }, ref) => {
    const render = 'img';

    const ImgContainer = useMemo(() => createContainer(render), [render]);

    return (
      <ImgContainer
        ref={ref}
        unoptimized={unoptimized}
        {...res}
      />
    );
  },
);

export default Img;
