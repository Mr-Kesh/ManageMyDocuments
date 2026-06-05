SELECT COALESCE(MAX(version_number), 0) + 1 AS next_version_number
FROM documents_versions
WHERE document_id = @DocumentId