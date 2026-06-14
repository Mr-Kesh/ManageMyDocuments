SELECT u.full_name as last_modified_by_name
FROM documents_versions dv
INNER JOIN users u ON dv.last_modified_by = u.user_id
WHERE dv.version_id = @VersionId;