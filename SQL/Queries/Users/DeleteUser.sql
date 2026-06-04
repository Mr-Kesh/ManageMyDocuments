/*  If deleting a user, we got to delete them from the entire database
    especially since there are foreign keys that depend on one another.
    A deleting cascade from bottom up
*/

/* Deleting From Attachments */
DELETE FROM attachments
WHERE document_version_id IN (
    SELECT version_id FROM documents_versions WHERE document_id IN (
        SELECT document_id FROM documents WHERE created_by = @UserId
    )
);


/* Deleting From Documents Versions That the User Created */
DELETE FROM documents_versions
WHERE document_id IN (
    SELECT document_id FROM documents WHERE created_by = @UserId

) OR last_modified_by = @UserId;


/* Deleting from Documents */
DELETE FROM documents
WHERE created_by = @UserId;


/* Deleting From Users */
DELETE FROM users
WHERE user_id = @UserId;