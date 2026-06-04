SELECT user_id, email, password_hash
FROM users
WHERE email = @Email;