import { useNavigate } from 'react-router-dom';
import qs, { type ParsedQuery } from 'query-string';
import { useMemo } from 'react';

import { useQuery } from '@/hooks/useQuery.ts';

interface QueryRouteOptions {
    hash?: string;
    query?: ParsedQuery;
    replace?: boolean;
    replaceHash?: boolean;
    withHash?: boolean;
}

interface GenHrefOptions extends QueryRouteOptions {
    prevQuery?: ParsedQuery;
    url: string;
}

const genHref = ({ hash, replace, url, prevQuery = {}, query = {} }: GenHrefOptions): string => {
    let href = qs.stringifyUrl({ query: replace ? query : { ...prevQuery, ...query }, url });

    if (hash) {
        href = [href, hash || location?.hash?.slice(1)].filter(Boolean).join('#');
    }

    return href;
};

export const useQueryRoute = () => {
    const navigate = useNavigate();
    const prevQuery = useQuery();

    return useMemo(
        () => ({
            push: (url: string, options: QueryRouteOptions = {}) => {
                return navigate(genHref({ prevQuery, url, ...options }));
            },
            replace: (url: string, options: QueryRouteOptions = {}) => {
                return navigate(genHref({ prevQuery, url, replace: true, ...options }), { replace: true });
            },
        }),
        [prevQuery],
    );
};
