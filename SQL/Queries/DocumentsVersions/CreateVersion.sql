INSERT INTO documents_versions (version_number, expiration_date, document_id, last_modified_by)
VALUES  (
            @VersionNumber,
            @ExpirationDate,
            @DocumentId,
            @LastModifiedBy
        );