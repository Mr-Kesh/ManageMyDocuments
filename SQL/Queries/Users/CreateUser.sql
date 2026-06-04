INSERT INTO users (email, password_hash, user_role, full_name)
VALUES  (
            @Email,
            @PasswordHash,
            @UserRole,
            @FullName
        );