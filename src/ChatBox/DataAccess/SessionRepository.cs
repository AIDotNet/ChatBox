using System.Collections.Generic;
using ChatBox.Models;
using Dapper;

namespace ChatBox.DataAccess;

[DapperAot]
public class SessionRepository(Lazy<DbContext> context)
{
    public async Task InsertAsync(Session session)
    {
        await context.Value.InsertAsync(session);
    }

    public async Task UpdateAsync(Session session)
    {
        await context.Value.UpdateAsync(session);
    }

    public async Task DeleteAsync(string id)
    {
        await context.Value.DeleteAsync<Session>(id);
    }

    [DapperAot]
    public async Task<IEnumerable<Session>> GetSessionsAsync()
    {
        return await context.Value.QueryAsync<Session>(x => true);
    }
}