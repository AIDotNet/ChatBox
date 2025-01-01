using System.Collections.Generic;
using ChatBox.Models;
using Dapper;

namespace ChatBox.DataAccess;

[DapperAot]
public class ChatMessageRepository(Lazy<DbContext> dbContext)
{
    public async Task InsertAsync(ChatMessage message)
    {
        await dbContext.Value.InsertAsync(message);
    }

    public async Task UpdateContent(string id, string content)
    {
        await dbContext.Value.GetConnection().ExecuteAsync(
            $"UPDATE {nameof(ChatMessage)} SET Content = @content WHERE Id = @id",
            new { content, id });
    }

    public async Task DeleteSession(string id)
    {
        await dbContext.Value.GetConnection().ExecuteAsync(
            $"DELETE FROM {nameof(ChatMessage)} WHERE SessionId = @id",
            new { id });
    }

    public async Task DeleteAsync(string id)
    {
        await dbContext.Value.DeleteAsync<ChatMessage>(id);
    }

    public async Task<IEnumerable<ChatMessage>> GetMessagesAsync()
    {
        return await dbContext.Value.QueryAsync<ChatMessage>(x => x.SessionId == "default");
    }
}