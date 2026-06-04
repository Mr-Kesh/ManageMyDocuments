UPDATE users
SET user_role = @UserRole
WHERE user_id = @UserId;