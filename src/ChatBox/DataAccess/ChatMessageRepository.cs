using System.Collections.Generic;
using System.Linq;
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

    public async Task<IEnumerable<ChatMessage>> GetMessagesAsync(string? sessionId)
    {
        return await dbContext.Value.QueryAsync<ChatMessage>(
            $"SELECT * FROM ChatMessage WHERE SessionId = '{sessionId}'");
    }

    /// <summary>
    /// 获取最后一条消息
    /// </summary>
    /// <returns></returns>
    public async Task<ChatMessage?> GetLastMessageAsync(string sessionId)
    {
        return (await dbContext.Value.QueryAsync<ChatMessage>(
                $"SELECT * FROM ChatMessage WHERE SessionId = '{sessionId}' ORDER BY CreatedAt DESC LIMIT 1"))
            .FirstOrDefault();
    }
    
    /// <summary>
    /// 重命名会话名称
    /// </summary>
    public async Task UpdateSessionAsync(Session session)
    {
        await dbContext.Value.UpdateAsync(session);
    } 
}