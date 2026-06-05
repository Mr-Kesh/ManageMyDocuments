SELECT * FROM documents
WHERE crew_name LIKE CONCAT('%', @Search, '%')
ORDER BY crew_name;