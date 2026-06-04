SELECT user_id, full_name, email, user_role FROM users
WHERE user_role = @UserRole;