UPDATE attachments
SET attachment_name = @AttachmentName,
    attachment_path = @AttachmentPath,
    attachment_type = @AttachmentType
WHERE attachment_id = @AttachmentId;