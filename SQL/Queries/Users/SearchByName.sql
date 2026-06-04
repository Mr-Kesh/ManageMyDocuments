SELECT user_id, full_name, email, user_role FROM users
WHERE full_name LIKE CONCAT('%', @Search, '%')
ORDER BY full_name;