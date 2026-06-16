SELECT full_name
FROM users u
INNER JOIN documents d ON u.user_id = d.created_by
WHERE d.document_id = @DocumentId
LIMIT 1;