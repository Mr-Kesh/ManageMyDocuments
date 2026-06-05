INSERT INTO attachments (attachment_name, attachment_path, attachment_type, document_version_id)
VALUES  (
            @AttachmentName,
            @AttachmentPath,
            @AttachmentType,
            @DocumentVersionId
        );