UPDATE documents
SET title = @Title, crew_name = @CrewName
WHERE document_id = @DocumentId;