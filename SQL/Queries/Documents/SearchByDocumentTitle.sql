SELECT * FROM documents
WHERE title LIKE CONCAT('%', @Search, '%')
ORDER BY title;