SELECT attachment_id, attachment_name, attachment_path, attachment_type, document_version_id
FROM attachments
WHERE attachment_id = @AttachmentId