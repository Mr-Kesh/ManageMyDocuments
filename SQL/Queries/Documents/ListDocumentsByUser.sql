SELECT document_id, title, crew_name, creation_time FROM documents
WHERE created_by = @CreatedBy
ORDER BY title;