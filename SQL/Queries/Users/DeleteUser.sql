DELETE FROM attachments
WHERE document_version_id IN (
    SELECT version_id FROM documents_versions WHERE document_id IN (
        SELECT document_id FROM documents WHERE created_by = @UserId
    )
);

DELETE FROM documents_versions
WHERE document_id IN (
    SELECT document_id FROM documents WHERE created_by = @UserId

) OR last_modified_by = @UserId;

DELETE FROM documents
WHERE created_by = @UserId;

DELETE FROM users
WHERE user_id = @UserId;