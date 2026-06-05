SELECT version_id, version_number, expiration_date, last_modified_time, document_id, last_modified_by
FROM documents_versions
WHERE version_id = @VersionId