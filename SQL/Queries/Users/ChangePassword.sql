UPDATE users
SET password_hash = @PasswordHash
WHERE user_id = @UserId;