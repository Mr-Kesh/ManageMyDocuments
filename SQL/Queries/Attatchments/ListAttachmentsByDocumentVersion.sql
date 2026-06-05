SELECT * FROM attachments
WHERE document_version_id = @DocumentVersionId
ORDER BY attachment_id;