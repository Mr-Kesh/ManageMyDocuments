using System.Text.RegularExpressions;
using Document_Management_System.Models;
using MySqlConnector;

namespace Document_Management_System.Services;

public class AppDb
{
    private readonly string _connectionString;
    private readonly QueryLoader _queries;

    public AppDb(IConfiguration configuration, QueryLoader queries)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _queries = queries;
    }

    private async Task<MySqlConnection> OpenConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    private static async Task<T?> ReadSingleAsync<T>(MySqlCommand command, Func<MySqlDataReader, T> map)
    {
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? map(reader) : default;
    }

    private static async Task<List<T>> ReadListAsync<T>(MySqlCommand command, Func<MySqlDataReader, T> map)
    {
        var results = new List<T>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(map(reader));
        }

        return results;
    }

    private static UserRecord MapUser(MySqlDataReader reader) => new()
    {
        UserId = reader.GetInt32("user_id"),
        Email = reader.GetString("email"),
        PasswordHash = reader.GetString("password_hash"),
        UserRole = reader.GetString("user_role"),
        FullName = reader.GetString("full_name"),
        CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? null : reader.GetDateTime("created_at")
    };

    private static UserRecord MapUserWithoutPassword(MySqlDataReader reader) => new()
    {
        UserId = reader.GetInt32("user_id"),
        Email = reader.GetString("email"),
        UserRole = reader.GetString("user_role"),
        FullName = reader.GetString("full_name"),
        CreatedAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? null : reader.GetDateTime("created_at")
    };

    private static DocumentRecord MapDocument(MySqlDataReader reader) => new()
    {
        DocumentId = reader.GetInt32("document_id"),
        Title = reader.GetString("title"),
        CrewName = reader.GetString("crew_name"),
        CreationTime = reader.IsDBNull(reader.GetOrdinal("creation_time"))
            ? default
            : reader.GetDateTime("creation_time"),
        CreatedBy = reader.GetInt32("created_by")
    };

    private static VersionRecord MapVersion(MySqlDataReader reader) => new()
    {
        VersionId = reader.GetInt32("version_id"),
        VersionNumber = reader.GetInt32("version_number"),
        ExpirationDate = reader.GetDateTime("expiration_date"),
        LastModifiedTime = reader.IsDBNull(reader.GetOrdinal("last_modified_time"))
            ? default
            : reader.GetDateTime("last_modified_time"),
        DocumentId = reader.GetInt32("document_id"),
        LastModifiedBy = reader.GetInt32("last_modified_by")
    };

    private static AttachmentRecord MapAttachment(MySqlDataReader reader) => new()
    {
        AttachmentId = reader.GetInt32("attachment_id"),
        AttachmentName = reader.GetString("attachment_name"),
        AttachmentPath = reader.GetString("attachment_path"),
        AttachmentType = reader.GetString("attachment_type"),
        DocumentVersionId = reader.GetInt32("document_version_id")
    };

    public async Task<UserRecord?> GetUserByEmailAsync(string email)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "GetUserByEmail.sql"), connection);
        command.Parameters.AddWithValue("@Email", email);
        return await ReadSingleAsync(command, reader => new UserRecord
        {
            UserId = reader.GetInt32("user_id"),
            Email = reader.GetString("email"),
            PasswordHash = reader.GetString("password_hash")
        });
    }

    public async Task<UserRecord?> GetUserByIdAsync(int userId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "GetUserByID.sql"), connection);
        command.Parameters.AddWithValue("@UserId", userId);
        return await ReadSingleAsync(command, MapUserWithoutPassword);
    }

    public async Task<List<UserRecord>> ListUsersAsync()
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "ListUsers.sql"), connection);
        return await ReadListAsync(command, reader => new UserRecord
        {
            UserId = reader.GetInt32("user_id"),
            Email = reader.GetString("email"),
            UserRole = reader.GetString("user_role"),
            FullName = reader.GetString("full_name")
        });
    }

    public async Task<List<UserRecord>> SearchUsersByNameAsync(string search)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "SearchByName.sql"), connection);
        command.Parameters.AddWithValue("@Search", search);
        return await ReadListAsync(command, reader => new UserRecord
        {
            UserId = reader.GetInt32("user_id"),
            Email = reader.GetString("email"),
            UserRole = reader.GetString("user_role"),
            FullName = reader.GetString("full_name")
        });
    }

    public async Task<List<UserRecord>> FilterUsersByRoleAsync(string role)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "FilterByRole.sql"), connection);
        command.Parameters.AddWithValue("@UserRole", role);
        return await ReadListAsync(command, reader => new UserRecord
        {
            UserId = reader.GetInt32("user_id"),
            Email = reader.GetString("email"),
            UserRole = reader.GetString("user_role"),
            FullName = reader.GetString("full_name")
        });
    }

    public async Task CreateUserAsync(string email, string passwordHash, string role, string fullName)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "CreateUser.sql"), connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
        command.Parameters.AddWithValue("@UserRole", role);
        command.Parameters.AddWithValue("@FullName", fullName);
        await command.ExecuteNonQueryAsync();
    }

    public async Task SelfEditProfileAsync(int userId, string email, string fullName)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "SelfEditProfile.sql"), connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@FullName", fullName);
        await command.ExecuteNonQueryAsync();
    }

    public async Task AdminEditProfileAsync(int userId, string email, string role, string fullName)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "AdminEditProfile.sql"), connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@UserRole", role);
        command.Parameters.AddWithValue("@FullName", fullName);
        await command.ExecuteNonQueryAsync();
    }

    public async Task ChangePasswordAsync(int userId, string passwordHash)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Users", "ChangePassword.sql"), connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var sql = _queries.Load("Users", "DeleteUser.sql");
        await using var connection = await OpenConnectionAsync();

        foreach (var statement in sql.Split(';'))
        {
            var cleaned = statement.Trim();
            if (string.IsNullOrWhiteSpace(cleaned))
            {
                continue;
            }

            await using var command = new MySqlCommand(cleaned, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<DocumentRecord>> ListDocumentsByUserAsync(int userId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "ListDocumentsByUser.sql"), connection);
        command.Parameters.AddWithValue("@CreatedBy", userId);
        return await ReadListAsync(command, reader => new DocumentRecord
        {
            DocumentId = reader.GetInt32("document_id"),
            Title = reader.GetString("title"),
            CrewName = reader.GetString("crew_name"),
            CreationTime = reader.IsDBNull(reader.GetOrdinal("creation_time"))
                ? default
                : reader.GetDateTime("creation_time"),
            CreatedBy = userId
        });
    }

    public async Task<List<DocumentRecord>> ListAllDocumentsAsync()
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "ListAllDocuments.sql"), connection);
        return await ReadListAsync(command, MapDocument);
    }

    public async Task<List<DocumentRecord>> SearchDocumentsByTitleAsync(string search)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "SearchByDocumentTitle.sql"), connection);
        command.Parameters.AddWithValue("@Search", search);
        return await ReadListAsync(command, MapDocument);
    }

    public async Task<List<DocumentRecord>> FilterDocumentsByCrewAsync(string search)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "FilterByCrewName.sql"), connection);
        command.Parameters.AddWithValue("@Search", search);
        return await ReadListAsync(command, MapDocument);
    }

    public async Task<DocumentRecord?> GetDocumentByIdAsync(int documentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "GetDocumentById.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        return await ReadSingleAsync(command, MapDocument);
    }

    public async Task<int> CreateDocumentAsync(string title, string crewName, int createdBy)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "CreateDocument.sql"), connection);
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@CrewName", crewName);
        command.Parameters.AddWithValue("@CreatedBy", createdBy);
        await command.ExecuteNonQueryAsync();
        return (int)command.LastInsertedId;
    }

    public async Task UpdateDocumentAsync(int documentId, string title, string crewName)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "UpdateDocument.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@CrewName", crewName);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteDocumentAsync(int documentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Documents", "DeleteDocument.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<VersionRecord>> ListVersionsByDocumentAsync(int documentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "ListAllVersionsOfDocument.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        return await ReadListAsync(command, reader => new VersionRecord
        {
            VersionId = reader.GetInt32("version_id"),
            VersionNumber = reader.GetInt32("version_number"),
            ExpirationDate = reader.GetDateTime("expiration_date"),
            LastModifiedTime = reader.IsDBNull(reader.GetOrdinal("last_modified_time"))
                ? default
                : reader.GetDateTime("last_modified_time"),
            DocumentId = documentId,
            LastModifiedBy = reader.GetInt32("last_modified_by")
        });
    }

    public async Task<VersionRecord?> GetVersionByIdAsync(int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "GetVersionById.sql"), connection);
        command.Parameters.AddWithValue("@VersionId", versionId);
        return await ReadSingleAsync(command, MapVersion);
    }

    public async Task<VersionRecord?> GetLatestVersionAsync(int documentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "GetLatestVersion.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        return await ReadSingleAsync(command, MapVersion);
    }

    public async Task<int> GetNextVersionNumberAsync(int documentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "GetNextVersion.sql"), connection);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> CreateVersionAsync(int versionNumber, DateTime expirationDate, int documentId, int lastModifiedBy)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "CreateVersion.sql"), connection);
        command.Parameters.AddWithValue("@VersionNumber", versionNumber);
        command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
        command.Parameters.AddWithValue("@DocumentId", documentId);
        command.Parameters.AddWithValue("@LastModifiedBy", lastModifiedBy);
        await command.ExecuteNonQueryAsync();
        return (int)command.LastInsertedId;
    }

    public async Task UpdateVersionAsync(int versionId, DateTime expirationDate, int lastModifiedBy)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "UpdateThisVersion.sql"), connection);
        command.Parameters.AddWithValue("@VersionId", versionId);
        command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
        command.Parameters.AddWithValue("@LastModifiedBy", lastModifiedBy);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteVersionAsync(int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "DeleteThisVersion.sql"), connection);
        command.Parameters.AddWithValue("@VersionId", versionId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<ExpiringVersionRecord>> ListVersionsExpiringSoonAsync(int months)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "ListVersionsExpiringSoon.sql"), connection);
        command.Parameters.AddWithValue("@Months", months);
        return await ReadListAsync(command, reader => new ExpiringVersionRecord
        {
            VersionId = reader.GetInt32("version_id"),
            VersionNumber = reader.GetInt32("version_number"),
            ExpirationDate = reader.GetDateTime("expiration_date"),
            DocumentId = reader.GetInt32("document_id"),
            Title = reader.GetString("title"),
            CrewName = reader.GetString("crew_name")
        });
    }

    public async Task<List<AttachmentRecord>> ListAttachmentsByVersionAsync(int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "ListAttachmentsByDocumentVersion.sql"), connection);
        command.Parameters.AddWithValue("@DocumentVersionId", versionId);
        return await ReadListAsync(command, MapAttachment);
    }

    public async Task<AttachmentRecord?> GetAttachmentByIdAsync(int attachmentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "GetAnAttachment.sql"), connection);
        command.Parameters.AddWithValue("@AttachmentId", attachmentId);
        return await ReadSingleAsync(command, MapAttachment);
    }

    public async Task<int> CountAttachmentsByVersionAsync(int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "CountAttachments.sql"), connection);
        command.Parameters.AddWithValue("@DocumentVersionId", versionId);
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task CreateAttachmentAsync(string name, string path, string type, int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "CreateAttachment.sql"), connection);
        command.Parameters.AddWithValue("@AttachmentName", name);
        command.Parameters.AddWithValue("@AttachmentPath", path);
        command.Parameters.AddWithValue("@AttachmentType", type);
        command.Parameters.AddWithValue("@DocumentVersionId", versionId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateAttachmentAsync(int attachmentId, string name, string path, string type)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "UpdateAttachment.sql"), connection);
        command.Parameters.AddWithValue("@AttachmentId", attachmentId);
        command.Parameters.AddWithValue("@AttachmentName", name);
        command.Parameters.AddWithValue("@AttachmentPath", path);
        command.Parameters.AddWithValue("@AttachmentType", type);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAttachmentAsync(int attachmentId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("Attachments", "DeleteAttachment.sql"), connection);
        command.Parameters.AddWithValue("@AttachmentId", attachmentId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<string?> GetLastModifiedByNameAsync(int versionId)
    {
        await using var connection = await OpenConnectionAsync();
        await using var command = new MySqlCommand(_queries.Load("DocumentsVersions", "LastModifiedByName.sql"), connection);
        command.Parameters.AddWithValue("@VersionId", versionId);
        return await ReadSingleAsync(command, reader => reader.GetString("last_modified_by_name"));
    }
}
