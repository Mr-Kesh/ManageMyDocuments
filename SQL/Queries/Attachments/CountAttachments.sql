SELECT COUNT(*) AS amount_of_attachments
FROM attachments
WHERE document_version_id = @DocumentVersionId