SELECT * FROM documents
WHERE title LIKE CONCAT('%', @TitleSearch, '%')
  AND crew_name LIKE CONCAT('%', @CrewSearch, '%')
ORDER BY title;