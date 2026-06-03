SELECT user_id, email, user_role, created_at, full_name
FROM users
WHERE user_id = @UserId