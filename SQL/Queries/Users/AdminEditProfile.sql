UPDATE users
SET email = @Email, user_role = @UserRole, full_name = @FullName
WHERE user_id = @UserId;