SELECT version_id, version_number, expiration_date, last_modified_time, last_modified_by
FROM documents_versions
WHERE document_id = @DocumentId
ORDER BY version_number DESC;