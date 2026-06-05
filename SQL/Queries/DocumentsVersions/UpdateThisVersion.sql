UPDATE documents_versions
SET expiration_date = @ExpirationDate, last_modified_by = @LastModifiedBy, last_modified_time = NOW()
WHERE version_id = @VersionId;