import { memo } from "react";
import Inbox from "./Inbox";

const SessionListContent = memo(() => {
    return (
        <>
          <Inbox />
        </>
    );
});

SessionListContent.displayName = "SessionListContent";

export default SessionListContent;