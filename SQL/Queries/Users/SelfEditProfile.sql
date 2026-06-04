UPDATE users
SET email = @Email, full_name = @FullName
WHERE user_id = @UserId;